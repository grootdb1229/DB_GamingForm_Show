using Gaming_Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB_GamingForm_Show
{
    public class CAddProduct
    {
        //public int ID { get; set; }

        //private void MemberFirm()
        //{
        //    ID = ClassUtility.MemberID;
        //}

        private decimal _Price_Value;
        private int _Stock_Value;
        private string _ProductName;
        public System.Drawing.Image _image;
        private int _listBox_Count;
        private int _Textbox_Text;


        public string Textbox_Text;
        public System.Windows.Forms.ListBox.ObjectCollection _listBox_Items;
        public static bool flag1 { get; set; }
        public static bool flag2 { get; set; }
        public static bool flag3 { get; set; }
        public static bool flag4 { get; set; }
        public static bool flag5 { get; set; }
        public static bool flag6 { get; set; }

        public int listBox_Count
        {
            get
            {
                if (_listBox_Count > 0)
                { return _listBox_Count; }
                else
                {
                    MessageBox.Show("請至少選擇一個標籤");
                    return 0;
                }
            }
            set
            {
                _listBox_Count = value;
                if (listBox_Count > 0) { flag3 = true; }

            }
        }
        public bool The_MainMethod()
        {

            DB_GamingFormEntities db = new DB_GamingFormEntities();
            using (TransactionScope ts = new TransactionScope())
            {
                byte[] bytes;
                ///當六面旗幟判定接成功時，才能開始儲存進資料庫。          
                try
                {
                    if (CAddProduct.flag1 == true && CAddProduct.flag2 == true && CAddProduct.flag3 == true && CAddProduct.flag4 == true && CAddProduct.flag5 == true && CAddProduct.flag6 == true)
                    {  //儲存圖片等等要檢查
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        this._image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bytes = ms.GetBuffer();
                        var ImgStore = new DB_GamingForm_Show.Image { Image1 = bytes };
                        db.Images.Add(ImgStore);
                        db.SaveChanges();


                        //抓圖片ID
                        int newImageId = ImgStore.ImageID;
                        //var picIDCheck = db.Images.AsEnumerable().Select(x => x.ImageID).Last();


                        var product = new Product { ProductName = ProductName, Price = SetPrice(), AvailableDate = DateTime.Now, UnitStock = SetStock(), StatusID = 1, MemberID = ClassUtility.MemberID, ProductContent = Textbox_Text, ImageID = newImageId };
                        db.Products.Add(product);
                        db.SaveChanges();

                        var LastProductId = db.Products.AsEnumerable().Select(x => x.ProductID).Last();

                        //儲存商品上架with交易

                        for (int i = 0; i < _listBox_Count; i++)
                        {
                            var aa = (from x in db.SubTags.AsEnumerable()
                                      where x.Name == _listBox_Items[i].ToString()
                                      select x.SubTagID).ToList();

                            var pTag = new ProductTag { ProductID = LastProductId, SubTagID = aa.First() };
                            db.ProductTags.Add(pTag);
                            db.SaveChanges();
                        }

                        //連續上架前必須將旗幟重新關閉，下次上架才能正常的判斷是否有遺漏未填的資訊。
                        flag1 = false;
                        flag2 = false;
                        flag3 = false;
                        flag4 = false;
                        flag5 = false;
                        flag6 = false;
                        ts.Complete();
                        MessageBox.Show("上架成功");
                        return true;
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                return false;
            }
        }
        //public System.Drawing.Image image
        //{
        //    get
        //    {
        //        if (image != null)
        //        {
        //            return _image;
        //        }
        //        else
        //        {
        //            MessageBox.Show("請選擇一張圖片");
        //            return null;
        //        }
        //    }
        //    set
        //    {
        //        _image = value;
        //        flag2 = (image != null);
        //    }
        //}

        public string ProductName///這裡有bug 要改掉
        {
            get
            {
                if (!string.IsNullOrEmpty(_ProductName))
                {
                    return _ProductName;
                }
                else
                {
                    MessageBox.Show("請輸入產品名稱");
                    return string.Empty;
                }
            }
            set
            {
                if (value != "")
                {
                    _ProductName = value;
                    flag1 = !string.IsNullOrEmpty(value);
                }
                else { MessageBox.Show("請輸入產品名稱"); }
            }
        }

        public void GetPrice_TextParse(string P)
        {
            if (decimal.TryParse(P, out _Price_Value)) { }
            else { MessageBox.Show("請為產品金額輸入正確的阿拉伯數字"); return; }

            if (_Price_Value > 0)
            { flag5 = true; }
            else { MessageBox.Show("請檢查輸入的產品金額"); }
        }
        public void Get_Textbox_TextCount(string P)
        {
            if (P.Length > 10) { flag4 = true; }
            else { MessageBox.Show("請至少為遊戲添加10個字的描述"); }
        }

        public decimal SetPrice()
        {
            return _Price_Value;
        }

        public void GetUnit_Stock_TextParse(string P)
        {
            if (int.TryParse(P, out _Stock_Value)) { }
            else { MessageBox.Show("請為庫存輸入正確的阿拉伯數字"); return; }

            if (_Stock_Value > 0)
            { flag6 = true; }
            else
            {
                MessageBox.Show("請確認輸入了正確的庫存數量");
            }
        }
        public int SetStock()
        {
            return _Stock_Value;
        }
    }
}
