using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CampusLifePlanner.Application.DTOs;
public class CourseDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatório")]
    [MinLength(3)]
    [MaxLength(120)]
    [DisplayName("Curso")]
    public string? Name { get; set; }
    public string? Sigla { get; set; }
    public List<EventDto>? Events { get; set; }
    public int EnrollmentCount { get; set; }
}
