using System.ComponentModel.DataAnnotations;

namespace HotelManagment.ViewModels
{
    public class PrenotazioniEditModel
    {
        public Guid IdPrenotazione { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Il campo Data di inizio è obbligatorio")]
        [DataType(DataType.Date)]
        public required DateTime? DataInizio { get; set; }
        [Required(ErrorMessage = "Il campo Data di fine è obbligatorio")]
        [DataType(DataType.Date)]
        public required DateTime? DataFine { get; set; }
        [Required]
        public Guid ClienteId { get; set; }
        [Required(ErrorMessage = "Il campo Camera è obbligatorio")]
        public Guid CameraId { get; set; }
        [Required]
        public string Stato { get; set; }
    }
}
