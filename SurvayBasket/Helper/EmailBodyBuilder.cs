namespace SurvayBasket.Helper
{
    public static class EmailBodyBuilder
    {
        public static async Task<string> GenerateEmailBody(string Template, Dictionary<string, string> templateModel)
        {
            var tempPath = $"{Directory.GetCurrentDirectory()}/Templates/{Template}.html";
            var streamReader = new StreamReader(tempPath);
            var body = await streamReader.ReadToEndAsync();
            streamReader.Close();
            foreach (var item in templateModel)
                body = body.Replace(item.Key, item.Value);

            return body;


        }
    }
}
