using System.ComponentModel.DataAnnotations;

namespace AppAcountsAuthentication.Models
{
    public class RegisterUser
    {
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
}
