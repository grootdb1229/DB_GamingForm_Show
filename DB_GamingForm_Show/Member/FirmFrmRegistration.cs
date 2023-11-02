using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using DB_GamingForm_Show;
using Image = DB_GamingForm_Show.Image;

namespace Gaming_Forum
{
    public partial class FirmFrmRegistration : Form
    {
          
        public FirmFrmRegistration()
        {
            InitializeComponent();
        }
        public class PasswordHash
        {
            public static string HashPassword(string password)
            {
                using (var sha256 = new SHA256Managed())
                {
                    var bytes = Encoding.UTF8.GetBytes(password);
                    var hash = sha256.ComputeHash(bytes);

                    return Convert.ToBase64String(hash);
                }
            }
        }
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        ClassUtility CS = new ClassUtility();
        private void button1_Click(object sender, EventArgs e)
        {
            Firm firm1 = new Firm();
            DB_GamingForm_Show.Image image = new Image();
            if (!(CS.FirmName&&CS.Password&&CS.Phone&&CS.Email&&CS.TaxID&&CS.FirmAddress))
            {
                MessageBox.Show("請重新檢查欄位資料");
            }
           
            else
            {   
                MemoryStream MS = new MemoryStream();
                this.pictureBox1.Image.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] ImgData = MS.GetBuffer();

                image = new Image { Name = "Image", Image1 = ImgData };
                this.db.Images.Add(image);
                this.db.SaveChanges();

                string password = PasswordHash.HashPassword(textBox8.Text);
                firm1 = new Firm { FirmName = textBox1.Text, TaxID = Int32.Parse(textBox2.Text), Contact = textBox3.Text, FirmAddress = textBox4.Text, FirmIntro = textBox5.Text, FirmScale = textBox6.Text, StatusID = 1, Email = textBox7.Text, Password = password , ImageID = image.ImageID };
                this.db.Firms.Add(firm1);
                try
                {
                    this.db.SaveChanges();
                    MessageBox.Show("註冊成功");
                }

                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            { 
               this.pictureBox1.Image = System.Drawing.Image.FromFile(openFileDialog1.FileName); 
            }
        }

        private void ControllerReset() 
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
           ControllerReset();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                this.label9.Text = "請輸入您的公司名稱";
                CS.FirmName = false;
            }
            else if (db.Firms.Any(Name => Name.FirmName == textBox1.Text))
            {
                this.label9.Text = "該名稱已有人註冊";
                this.textBox1.Text = "";
                CS.FirmName = false;
            }
            else
            {
                this.label9.Text = "該名稱可使用";
                CS.FirmName = true;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                this.label10.Text = "請輸入您的統編";
                CS.TaxID = false;
            }
            else if (textBox2.Text.Length < 8)
            {
                this.label10.Text = "您輸入的統編格式不對";
                this.textBox2.Text = "";
                CS.TaxID = false;
            }
            else 
            {
                this.label10.Text = "輸入正確";
                CS.TaxID = true;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            
            string ContactPhone = @"^\(0[0-9]\)[0-9]{3,4}-[0-9]{4}$"; //(0x)xxxx-xxxx
            if ( Regex.IsMatch(textBox3.Text, ContactPhone))
            {
                this.label11.Text = "格式正確";
                CS.Phone = true;
            }

            else if (db.Firms.Any(c => c.Contact == textBox3.Text))
            {
                this.label11.Text = "該電話號碼已有人註冊";
                CS.Phone = false;
            }

            else
            {
                label11.Text = "格式錯誤";
                CS.Phone = false;
                textBox3.Text = "";
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                label12.Text = "請輸入資料";
                CS.FirmAddress = false;
            }

            else
            {
                label12.Text = "輸入正確";
                CS.FirmAddress = true;
            }
        }
        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                label12.Text = "請輸入資料";
                CS.FirmScale = false;
            }
            else 
            {
                label12.Text = "輸入正確";
                CS.FirmScale = true;
            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            string ContactEmail = @"^[a-zA-Z0-9_.+\-!@#$%^&*]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (textBox7.Text == "")
            {
                label15.Text = "請輸入資料";
                CS.Email = false;
            }
            else if (db.Firms.Any(Firm => Firm.Email == textBox7.Text)) 
            {
                label15.Text = "該Email已被註冊";
            }
            else if (Regex.IsMatch(textBox7.Text, ContactEmail))
            {
                label15.Text = "格式正確";
                CS.Email = true;
            }
            else
            {
                label15.Text = "格式錯誤";
                CS.Email = false;
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            string Password1 = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$"; // 至少有一個數字 至少有一個小寫英文字母 至少有一個大寫英文字母 字串長度在 6 ~30 個字母之間

            if (textBox8.Text == "")
            {
                label16.Text = "請輸入密碼";
                CS.Password = false;
            }

            else if (Regex.IsMatch(textBox8.Text, Password1))
            {
                label16.Text = "格式正確";
                CS.Password = true;
            }

            else if (db.Firms.Any(firm => firm.Password == textBox8.Text))
            {
                label16.Text = "該密碼已有人使用";
                CS.Password = false;
            }

            else 
            {
                label16.Text = "格式錯誤";
                CS.Password = false;
            }
        }
    }
}

