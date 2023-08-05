using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CampusLifePlanner.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatório")]
    [MinLength(3)]
    [MaxLength(120)]
    [DisplayName("Título")]
    public string? Title { get; set; }

    [MinLength(5)]
    [MaxLength(500)]
    [DisplayName("Descrição")]
    public string? Description { get; set; }

    [MinLength(3)]
    [MaxLength(120)]
    [DisplayName("Local")]
    public string? Local { get; set; }
    public CourseDto? Course { get; set; }
}
