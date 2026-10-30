using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Miluc.Shared.DTOs.Nomina.Validations
{
    public class MayorDeEdadAttribute : ValidationAttribute
    {
 
        public override bool IsValid(object value)
        {

            bool bandera = true;


            DateTime fechanacimiento = Convert.ToDateTime(value);


            var fechaMinima = DateTime.Today.AddYears(-18);

            var fechamaximaedad = DateTime.Today.AddYears(-80);



            
             if ( fechanacimiento>fechaMinima)
            {
                bandera = false;
            }
            else if(fechamaximaedad>fechanacimiento)
            {
                bandera = false;
            }
            else
            {
                bandera = true;
            }



          

            return bandera;





           
        }
    }
}



