using Microsoft.AspNetCore.Identity;

namespace HotelManagment.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}
