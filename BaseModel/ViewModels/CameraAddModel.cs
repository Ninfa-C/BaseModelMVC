using System.ComponentModel.DataAnnotations;

namespace HotelManagment.ViewModels
{
    public class CameraAddModel
    {
        [Required]
        public string Numero { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public decimal Prezzo { get; set; }
        [Required]
        public IFormFile img { get; set; }
    }
}
