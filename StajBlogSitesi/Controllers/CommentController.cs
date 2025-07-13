using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.Controllers
{
    public class CommentController : Controller
    {
        CommentManager cm = new CommentManager(new EfCommentRepository());
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult PartialAddComment()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult PartialAddComment(Comment c, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity.Name;
                using (var context = new Context())
                {
                    var writer = context.Writers.FirstOrDefault(x => x.WriterMail == userEmail);
                    if (writer != null)
                    {
                        c.CommentUserName = writer.WriterName; // Yorumda kullanıcının adını otomatik ekle
                    }
                }
            }

            c.CommentDate = DateTime.Now;
            c.CommentStatus = true;
            c.BlogID = id;
            cm.CommentAdd(c);
            return RedirectToAction("BlogDetails", "Blog", new { id = id });
        }
        public PartialViewResult CommentListByBlog(int id)
        {
            var values = cm.GetList(id);
            return PartialView(values);
        }
    }
}
