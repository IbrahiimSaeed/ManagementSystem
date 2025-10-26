using Demo.BusinessLogic.Services.EmailSender;
using Demo.DataAccess.Models.IdentityModule;
using Demo.DataAccess.Models.Shared;
using Demo.Presentation.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.JavaScript;

namespace Demo.Presentation.Controllers
{
    public class AccountController(UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> _signInManager,
        IEmailSender _emailSender) : Controller
    {
        #region Register
        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);
            var user = new ApplicationUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName
            };
            var result = _userManager.CreateAsync(user, registerViewModel.Password).Result;
            if (result.Succeeded)
                return RedirectToAction("Login");
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(registerViewModel);
            }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) return View(loginViewModel);
            var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if(user is not  null)
            {
                var flag =_userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;
                if (flag)
                {
                    var result = _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false).Result;
                    if (result.IsNotAllowed)
                        ModelState.AddModelError("", "Your Account is not allowed");
                    if (result.IsLockedOut)
                        ModelState.AddModelError("", "Your Account is locked");
                    if (result.Succeeded)
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            ModelState.AddModelError("", "Invalid Login!");
            return View(loginViewModel);
        }

        #endregion

        #region SignOut
        [HttpGet]
        public new ActionResult SignOut()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region FordetPassword
        [HttpGet]
        public IActionResult ForgetPassword() => View();
        [HttpPost]
        public IActionResult SendResetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(forgetPasswordViewModel.Email).Result;
                if (user is not null)
                {
                    //Request.Scheme ==> BaseUrl
                    var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                    var url = Url.Action("ResetPassword", "Account", new { email = forgetPasswordViewModel.Email, token = token }, Request.Scheme);
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = url
                    };
                    _emailSender.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Operation Please Try Again");
                }
            }
            return View(forgetPasswordViewModel);
        }
        [HttpGet]
        public IActionResult CheckYourInbox() => View();
        #endregion
        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if(ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var user = _userManager.FindByEmailAsync(email).Result;
                if (user != null)
                {
                    var result = _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.NewPassword).Result;
                    if (result.Succeeded)
                        return RedirectToAction(nameof(Login));
                }
            }
            ModelState.AddModelError("", "Invalid Operation , Please Try Again");
            return View(resetPasswordViewModel);
        }
        #endregion
    }
}
