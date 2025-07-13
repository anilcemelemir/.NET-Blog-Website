using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.ViewComponents.Category
{
    public class CategoryList : ViewComponent
    {
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());
        BlogManager bm = new BlogManager(new EfBlogRepository());

        public IViewComponentResult Invoke()
        {
            var categoryBlogCounts = cm.GetList().ToDictionary(
            category => category.CategoryName,
            category => bm.GetList().Count(blog => blog.CategoryID == category.CategoryID)
        );

            return View(categoryBlogCounts);
        }
    }
}
