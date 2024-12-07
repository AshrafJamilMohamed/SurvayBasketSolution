using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Account
{
    public class ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
