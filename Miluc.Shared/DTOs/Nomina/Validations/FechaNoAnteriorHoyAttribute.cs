using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.Validations
{
    public class FechaNoAnteriorHoyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime fecha)
            {
                if (fecha.Date < DateTime.Today)
                {
                    return new ValidationResult(
                        "La fecha de finalización no puede ser anterior a la fecha actual.");
                }
            }

            return ValidationResult.Success;
        }
    }
}