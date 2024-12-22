namespace SurvayBasket.Contracts.User
{
    public record UserResponse(
        string id,
        string FirstName,
        string LastName,
        string Email,
        bool IsDisabled,
        IEnumerable<string> Roles

        );

}
