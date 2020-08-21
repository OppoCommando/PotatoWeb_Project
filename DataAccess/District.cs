using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class District
    {
        public District()
        {
        }

        public static void Save(Entity.District District)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pDistrictId", SqlDbType.Int, ParameterDirection.Input, District.DistrictId);
                oDm.Add("@pDistrictName", SqlDbType.VarChar,50, ParameterDirection.Input, District.DistrictName);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_District_Save");
            }
        }

        public static DataTable GetAll(string Statename)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pStatename", SqlDbType.VarChar, 50, ParameterDirection.Input, Statename);
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_District_GetAll");
            }
        }

        public static void Delete(int DistrictId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pDistrictId", SqlDbType.Int, ParameterDirection.Input, DistrictId);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_District_Delete");
            }
        }
    }
}
