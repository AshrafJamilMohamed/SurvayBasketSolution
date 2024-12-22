using Microsoft.AspNetCore.Identity;

namespace SurvayBasket.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FristName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;

        public bool IsDisabled { get; set; }
    }
}
