using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}


        #region Sign Up

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid) //sersver side validation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if(user is null) 
                {
					user = new ApplicationUser()
					{
						UserName = model.Email.Split("@")[0], //alaaatefe@gmail.com ==> alaaatefe
						Email = model.Email,
						IsAgree = model.IsAgree,
						FName = model.FName,
						LName = model.LName,
					};
					var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
				}

                ModelState.AddModelError(string.Empty, " user name is already existes :(");
                
            }
            return View(model);
        }

		#endregion

		#region sign In

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user=await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flag= await _userManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false);
                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
            return View(model);

        }

		#endregion

		#region Sign Out

        public async new Task<IActionResult> SignOut() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendResetPasswordUrl (ForgetPasswordViewModel model)
        {
            
            if(ModelState.IsValid)
            {
				var user = await _userManager.FindByEmailAsync(model.Email);
				
                if (user is null) 
                {
                    var token=await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordUrl = Url.Action("ResetPassword", "Account", new {email=model.Email,token=token},Request.Scheme);
                    //

					var email = new Email()
                    {
                        Subject = "Reset your passwordd",
                        Recipients = model.Email,
                        Body = ResetPasswordUrl
                    };
                    EmailSettings.SenEmail(email);
                    return RedirectToAction(nameof(ChickYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
			}
            return View(model);

        }

        public IActionResult ChickYourInbox()
        {
            return View();
        }

		#endregion

		#region Reset Password

        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"]=email;
            TempData["token"]=token;
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);

				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

				if (result.Succeeded)
					return RedirectToAction(nameof(SignIn));
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
            
            return View(model);
        }

		#endregion

		//ctrl + k + s

	}
}
