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
            LoadData();

        }
        DB_GamingFormEntities entities = new DB_GamingFormEntities();
        private List<DeputeClass.CDepute> _list = new List<DeputeClass.CDepute>();
        private List<DeputeClass.CDepute> _dgvList = new List<DeputeClass.CDepute>();

        public List<DeputeClass.CDepute> List { get { return _list; } set { _list = value; } }

        public List<DeputeClass.CDepute> DgvList {get { return _dgvList; } set {  _dgvList = value; } }
        public List<DeputeClass.CDepute> LoadData()
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
                DeputeClass.CDepute x = new DeputeClass.CDepute();
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

        public List<DeputeClass.CDepute> Search(string input)
        {
            var data = from n in _list.AsEnumerable()
                       where n.content.ToLower().Contains(input.ToLower())
                       orderby n.startdate descending
                       select n;
                return data.ToList();
            
        }

        public List<DeputeClass.CDepute> DgvDataLoad(int sourcecount, DataGridView data)
        {
            _dgvList.Clear();
            for (int i = 0; i < sourcecount; i++)
            {
                _dgvList.Add(new DeputeClass.CDepute
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

    }
}
