﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Carga_Masiva_POS
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SumaLealtadEntities : DbContext
    {
        public SumaLealtadEntities()
            : base("name=SumaLealtadEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CIUDAD> CIUDADES { get; set; }
        public DbSet<ESTADO> ESTADOS { get; set; }
        public DbSet<MUNICIPIO> MUNICIPIOS { get; set; }
        public DbSet<PAIS> PAISES { get; set; }
        public DbSet<PAIS_ESTADO> PAISES_ESTADOS { get; set; }
        public DbSet<PARAMETRO> PARAMETROS { get; set; }
        public DbSet<PARROQUIA> PARROQUIAS { get; set; }
        public DbSet<RESTRICCION> RESTRICCIONES { get; set; }
        public DbSet<URBANIZACION> URBANIZACIONES { get; set; }
    }
}
