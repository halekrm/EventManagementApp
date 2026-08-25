using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Ad alanı zorunludur!")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
       
        [Required(ErrorMessage ="Soyad alanı zorunludur!")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage ="E-posta alanı zorunludur!")]
        [EmailAddress(ErrorMessage ="Geçerli bir e-posta giriniz.")]
        [StringLength(255)]
        public string Email {get; set;} =string.Empty;

        [Required(ErrorMessage ="Şifre alanı zorunludur!")]
        [MinLength(8,ErrorMessage ="Şifre en az 8 karakter olmalıdır.")]
        [RegularExpression( @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",ErrorMessage ="Şifreniz en az bir büyük harf,bir küçük harf ve bir rakam içermelidir.")]
        public string Password { get; set; } =string.Empty;

        [Required(ErrorMessage ="Şifre tekrar alanı zorunludur!")]
        [Compare("Password",ErrorMessage ="Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } =string.Empty;

        [Required(ErrorMessage ="Doğum tarihi alanı zorunludur!")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


    }
}