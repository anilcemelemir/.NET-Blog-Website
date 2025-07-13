using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace StajBlogSitesi.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            using (var context = new Context())
            {
                var userEmail = User.Identity.Name;

                var writer = context.Writers.FirstOrDefault(x => x.WriterMail == userEmail);

                if (writer != null)
                {
                    var writerId = writer.WriterID;

                    ViewBag.TotalBlogCount = context.Blogs.Count().ToString();
                    ViewBag.TotalWritersBlogCount = context.Blogs.Where(x => x.WriterID == writerId).Count();
                    ViewBag.CategoryCount = context.Categories.Count();
                }
                else
                {
                    ViewBag.TotalBlogCount = "0";
                    ViewBag.TotalWritersBlogCount = "0";
                    ViewBag.CategoryCount = "0";
                }

                return View();
            }
        }
    }
}
