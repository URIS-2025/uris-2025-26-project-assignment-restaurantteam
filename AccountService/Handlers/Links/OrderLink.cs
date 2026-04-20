namespace AccountService.Handlers.Links
{
    public class OrderLink : IOrderLink
    {
        private readonly string hostUrl = "https://orderservice-40ju.onrender.com/api/orders/user/";
        private readonly string localUrl = "https://localhost:7150/api/orders/user/";
        public async Task<bool?> IsUserInUse(int idUser, string? token)
        {
            
            string stringUrl = hostUrl + idUser.ToString();

            using HttpClient httpClient = new HttpClient();
            Uri url = new(stringUrl);

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

