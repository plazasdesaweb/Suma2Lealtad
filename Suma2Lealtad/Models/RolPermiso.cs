namespace Suma2Lealtad.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class RolPermiso
    {

        public short Id { get; set; }

        [Required(ErrorMessage = "* El Rol es Requerido.")]
        public byte IdRol { get; set; }

        [Required(ErrorMessage = "* El Menú es Requerido.")]
        public short IdMenu { get; set; }

        [Required(ErrorMessage = "* La Permisología es Requerida.")]
        public byte NivelPermiso { get; set; }

        //[Required(ErrorMessage = "* El Nivel es Requerido.")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "* El Nivel debe tener una longitud entre 3 y 50 caracteres.")]
        public string NombreNivel { get; set; }
    
        public virtual Menu Menu { get; set; }
        public virtual Rol Rol { get; set; }
    }
}
