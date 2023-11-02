using DB_GamingForm_Show.Job.DeputeClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_GamingForm_Show.Job
{
    public class CDepluteMainPageLoad
    {

        public CDepluteMainPageLoad() 
        {
            deputeLoad();

        }
        DB_GamingFormEntities entities = new DB_GamingFormEntities();
        private List<CDepute> _list = new List<CDepute>();
        private List<CDepute> _dgvList = new List<CDepute>();

        public List<CDepute> List { get { return _list; } set { _list = value; } }

        public List<CDepute> DgvList {get { return _dgvList; } set {  _dgvList = value; } }
        public List<CDepute> deputeLoad()
        {   
            var data = from n in this.entities.Deputes.AsEnumerable()
                       orderby n.StartDate descending
                       select new
                       {    
                           n.DeputeID,
                           n.Member.Name,
                           SrartDate = n.StartDate.ToString("d"),
                           Modifiedate = n.Modifiedate.ToString("d"),
                           n.DeputeContent,
                           n.Salary,
                           Status = n.Status.Name,
                           n.Region.City
                       };
            foreach ( var item in data ) 
            {
                CDepute x = new CDepute();
                x.id = item.DeputeID.ToString();
                x.providername = item.Name;
                x.startdate = item.SrartDate;
                x.modifieddate = item.Modifiedate;
                x.content = item.DeputeContent;
                x.salary = item.Salary.ToString();
                x.status = item.Status;
                x.region = item.City;
                _list.Add( x );

            }

            return _list;


        }

        public List<CDepute> dataSearch(List<CDepute> list,string input)
        {
            var data = from n in list.AsEnumerable()
                       where n.content.ToLower().Contains(input.ToLower())
                       orderby n.modifieddate descending
                       select n;
                return data.ToList();
            
        }

        public void hotKey(string label1, string label2, string label3)
        {

            

                var value = (from n in this.entities.SerachRecords.AsEnumerable()
                             where n.IsMember == true
                             group n by n.Name into q
                             orderby q.Count() descending
                             select q.Key).ToList();
                label1 = value[0].ToString();
                label2 = value[1].ToString();
                label3= value[2].ToString();
            


           



        }

        public List<CDepute> dgvDataLoad(int sourcecount, DataGridView data)
        {
            _dgvList.Clear();
            for (int i = 0; i < sourcecount; i++)
            {
                _dgvList.Add(new CDepute
                {
                    id = data.Rows[i].Cells[0].Value.ToString(),
                    providername = data.Rows[i].Cells[1].Value.ToString(),
                    startdate = data.Rows[i].Cells[2].Value.ToString(),
                    modifieddate = data.Rows[i].Cells[3].Value.ToString(),
                    content = data.Rows[i].Cells[4].Value.ToString(),
                    salary = data.Rows[i].Cells[5].Value.ToString(),
                    status = data.Rows[i].Cells[6].Value.ToString(),
                    region = data.Rows[i].Cells[7].Value.ToString()

                });
            }

            return _dgvList;

        }
        public void dataRefresh(BindingSource binding,DataGridView dgv,List<CDepute> value)
        {
            binding.Clear();
            binding.DataSource = value;
            dgv.DataSource = binding;
           
        }
    }
}
