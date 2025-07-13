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
    public class WriterManager : IWriterService
    {
        IWriterDal _writerDal;

        public WriterManager(IWriterDal writerDal)
        {
            _writerDal = writerDal;
        }

        public void Add(Writer t)
        {
            _writerDal.Insert(t);
        }

        public void Delete(Writer t)
        {
            throw new NotImplementedException();
        }

        public Writer GetbyId(int id)
        {
            return _writerDal.GetByID(id);
        }

        public List<Writer> GetList()
        {
            throw new NotImplementedException();
        }

        public Writer GetWriterByEmail(string Email)
        {
            return _writerDal.List(x => x.WriterMail == Email).FirstOrDefault();
        }

        public List<Writer> GetWriterById(int id)
        {
            return _writerDal.List(x => x.WriterID == id);
        }

        public List<Writer> GetWriterByName(string name)
        {
            return _writerDal.List(x => x.WriterName.Contains(name));
        }


        public void Update(Writer t)
        {
            _writerDal.Update(t);
        }


    }
}
