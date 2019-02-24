//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BitSolutions.Models.SAS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ticket()
        {
            this.Employee_Ticket = new HashSet<Employee_Ticket>();
            this.Ticket_Attachments = new HashSet<Ticket_Attachments>();
            this.Ticket_History = new HashSet<Ticket_History>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ID_Client { get; set; }
        public Nullable<int> ID_User_In_Charge { get; set; }
        public System.DateTime DateIn { get; set; }
        public System.DateTime DateOut { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    
        public virtual DB_Client DB_Client { get; set; }
        public virtual DB_RRHH_Employee DB_RRHH_Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee_Ticket> Employee_Ticket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket_Attachments> Ticket_Attachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket_History> Ticket_History { get; set; }
    }
}
