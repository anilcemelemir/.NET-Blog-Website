using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StajBlogSitesi.ViewComponents.Writer
{
    public class EfMessageManagerRepository : GenericRepository<Message>, IMessageDal
    {
      
    }
}