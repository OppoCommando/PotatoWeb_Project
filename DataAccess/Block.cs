using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class Block
    {
        public Block()
        {
        }

        public static void Save(Entity.Block Block)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pBlockId", SqlDbType.Int, ParameterDirection.Input, Block.BlockId);
                oDm.Add("@pDistrictId", SqlDbType.Int, ParameterDirection.Input, Block.DistrictId);
                oDm.Add("@pBlockName", SqlDbType.VarChar, 50, ParameterDirection.Input, Block.BlockName);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_Block_Save");
            }
        }

        public static void SaveBlockWithState(Entity.Block Block)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pBlockWithStateId", SqlDbType.Int, ParameterDirection.Input, Block.BlockId);
                oDm.Add("@pStateId", SqlDbType.Int, ParameterDirection.Input, Block.StateId);
                oDm.Add("@pBlockName", SqlDbType.VarChar, 50, ParameterDirection.Input, Block.BlockName);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_BlockWithState_Save");
            }
        }

        public static void UpSave(Entity.Block Block)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pBlockId", SqlDbType.Int, ParameterDirection.Input, Block.BlockId);
                oDm.Add("@pDistrictId", SqlDbType.Int, ParameterDirection.Input, Block.DistrictId);
                oDm.Add("@pBlockName", SqlDbType.VarChar, 50, ParameterDirection.Input, Block.BlockName);
                oDm.Add("@pPotatoTypeId", SqlDbType.Int, ParameterDirection.Input, Block.PotatoTypeId);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_UpBlock_Save");
            }

        }
        public static DataTable UPBlockGetAll(string statename)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pStatename", SqlDbType.VarChar, 50, ParameterDirection.Input, statename);
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_UPBlock_GetAll");
            }

        }


        public static DataTable GetAll(string Statename)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pStatename", SqlDbType.VarChar, 50, ParameterDirection.Input, Statename);
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_Block_GetAll");
            }
        }

        public static DataTable GetAllBlockWithState()
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_BlockWithState_GetAll");
            }
        }

         public static void Delete(int BlockId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pBlockId", SqlDbType.Int, ParameterDirection.Input, BlockId);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_Block_Delete");
            }
        }

        public static void DeleteFromBlockWithState(int BlockId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pBlockWithStateId", SqlDbType.Int, ParameterDirection.Input, BlockId);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_BlockWithState_Delete");
            }
        }

        public static void Exchange(int BlockId1, int BlockId2)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pBlockId1", SqlDbType.Int, ParameterDirection.Input, BlockId1);
                oDm.Add("@pBlockId2", SqlDbType.Int, ParameterDirection.Input, BlockId2);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_Block_Exchange");
            }
        }
        public static void Exchange_BlockWithState(int BlockId1, int BlockId2)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pBlockId1", SqlDbType.Int, ParameterDirection.Input, BlockId1);
                oDm.Add("@pBlockId2", SqlDbType.Int, ParameterDirection.Input, BlockId2);
                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("usp_BlockWithState_Exchange");
            }
        }
    }
}
