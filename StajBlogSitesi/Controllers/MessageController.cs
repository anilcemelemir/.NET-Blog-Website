using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.Controllers
{
    public class MessageController : Controller
    {
        Message2Manager mm = new Message2Manager(new EfMessage2Repository());
        WriterManager wm = new WriterManager(new EfWriterRepository());

        [AllowAnonymous]
        public IActionResult Inbox()
        {
            var userEmail = User.Identity.Name;

            using (var context = new Context())
            {
                var writer = context.Writers.FirstOrDefault(x => x.WriterMail == userEmail);

                if (writer != null)
                {
                    var writerId = writer.WriterID;
                    var values = mm.GetInboxListByWriter(writerId);
                    return View(values);
                }
                else
                {
                    return NotFound("Yazar bulunamadı.");
                }
            }
        }

        [AllowAnonymous]
        public IActionResult MessageDetails(int id)
        {
            var values = mm.GetbyId(id);
            return View(values);
        }
        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(Message2 message, string ReceiverName)
        {
            var senderMail = User.Identity.Name;
            using (var context = new Context())
            {
                var sender = context.Writers.FirstOrDefault(x => x.WriterMail == senderMail);
                if (sender == null)
                {
                    ViewBag.ErrorMessage = "Gönderici kullanıcı bulunamadı.";
                    return View(message);
                }

                var receiver = context.Writers.FirstOrDefault(x => x.WriterName == ReceiverName);
                if (receiver == null)
                {
                    ViewBag.ErrorMessage = "Alıcı kullanıcı bulunamadı.";
                    return View(message);
                }

                MessageValidator mv = new MessageValidator();
                ValidationResult results = mv.Validate(message);

                if (results.IsValid)
                {

                    message.SenderID = sender.WriterID;
                    message.ReceiverID = receiver.WriterID;
                    message.MessageDate = DateTime.Now;
                    message.MessageStatus = true;

                    context.Message2s.Add(message);
                    context.SaveChanges();
                }
                else
                {
                    foreach (var error in results.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(message);
                }


            }

            return RedirectToAction("Inbox", "Message");
        }

    }
}