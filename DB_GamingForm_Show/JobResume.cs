//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DB_GamingForm_Show
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobResume
    {
        public int ID { get; set; }
        public int JobID { get; set; }
        public int ResumeID { get; set; }
        public int ApplyStatusID { get; set; }
    
        public virtual Job_Opportunity Job_Opportunities { get; set; }
        public virtual Resume Resume { get; set; }
        public virtual Status Status { get; set; }
    }
}
