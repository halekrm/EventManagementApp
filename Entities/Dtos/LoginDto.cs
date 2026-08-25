using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage ="E-posta alanı zorunludur!")]
        [EmailAddress(ErrorMessage ="Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="Şifre alanı zorunludur!")]
        public string Password { get; set; } = string.Empty;
    }
}