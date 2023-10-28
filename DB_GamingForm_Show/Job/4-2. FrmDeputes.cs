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

namespace Groot
{
    public partial class FrmMakeJobRequire : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        string currentID;

        public FrmMakeJobRequire()
        {
            InitializeComponent();

            Text = "廠商";
            
            LoadID();
            LoadInfo();
            
            LoadSkillClasses();

            LoadReceiveMembers();
            LoadMyDepute();

            ConfirmResumes();

            LoadTalent();
        }

        private void ConfirmResumes()
        {
            var q = from p in this.db.DeputeRecords.AsEnumerable()
                    where p.MemberID == int.Parse(currentID) && p.ApplyStatusID == 5
                    select p;
            if (q.Any())
            {
                MessageBox.Show($"您有{q.Count()}份新履歷");
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


            //var q= from p in this.db.Deputes.AsEnumerable()
            //       where p.StatusID==1
            //       select new
            //       {
            //           履歷編號=p.DeputeID,
            //           會員編號=p.ProviderID,
            //           會員姓名=MaskString(p.n),
            //           身份證字號= MaskString(p.IdentityID),
            //           電話號碼= MaskString(p.PhoneNumber),
            //           擁有技能=p.ResumeSkills.Select(_=>_.Skill.Name).FirstOrDefault()+"等"+ p.ResumeSkills.Select(_ => _.Skill.Name).Count() + "項技能",
            //           履歷內容=p.ResumeContent,
            //           工作經驗=p.WorkExp+"年",
            //           學歷=p.Education.Name,
            //           狀態=p.Status.Name
            //       };
            //this.dataGridView3.DataSource = q.ToList();
            //this.dataGridView3.Columns.Add(f);
        }

        

        private void LoadID()
        {
            currentID = Gaming_Forum.ClassUtility.FirmID.ToString();
            
        }

        private void LoadMyDepute()//ok
        {
            var q = from p in this.db.Deputes.AsEnumerable()
                    where p.Member.MemberID== int.Parse(currentID)
                    select new
                    {
                        委託編號=p.DeputeID,
                        委託內容=p.DeputeContent,
                        刊登時間=p.StartDate,
                        更新時間=p.Modifiedate,
                        目前狀態=p.StatusID
                        //公司編號=p.FirmID,
                        //公司名稱=p.Firm.FirmName,
                        //工作編號=p.JobID,
                        //應徵內容=p.JobContent,
                        //需求人數= p.RequiredNum+"人",
                        //薪水=p.Salary,
                        //狀態=p.Status.Name,
                        //工作經驗=p.JobExp,
                        //更新時間=p.ModifiedDate
                    };
            this.dataGridView2.DataSource = q.ToList();

            //標題
            this.listBox4.Items.Add($"{"工作編號",-15}-{"應徵內容",-15}-{"狀態",-15}");

            //以下為listbox內容
            foreach(var i in q)
            {
                this.listBox4.Items.Add($"{i.委託編號,-15}-{i.委託內容,-15}-{i.目前狀態,-15}");
            }
        }

        private void LoadInfo()//ok
        {
            var q = from p in this.db.Members.AsEnumerable()
                    where p.MemberID == int.Parse(currentID)
                    select p;

            if (q.Any(n => n.MemberID == int.Parse(currentID)))
            {
                //會員編號
                this.textBox6.Text = currentID;
                //會員名稱
                this.textBox3.Text = q.FirstOrDefault().Name;
                //聯絡方式
                this.textBox1.Text = q.FirstOrDefault().Phone;
                //電子信箱
                this.textBox8.Text = q.FirstOrDefault().Email;
            }
            else
            {
                MessageBox.Show("看到趕快來加東西");
            }
        }

        private void LoadReceiveMembers()//應徵者ok
        {   
            db = new DB_GamingFormEntities();

            var q = from p in this.db.DeputeRecords.AsEnumerable()
                    where p.Depute.Member.MemberID == int.Parse(currentID)
                    select new
                    {
                        委託編號 = p.DeputeID,
                        應徵會員編號 = p.MemberID,
                        姓名 = p.Member.Name,
                        //身份證字號 = p.Resume.IdentityID,
                        手機號碼 = p.Member.Phone,
                        //工作經驗 = p.Resume.WorkExp,
                        狀態=p.Status.Name,
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        private void LoadSkillClasses()//ok
        {
            var q = from p in db.SkillClasses
                    select p;
            foreach (var i in q)
            {
                this.listBox1.Items.Add(i.Name);
            }
        }

        private void ChangeApplyStatusID(int s)//ok
        {
            //狀態更動
            var x = (from p in this.db.DeputeRecords.AsEnumerable()
                     where p.DeputeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                     && p.MemberID==int.Parse(this.dataGridView1.CurrentRow.Cells[1].Value.ToString())
                     select p).FirstOrDefault();

            x.ApplyStatusID = s;

            this.db.SaveChanges();
            LoadReceiveMembers();
        }

        //============================================================================


        private void button4_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex += 1;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex -= 1;
        }

        ListBox llb = new ListBox();

        ListBox lb = new ListBox();

        private void button8_Click(object sender, EventArgs e)//ok
        {
            //=========================
            //基本資料
            Depute f = new Depute
            {
                ProviderID = int.Parse(currentID),
                StartDate = DateTime.Now,
                Modifiedate = DateTime.Now,
                DeputeContent = this.richTextBox3.Text,
                //Salary = this.textBox4.Text,
                StatusID = 3,
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
            this.tabControl2.SelectedIndex = 2;
            LoadReceiveMembers();
            LoadMyDepute();
        }
        
        private void checkBox1_Click(object sender, EventArgs e)
        {//todo改r
            if (this.checkBox1.Checked)
            {
                this.button8.Enabled = true;
            }
            else
            {
                this.button8.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)//ok
        {
            //待定
            ChangeApplyStatusID(7);
        }

        private void button5_Click(object sender, EventArgs e)//ok
        {
            //拒絕
            ChangeApplyStatusID(8);
        }

        private void button6_Click(object sender, EventArgs e)//ok
        {
            //面試邀請
            ChangeApplyStatusID(9);
        }

        private void button10_Click(object sender, EventArgs e)//ok
        {
            //錄取
            ChangeApplyStatusID(10);
        }

        private void button1_Click(object sender, EventArgs e)//ok
        {
            //狀態開啟關閉
            var q = (from p in this.db.Deputes.AsEnumerable()
                    where p.DeputeID == int.Parse(this.dataGridView2.CurrentRow.Cells[0].Value.ToString())
                    select p).FirstOrDefault();

            if (q == null) return;

            if (q.StatusID == 3)
            {
                q.StatusID = 4;
            }
            else if(q.StatusID == 4)
            {
                q.StatusID = 3;
            }
            
            q.Modifiedate= DateTime.Now;

            this.db.SaveChanges();
            LoadMyDepute();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)//待刪
        {
            var q = (from p in this.db.DeputeRecords.AsEnumerable()
                    where p.DeputeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString()) 
                    && p.ApplyStatusID == 5
                    select p).FirstOrDefault();
            if (q == null) return;
            ChangeApplyStatusID(6);
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

            LoadMyDepute();
        }

        private void button13_Click(object sender, EventArgs e)//ok
        {
            var q =from p in this.db.DeputeRecords.AsEnumerable()
                   where p.DeputeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString()) 
                   &&p.MemberID== int.Parse(this.dataGridView1.CurrentRow.Cells[1].Value.ToString())
                   select p;

            foreach(var r in q)
            {
                this.db.DeputeRecords.Remove(r);
            }
            this.db.SaveChanges();
            LoadReceiveMembers();
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
                this.llb.Items.Clear();
                this.listBox2.Items.Clear();
                //===========================
                var id = from p in this.db.SkillClasses
                         select p;

                var q = from p in db.Skills.AsEnumerable()
                        where p.SkillClassID == id.ToList()[this.listBox1.SelectedIndex].SkillClassID
                        select p;

                foreach (var item in q)
                {
                    this.llb.Items.Add(item.Name);
                }
                foreach (var item in llb.Items)
                {
                    this.listBox2.Items.Add(item);
                }
            }
            else { }
        }

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
        }
    }
}
