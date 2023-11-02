using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB_GamingForm_Show;
using DB_GamingForm_Show.Job;
using Gaming_Forum;

namespace Groot
{
    public partial class FrmDepute : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        CMyInfo _CMyInfo = new CMyInfo();

        public FrmDepute()
        {
            InitializeComponent();
            Text = "委託管理系統";
            showMyInfo();
            loadComboboxRegion();
            
            loadSkillClasses();

            loadReceiveMembers();
            loadMyDepute();

            ConfirmResumes();

            LoadTalent();
        }

        private void loadComboboxRegion()
        {
            var q = from p in this.db.Regions
                    select p;
            foreach(var g in q)
            {
                this.comboBox1.Items.Add(g.City);
            }
        }

        private void showMyInfo()//ok
        {
            //會員編號
            this.textBox6.Text = CMyInfoDetial.int提供者編號.ToString();
            //會員名稱
            this.textBox3.Text = CMyInfoDetial.string提供者名稱;
            //聯絡方式
            this.textBox1.Text = CMyInfoDetial.string提供者手機;
            //電子信箱
            this.textBox8.Text = CMyInfoDetial.string提供者信箱;
        }

        private void ConfirmResumes()
        {
            var d = from p in this.db.Deputes.AsEnumerable()
                    where p.ProviderID == CMyInfoDetial.int提供者編號
                    select p;

            if (!d.Any())
            {
                MessageBox.Show("尚無委託紀錄");
            }

            var q = from p in this.db.DeputeRecords.AsEnumerable()
                    where p.MemberID == ClassUtility.MemberID && p.ApplyStatusID == 5
                    select p;
            if (q.Any())
            {
                MessageBox.Show($"有{d.Count()}位會員想接下您的懸賞");
            }
        }

        public string MaskString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // 取得第一個字符
            char firstChar = input[0];

            // 使用 LINQ 將其餘字符替換為 *
            string maskedString = new string(input.Skip(1).Select(_ => '*').ToArray());

