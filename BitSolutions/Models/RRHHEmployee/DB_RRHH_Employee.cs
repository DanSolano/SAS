//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BitSolutions.Models.RRHHEmployee
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DB_RRHH_Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DB_RRHH_Employee()
        {
            this.DB_RRHH_History1 = new HashSet<DB_RRHH_History>();
            this.DB_RRHH_History2 = new HashSet<DB_RRHH_History>();
        }
    
        public int ID { get; set; }
        public string identification { get; set; }

        [Display (Name = "User name")]
        [Required (ErrorMessage = "The user name is required")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The password is required")]
        [DataType (DataType.Password)]
        public string Password { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public System.DateTime DateBirth { get; set; }
        public System.DateTime DateIn { get; set; }
        public string Status { get; set; }
    
        public virtual DB_RRHH_History DB_RRHH_History { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DB_RRHH_History> DB_RRHH_History1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DB_RRHH_History> DB_RRHH_History2 { get; set; }
        public virtual Direction Direction { get; set; }
    }
}
