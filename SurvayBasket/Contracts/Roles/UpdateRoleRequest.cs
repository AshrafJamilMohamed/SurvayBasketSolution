
namespace SurvayBasket.Contracts.Roles
{
    public class UpdateRoleRequest
    {
        [Required]
        [Length(3, 50)]
        public string Name { get; set; } = string.Empty;
 
    }
}
