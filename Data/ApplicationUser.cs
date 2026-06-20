using Microsoft.AspNetCore.Identity;

namespace DiaspoDirect.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public string? Country { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
