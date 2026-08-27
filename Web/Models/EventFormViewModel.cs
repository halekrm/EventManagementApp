using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class EventFormViewModel
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Etkinlik başlığı zorunludur!")]
        [StringLength(255, ErrorMessage = "Etkinlik başlığı en fazla 255 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur!")]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur!")]
        public DateTime EndDateTime { get; set; }

        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Kısa açıklama zorunludur!")]
        [StringLength(512, ErrorMessage = "Kısa açıklama en fazla 512 karakter olabilir")]
        public string ShortDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Detaylı açıklama zorunludur!")]
        public string LongDescription { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}