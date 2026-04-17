namespace AccountService.Handlers.Links
{
    public class OrderLink : IOrderLink
    {
        public async Task<bool?> IsUserInUse(int idUser, string? token)
        {
            using HttpClient httpClient = new HttpClient();
            Uri url = new($"https://localhost:7150/api/orders/user/{idUser}");

            if (token != null)
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            bool? isInUse;
            if (response.IsSuccessStatusCode)
            {
                isInUse = await response.Content.ReadFromJsonAsync<bool>();

            }
            else
                isInUse = null;


            return isInUse;
        }
    }
}

