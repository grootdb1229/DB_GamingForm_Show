using Gaming_Forum;
using Groot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_GamingForm_Show.Job
{
    public partial class FrmDeputeA : Form
    {
        public FrmDeputeA()
        {
            InitializeComponent();
        }
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        private void button2_Click(object sender, EventArgs e)
        {
            FrmDeputeA f= new FrmDeputeA();
            f.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Article a= new Article()
            {
                SubBlogID=33,
                Title="私訊內容",
                ArticleContent=richTextBox1.Text,
                ModifiedDate=DateTime.Now,
                MemberID=ClassUtility.MemberID,//發送者
                ReplyArticleID=CMyInfo.selectedMemberid,//接收者
            };
            this.richTextBox2.Text = a.ArticleContent;
            this.richTextBox1.Clear();
            this.db.Articles.Add(a);
            this.db.SaveChanges();
        }

        private void FrmDeputeA_FormClosed(object sender, FormClosedEventArgs e)
        {

            var q=from p in this.db.Articles
                  where p.MemberID== ClassUtility.MemberID&&p.ReplyArticleID== CMyInfo.selectedMemberid
                  select p;
            if (q.Any())
            {
                foreach (var g in q)
                {
                    this.db.Articles.Remove(g);
                }
                this.db.SaveChanges();
            }
        }
    }
}
