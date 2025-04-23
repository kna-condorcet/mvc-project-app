using System.ComponentModel.DataAnnotations;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;

namespace Condorcet.B2.AspnetCore.MVC.Application.Models.Validation;

public class ProjectCodeUniqueValidationAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Le nom est requis");
        }

        var repository = validationContext.GetRequiredService<IProjectRepository>();

        var projectExistsTask = repository.ProjectCodeExists(value as string);

        if (projectExistsTask.GetAwaiter().GetResult())
            return new ValidationResult("Ce code projet existe déjà");
        
        return ValidationResult.Success;
    }
}