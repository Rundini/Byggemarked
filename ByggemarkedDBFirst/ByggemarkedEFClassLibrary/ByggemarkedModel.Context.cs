﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ByggemarkedEFClassLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ByggemarkedEntities : DbContext
    {
        public ByggemarkedEntities()
            : base("name=ByggemarkedEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bookinger> Bookinger { get; set; }
        public virtual DbSet<Kunder> Kunder { get; set; }
        public virtual DbSet<Vaerktoej> Vaerktoej { get; set; }
    }
}
