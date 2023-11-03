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
        private List<CDepute.CDeputeA> _list = new List<CDepute.CDeputeA>() { };
        private List<CDepute.CDeputeA> _dgvList = new List<CDepute.CDeputeA>();

        public List<CDepute.CDeputeA    > List { get { return _list; } set { value = _list; } }

        public List<CDepute.CDeputeA> DgvList {get { return _dgvList; } set { value = _dgvList; } }
        public List<CDepute.CDeputeA> deputeLoad()
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
                CDepute.CDeputeA x = new CDepute.CDeputeA() 
                { 
                id = item.DeputeID.ToString(),
                providername = item.Name,
                startdate = item.SrartDate,
                modifieddate = item.Modifiedate,
                content = item.DeputeContent,
                salary = item.Salary.ToString(),
                status = item.Status,
                region = item.City
                                                  };
                
                _list.Add( x );

            }

            return _list;


        }

        public List<CDepute.CDeputeA> dataSearch(List<CDepute.CDeputeA> list,string input)
        {
            var data = from n in list.AsEnumerable()
                       where n.content.ToLower().Contains(input.ToLower())
                       orderby n.modifieddate descending
                       select n;
                return data.ToList();
            
        }

        public void Hotkey(string a, string b, string c)
        {

            var value = (from n in this.entities.SerachRecords.AsEnumerable()
                         where n.IsMember == true
                         group n by n.Name into q
                         orderby q.Count() descending
                         select q.Key).ToList();
            a = value[0].ToString();
            b = value[1].ToString();
            c = value[2].ToString();
        }

        
        public List<CDepute.CDeputeA> dgvDataLoad(int sourcecount, DataGridView data)
        {
            _dgvList.Clear();
            for (int i = 0; i < sourcecount; i++)
            {
                _dgvList.Add(new CDepute.CDeputeA
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
        public void dataRefresh(BindingSource binding,DataGridView dgv,List<CDepute.CDeputeA> value)
        {
            binding.Clear();
            binding.DataSource = value.ToList();
            dgv.DataSource = binding;
           
        }
    }
}
