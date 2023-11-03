using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.Job.DeputeClass
{
    public class CDeputeRecord
    {
        public int int委託編號 {  get; set; }
        public int int接案會員編號 {  set; get; }
        public string string接案會員名稱 {  set; get; }
        public string string接案會員手機 { set; get; }
        public string string目前狀態 { set; get; }
        public int int目前狀態編號 { get; set; }
    }
}
