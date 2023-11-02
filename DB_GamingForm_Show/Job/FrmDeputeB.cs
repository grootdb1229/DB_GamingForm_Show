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

namespace DB_GamingForm_Show.Job
{
    public partial class FrmDeputeB : Form
    {
        public FrmDeputeB()
        {
            InitializeComponent();
        }
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        private void button2_Click(object sender, EventArgs e)
        {
            FrmDeputeB f = new FrmDeputeB();
            f.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Article a = new Article()
            {
                SubBlogID = 33,
                Title = "私訊內容",
                ArticleContent = richTextBox1.Text,
                ModifiedDate = DateTime.Now,
                MemberID = ClassUtility.MemberID,//發送者
            };
            //this.richTextBox1.Text = a.ArticleContent;
            this.richTextBox2.Clear();
            this.db.Articles.Add(a);
            this.db.SaveChanges();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string s = "";
            var q = from p in this.db.Articles.AsEnumerable()
                    where p.MemberID == ClassUtility.MemberID && p.ReplyArticleID == 38
                    select p;
            foreach( var item in q)
            {
                s += $"{item.ArticleContent}+\r";
            }
            this.richTextBox1.Text = s;
        }

        private void FrmDeputeB_FormClosed(object sender, FormClosedEventArgs e)
        {
            var q = from p in this.db.Articles
                    where p.MemberID == ClassUtility.MemberID && p.ReplyArticleID == CMyInfo.selectedMemberid
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

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
