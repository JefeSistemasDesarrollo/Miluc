using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.Models.Validaciones
{
    public class ValidarAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            bool bandera = false;
            if (Convert.ToInt32(value) == 0)
            {
                bandera = false;
            }
            else if(Convert.ToInt32(value) >= 0)
            {
                bandera = true;
            }else if (Convert.ToInt32(value) == -1)
            {
                bandera= true;
            }
                return bandera;
        }
    }

}   
