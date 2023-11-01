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

        private List<CDeputeBian> _list = new List<CDeputeBian>();

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

            CDeputeBian x = new CDeputeBian();
            CMyInfoDetial.int提供者編號 = ClassUtility.MemberID;
            CMyInfoDetial.string提供者名稱 = m.Name;
            CMyInfoDetial.string提供者手機 = m.Phone;
            CMyInfoDetial.string提供者信箱 = m.Email;

            foreach (var p in q)
            {
                x.date開始時間 = p.StartDate;
                x.date修改時間 = p.Modifiedate;
                x.string懸賞內容 = p.DeputeContent;
                x.int狀態編號 = p.StatusID;
                x.int報酬 = p.Salary;
                x.int地區編號 = p.RegionID;
                _list.Add(x);
            }
        }
        public List<CDeputeBian> allMyDetpue { get { return _list; } }

        
    }


}
