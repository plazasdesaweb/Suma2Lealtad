//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Role
    {
        public int id { get; set; }

        [Required(ErrorMessage = "* EL Nombre es requerido.")]
        public string name { get; set; }

        [Required(ErrorMessage = "* EL Nivel es requerido.")]
        public int level { get; set; }
    }
}
