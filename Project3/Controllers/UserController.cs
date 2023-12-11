using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Mail;
using Project3.Models;
using Project3.Shared;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Project3.Controllers
{
    public class UserController : BaseController
    {
        private readonly IMailService _mail;

        public UserController(MyDbContext context, IMailService mail) : base(context)
        {
            _mail = mail;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/api/user/listfriend/{id}/{room}")]
        public async Task<IActionResult> GetListFriend(int id, string room)
        {
            var roomId = room.Split("_s")[1];
            var roomData = await _context.RoomMembers
                .Include(r => r.user)
                .Where(r => r.RoomId == int.Parse(roomId))
                .Select(r => new FriendDto
                {
                    Avatar = r.user.Avatar,
                    Name = r.user.UserName,
                    id = r.user.Id,
                    IsSender = r.IsMember,

                    Description = r.user.Description
                }).ToListAsync();

            var result = await _context.Friends
                .Where(f => f.SendId == id
                && f.status == "Accept"
               )
                .Join(
                _context.Users,
                f => f.RecieveId,
                u => u.Id,
                (f, u) => new FriendDto
                {
                    Avatar = u.Avatar,
                    Name = u.UserName,
                    id = u.Id,
                    Description = u.Description,
                }
                ).Union(
                 _context.Friends.Where(f => f.RecieveId == id
                 && f.status == "Accept")
                .Join(
                _context.Users,
                f => f.SendId,
                u => u.Id,
                (f, u) => new FriendDto
                {
                    Avatar = u.Avatar,
                    Name = u.UserName,
                    id = u.Id,
                    Description = u.Description,
                }
                )
                ).ToListAsync();

            result.RemoveAll(r => roomData.Any(d => d.id == r.id && d.IsSender));
            return Ok(result);
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
            if (check)
            {
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
        public async Task<IActionResult> Verify(int id, string code, string device_id)
        {
            var check = _context.verifieds.FirstOrDefault(v => v.UserId == id && v.Code == code && v.IsLife == true);
            if (check == null)
            {
                ViewData["error"] = "your code wrong, please checking again";
                return View("verify", id);
            }

            if (BaseMethod.ConvertToUnixTimestamp(DateTime.Now) - BaseMethod.ConvertToUnixTimestamp(check.CreatedDate) > 600)
            {
                check.IsLife = false;
                await _context.SaveChangesAsync();
                ViewData["error"] = "your code expired, please click to resend code ";
                return View("verify", id);
            }

            var verify = _context.verifieds.FirstOrDefault(v => v.UserId == id && v.Code == code && v.IsLife == true);
            verify.IsLife = false;
            await _context.SaveChangesAsync();
            var user = await _context.Users.FindAsync(id);
            user.Verified = true;
            await _context.SaveChangesAsync();
            await this.Handle(user, device_id);
            return RedirectToAction("Index", "Home");
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
                var check = await _context.Users.FirstOrDefaultAsync(u => u.Phone == $"{data.Head}{data.Phone}");
                if (check != null)
                {
                    ViewData["error"] = "This phone was registed, please cheking again, or use orther phone";
                    return View();
                }
                var CheckName = _context.Users
                    .FirstOrDefaultAsync(u => u.UserName.ToLower() == data.Name.ToLower());
                if (check != null)
                {
                    ViewData["error"] = "This phone was used, please use another name";
                    return View();
                }

                var user = new User(data);

                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                await _context.UserRoles.AddAsync(new UserRole
                {
                    UserId = user.Id,
                    RoleId = _context.Roles.FirstOrDefault(r => r.RoleName == "user").Id
                });
                await _context.SaveChangesAsync();

                var code = new Verified()
                {
                    UserId = user.Id,
                };

                await _context.AddAsync(code);
                await _context.SaveChangesAsync();

                this.SendMessage(user.Phone, code.Code);

                return RedirectToAction("Verify", new
                {
                    id = user.Id
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
                    ViewData["Error"] = "Your phone is wrong. Please checking again";
                    return View("Login");
                }
                //check password is correct
                else if (!BCrypt.Net.BCrypt.Verify(data.Password, check.Password))
                {
                    ViewData["Error"] = "Your Password is wrong Please checking again";
                    return View("Login");
                }
                else if (!check.Verified)
                {
                    TempData["verify"] = "your was not verified, Please verify your phone";
                    return RedirectToAction("Verify", new { id = check.Id });
                }
                else if (check.Devices != null && check.Devices.Where(d => d.DeviceId == data.Device_id).FirstOrDefault() == null)
                {
                    TempData["verify"] = "your account doesn't access in this device. please verify ";
                    return RedirectToAction("Verify", new { id = check.Id });
                }
                else
                {
                    await this.Handle(check, data.Device_id);
                    if (check.Roles.Where(r => r.Role.RoleName == "admin"
                    || r.Role.RoleName == "writer"
                    || r.Role.RoleName == "manager").ToList() != null)
                    {
                        return RedirectToAction("Admin", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View("Login");
        }

        public async Task Handle(User check, string device_id)

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
                        UserId = check.Id
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
                    claims.Add(new Claim(ClaimTypes.Role, i.Role.RoleName));
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

        public void SendMessage(string phone, string code)
        {
            var accountSid = "ACc0caca8533ac9ab66cabf5bf31e6cd3c";
            var authToken = "a48f320bee41bf408399de2c929445c6";
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
            body: $"Your verify code : {code}",
            from: new Twilio.Types.PhoneNumber("+13348010639"),
            to: new Twilio.Types.PhoneNumber(phone)
        );
        }

        //forgot password
        [HttpGet]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    // Tao code
                    user.ResetToken = Guid.NewGuid().ToString();
                    user.ResetTokenExpiryTime = DateTime.UtcNow.AddHours(1);

                    await _context.SaveChangesAsync();

                    // send link
                    var callbackUrl = Url.Action("ConfirmResetPassword", null, new { userId = user.Id, token = user.ResetToken }, protocol: HttpContext.Request.Scheme); ;
                    var emailMessage = new MailData
                    {
                        EmailTo = user.Email,
                        EmailSubject = "Reset Password",
                        EmailBody = $"Please reset your password by clicking <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>here</a>."
                    };

                    _mail.SendMail(emailMessage);

                    TempData["success"] = "Reset password link has been sent to your email.";
                    return RedirectToAction("ForgotPassword");
                }

                ModelState.AddModelError(string.Empty, "User with this email does not exist.");
            }

            return View("ForgotPassword", model);
        }

        //confirm
        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmResetPassword")]
        public IActionResult ConfirmResetPassword(string userId, string token)
        {
            // Check user id vs token
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error");
            }

            ViewData["UserId"] = userId;
            ViewData["Token"] = token;

            return View("ConfirmResetPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ConfirmResetPassword")]
        public async Task<IActionResult> ConfirmResetPassword(string userId, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error");
            }

            // Lấy thông tin user từ userId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            // Kiểm tra user và token có hợp lệ
            if (user != null && user.ResetToken == token && user.ResetTokenExpiryTime > DateTime.UtcNow)
            {
                user.ResetToken = null;
                user.ResetTokenExpiryTime = null;

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

                await _context.SaveChangesAsync();

                TempData["success"] = "Password reset successfully. You can now log in with your new password.";

                return RedirectToAction("Login");
            }

            return RedirectToAction("Error");
        }
    }
}