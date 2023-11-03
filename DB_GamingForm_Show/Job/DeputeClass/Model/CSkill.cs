using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_GamingForm_Show.Job.DeputeClass.View
{
    public class CSkill
    {
        public int int流水號 {  get; set; }
        public int int技能類別編號 {  get; set; }
        public string string技能類別名稱 {  get; set; }
        public int int技能編號 {  set; get; }
        public string string技能名稱 {  get; set; }

        public override string ToString()
        {
            return $"{string技能類別名稱}-{string技能名稱}";
        }

    }
}
