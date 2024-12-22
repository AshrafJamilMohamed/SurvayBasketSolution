namespace SurvayBasket.Contracts.User
{
    public class UpdateUserRequest
    {
        [Required]
        [Length(3, 100, ErrorMessage = "MinLength is 3 ,MaxLength is 100")]
        public string FirstName { get; set; } = string.Empty;
        [Required]

        [Length(3, 100, ErrorMessage = "MinLength is 3 ,MaxLength is 100")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Length(1, int.MaxValue, ErrorMessage = "Roles can not be null or empty!")]
        public IList<string> Roles { get; set; } = default!;
    }
}
