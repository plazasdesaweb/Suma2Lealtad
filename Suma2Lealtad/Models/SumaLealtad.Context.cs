﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Suma2Lealtad.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LealtadEntities : DbContext
    {
        public LealtadEntities()
            : base("name=LealtadEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<AffiliateAud> AffiliateAuds { get; set; }
        public DbSet<Auditing> Auditings { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<CLIENTE> CLIENTES { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAffiliate> CompanyAffiliates { get; set; }
        public DbSet<CustomerInterest> CustomerInterests { get; set; }
        public DbSet<FACTURA> FACTURAs { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Object> Objects { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PARAMETRO> PARAMETROS { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<RESTRICCIONE> RESTRICCIONES { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SecurityLevel> SecurityLevels { get; set; }
        public DbSet<SecurityMenu> SecurityMenus { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRol> UserRols { get; set; }
    }
}
