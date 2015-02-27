namespace Suma2Lealtad.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Interes
    {
        public Interes()
        {
            this.AfiliadoInteres = new HashSet<AfiliadoInteres>();
        }
    
        public short Id { get; set; }

        [Required(ErrorMessage = "* El Campo Nombre es requerido.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]*$", ErrorMessage = "* El Interés debe contener solo letras y mas de 3 caracteres.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "* El Nombre debe tener una longitud entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }

        public bool Activo { get; set; }
    
        public virtual ICollection<AfiliadoInteres> AfiliadoInteres { get; set; }
    }
}
