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
    public partial class BlogAndArtical : Form
    {
        string blogtitle = ""; 
        string blogca = "";
        
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        public BlogAndArtical(string a )
        {
            InitializeComponent();
           

            var q = from n in db.Articles
                    where n.SubBlog.Blog.Title == a
                    select new { 主板名稱 = n.SubBlog.Blog.Title, 文章分類 = n.SubBlog.Title, 文章編號=n.ArticleID, 作者_個人頁 = n.Member.Name, 標題=n.Title,發表時間=n.ModifiedDate, 內文預覽 = n.ArticleContent };
           
            this.dataGridView1.DataSource = q.ToList();

            blogtitle = a;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            NewART x = new NewART(blogtitle,blogca);
            x.Show();



        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string headerText = dataGridView1.Columns[e.ColumnIndex].HeaderText;
                if (headerText == "作者_個人頁")
                {
                    // 在這裡打開 Memberhome 頁面                    


                    string MemberName = dataGridView1.Rows[e.RowIndex].Cells["作者_個人頁"].Value.ToString();
                    var z = (from n in db.Members
                             where n.Name == MemberName
                             select n.MemberID).FirstOrDefault();
                    if (z != null)
                    {
                        int mid = z;
                        ClassUtility.MemberID = mid;
                        Memberhome x = new Memberhome(mid);
                        x.Show();
                    }
                }
                else
                {
                    // 點擊到其他格的處理邏輯
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
}
