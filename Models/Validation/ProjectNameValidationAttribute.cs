using System.ComponentModel.DataAnnotations;

namespace Condorcet.B2.AspnetCore.MVC.Application.Models.Validation;

public class ProjectNameValidationAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Le nom est requis");
        }

        if (!value.ToString().StartsWith("AB"))
        {
            return new ValidationResult("le nom doit commencer par AB");
        }
        
        return ValidationResult.Success;
    }
}