using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLayer
{
    public class PotatoRate
    {
        public PotatoRate()
        {
        }

        public void Save(string Date, DataTable dtRate)
        {
            DataAccess.PotatoRate.Save(Date, dtRate);
        }

        public DataTable GetRateByDate(string Date)
        {
            return DataAccess.PotatoRate.GetRateByDate(Date);
        }
       



        public DataSet GetAll(string StartDate, string EndDate)
        {
            return DataAccess.PotatoRate.GetAll(StartDate, EndDate);
        }

        public DataTable GetUpRateByDate(string date, int potatoTypeId)
        {
            return DataAccess.PotatoRate.GetUpRateByDate(date, potatoTypeId);
        }

        public void UPSave(string date, DataTable dt, int potatoTypeId)
        {
            DataAccess.PotatoRate.UPSave(date, dt, potatoTypeId);
        }
    }
}
