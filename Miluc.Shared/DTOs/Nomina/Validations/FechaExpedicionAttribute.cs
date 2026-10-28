using System;
using System.ComponentModel.DataAnnotations;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;



namespace Miluc.Shared.DTOs.Nomina.Validations
{
    public class FechaExpedicionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var empleado = (EmpleadoCreateDto)validationContext.ObjectInstance;

            if (!empleado.FechaNacimiento.HasValue)
            {
                return new ValidationResult(
                    "Debe seleccionar primero la fecha de nacimiento.");
            }

            DateTime fechaNacimiento = empleado.FechaNacimiento.Value;
            DateTime fechaExpedicion = (DateTime)value;

            if (fechaExpedicion > DateTime.Today)
            {
                return new ValidationResult(
                    "La fecha de expedición no puede ser futura.");
            }

            if (fechaExpedicion < fechaNacimiento)
            {
                return new ValidationResult(
                    "La fecha de expedición no puede ser anterior a la fecha de nacimiento.");
            }

            //if (fechaExpedicion > fechaNacimiento.AddYears(18))
            //{
            //    return new ValidationResult(
            //        "La fecha de expedición debe ser posterior a los 18 años.");
            //}

            return ValidationResult.Success;
        }

    }
}