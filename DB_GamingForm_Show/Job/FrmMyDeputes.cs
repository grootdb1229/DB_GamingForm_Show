using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB_GamingForm_Show;
using DB_GamingForm_Show.Job;
using DB_GamingForm_Show.Job.DeputeClass;
using DB_GamingForm_Show.Job.DeputeClass.View;
using Gaming_Forum;

namespace Groot
{
    public partial class FrmDepute : Form
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();
        

        public FrmDepute()
        {
            InitializeComponent();
            Text = "委託管理系統";
            loadController();
            showMyInfo();

            showReceiveMembers();
            showMyDepute();

            ConfirmResumes();
            loadSkillsBox();
        }
        private int selectIndex = -1;
        private void showMyInfo()//ok
        {
            //會員編號
            this.textBox6.Text = CMyInfo.int提供者編號.ToString();
            //會員名稱
            this.textBox3.Text = CMyInfo.string提供者名稱;
            //聯絡方式
            this.textBox1.Text = CMyInfo.string提供者手機;
            //電子信箱
            this.textBox8.Text = CMyInfo.string提供者信箱;
        }

        private void ConfirmResumes()
        {
            CInfo x = new CInfo();
            var d = from p in x.allMyDeputeRecords.AsEnumerable()
                    select p;

            if (d.Any())
            {
                var q = from p in x.allMyDeputeRecords.AsEnumerable()
                        where p.int目前狀態編號 == 5
                        select p;
                if (q.Any())
                {
                    MessageBox.Show($"您收到{q.Count()}位會員的新申請");
                }
            }
            else
            {
                MessageBox.Show("尚無委託紀錄");
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

        private void showMyDepute()//ok
        {
            CInfo CInfo = new CInfo();

            var f = from p in CInfo.allMyDetpue
                    select new
                    {
                        委託編號 = p.int委託編號,
                        目前申請人數 = p.int目前申請人數,
                        刊登時間 = p.date開始時間,
                        最後更新時間 = p.date修改時間,
                        目前狀態 = p.string目前狀態,
                        提供報酬 = p.int報酬,
                    };

            this.richTextBox1.Clear();
            this.dataGridView2.DataSource = f.ToList();
        }

        private void showReceiveMembers()//應徵者ok
        {   
            CInfo cInfo = new CInfo();
            
            var q = from p in cInfo.allMyDeputeRecords.AsEnumerable()
                    select new
                    {
                        委託編號 = p.int委託編號,
                        應徵會員編號 = p.int接案會員編號,
                        應徵會員姓名 = p.string接案會員名稱,
                        手機號碼 = p.string接案會員手機,
                        狀態 = p.string目前狀態,
                    };

            this.dataGridView1.DataSource = q.ToList();
        }

        private void loadController()//ok
        {
            //listboxskill
            var q = from p in db.SkillClasses
                    select p;
            foreach (var i in q)
            {
                this.listBox1.Items.Add(i.Name);
            }

            //comboboxskill
            var r = from p in this.db.Regions
                    select p;
            foreach (var g in r)
            {
                this.comboBox1.Items.Add(g.City);
            }
        }

        private void changeApplyStatusID(int s)//ok
        {
            //狀態更動
            var x = (from p in this.db.DeputeRecords.AsEnumerable()
                     where p.DeputeID == int.Parse(this.dataGridView1.CurrentRow.Cells[0].Value.ToString())
                     && p.MemberID == int.Parse(this.dataGridView1.CurrentRow.Cells[1].Value.ToString())
                     select p).FirstOrDefault();

            x.ApplyStatusID = s;

            this.db.SaveChanges();
            showReceiveMembers();
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

            if (q.StatusID == 18) q.StatusID = 19;
            else if (q.StatusID == 19) q.StatusID = 18;

            q.Modifiedate = DateTime.Now;

            this.db.SaveChanges();
            showMyDepute();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)//OK
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

            showMyDepute();
        }

        //=========================================================================================
        public int skillClassID = -1;
        public string skillClassName = "";
        public int skillID = -1;
        public string skillName = "";

        List<CSkill> skillList = new List<CSkill>();
        List<CSkill> chosedSkillList = new List<CSkill>();

        private void loadSkillsBox()
        {
            var q = from p in this.db.Skills.AsEnumerable()
                    select p;
            foreach (var item in q)
            {
                CSkill skill = new CSkill();
                skill.int技能編號 = item.SkillID;
                skill.string技能名稱 = item.Name;
                skill.int技能類別編號 = item.SkillClassID;
                skill.string技能類別名稱 = item.SkillClass.Name;
                skillList.Add(skill);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex < 0)
                return;
            skillClassID = this.listBox1.SelectedIndex + 1;
            skillClassName = this.listBox1.SelectedItem.ToString();

            this.listBox2.Items.Clear();
            //===========================
            var s = from p in skillList.AsEnumerable()
                    where p.int技能類別編號 == skillClassID
                    select p;

            foreach (var item in s)
            {
                this.listBox2.Items.Add(item.string技能名稱);
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex < 0) return;
            skillName = this.listBox2.SelectedItem.ToString();
            var u = (from p in skillList
                    where p.int技能類別編號 == skillClassID && p.string技能名稱 == skillName
                    select p).FirstOrDefault();
            skillID = u.int技能編號;

            CSkill c = new CSkill();
            c.int技能類別編號 = skillClassID;
            c.string技能類別名稱 = skillClassName;
            c.int技能編號 = skillID;
            c.string技能名稱 = skillName;
            chosedSkillList.Add(c);

            var i = (from p in skillList.AsEnumerable()
                     where p.int技能編號 == skillID
                     select p).FirstOrDefault();
            skillList.Remove(i);

            this.listBox3.Items.Clear();
            
            foreach (var choseSkills in chosedSkillList)
            {
                this.listBox3.Items.Add($"{choseSkills.string技能類別名稱}-{choseSkills.string技能名稱}");
                this.richTextBox3.Text = "具備以下技能為佳:\r";
                for (int o = 0; o < this.listBox3.Items.Count; o++)
                {
                    this.richTextBox3.Text += $"{o + 1}.{this.listBox3.Items[o]}\r";
                }
            }
            
            var s = from p in skillList.AsEnumerable()
                    where p.int技能類別編號 == skillClassID
                    select p;
            this.listBox2.Items.Clear();
            foreach (var skills in s)
            {
                this.listBox2.Items.Add(skills.string技能名稱);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex < 0) return;
            skillName = this.listBox2.SelectedItem.ToString();
            var u = (from p in skillList
                     where p.int技能類別編號 == skillClassID && p.string技能名稱 == skillName
                     select p).FirstOrDefault();
            skillID = u.int技能編號;

            CSkill c = new CSkill();
            c.int技能類別編號 = skillClassID;
            c.string技能類別名稱 = skillClassName;
            c.int技能編號 = skillID;
            c.string技能名稱 = skillName;
            chosedSkillList.Add(c);

            var i = (from p in skillList.AsEnumerable()
                     where p.int技能編號 == skillID
                     select p).FirstOrDefault();
            skillList.Remove(i);

            this.listBox3.Items.Clear();

            foreach (var choseSkills in chosedSkillList)
            {
                this.listBox3.Items.Add($"{choseSkills.string技能類別名稱}-{choseSkills.string技能名稱}");
                this.richTextBox3.Text = "具備以下技能為佳:\r";
                for (int o = 0; o < this.listBox3.Items.Count; o++)
                {
                    this.richTextBox3.Text += $"{o + 1}.{this.listBox3.Items[o]}\r";
                }
            }

            var s = from p in skillList.AsEnumerable()
                    where p.int技能類別編號 == skillClassID
                    select p;
            this.listBox2.Items.Clear();
            foreach (var skills in s)
            {
                this.listBox2.Items.Add(skills.string技能名稱);
            }
            this.listBox2.SelectedIndex = 0;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
        }
        //=========================================================================================













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
            loadController();
            showReceiveMembers();
            showMyDepute();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
                this.button4.Enabled = true;
            else
                this.button4.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //重設
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

            CInfo cInfo = new CInfo();
            cInfo.moveTo(e.RowIndex);
            this.richTextBox1.Text = cInfo.current.string懸賞內容;
        }

        
    }
}
