using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLayer
{
   public class District
    {
       public District()
       {
       }

       public void Save(Entity.District District)
       {
           DataAccess.District.Save(District);
       }

       public DataTable GetAll(string Statename)
       {
           return DataAccess.District.GetAll(Statename);
       }

       public void Delete(int DistrictId)
       {
           DataAccess.District.Delete(DistrictId);
       }
    }
}
