 
namespace SurvayBasket.Contracts.Roles
{
    public class AddRoleRequest
    {
        [Required]
        [Length(3, 50)]
        public string Name { get; set; } = string.Empty;
    }
}
