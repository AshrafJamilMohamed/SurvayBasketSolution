using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Account
{
    public sealed class ResendConfirmEmailRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