            // 重新組合第一個字符和遮蔽後的字符
            return firstChar + maskedString;
        }

        private void LoadTalent()//todo bian
        {
            //DataGridViewButtonColumn f = new DataGridViewButtonColumn();
            //f.Name = "邀請";
            //f.HeaderText = "邀請面試";
            //f.DefaultCellStyle.NullValue = "邀請";


            //var q = from p in this.db.Blogs.AsEnumerable()
            //        select new
            //        {
            //            p.
            //        };
            //this.dataGridView3.DataSource = q.ToList();
            //this.dataGridView3.Columns.Add(f);
        }

        



        private void loadMyDepute()//ok
        {
            this.richTextBox1.Clear();

            var q = from p in this.db.Deputes.AsEnumerable()
                    where p.ProviderID== ClassUtility.MemberID
                    select new
                    {
                        委託編號=p.DeputeID,
                        //委託內容 = p.DeputeContent,
                        目前申請人數 =p.DeputeRecords.Where(x=>x.ID== ClassUtility.MemberID).Count(),
                        刊登時間=p.StartDate,
                        最後更新時間=p.Modifiedate,
                        目前狀態=p.Status.Name,
                        提供報酬 = p.Salary,
                    };
            this.dataGridView2.DataSource = q.ToList();

            this.listBox4.Items.Clear();

            //標題
            this.listBox4.Items.Add($"{"委託編號",-24}-{"狀態",-24}");//-{"應徵內容",-24}

            //以下為listbox內容
            foreach (var i in q)
            {
                this.listBox4.Items.Add($"{i.委託編號,-24}-{i.目前狀態,-24}");//-{"i.應徵內容",-24}
            }
        }

        

        private void loadReceiveMembers()//應徵者ok
        {   
            db = new DB_GamingFormEntities();

            var q = from p in this.db.DeputeRecords.AsEnumerable()
                    where p.Depute.Member.MemberID == ClassUtility.MemberID
                    select new
                    {
                        委託編號 = p.DeputeID,
                        應徵會員編號 = p.MemberID,
                        姓名 = p.Member.Name,
                        手機號碼 = p.Member.Phone,
                        狀態 = p.Status.Name,
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        private void loadSkillClasses()//ok
        {
            var q = from p in db.SkillClasses
                    select p;
            foreach (var i in q)
            {
                this.listBox1.Items.Add(i.Name);
            }
        }

        private void changeApplyStatusID(int s)//ok
        {
            //狀態更動
            var x = (from p in this.db.DeputeRecords.AsEnumerable()
                     where p.DeputeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                     && p.MemberID==int.Parse(this.dataGridView1.CurrentRow.Cells[1].Value.ToString())
                     select p).FirstOrDefault();

            x.ApplyStatusID = s;

            this.db.SaveChanges();
            loadReceiveMembers();
        }

        //============================================================================
        

        private void button3_Click(object sender, EventArgs e)//ok
        {
            //待定
            changeApplyStatusID(7);
        }

        private void button5_Click(object sender, EventArgs e)//ok
        {
            //拒絕
            changeApplyStatusID(8);
        }

        private void button6_Click(object sender, EventArgs e)//ok
        {
            //面試邀請
            changeApplyStatusID(9);
        }

        private void button10_Click(object sender, EventArgs e)//ok
        {
            //錄取
            changeApplyStatusID(10);
        }

        private void button1_Click(object sender, EventArgs e)//ok
        {
            //狀態開啟關閉
            var q = (from p in this.db.Deputes.AsEnumerable()
                    where p.DeputeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                    select p).FirstOrDefault();

            if (q == null) return;

            if (q.StatusID == 18)
            {
                q.StatusID = 19;
            }
            else if(q.StatusID == 19)
            {
                q.StatusID = 18;
            }

            q.Modifiedate = DateTime.Now;

            this.db.SaveChanges();
            loadMyDepute();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)//待刪
        {
            var q = (from p in this.db.DeputeRecords.AsEnumerable()
                    where p.DeputeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString()) 
                    && p.ApplyStatusID == 5
                    select p).FirstOrDefault();
            if (q == null) return;
            changeApplyStatusID(6);
        }

        private void button12_Click(object sender, EventArgs e)//ok
        {
            int deID = int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString());

            //=============================================
            //DeputeSkills
            var qqq = from p in this.db.DeputeSkills.AsEnumerable()
                       where p.DeputeID == deID
                       select p;
            foreach (var i in qqq)
            {
                this.db.DeputeSkills.Remove(i);
            }

            //=============================================
            //DeputeRecords
            var qq =from p in this.db.DeputeRecords.AsEnumerable()
                      where p.DeputeID == deID
                      select p;
            foreach (var i in qq)
            {
                this.db.DeputeRecords.Remove(i);
            }

            this.db.SaveChanges();
            //=============================================
            //Deputes(PK)
            var q = (from p in this.db.Deputes.AsEnumerable()
                    where p.DeputeID == deID
                     select p).FirstOrDefault();

            this.db.Deputes.Remove(q);
            this.db.SaveChanges();

            loadMyDepute();
        }
               

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)//todo bian
        {
            //if (dataGridView3.Columns[e.ColumnIndex].Name == "邀請" && e.RowIndex >= 0)
            //{
            //    if (this.listBox4.SelectedItems != null)
            //    {
            //        string selectJobID;
            //        selectJobID = "10";

            //        var q = (from p in this.db.JobResumes.AsEnumerable()
            //                 where p.JobID == int.Parse(selectJobID)
            //                 && p.ResumeID == int.Parse(this.dataGridView3.CurrentRow.Cells["履歷編號"].Value.ToString())
            //                 select p).FirstOrDefault();

            //        if (q != null)
            //        {
            //            MessageBox.Show("該會員已在您的應徵清單中");
            //            return;
            //        };

            //        if (selectJobID != null)
            //        {
            //            JobResume jr = new JobResume
            //            {
            //                JobID = int.Parse(selectJobID),
            //                ResumeID = int.Parse(this.dataGridView3.CurrentRow.Cells[1].Value.ToString()),
            //                ApplyStatusID = 9,
            //            };
            //            this.db.JobResumes.Add(jr);
            //        }

            //        MessageBox.Show("邀請成功");
            //        this.db.SaveChanges();
            //        LoadReceiveMembers();
            //    }
            //}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex >= 0)
            {
                this.listBox2.Items.Clear();
                //===========================
                var id = from p in this.db.SkillClasses
                         select p;

                var q = from p in db.Skills.AsEnumerable()
                        where p.SkillClassID == id.ToList()[this.listBox1.SelectedIndex].SkillClassID
                        select p;

                foreach(var item in q)
                {
                    this.listBox2.Items.Add(item.Name);
                }
            }
            else { }
        }

        ListBox lb = new ListBox();
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {

            //===============================
            //技能選項listbox
            this.listBox3.Items.Clear();

            var x = from p in this.db.Skills
                    where p.Name == this.listBox2.Text
                    select p;

            foreach (var g in x)
            {
                this.lb.Items.Add($"{g.SkillClass.Name}-{g.Name}");
            }

            foreach (var j in lb.Items)
            {
                this.listBox3.Items.Add(j);
            }

            List<string> skics = new List<string>();

            var a = from p in this.db.SkillClasses
                    select p;
            foreach (var g in a)
            {
                skics.Add(g.Name);
            }
            var b = from p in this.db.Skills
                    select p;

            //=================================
            this.listBox2.Items.Remove(this.listBox2.SelectedItem);

            //=====================================
            this.richTextBox3.Text = "具備以下技能為佳:\r";
            for (int i = 0; i < this.listBox3.Items.Count; i++)
            {
                this.richTextBox3.Text += $"{i + 1}.{this.listBox3.Items[i]}\r";
            }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            FrmDeputeA f =new FrmDeputeA();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //儲存委託
            //=========================
            //判斷有無填寫報酬欄位
            if (this.textBox4 == null)
            {
                DialogResult result = MessageBox.Show("未填寫提供報酬，請問後續再與會員討論？", "儲存檔案", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel) return;
                else this.textBox4.Text = "0";
            }
            //=========================
            //基本資料
            var q = from p in this.db.Regions
                    select p;
            
            Depute f = new Depute
            {
                ProviderID = ClassUtility.MemberID,
                StartDate = DateTime.Now,
                Modifiedate = DateTime.Now,
                DeputeContent = this.richTextBox3.Text,
                Salary = int.Parse(this.textBox4.Text),
                StatusID = 18,
                RegionID = q.ToList()[this.comboBox1.SelectedIndex].RegionID
            };

            this.db.Deputes.Add(f);
            this.db.SaveChanges();

            //=========================
            //技能專長
            int lb3Length = this.listBox3.Items.Count;
            string[] lb3items = new string[lb3Length];

            for (var l = 0; l < lb3Length; l++)
            {
                lb3items[l] = this.listBox3.Items[l].ToString();
            }

            for (var o = 0; o < lb3items.Length; o++)
            {
                string[] skillskill = lb3items[o].Split('-');
                var s = this.db.Skills.AsEnumerable().Where(p => p.Name == skillskill[1]).Select(p => p.SkillID);

                int skillid = s.SingleOrDefault();
                DeputeSkill jobSkill = new DeputeSkill
                {
                    DeputeID = f.DeputeID,
                    SkillID = skillid,
                };
                this.db.DeputeSkills.Add(jobSkill);
            }
            this.db.SaveChanges();
            //=========================
            MessageBox.Show("新增成功");
            this.tabControl2.SelectedIndex = 0;
            loadReceiveMembers();
            loadMyDepute();

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.button4.Enabled = true;
            }
            else
            {
                this.button4.Enabled = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.textBox4.Clear();
            this.listBox3.Items.Clear();
            this.richTextBox3.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FrmDeputeB f = new FrmDeputeB();
            f.Show();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var q = (from p in this.db.Deputes.AsEnumerable()
                    where p.DeputeID == Convert.ToInt32(this.dataGridView2.CurrentRow.Cells[0].Value)
                    select p).FirstOrDefault();
            this.richTextBox1.Text = q.DeputeContent;
        }
    }
}
