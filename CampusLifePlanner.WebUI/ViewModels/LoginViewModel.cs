using System.ComponentModel.DataAnnotations;

namespace CampusLifePlanner.WebUI.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Forneça um email válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório")]
    [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max " +
        "{1} characters long.", MinimumLength = 10)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
