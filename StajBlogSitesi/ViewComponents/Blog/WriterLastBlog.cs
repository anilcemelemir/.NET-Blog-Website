using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.ViewComponents.Blog
{
    public class WriterLastBlog : ViewComponent
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());

        public IViewComponentResult Invoke(int writerId)
        {
            var values = bm.GetBlogListByWriter(writerId)
                          .OrderByDescending(x => x.BlogCreateDate)
                          .Take(3)
                          .ToList();
            return View(values);
        }
    }
}
