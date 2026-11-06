using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Nomina.Validations
{
    public class FechaExpedicionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var fechaNacimientoProperty =
                validationContext.ObjectType.GetProperty("FechaNacimiento");

            if (fechaNacimientoProperty == null)
                return ValidationResult.Success;

            var fechaNacimiento =
                (DateTime?)fechaNacimientoProperty.GetValue(validationContext.ObjectInstance);

            if (!fechaNacimiento.HasValue)
            {
                return new ValidationResult(
                    "Debe seleccionar primero la fecha de nacimiento.");
            }

            var fechaExpedicion = (DateTime)value;

            if (fechaExpedicion.Date > DateTime.Today)
            {
                return new ValidationResult(
                    "La fecha de expedición no puede ser futura.");
            }

            if (fechaExpedicion.Date < fechaNacimiento.Value.Date)
            {
                return new ValidationResult(
                    "La fecha de expedición no puede ser anterior a la fecha de nacimiento.");
            }

            int edad = fechaExpedicion.Year - fechaNacimiento.Value.Year;

            if (fechaNacimiento.Value.Date > fechaExpedicion.AddYears(-edad))
            {
                edad--;
            }

            if (edad < 18)
            {
                return new ValidationResult(
                    "La fecha de expedición debe corresponder a una persona mayor de 18 años.");
            }

            return ValidationResult.Success;
        }
    }
}