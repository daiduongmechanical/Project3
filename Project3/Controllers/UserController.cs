using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project3.Data;
using Project3.Dtos;
using Project3.Shared;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Identity.Client;
using System.Numerics;
using Newtonsoft.Json;
using Project3.Mail;
using Twilio.TwiML.Messaging;
using YamlDotNet.Core.Tokens;

namespace Project3.Controllers
{
    public class UserController : BaseController
    {
        private readonly IMailService _mail;

        public UserController(MyDbContext context, IMailService mail) : base(context)
        {
            _mail=mail;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ResendPhone/{id}")]
        public async Task<IActionResult> ReSendPone(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                TempData["verify"] = "Your account doesn't exist please checking again";
                return RedirectToAction("verify", new { id = id });
            }

            var code = new Verified()
            {
                UserId = id
            };

           await _context.AddAsync(code);
           await _context.SaveChangesAsync();

            this.SendMessage(user.Phone, code.Code);
            TempData["success"] = "Code send to your number. Please click again if not recieve code ";
            return RedirectToAction("verify", new { id = id });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("verifyMail/{id}")]
        public async Task<IActionResult> VerifyMail(int id)
        {
            var email = await _context.Users.FindAsync(id);
            if (email == null)
            {
                TempData["verify"] = "Your account doesn't exist please checking again";
                return RedirectToAction("verify", new { id = id });
            }

            if (email.Email == null)
            {
                TempData["verify"] = "Your account doesn't contain an email address";
                return RedirectToAction("verify", new { id = id });
            }

            var Code = new Verified() { UserId = id };
            _context.verifieds.Add(Code);
            await _context.SaveChangesAsync();
            var message = new MailData()
            {
                EmailTo = email.Email,
                EmailSubject = "confirm your account",
                EmailBody = $"Your active code is : {Code.Code}"
            };
            var check = _mail.SendMail(message);
           if(check) {

                TempData["verify"] = "Verify code was sent to your email. please checking agin. remember this code just survie in 10 minute";
                return RedirectToAction("verify", new { id = id });
            }
            TempData["error"] = "have error occur. please contect to support";
            return RedirectToAction("verify", new { id = id });

        }

        [Route("verify/{id}")]
        [AllowAnonymous]
        public IActionResult Verify(int id)
        {
            BaseMethod.ConvertTempData(TempData, ViewData, "error");
            BaseMethod.ConvertTempData(TempData, ViewData, "verify");
            BaseMethod.ConvertTempData(TempData, ViewData, "success");

            return View("Verify", id);
        }
        [Route("verify/{id}")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Verify(int id,string code,string device_id)
        {
            var check = _context.verifieds.FirstOrDefault(v => v.UserId == id && v.Code == code && v.IsLife==true );
            if (check==null)
            {
                ViewData["error"] = "your code wrong, please checking again" ;
                return View("verify",id);
      
            }

          if(BaseMethod.ConvertToUnixTimestamp(DateTime.Now)-BaseMethod.ConvertToUnixTimestamp(check.CreatedDate) > 600)
            {
                check.IsLife = false;
              await  _context.SaveChangesAsync();
                ViewData["error"] = "your code expired, please click to resend code ";
                return View("verify", id);
            }
            
          var verify=  _context.verifieds.FirstOrDefault(v => v.UserId == id && v.Code == code && v.IsLife == true);
            verify.IsLife = false;
            await _context.SaveChangesAsync();
         var  user= await _context.Users.FindAsync(id);
            user.Verified = true;
            await _context.SaveChangesAsync();
            await this.Handle(user, device_id);
            return RedirectToAction("Index","Home");

            

        }

     


     
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
           
            return View("Register");
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
      

        public async Task<IActionResult> Register(RegisterDto data)
        {
         
            if (ModelState.IsValid)
            {
                var check= await _context.Users.FirstOrDefaultAsync(u=>u.Phone== $"{data.Head}{data.Phone}");
                if (check !=null)
                {
                    ViewData["error"] = "This phone was registed, please cheking again, or use orther phone";
                    return View();
                }
                var user = new User(data);

                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                await _context.UserRoles.AddAsync(new UserRole
                {
                    UserId=user.Id,
                    RoleId= _context.Roles.FirstOrDefault(r=>r.RoleName=="user").Id

                });
                await _context.SaveChangesAsync();

                var code = new Verified()
                {
                    UserId = user.Id,
                };
             
               await _context.AddAsync(code);
               await _context.SaveChangesAsync();

                this.SendMessage(user.Phone,code.Code);

                return RedirectToAction("Verify", new
                {
                    id=user.Id
                });
            }
            return View();
        }

        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
      /*  BaseMethod.ConvertTempData(TempData, ViewData,)*/

            
            return View("Login");
        }

        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto data)
        {
          
            if (ModelState.IsValid)
            {
                //check phone correct
                var check = await _context.Users.Where(u => u.Phone == $"{data.Head}{data.Phone}")
                    .Include(u => u.Roles).ThenInclude(UserRole => UserRole.Role)
                    .Include(u => u.Devices)
                    .FirstOrDefaultAsync();

                if (check == null)
                {
                    ViewData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng vui lòng kiểm tra lại";
                    return View("Login");

                }
                //check password is correct
                else if (!BCrypt.Net.BCrypt.Verify(data.Password, check.Password))
                {
                    ViewData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng vui lòng kiểm tra lại";
                    return View("Login");
                } else if (!check.Verified)
                {
                    TempData["verify"] = "your was not verified, Please verify your phone";
                    return RedirectToAction("Verify", new {id=check.Id});
                } else if (check.Devices != null && check.Devices.Where(d => d.DeviceId == data.Device_id).FirstOrDefault() == null)
                {
                    TempData["verify"] = "your account doesn't access in this device. please verify ";
                    return RedirectToAction("Verify", new { id = check.Id });
                } else
                {

                    await this.Handle(check, data.Device_id);
                    return RedirectToAction("Index", "Home");
                } 
            }
            return View("Login");
        }

        public async Task Handle(User check,string device_id)
       
        {
            var checkDevice = await _context.Devices.FirstOrDefaultAsync(d => d.Id == check.Id && d.DeviceId == device_id);

            if (checkDevice == null)
            {
                var info = this.GetIp();
                if (info != null)
                {
                    await _context.Devices.AddAsync(new Models.Device
                    {
                        DeviceId = device_id,
                        DeviceName = info.Os,
                        DeviceType = info.Device,
                        UserId=check.Id
                    });
                   await _context.SaveChangesAsync();
                }
            }

          

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, check.UserName));
            claims.Add(new Claim("id", check.Id.ToString()));
			claims.Add(new Claim("avatar", check.Avatar));
			if (check.Roles != null)
            {
                foreach (var i in check.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role,i.Role.RoleName));
                };
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };
            await HttpContext
                .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
        }
        public void SendMessage(string phone,string code)
        {
            var accountSid = "ACc0caca8533ac9ab66cabf5bf31e6cd3c";
            var authToken = "57eb1aac0aaa5093de32433ec8fb1072";
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
            body: $"Your verify code : {code}",
            from: new Twilio.Types.PhoneNumber("+13348010639"),
            to: new Twilio.Types.PhoneNumber(phone)
        );
        }

    }
        
    }

