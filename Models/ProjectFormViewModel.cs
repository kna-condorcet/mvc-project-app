using System.ComponentModel.DataAnnotations;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;
using Condorcet.B2.AspnetCore.MVC.Application.Models.Validation;

namespace Condorcet.B2.AspnetCore.MVC.Application.Models;

public class ProjectFormViewModel : IValidatableObject
{
    public int? Id { get; set; }
    [Required(ErrorMessage = "Le titre est obligatoire")]
    [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Le code projet est obligatoire")]
    [RegularExpression("^[A-Z]{2}\\d{4}$", ErrorMessage = "Le code projet doit commencer par 2 lettres suivies de 4 chiffres")]
    [ProjectCodeUniqueValidation]
    public string ProjectCode { get; set; }

    [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "La date de début est obligatoire")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La priorité est obligatoire")]
    [Range(1, 5, ErrorMessage = "la priorité doit être entre 1 et 5")]
    public ProjectPriority Priority { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Le budget doit être positif")]
    public decimal Budget { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ExpectedEndDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        if (ExpectedEndDate.HasValue && ExpectedEndDate < StartDate)
        {
            results.Add(new ValidationResult(
                "La date de fin prévue doit être postérieure à la date de début",
                new[] { nameof(ExpectedEndDate) }));
        }

        if (Priority == ProjectPriority.Critical && Budget < 100000)
        {
            results.Add(new ValidationResult(
                "Les projets de priorité 1 doivent avoir un budget minimum de 100 000 €",
                new[] { nameof(Budget), nameof(Priority) }));
        }

        return results;
    }
}