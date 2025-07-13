using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StajBlogSitesi.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(Writer w, bool rememberMe)
        {
            LoginValidator lv = new LoginValidator();
            ValidationResult results = lv.Validate(w);

            if (!results.IsValid)
            {
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.LoginError = "Geçersiz giriş bilgileri.";
                return View(w);
            }

            Context c = new Context();
            var writer = c.Writers.FirstOrDefault(x => x.WriterMail == w.WriterMail);

            if (writer != null)
            {
                if (!writer.WriterStatus)
                {
                    ViewBag.LoginError = "Hesabınız henüz onaylanmamış. Lütfen e-posta adresinize gelen linkten onay verin.";
                    return View(w);
                }

                PasswordHasher<Writer> passwordHasher = new PasswordHasher<Writer>();
                var result = passwordHasher.VerifyHashedPassword(writer, writer.WriterPassword, w.WriterPassword);

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, w.WriterMail),
                new Claim(ClaimTypes.GivenName, writer.WriterName)
            };
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = rememberMe
                    };
                    var useridentity = new ClaimsIdentity(claims, "a");
                    ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "Blog");
                }
                else
                {
                    ViewBag.LoginError = "Şifre yanlış.";
                    return View(w);
                }
            }
            else
            {
                ViewBag.LoginError = "Kullanıcı bulunamadı.";
                return View(w);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Blog");
        }
        [AllowAnonymous]
        public IActionResult ConfirmEmail(string email)
        {
            Context c = new Context();
            var user = c.Writers.FirstOrDefault(x => x.WriterMail == email);
            if (user != null && !user.WriterStatus)
            {
                user.WriterStatus = true;  // Email onaylandı
                c.SaveChanges();
                TempData["Message"] = "Email başarıyla onaylandı, giriş yapabilirsiniz.";
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
