using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLayer
{
    public class Block
    {
        public Block()
        {
        }

        public void Save(Entity.Block Block)
        {
            DataAccess.Block.Save(Block);
        }

        public void SaveBlockWithState(Entity.Block Block)
        {
            DataAccess.Block.SaveBlockWithState(Block);
        }

        public DataTable GetAll(string Statename)
        {
            return DataAccess.Block.GetAll(Statename);
        }

        public DataTable GetAllBlockWithState()
        {
            return DataAccess.Block.GetAllBlockWithState();
        }

        public void Delete(int BlockId)
        {
            DataAccess.Block.Delete(BlockId);
        }

        public void DeleteFromBlockWithState(int BlockId)
        {
            DataAccess.Block.DeleteFromBlockWithState(BlockId);
        }


        public void Exchange(int BlockId1, int BlockId2)
        {
            DataAccess.Block.Exchange(BlockId1, BlockId2);
        }

        public void Exchange_BlockWithState(int BlockId1, int BlockId2)
        {
            DataAccess.Block.Exchange_BlockWithState(BlockId1, BlockId2);
        }
    }
}
