﻿

namespace SurvayBasket.Controllers
{
    [Route("errors/{Code:int}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error(int Code) => NotFound(new APIErrorResponse(Code, "Error was occured"));



    }
}
