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
        private List<CDepute> _list = new List<CDepute>();
        private List<CDepute> _dgvList = new List<CDepute>();

        public List<CDepute> List { get { return _list; } set { _list = value; } }

        public List<CDepute> DgvList {get { return _dgvList; } set {  _dgvList = value; } }
        public List<CDepute> LoadData()
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

        public List<CDepute> Search(string input)
        {
            var data = from n in _list.AsEnumerable()
                       where n.content.ToLower().Contains(input.ToLower())
                       orderby n.startdate descending
                       select n;

            DgvDataLoad();

        }

        public List<CDepute> DgvDataLoad(int sourcecount)
        {
            _dgvList.Clear();
            DataGridView a = new DataGridView();
            for (int i = 0; i < sourcecount; i++)
            {
                _dgvList.Add(new CDepute
                {
                    id = a.Rows[i].Cells[0].Value.ToString(),
                    providername = a.Rows[i].Cells[1].Value.ToString(),
                    startdate = a.Rows[i].Cells[2].Value.ToString(),
                    modifieddate = a.Rows[i].Cells[3].Value.ToString(),
                    content = a.Rows[i].Cells[4].Value.ToString(),
                    salary = a.Rows[i].Cells[5].Value.ToString(),
                    status = a.Rows[i].Cells[6].Value.ToString(),
                    region = a.Rows[i].Cells[7].Value.ToString()

                });
            }

            return _dgvList;

        }

    }
}
