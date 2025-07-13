using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.ViewComponents.Writer
{
    public class WriterMessageNotification : ViewComponent
    {
        Message2Manager mm = new Message2Manager(new EfMessage2Repository());
        public IViewComponentResult Invoke()
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
                    return View("Error", "Yazar bulunamadı.");
                }
            }
        }
    }
}
