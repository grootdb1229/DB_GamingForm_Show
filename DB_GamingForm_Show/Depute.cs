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
    
    public partial class Depute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Depute()
        {
            this.DeputeRecords = new HashSet<DeputeRecord>();
            this.DeputeSkills = new HashSet<DeputeSkill>();
        }
    
        public int DeputeID { get; set; }
        public int ProviderID { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> Modifiedate { get; set; }
        public string DeputeContent { get; set; }
        public int Salary { get; set; }
        public int StatusID { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeputeRecord> DeputeRecords { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeputeSkill> DeputeSkills { get; set; }
    }
}