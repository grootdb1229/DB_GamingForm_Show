using DB_GamingForm_Show.Job.DeputeClass;
using Gaming_Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.Job
{
    public class CInfo
    {
        DB_GamingFormEntities db = new DB_GamingFormEntities();

        private List<CDepute> _deputeList = new List<CDepute>();

        private List<CDeputeRecord> _recordList = new List<CDeputeRecord>();

        private int _position = 0;

        public CDepute current { get { return _deputeList[_position]; } }
        
        public static int selectedMemberid {  get; set; }
        
        public CInfo() 
        {
            loadDatas();
        }

        private void loadDatas()
        {
            var m=(from p in this.db.Members
                  where p.MemberID==ClassUtility.MemberID
                  select p).FirstOrDefault();
            CMyInfo.int提供者編號 = ClassUtility.MemberID;
            CMyInfo.string提供者名稱 = m.Name;
            CMyInfo.string提供者手機 = m.Phone;
            CMyInfo.string提供者信箱 = m.Email;

            var q = from p in this.db.Deputes
                    where p.ProviderID == ClassUtility.MemberID
                    select p;
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
                _deputeList.Add(x);
            }
            //==============================================================
            var s = from p in this.db.DeputeRecords
                    where p.Depute.Member.MemberID == ClassUtility.MemberID
                    select p;
            foreach(var p in s)
            {
                CDeputeRecord x = new CDeputeRecord();
                x.int委託編號=p.DeputeID;
                x.int接案會員編號 = p.MemberID;
                x.string接案會員名稱 = p.Member.Name;
                x.string接案會員手機 = p.Member.Phone;
                x.string目前狀態 = p.Status.Name;
                x.int目前狀態編號 = p.ApplyStatusID;
                _recordList.Add(x);
            }
        }

        public void moveTo(int to)
        {
            _position = to;
        }

        public List<CDeputeRecord> allMyDeputeRecords
        {
            get
            {
                _recordList.Clear();
                loadDatas();
                return _recordList;
            }
        }

        public List<CDepute> allMyDetpue
        {
            get
            {
                _deputeList.Clear();
                loadDatas();
                return _deputeList;
            }
        }
    }
}
