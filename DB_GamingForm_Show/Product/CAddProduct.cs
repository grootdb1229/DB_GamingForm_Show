using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DB_GamingForm_Show
{
    public class CAddProduct
    {
        public decimal _Price_Value;

        public static bool flag1 { get; set; }
        public static bool flag2 { get; set; }
        public static bool flag3 { get; set; }
        public static bool flag4 { get; set; }
        public static bool flag5 { get; set; }
        public static bool flag6 { get; set; }

        public void Price_TextParse (string P)
        {
            if (decimal.TryParse(P, out _Price_Value)) { }
            else { flag5 = false; MessageBox.Show("請確認輸入金額為正確的阿拉伯數字"); }
            if (_Price_Value > 0)
            { flag5 = true; }
            else { MessageBox.Show("請檢察輸入金額"); }
        }

  
    }
}
