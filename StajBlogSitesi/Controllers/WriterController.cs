using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.Controllers
{
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterRepository());
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WriterProfile()
        {
            return View();
        }
        public IActionResult WriterMail()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult WriterNavbarPartial()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult WriterFooterPartial()
        {
            return PartialView();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult WriterEditProfile()
        {
            var usermail = User.Identity.Name;
            var writer = wm.GetWriterByEmail(usermail);
            return View(writer);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult WriterEditProfile(Writer w, IFormFile file)
        {
            WriterProfileEditValidator wpev = new WriterProfileEditValidator();
            ValidationResult results = wpev.Validate(w);

            if (results.IsValid)
            {
                var usermail = User.Identity.Name;
                var writerFromDb = wm.GetWriterByEmail(usermail);

                if (writerFromDb == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View();
                }

                // Güncellenebilir alanları View'den gelen değerlerle değiştir
                writerFromDb.WriterName = w.WriterName;
                writerFromDb.WriterAbout = w.WriterAbout;

                // Dosya varsa güncelle
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadsWriter", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    writerFromDb.WriterImage = "/uploadsWriter/" + fileName;
                }

                // Şifre güncellenmiyor, eski şifre korunuyor!
                wm.Update(writerFromDb);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string CurrentPassword, string NewPassword, string ConfirmNewPassword)
        {
            var usermail = User.Identity.Name;
            var writer = wm.GetWriterByEmail(usermail);

            if (writer == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View();
            }

            PasswordHasher<Writer> passwordHasher = new PasswordHasher<Writer>();
            var result = passwordHasher.VerifyHashedPassword(writer, writer.WriterPassword, CurrentPassword);

            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("CurrentPassword", "Mevcut şifre yanlış.");
                return View();
            }

            // Yeni şifre validasyonları
            if (string.IsNullOrEmpty(NewPassword))
            {
                ModelState.AddModelError("NewPassword", "Şifre boş olamaz.");
            }
            else
            {
                if (NewPassword.Length < 6)
                    ModelState.AddModelError("NewPassword", "Şifre en az 6 karakter olmalıdır.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(NewPassword, @"[A-Z]+"))
                    ModelState.AddModelError("NewPassword", "Şifre en az bir büyük harf içermelidir.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(NewPassword, @"[a-z]+"))
                    ModelState.AddModelError("NewPassword", "Şifre en az bir küçük harf içermelidir.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(NewPassword, @"[0-9]+"))
                    ModelState.AddModelError("NewPassword", "Şifre en az bir rakam içermelidir.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(NewPassword, @"[\W]+"))
                    ModelState.AddModelError("NewPassword", "Şifre en az bir özel karakter içermelidir.");
            }

            if (NewPassword != ConfirmNewPassword)
            {
                ModelState.AddModelError("ConfirmNewPassword", "Yeni şifreler eşleşmiyor.");
            }

            if (ModelState.IsValid)
            {
                writer.WriterPassword = passwordHasher.HashPassword(writer, NewPassword);
                wm.Update(writer);
                ViewBag.SuccessMessage = "Şifre başarıyla değiştirildi.";
            }

            return View();
        }



    }
}
