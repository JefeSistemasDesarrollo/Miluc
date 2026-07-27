using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Roles
{
    public class RolCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MinLength(5, ErrorMessage = "El campodebe tener al al menos 5 caracteres")]
        [MaxLength(100, ErrorMessage = "El campo puede tener un maximo de 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;


        [MinLength(5, ErrorMessage = "El campodebe tener al al menos 5 caracteres")]
        [MaxLength(100, ErrorMessage = "El campo puede tener un maximo de 100 caracteres")]

        public string Descripcion { get; set; }= string.Empty;

    }
}
