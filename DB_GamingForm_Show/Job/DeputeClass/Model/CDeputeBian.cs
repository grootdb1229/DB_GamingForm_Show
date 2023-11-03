using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.Job.DeputeClass.Model
{
    public class CDeputeBian
    {
        public int int委託編號 { get; set; }
        public DateTime date開始時間 { get; set; }
        public DateTime date修改時間 { get; set; }
        public string string懸賞內容 { get; set; }
        public int int報酬 { get; set; }
        public int int狀態編號 { get; set; }
        public int int地區編號 { get; set; }
        public int int目前申請人數 { get; set; }
        public string string目前狀態 { get; set; }
    }
}
