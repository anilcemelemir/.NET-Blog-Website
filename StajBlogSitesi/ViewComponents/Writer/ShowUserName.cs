using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.ViewComponents.Writer
{
    public class ShowUserName : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string userName = string.Empty;

            if (User.Identity.IsAuthenticated)
            {
                using (var context = new Context())
                {
                    var userEmail = User.Identity.Name;
                    var writer = context.Writers.FirstOrDefault(x => x.WriterMail == userEmail);
                    if (writer != null)
                    {
                        userName = writer.WriterName;
                    }
                }
            }

            return View("Default", userName);
        }
    }
}
