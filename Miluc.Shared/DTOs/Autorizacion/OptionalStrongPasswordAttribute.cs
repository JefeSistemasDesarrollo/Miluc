using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Miluc.Shared.DTOs.Autorizacion
{
    public class OptionalStrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            // Si está vacía → no validar
            if (string.IsNullOrWhiteSpace(password))
                return ValidationResult.Success;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

            if (!regex.IsMatch(password))
                return new ValidationResult("Debe tener mínimo 8 caracteres, mayúscula, minúscula, número y carácter especial.");

            return ValidationResult.Success;
        }
    }
}
