using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Linq;

namespace StajBlogSitesi.Controllers
{
    public class RegisterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterRepository());

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(Writer w, string confirmPassword)
        {
            // Kullanıcı zaten var mı kontrolü
            Context c = new Context();
            var existingWriter = c.Writers.FirstOrDefault(x => x.WriterMail == w.WriterMail);
            if (existingWriter != null)
            {
                ModelState.AddModelError("", "Bu e-posta adresi zaten kullanılıyor.");
                return View(w);
            }

            // Şifre ve validasyon kontrolü
            WriterValidator wv = new WriterValidator();
            ValidationResult results = wv.Validate(w);
            if (!results.IsValid)
            {
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(w);
            }

            if (w.WriterPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Şifreler eşleşmiyor.");
                return View(w);
            }

            PasswordHasher<Writer> passwordHasher = new PasswordHasher<Writer>();
            w.WriterPassword = passwordHasher.HashPassword(w, w.WriterPassword);
            // Kayıt başarılı ise kullanıcıyı kaydet ve mail gönder
            w.WriterStatus = false; // Onay beklediği için durumu false
            w.WriterAbout = "Yazar Hakkında Kısmıdır.";
            w.WriterImage = "/uploadsWriter/default.jpg";
            wm.Add(w);

            // Kullanıcıya mail gönderme
            SendConfirmationEmail(w.WriterMail);

            TempData["RegisterSuccess"] = true;
            return RedirectToAction("Index", "Login");
        }

        // Mail gönderme fonksiyonu kısımı
        public void SendConfirmationEmail(string userEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Blog Sitesi", "blogsite@example.com"));
            message.To.Add(new MailboxAddress(userEmail, userEmail));
            message.Subject = "Hesap Onaylama";

            string confirmationUrl = Url.Action("ConfirmEmail", "Login", new {email = userEmail}, Request.Scheme);

            message.Body = new TextPart("plain")
            {
                Text = "Hesabınızı onaylamak için lütfen aşağıdaki linke tıklayın:\n\n" +
                       confirmationUrl
            };

            using (var client = new SmtpClient())
            {
                client.Connect("sandbox.smtp.mailtrap.io", 587, false);
                client.Authenticate("af4a1da2d54bd6", "6928f770c14083");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
