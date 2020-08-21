using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
   public class PotatoRate
    {
       public PotatoRate()
       {
       }

       public static void Save(string Date, DataTable dtRate)
       {
           using (DataManager oDm = new DataManager())
           {
               oDm.Add("@pDate", SqlDbType.DateTime, Convert.ToDateTime(Date));
               string RateDetailsXML = string.Empty;
               if (dtRate != null && dtRate.Rows.Count > 0)
               {
                   using (DataSet ds = new DataSet())
                   {
                       ds.Tables.Add(dtRate);

                       RateDetailsXML = ds.GetXml();
                   }
               }

               oDm.Add("@pRateDetails", SqlDbType.Xml, RateDetailsXML);
               oDm.CommandType = CommandType.StoredProcedure;
               oDm.ExecuteNonQuery("usp_PotatoRate_Save");
           }
       }

       public static DataTable GetRateByDate(string Year)
       {
            DateTime d = Convert.ToDateTime(Year);
           using (DataManager oDm = new DataManager())
           {
               oDm.Add("@pDate", SqlDbType.DateTime, Convert.ToDateTime(Year));
               oDm.CommandType = CommandType.StoredProcedure;
               return oDm.ExecuteDataTable("usp_PotatoRate_GetAllByDate");
           }
       }

        


        public static DataSet GetAll(string StartDate, string EndDate)
       {
           using (DataManager oDm = new DataManager())
           {
               oDm.Add("@pStartDate", SqlDbType.DateTime, Convert.ToDateTime(StartDate));
               oDm.Add("@pEndDate", SqlDbType.DateTime, Convert.ToDateTime(EndDate));
               oDm.CommandType = CommandType.StoredProcedure;
               DataSet ds = new DataSet();
               return oDm.GetDataSet("usp_PotatoRate_GetAll", ref ds, "Table1");
           }
       }

    }
}
