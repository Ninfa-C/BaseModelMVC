using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagment.Models
{
    public class Prenotazione
    {
        [Key]
        public Guid PrenotazioneId { get; set; }
        [Required]
        public Guid ClienteId { get; set; }
        [Required]
        public Guid CameraId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public required DateTime? DataInizio { get; set; }
        [Required]
        public required DateTime? DataFine { get; set; }
        [Required]
        [StringLength(50)]
        public string Stato { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
