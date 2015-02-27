namespace Suma2Lealtad.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Usuario
    {
        public Usuario()
        {
            this.Auditorias = new HashSet<Auditoria>();
        }
    
        public short Id { get; set; }

        [Required(ErrorMessage = "* El Login es Requerido.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]*$", ErrorMessage = "* El Id debe contener solo letras y mas de 3 caracter.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "* El Login debe tener una longitud entre 3 y 50 caracteres.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "* La Clave es Requerida.")]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{5,}", ErrorMessage = "La clave debe contener al menos 5 caracteres y tener 1 letra y 1 numero.")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "* El Nombre es Requerido.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]*$", ErrorMessage = "* El Nombre debe contener solo letras y mas de 3 caracter.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "* El Nombre debe tener una longitud entre 3 y 50 caracteres.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "* El Apellido es Requerido.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]*$", ErrorMessage = "* El Apellido debe contener solo letras y mas de 3 caracter.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "* El Apellidos debe tener una longitud entre 3 y 50 caracteres.")]
        public string Apellidos { get; set; }

        public bool Activo { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreEstatus { get; set; }

        [Required(ErrorMessage = "* El Rol es Requerido.")]
        public byte IdRol { get; set; }
        public virtual ICollection<Auditoria> Auditorias { get; set; }
        public virtual Rol Rol { get; set; }
    }
}