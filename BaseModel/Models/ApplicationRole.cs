using Microsoft.AspNetCore.Identity;

namespace BaseModel.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}
