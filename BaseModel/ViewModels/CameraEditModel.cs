using System.ComponentModel.DataAnnotations;

namespace HotelManagment.ViewModels
{
    public class CameraEditModel
    {
        public Guid IdCamera { get; set; }
        [Required]
        public string Numero { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public decimal Prezzo { get; set; }
        public IFormFile? imgFile { get; set; }
        public string? Img { get; set; }
    }
}
