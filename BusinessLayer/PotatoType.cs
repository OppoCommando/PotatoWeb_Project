using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public class PotatoType
    {
        public PotatoType()
        {
        }

        public DataTable GetAll(string statename)
        {
            return DataAccess.PotatoType.GetAll(statename);
        }
    }
}
