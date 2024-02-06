using APIServer.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Models
{
   
    public class ApplicationUser : IdentityUser
    {
        // Add a key property
        public string Id { get; set; }

        public Role Role { get; set; }
    }
}
