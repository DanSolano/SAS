﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BitSolutions.Models.Client
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DB_ClientEntities : DbContext
    {
        public DB_ClientEntities()
            : base("name=DB_ClientEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Belong_To> Belong_To { get; set; }
        public virtual DbSet<DB_RRHH_Employee> DB_RRHH_Employee { get; set; }
        public virtual DbSet<Enterprise> Enterprises { get; set; }
        //public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    
        public virtual ObjectResult<spSharedData_Result> spSharedData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spSharedData_Result>("spSharedData");
        }
    }
}
