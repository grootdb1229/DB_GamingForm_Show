﻿using Shopping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using WindowsFormsApp1;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Image = System.Drawing.Image;
using DB_GamingForm_Show;
using Gaming_Forum;
using System.Web.UI.WebControls;


namespace 其中專題
{
    public partial class AddProduct : Form
    {
        public int ID { get; set; }

        private void MemberFirm()
        {
            ID = ClassUtility.MemberID;
        }
        public AddProduct()
        {
            InitializeComponent();
            MemberFirm();

        }
        CAddProduct ClassAddProduct = new CAddProduct();
        private void Form1_Load(object sender, EventArgs e)
        {
            DB_GamingFormEntities db = new DB_GamingFormEntities();
            //資料讀入各種控制項

            var Tags = from p in db.Tags
                       where p.TagID != 4
                       select new { p.Name, p.TagID };

            comboBox1.DataSource = Tags.ToList();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "TagID";
            comboBox1.SelectedIndex = 0;


            var SubTags = from p in db.SubTags
                          where p.TagID == 1
                          select new { p.Name };
            comboBox2.DataSource = SubTags.ToList();
            comboBox2.DisplayMember = "Name";



        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearEveryContorl();
        }

        private void ClearEveryContorl()
        {
            pictureBox1.Image = null;
            textBox1.Clear();
            ProductNameTextBox.Clear();
            UnitStockTextBox.Clear();
            PriceTextBox.Clear();
            listBox2.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassAddProduct.GetPrice_TextParse(this.PriceTextBox.Text);
            ClassAddProduct.GetUnit_Stock_TextParse(this.UnitStockTextBox.Text);
            ClassAddProduct.ProductName = this.ProductNameTextBox.Text;                      
            if (pictureBox1.Image != null)
            { CAddProduct.flag2 = true;
                ClassAddProduct._image = pictureBox1.Image;
            }
            else { MessageBox.Show("請選擇一張圖片"); }
            
            ClassAddProduct.listBox_Count=  this.listBox2.Items.Count;
            ClassAddProduct._listBox_Items= listBox2.Items;
            ClassAddProduct.Get_Textbox_TextCount(textBox1.Text);
            ClassAddProduct.Textbox_Text=textBox1.Text;

            bool aa= ClassAddProduct.The_MainMethod();

            if (aa == true)
            {
                ClearEveryContorl();
            } 

            //DB_GamingFormEntities db = new DB_GamingFormEntities();
            //using (TransactionScope ts = new TransactionScope())
            //{        
            //    byte[] bytes;
            //    ///當六面旗幟判定接成功時，才能開始儲存進資料庫。          
            //    try
            //    {
            //        ClassAddProduct.GetPrice_TextParse(this.PriceTextBox.Text);
            //        ClassAddProduct.GetUnit_Stock_TextParse(this.UnitStockTextBox.Text);
            //        ClassAddProduct.ProductName=this.ProductNameTextBox.Text;

            //if (pictureBox1.Image != null)
            //{ CAddProduct.flag2 = true; }
            //else { MessageBox.Show("請選擇一張圖片"); }

            //if (listBox2.Items.Count > 0)
            //        { CAddProduct.flag3 = true; }
            //        else { MessageBox.Show("請至少選擇一個標籤"); }

            //        if (textBox1.Text.Length > 10)
            //        { CAddProduct.flag4 = true; }
            //        else { MessageBox.Show("請輸入至少10個字元"); }

            //        if (CAddProduct.flag1 == true && CAddProduct.flag2 == true && CAddProduct.flag3 == true && CAddProduct.flag4 == true && CAddProduct.flag5 == true && CAddProduct.flag6 == true)
            //        {  //儲存圖片等等要檢查
            //            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //            this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //            bytes = ms.GetBuffer();
            //            var ImgStore = new DB_GamingForm_Show.Image { Image1 = bytes };
            //            db.Images.Add(ImgStore);
            //            db.SaveChanges();


            //            //抓圖片ID
            //            int newImageId = ImgStore.ImageID;
            //            //var picIDCheck = db.Images.AsEnumerable().Select(x => x.ImageID).Last();


            //            var product = new Product { ProductName = ClassAddProduct.ProductName, Price = ClassAddProduct.SetPrice(), AvailableDate = DateTime.Now, UnitStock = ClassAddProduct.SetStock(), StatusID = 1, MemberID = ID, ProductContent = textBox1.Text, ImageID = newImageId };
            //            db.Products.Add(product);
            //            db.SaveChanges();

            //            var LastProductId = db.Products.AsEnumerable().Select(x => x.ProductID).Last();

            //            //儲存商品上架with交易

            //            for (int i = 0; i < listBox2.Items.Count; i++)
            //            {
            //                var aa = (from x in db.SubTags.AsEnumerable()
            //                          where x.Name == listBox2.Items[i].ToString()
            //                          select x.SubTagID).ToList();

            //                var pTag = new ProductTag { ProductID = LastProductId, SubTagID = aa.First() };
            //                db.ProductTags.Add(pTag);
            //                db.SaveChanges();
            //            }
            //            //連續上架前必須將旗幟重新關閉，下次上架才能正常的判斷是否有遺漏未填的資訊。
            //            CAddProduct.flag1 = false;
            //            CAddProduct.flag2 = false;
            //            CAddProduct.flag3 = false;
            //            CAddProduct.flag4 = false;
            //            CAddProduct.flag5 = false;
            //            CAddProduct.flag6 = false;
            //            ts.Complete();
            //            MessageBox.Show("上架成功");
            //            ClearEveryContorl();
            //        }
            //    }
            //    catch (Exception ex) { MessageBox.Show(ex.Message); }
            //}
        }


        private void buttonSelectPic_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //這段程式碼將.SelectedValue轉換成字串後，偵查他的字元返還數字
            string SelectTag = comboBox1.SelectedValue.ToString();
            var aa = new string(SelectTag.Where(char.IsDigit).ToArray());
            int parthSelectItem = int.Parse(aa);



            DB_GamingFormEntities db = new DB_GamingFormEntities();
            var SubTags = from p in db.SubTags
                          where p.TagID == parthSelectItem
                          select new { p.Name, p.SubTagID };


            comboBox2.DataSource = SubTags.ToList();
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "SubTagID";




        }

        private void button3_Click(object sender, EventArgs e)
        {
            //我只能在list中放置一個同名標籤
            string SelectCoboBox2Iteam = comboBox2.Text;
            if (!listBox2.Items.Contains(SelectCoboBox2Iteam))
            {
                listBox2.Items.Add(comboBox2.Text);
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1) //你有選值
            {
                this.listBox2.Items.RemoveAt(listBox2.SelectedIndex);//移除雙擊的標籤
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UnitStockTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void UnitStockTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PriceTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

