using System.ComponentModel.DataAnnotations;

namespace CampusLifePlanner.WebUI.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Sobrenome é obrigatório.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email não é um endereço de email válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Comfirme a senha")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirme a senha")]
    [Compare("Password", ErrorMessage = "As senhas não são iguais.")]
    public string ConfirmPassword { get; set; }
}
