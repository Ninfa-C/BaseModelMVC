using System.ComponentModel.DataAnnotations;

namespace HotelManagment.Models
{
    public class Cliente
    {
        [Key]
        public Guid ClienteId { get; set; }
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
        public ICollection<Prenotazione> Prenotazione { get; set; }
    }
}
