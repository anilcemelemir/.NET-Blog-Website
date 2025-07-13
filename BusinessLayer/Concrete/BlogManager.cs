using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class BlogManager : IBlogService
    {
        IBlogDal  _IBlogDal;

        public BlogManager(IBlogDal blogDal)
        {
            _IBlogDal = blogDal;
        }

        public Blog GetbyId(int id)
        {
            return _IBlogDal.GetByID(id);
        }

        public List<Blog> GetList()
        {
            return _IBlogDal.GetListAll();
        }
        public List<Blog> GetLast3BlogPost()
        {
            using (var context = new Context())
            {
                return context.Blogs
                              .OrderByDescending(b => b.BlogCreateDate)
                              .Take(3)
                              .ToList();
            }
        }

        public List<Blog> GetBlogListWithCategory()
        {
            return _IBlogDal.GetListWithCat();
        }

        public List<Blog> GetBlogListByWriter(int id)
        {
            return _IBlogDal.List(x => x.WriterID == id);
        }
        public List<Blog> GetListWithCategoryByWriterBm(int id)
        {
            return _IBlogDal.GetListWithCategoryByWriter(id);
        }

        public void Add(Blog t)
        {
            _IBlogDal.Insert(t);
        }

        public void Delete(Blog t)
        {
            _IBlogDal.Delete(t);
        }

        public void Update(Blog t)
        {
            _IBlogDal.Update(t);
        }

        
    }
}
