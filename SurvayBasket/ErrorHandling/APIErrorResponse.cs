namespace SurvayBasket.ErrorHandling
{
    public class APIErrorResponse
    {
        public APIErrorResponse(int code, string? errorMessage = default)
        {
            Code = code;
            ErrorMessage = errorMessage ?? GetErrorMessage(code);
        }


        public int Code { get; set; }
        public string ErrorMessage { get; set; }

        private string GetErrorMessage(int code)
        {
            return code switch
            {
                400 => "BadRequest ",
                401 => "UnAuthorized ",
                404 => "Not Found ",
                500 => "Internal Server Error",
                _ => "Error has been occured"
            };
        }
    }
}
