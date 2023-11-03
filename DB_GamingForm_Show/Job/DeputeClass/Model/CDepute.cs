using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.Job.DeputeClass
{
    public class CDepute
    {
        public string s1 { get; set; }
        public string s2 { get; set; }
        public string s3 { get; set; }
        public class CDeputeA { 
        
            public string id { get; set; }
            public string providername { get; set; }
            public string startdate { get; set; }

            public string modifieddate { get; set; }

            public string content { get; set; }

            public string salary { get; set; }

            public string status { get; set; }

            public string region { get; set; }
    }

    public class CDeputeB
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
}
