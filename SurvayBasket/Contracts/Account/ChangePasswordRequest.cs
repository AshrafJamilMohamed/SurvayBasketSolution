using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Account
{
    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*\\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\\w\\d\\s:])([^\\s]){8,16}$",
           ErrorMessage = @"password must contain 1 number (0-9)
                             password must contain 1 uppercase letters
                             password must contain 1 lowercase letters
                             password must contain 1 non-alpha numeric number
                             password is 8-16 characters with no space")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
