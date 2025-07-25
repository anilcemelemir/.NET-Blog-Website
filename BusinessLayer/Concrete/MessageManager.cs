﻿using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public void Add(Message t)
        {
            _messageDal.Insert(t);
        }

        public void Delete(Message t)
        {
            throw new NotImplementedException();
        }

        public Message GetbyId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetList()
        {
            throw new NotImplementedException();
        }

        public List<Message> GetInboxListWithWriter(string p)
        {
            return _messageDal.List(x => x.Receiver == p);
        }

        public void Update(Message t)
        {
            throw new NotImplementedException();
        }
    }
}
