using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PastOrPresentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success; // [Required] se encarga de gritar si está vacío.

            if (value is DateTime dateTime)
            {
                if (dateTime.Date > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage ?? "La fecha no puede ser futura.");
                }
            }
            return ValidationResult.Success;
        }
    }
}