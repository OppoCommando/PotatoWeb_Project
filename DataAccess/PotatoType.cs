using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PotatoType
    {
        public PotatoType()
        {
        }

        public static DataTable GetAll(string statename)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pStatename", SqlDbType.VarChar, 50, ParameterDirection.Input, statename);
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_PotatoType_GetAllStateWise");
            }

        }
    }
}
