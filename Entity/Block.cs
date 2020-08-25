using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Block
    {
        public Block()
        {

        }

        public int BlockId { get; set; }
        public int DistrictId { get; set; }
        public string BlockName { get; set; }
        public int StateId { get; set; }

        public int PotatoTypeId { get; set; }
        public string PotatoTypeName { get; set; }
    }
}
