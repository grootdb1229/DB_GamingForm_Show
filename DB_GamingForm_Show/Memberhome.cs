using Gaming_Forum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace DB_GamingForm_Show
{
    public partial class Memberhome : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();




        public Memberhome(int M)
        {
            InitializeComponent();

            LoadMemberInfo(M);


        }

        private void LoadMemberInfo(int M)
        {

            var q = from n in db.Members
                    where n.MemberID == M
                    select n;
            label2.Text = q.ToList().FirstOrDefault().Name.ToString();
            label4.Text = q.ToList().FirstOrDefault().Birth.ToString();
            label6.Text = q.ToList().FirstOrDefault().Email.ToString();


            var a = from n in db.Articles
                    where n.MemberID == M
                    select new { 作者 = n.Member.Name, 版名 = n.SubBlog.Blog.Title, 文章分類 = n.SubBlog.Title, 文章編號 = n.ArticleID, 標題 = n.Title, 發表時間 = n.ModifiedDate, 內文預覽 = n.ArticleContent };

            this.dataGridView1.DataSource = a.ToList();

            var q2 = db.Members.AsEnumerable()
                  .Where(s => s.MemberID == M)
                  .Select(s => s.Image.Image1);
            byte[] bytes = q2.FirstOrDefault();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox1.Image = System.Drawing.Image.FromStream(ms);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                string selectedTitle = dataGridView1.Rows[e.RowIndex].Cells["標題"].Value.ToString();
                string selectedContent = dataGridView1.Rows[e.RowIndex].Cells["內文預覽"].Value.ToString();
                int articleID = (int)dataGridView1.Rows[e.RowIndex].Cells["文章編號"].Value;

                ClassUtility.aid = articleID;

                // 數據傳遞到下一個視窗
                Art_Reply artReplyForm = new Art_Reply(selectedTitle, selectedContent, articleID);
                artReplyForm.ShowDialog();




            }
        }
    }
}
