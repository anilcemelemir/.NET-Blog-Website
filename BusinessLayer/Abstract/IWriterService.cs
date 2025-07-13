using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    interface IWriterService : IGenericService<Writer>
    {
        List<Writer> GetWriterById(int id);
        public List<Writer> GetWriterByName(string name);
        Writer GetWriterByEmail(string Email);
    }
}
