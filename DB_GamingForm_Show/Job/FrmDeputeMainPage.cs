using Gaming_Forum;
using Groot;
//using Groot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_GamingForm_Show
{
    public partial class FrmDeputeMainPage : Form
    {
        public FrmDeputeMainPage()
        {

            
            InitializeComponent();
            this.button3.Enabled = false;
            ComboLoad();
            LoadData();
            HotSearch();
            





        }
        #region OfficalCode
        public int count = 1;
        public int sourcecount = 0;
        public int page = 0;
        public int pagecount = 25;
        public bool flag = true;
        DB_GamingFormEntities entities = new DB_GamingFormEntities();
        List<Result> list = new List<Result>();
        public class Result
        {
            public string DeputeID { get; set; }
            public string ProvideMember { get; set; }
            public string StartDate { get; set; }

            public string ModerfiedDate { get; set; }

            public string DeputeContent { get; set; }

            public string Salary { get; set; }

            public string Status { get; set; }
            

        }
        private void HotSearch()
        {
            try {



                var value = (from n in this.entities.SerachRecords.AsEnumerable()
                             where n.IsMember == true
                             group n by n.Name into q
                             orderby q.Count() descending
                             select q.Key).ToList();
                this.linkLabel1.Text = value[0].ToString();
                this.linkLabel2.Text = value[1].ToString();
                this.linkLabel3.Text = value[2].ToString();
            } 
            
            
            catch(Exception) 
            {

                this.linkLabel1.Visible = false;
                this.linkLabel2.Visible = false;
                this.linkLabel3.Visible = false;


            }
           


        }

        private void ComboLoad()
        {
            this.comboBox1.Items.Add("請選擇...");
            this.comboBox2.Items.Add("請選擇...");
            this.comboBox3.Items.Add("請選擇...");
            this.comboBox5.Items.Add("請選擇...");
            this.comboBox7.Items.Add("請選擇...");
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.comboBox5.SelectedIndex = 0;
            this.comboBox6.SelectedIndex = 0;
            this.comboBox7.SelectedIndex = 0;
            var q = from n in this.entities.Educations
                    select n.Name;
            foreach (var educations in q)
            {
                this.comboBox1.Items.Add(educations);
            }
            var q2 = from n in this.entities.Certificates
                     select n.Name;
            foreach (var cert in q2)
            {
                this.comboBox2.Items.Add(cert);
            }
            var q3 = from n in this.entities.SkillClasses
                     select n.Name;
            foreach (var skillClass in q3)
            {
                this.comboBox3.Items.Add(skillClass);
            }
            var q5 = from n in this.entities.Regions
                     select n.City;
            foreach (var region in q5)
            {
                this.comboBox5.Items.Add(region);
            }
            var q7 = from n in this.entities.Status
                     where n.StatusID == 3 || n.StatusID == 4
                     select n.Name;
            foreach (var status in q7)
            {
                this.comboBox7.Items.Add(status);
            }


        }

        private void LoadData()
        {
            this.button1.Enabled = true;
            this.bindingSource1.Clear();
            var data = from n in this.entities.Deputes.AsEnumerable()
                       orderby n.StartDate descending
                       select new
                       {
                           n.DeputeID,
                           n.Member.Name,
                           SrartDate = n.StartDate.ToString("d"),
                           Modifiedate = n.Modifiedate.ToString("d"),
                           n.DeputeContent,
                           //todo bian1028 找不到salary
                           //n.salary,
                           //status = n.status.name
                       };
            this.bindingSource1.DataSource = data.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
            sourcecount = this.dataGridView1.RowCount;
            ListLoad(sourcecount);

        }



        private void Clear()
        {
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.comboBox5.SelectedIndex = 0;
            this.comboBox6.SelectedIndex = 0;
            this.comboBox7.SelectedIndex = 0;





        }




        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            list.Clear();
            var data = from n in this.entities.Deputes.AsEnumerable()
                       where n.DeputeContent.Contains(this.checkedListBox1.Text)
                       orderby n.StartDate descending
                       select new
                       {
                           n.DeputeID,
                           n.Member.MemberID,
                           SrartDate = n.StartDate.ToString("d"),
                           n.Modifiedate,
                           n.DeputeContent,
                           Status = n.Status.Name
                       };
            if (flag)
            {
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = data.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"{page * pagecount}/{list.Count()}筆";
                flag = !flag;
            }
            else
            {
                LoadData();
                flag = !flag;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                LoadData();
            }
            Search(this.textBox1.Text);
        }

        private void Remove()
        {
            //var value = (from p in this.entities.SerachRecords
            //               where p.CreateDays.Day <=DateTime.Now.Day-7
            //               select p).FirstOrDefault();

            //if (value == null) return;

            this.entities.SerachRecords.Remove
                                (new SerachRecord { ID = 2 });
            this.entities.SaveChanges();

        }
        private void Search(string source)
        {
            if (source == "")
            {
                Clear();
                LoadData();
            }
            else
            {
                this.entities.SerachRecords.Add
                                (new SerachRecord { Name = source, CreateDays = (DateTime.Now.Date), IsMember = true });
                this.entities.SaveChanges();
                var data = from n in this.entities.Deputes.AsEnumerable()
                           where n.DeputeContent.ToLower().Contains(source.ToLower())
                           orderby n.StartDate descending
                           select new
                           {
                               n.DeputeID,
                               n.Member.MemberID,
                               SrartDate = n.StartDate.ToString("d"),
                               n.Modifiedate,
                               n.DeputeContent,
                               Status = n.Status.Name
                           };
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = data.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"{page* pagecount}/{this.dataGridView1.RowCount}筆";
                sourcecount = data.Count();
                ListLoad(sourcecount);

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Search(this.linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Search(this.linkLabel2.Text);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Search(this.linkLabel3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Filter();

        }




        private void button4_Click_1(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBox1.SelectedIndex == 0)
            //{
            //    Clear();
            //    LoadData();

            //}
            //else
            //{
            //    var value = from n in list.AsEnumerable()
            //                where n.EducationRequirements == this.comboBox1.Text
            //                orderby n.ModerfiedDate descending
            //                select n;
            //    this.bindingSource1.Clear();
            //    this.bindingSource1.DataSource = value.ToList();
            //    this.dataGridView1.DataSource = this.bindingSource1;
            //    sourcecount = value.Count();
            //    ListLoad(sourcecount);
            //}
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedIndex == 0)
            {
                Clear();
                LoadData();
            }
            else
            {
                var value = from n in list.AsEnumerable()
                            where n.DeputeContent.ToLower().Contains(this.comboBox2.Text.ToLower())
                            orderby n.StartDate
                            select n;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = value.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                sourcecount = value.ToList().Count();
                ListLoad(sourcecount);
            }

        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q4 = from n in this.entities.Skills.AsEnumerable()
                     where n.SkillClass.Name == this.comboBox3.Text
                     select n.Name;
            this.comboBox4.DataSource = q4.ToList();
            if (this.comboBox3.SelectedIndex == 0)
            {
                this.comboBox4.Text = "";
                Clear();
                LoadData();
            }

            //this.label12.Text = $"10/{this.dataGridView1.RowCount}筆";
        }



        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = from n in list.AsEnumerable()
                        where n.DeputeContent.ToLower().Contains(this.comboBox4.Text.ToLower())
                        orderby n.StartDate descending
                        select n;
            this.bindingSource1.Clear();
            this.bindingSource1.DataSource = value.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
            sourcecount = value.Count();
            ListLoad(sourcecount);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Todo 沒有Region
            //if (this.comboBox5.SelectedIndex == 0)
            //{
            //    Clear();
            //    LoadData();
            //}
            //else
            //{
            //    var value = from n in list.AsEnumerable()
            //                where n.Region == this.comboBox5.Text
            //                orderby n.ModerfiedDate descending
            //                select n;

            //    this.bindingSource1.Clear();
            //    this.bindingSource1.DataSource = value.ToList();
            //    this.dataGridView1.DataSource = this.bindingSource1;
            //    sourcecount = value.Count();
            //    ListLoad(sourcecount);
            //}
            //this.label12.Text = $"10/{this.dataGridView1.RowCount}筆";
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox6.SelectedIndex == 0)
            {
                Clear();
                LoadData();
            }
            else
            {
                var value = this.list.AsEnumerable()
                            .Where(n => Convert.ToInt32(n.Salary) >= int.Parse(this.comboBox6.Text))
                            .Select(n => n).OrderByDescending(n => n.ModerfiedDate);

                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = value.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                sourcecount = value.Count();
                ListLoad(sourcecount);
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox7.SelectedIndex == 0)
            {
                Clear();
                LoadData();
            }
            else
            {
                var value = from n in list.AsEnumerable()
                            where n.Status == this.comboBox7.Text
                            orderby n.ModerfiedDate descending
                            select n;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = value.ToList();
                this.dataGridView1.DataSource = this.bindingSource1;
                sourcecount = value.Count();
                ListLoad(sourcecount);
            }
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            Clear();
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Remove();
        }



        private void ListLoad(int sourcecount)
        {
            
            count += 1;
            page = 0;
            list.Clear();
            for (int i = 0; i < sourcecount; i++)
            {
                list.Add(new Result
                {
                    DeputeID = this.dataGridView1.Rows[i].Cells[0].Value.ToString(),
                    ProvideMember = this.dataGridView1.Rows[i].Cells[1].Value.ToString(),
                    StartDate = this.dataGridView1.Rows[i].Cells[2].Value.ToString(),
                    ModerfiedDate = this.dataGridView1.Rows[i].Cells[3].Value.ToString(),
                    DeputeContent = this.dataGridView1.Rows[i].Cells[4].Value.ToString(),
                    Salary= this.dataGridView1.Rows[i].Cells[5].Value.ToString(),
                    Status = this.dataGridView1.Rows[i].Cells[6].Value.ToString(),

                });
            }
            this.bindingSource1.Clear();
            this.bindingSource1.DataSource = list.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
            if (list.Count == 0 && count != 1)
            {
                MessageBox.Show("No Match");
                Clear();
                LoadData();

            }
            else if (list.Count <= pagecount)
            {
                this.label12.Text = $"{list.Count} /{list.Count}筆";
                this.button1.Enabled = false;
                this.button3.Enabled = false;
            }
            else
            {
                this.label12.Text = $"{list.Count} /{list.Count}筆";
                this.button3.Enabled = false;
            }


        }

        



        public int Id { get; set; }


        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Gaming_Forum.ClassUtility.MemberID != 0)
            {
                FrmMakeJobRequire re = new FrmMakeJobRequire();
                re.Show();
            }
            else
            {
                MessageBox.Show("請登入後再嘗試");

            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position -= 1;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Position += 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            page -= 1;
            if (page < 0)
            {
                page = 0;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip(page  * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.button3.Enabled = false;
                this.label12.Text = $"已是第一頁";

            }
            else
            {
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip(page * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.button1.Enabled = true;
                this.label12.Text = $"{page * pagecount} /{list.Count}筆";

            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            page += 1;
            if (page * pagecount > list.Count)
            {

                page -= 1;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip(page * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"已是最後一頁";
                this.button1.Enabled = false;
                
            }
            else
            {
                this.button3.Enabled = true;
                this.bindingSource1.Clear();
                this.bindingSource1.DataSource = list.ToList().Skip(page * pagecount).Take(pagecount);
                this.dataGridView1.DataSource = this.bindingSource1;
                this.label12.Text = $"{page * pagecount} /{list.Count}筆";

            }
            
        }
        #endregion
        #region 再研究
        private void Filter()
        {
            string f1 = this.comboBox1.Text;
            string f2 = this.comboBox2.Text;
            string f3 = this.comboBox3.Text;
            string f4 = this.comboBox4.Text;
            string f5 = this.comboBox5.Text;
            string f6 = this.comboBox6.Text;
            string f7 = this.comboBox7.Text;
            



        }


        #endregion
        #region Pratice
        private void Blog()
        {
            var q = this.entities.Blogs.Where(n => n.SubTag.TagID == 4).Select(n => n);
            this.dataGridView1.DataSource = q.ToList();

            if (ClassUtility.FirmID == 0)
            {
                Id = ClassUtility.FirmID;

            }
            else
            {
                Id = ClassUtility.MemberID;

            }


        }
        #endregion
    }
}
