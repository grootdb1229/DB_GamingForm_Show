using DB_GamingForm_Show.Job.DeputeClass;
using Gaming_Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.Job
{
    public class CMyInfo
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        private List<CDepute> _list = new List<CDepute>();

        public static int selectedMemberid;

        public CMyInfo() 
        {
            loadData();
        }

        private void loadData()
        {
            var m=(from p in this.db.Members
                  where p.MemberID==ClassUtility.MemberID
                  select p).FirstOrDefault();
            
            var q = from p in this.db.Deputes
                    where p.ProviderID == ClassUtility.MemberID
                    select p;

            


            CMyInfoDetial.int提供者編號 = ClassUtility.MemberID;
            CMyInfoDetial.string提供者名稱 = m.Name;
            CMyInfoDetial.string提供者手機 = m.Phone;
            CMyInfoDetial.string提供者信箱 = m.Email;

            foreach (var p in q)
            {
                var dr = from f in this.db.DeputeRecords
                         where f.DeputeID ==p.DeputeID
                         select f;

                CDepute x = new CDepute();
                x.int委託編號 = p.DeputeID;
                x.date開始時間 = p.StartDate;
                x.date修改時間 = p.Modifiedate;
                x.string懸賞內容 = p.DeputeContent;
                x.int狀態編號 = p.StatusID;
                x.int報酬 = p.Salary;
                x.int地區編號 = p.RegionID;
                x.string目前狀態 = p.Status.Name;
                x.int目前申請人數 = dr.Count();
                _list.Add(x);
            }

            
        }
        public List<CDepute> allMyDetpue { get { return _list; } }

        
    }


}
