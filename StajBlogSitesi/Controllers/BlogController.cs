using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StajBlogSitesi.Controllers
{
    public class BlogController : Controller
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        [AllowAnonymous]
        public IActionResult Index()
        {
            var values = bm.GetBlogListWithCategory();
            return View(values);
        }
        [AllowAnonymous]
        public IActionResult BlogDetails(int id)
        {
            ViewBag.ID = id;
            Blog myBlog = bm.GetbyId(id);
            ViewBag.WriterID = myBlog.WriterID;
            return View(myBlog);
        }
        public IActionResult BlogListByWriter()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity.Name;

                using var context = new Context();

                var writer = context.Writers.FirstOrDefault(x => x.WriterMail == userEmail);

                if (writer != null)
                {
                    var writerId = writer.WriterID;

                    var values = bm.GetListWithCategoryByWriterBm(writerId);

                    return View(values);
                }
                else
                {
                    return NotFound("Writer not found.");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult BlogAdd()
        {
            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Value = x.CategoryID.ToString(),
                                                       Text = x.CategoryName

                                                   }).ToList();
            ViewBag.cv = categoryValues;
            Blog myBlog = new Blog();
            return View(myBlog);
        }
        [HttpPost]
        public IActionResult BlogAdd(Blog b, IFormFile file)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity.Name;
                using (var context = new Context())
                {
                    var writer = context.Writers.FirstOrDefault(x => x.WriterMail == userEmail);
                    if (writer != null)
                    {
                        b.WriterID = writer.WriterID; // WriterID'yi formdan çekicem
                    }
                }
            }

            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            var categoryValues = cm.GetList().Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.CategoryName
            }).ToList();

            ViewBag.cv = categoryValues;

            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                b.BlogImage = "/uploads/" + fileName;
            }

            BlogValidator bv = new BlogValidator();
            ValidationResult results = bv.Validate(b);

            if (results.IsValid)
            {

                b.BlogCreateDate = DateTime.Now;
                b.BlogStatus = true;

                bm.Add(b);
                return RedirectToAction("BlogListByWriter", "Blog");
            }
            else
            {
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(b);
            }
        }

        [HttpGet]
        public IActionResult BlogDelete(int id)
        {
            var toDelete = bm.GetbyId(id);
            bm.Delete(toDelete);
            return RedirectToAction("BlogListByWriter");
        }
        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Value = x.CategoryID.ToString(),
                                                       Text = x.CategoryName
                                                   }).ToList();
            ViewBag.cvFromEdit = categoryValues;
            var blogToEdit = bm.GetbyId(id);
            return View(blogToEdit);
        }
        [HttpPost]
        public IActionResult EditBlog(Blog b, IFormFile file)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity.Name;
                using (var context = new Context())
                {
                    var writer = context.Writers.FirstOrDefault(x => x.WriterMail == userEmail);
                    if (writer != null)
                    {
                        b.WriterID = writer.WriterID;
                    }
                }
            }

            // Kategori seçeneklerini yeniden sağla
            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            ViewBag.cvFromEdit = cm.GetList().Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.CategoryName
            }).ToList();

            BlogEditValidator bev = new BlogEditValidator();
            ValidationResult results = bev.Validate(b);

            if (results.IsValid)
            {
                // Dosya yüklemesi varsa, yeni dosyayı yükle
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    // Eski resmi sil (opsiyonel, eski resmi silmek istiyorsanız bu kısmı ekleyebilirsiniz)
                    var existingBlog = bm.GetbyId(b.BlogID);
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", Path.GetFileName(existingBlog.BlogImage));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    b.BlogImage = "/uploads/" + fileName;
                }
                else
                {
                    // Yeni dosya yüklenmemişse, mevcut resmi koru
                    var existingBlog = bm.GetbyId(b.BlogID);
                    b.BlogImage = existingBlog.BlogImage;
                }

                b.BlogCreateDate = DateTime.Now;
                b.BlogStatus = true;
                bm.Update(b);
                return RedirectToAction("BlogListByWriter");
            }
            else
            {
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(b);
            }
        }
    }
}
