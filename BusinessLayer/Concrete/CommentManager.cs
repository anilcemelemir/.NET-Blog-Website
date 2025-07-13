using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CommentManager : ICommentService
    {
        ICommentDal _CommentDal;

        public CommentManager(ICommentDal commentDal)
        {
            _CommentDal = commentDal;
        }

        public void CommentAdd(Comment comment)
        {
            _CommentDal.Insert(comment);
        }
        public List<Comment> GetList(int id)
        {
            return _CommentDal.List(x => x.BlogID == id);
        }
    }
}
