using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;

namespace HotelManagment.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
        public ICollection<Prenotazione> Prenotazione { get; set; }
    }
}
