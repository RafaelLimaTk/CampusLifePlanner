using System.ComponentModel.DataAnnotations;

namespace CampusLifePlanner.WebUI.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Forneça um email válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
