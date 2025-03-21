using System.ComponentModel.DataAnnotations;

namespace HotelManagment.Models
{
    public class Camera
    {
        [Key]
        public Guid CameraId { get; set; }
        [Required]
        public string Numero { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public decimal Prezzo { get; set; }
        [Required]
        public bool IsLoan { get; set; }
        public ICollection<Prenotazione> Prenotazione { get; set; }
    }
}
