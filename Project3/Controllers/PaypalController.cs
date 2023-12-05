using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using Project3.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Project3.Shared;
using Project3.Data;
using Microsoft.EntityFrameworkCore;

namespace Project3.Controllers
{
    [Authorize]
    public class PaypalController : BaseController
    {
        private readonly ILogger<PaypalController> _logger;
        private static Payment createPayment;
        private IHttpContextAccessor httpContextAccessor;
        private IConfiguration _configuration;

        public PaypalController(ILogger<PaypalController> logger, IHttpContextAccessor accessor, IConfiguration iconfiguration, MyDbContext context) : base(context)
        {
            _logger = logger;
            httpContextAccessor = accessor;
            _configuration = iconfiguration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentSuccess()
        {
            return View();
        }

        public ActionResult PaymentFailed()
        {
            return View();
        }

        public ActionResult SuccessView()
        {
            return View();
        }

        public async Task<ActionResult> PaymentWithPaypal(string Cancel = null, string blogId = "", string PayerID = "", string guidd = "")
        {
            var ClientID = _configuration.GetValue<string>("PayPal:Key");
            var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
            var mode = _configuration.GetValue<string>("PayPal:mode");
            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
            try
            {
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/Paypal/PaymentWithPaypal?";
                    var guid = Guid.NewGuid().ToString().Substring(0, 5);
                    /* guid = guidd;*/
                    createPayment = await this.CreatePayment(apiContext, baseURI + "guidd=" + guid, blogId);
                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    /*  httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);*/
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var paymentId = createPayment.id;

                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("PaymentFailed");
                    }
                    var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                    var data = await _context.ServiceRegistereds
                             .Where(s => s.UserId == int.Parse(id) && s.Status == "Pending")
                             .ToListAsync();
                    foreach (var i in data)
                    {
                        i.Status = "Purchased";
                    }

                    await _context.SaveChangesAsync();

                    return View("PaymentSuccess");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            return View("SuccessView");
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private async Task<Payment> CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            var userId = HttpContext.User.FindFirst(c => c.Type == "id").Value;
            var registered = await _context.ServiceRegistereds
                         .Include(x => x.ServicePrice)
                         .ThenInclude(x => x.Service)
                         .Where(x => x.UserId == Int32.Parse(userId))
                         .ToListAsync();
            foreach (var item in registered)
            {
                itemList.items.Add(new Item()
                {
                    name = item.ServicePrice.Service.Name,
                    currency = "USD",
                    price = item.ServicePrice.Price.ToString(),
                    quantity = "1",
                    sku = "asd"
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = registered.Sum(x => x.ServicePrice.Price).ToString(),
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return this.payment.Create(apiContext);
        }
    }
}