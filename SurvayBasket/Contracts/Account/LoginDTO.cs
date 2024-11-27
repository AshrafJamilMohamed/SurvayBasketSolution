using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Account
{
    public sealed class LoginDTO
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This Field is Required")]
        public string Email { get; set; } = string.Empty!;

        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty!;
    }
}
