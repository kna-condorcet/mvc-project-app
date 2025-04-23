using System.ComponentModel.DataAnnotations;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;
using Condorcet.B2.AspnetCore.MVC.Application.Models.Validation;

namespace Condorcet.B2.AspnetCore.MVC.Application.Models;

public class CreateProjectViewModel : IValidatableObject
{
    public required string? Name { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        if (Name == null)
        {
            results.Add(
                new ValidationResult("Le nom est requis", 
                memberNames: [nameof(Name)]
                ));
        }

        if (Name is not null && !Name.StartsWith("AB"))
        {
            results.Add(new ValidationResult("le nom doit commencer par AB"));
        }

        var projectRepository = validationContext.GetRequiredService<IProjectRepository>();
        var exists = projectRepository.Exists(Name);
        if (exists.GetAwaiter().GetResult())
        {
            results.Add(
                new ValidationResult("Le nom doit être unique", 
                    memberNames: [nameof(Name)]
                ));
        }
        
        return results;
    }
}