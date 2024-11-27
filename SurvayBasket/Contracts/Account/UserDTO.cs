
using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts
{
    public class UserDTO
    {
      
        public string FirstName { get; set; } = string.Empty!;
     
        public string LastName { get; set; } = string.Empty!;

        public string Token { get; set; } = string.Empty!;
        public string Email { get; set; } = string.Empty!;



    }
}
