

namespace SurvayBasket.Controllers
{
    [Route("errors/{Code:int}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error(int Code) => Problem(statusCode:Code,detail:"There is a Problem");



    }
}
