using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StajBlogSitesi.Controllers
{
    public class NewsLetterController : Controller
    {
        NewsLetterManager nm = new NewsLetterManager(new EfNewsLetterRepository());
        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult SubscribeMail()
        {
            return PartialView();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SubscribeMail(NewsLetter p)
        {
            p.MailStatus = true;
            nm.AddNewsLetter(p);
            TempData["SubscriptionSuccess"] = "Bize abone olduğunuz için teşekkür ederiz!";
            return RedirectToAction("BlogDetails", "Blog");
        }
    }
}
