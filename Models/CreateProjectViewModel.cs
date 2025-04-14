using System.ComponentModel.DataAnnotations;

namespace Condorcet.B2.AspnetCore.MVC.Application.Models;

public class CreateProjectViewModel
{
    [Required]
    public required string Name { get; set; }
}