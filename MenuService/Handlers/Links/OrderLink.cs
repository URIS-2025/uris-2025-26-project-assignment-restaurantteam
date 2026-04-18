namespace MenuService.Handlers.Links
{
    public class OrderLink : IOrderLink
    {
        private readonly string hostUrl = "https://orderservice-40ju.onrender.com/api/orders/items/";
        private readonly string localUrl = "https://localhost:7150/api/orders/items/";
        public async Task<bool?> IsMenuItemInUse(int idMenuItem, string? token)
        {
            using HttpClient httpClient = new HttpClient();
            Uri url = new($"{hostUrl}{idMenuItem}");

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
