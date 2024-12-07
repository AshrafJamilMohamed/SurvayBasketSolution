using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Account
{
    public class UpdateUserProfileRequest
    {
        [Required]
        [Length(3, 100, ErrorMessage = "MinLength is 3 ,MaxLength is 100")]
        public string FirstName { get; set; } = string.Empty;
        [Required]

        [Length(3, 100, ErrorMessage = "MinLength is 3 ,MaxLength is 100")]
        public string LastName { get; set; } = string.Empty;

    }
}
