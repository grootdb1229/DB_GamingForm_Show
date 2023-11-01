using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.product
{
    public class Purchase
    {
       
        public class Result
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public byte[] Image1 { get; set; }

            public decimal Price { get; set; }

            

        }
    }
}
