using Newtonsoft.Json;
using OrderService.DTO.MenuItem;

namespace OrderService.Handlers.Links
{
    public class MenuItemLink : IMenuItemLink
    {
        private readonly string hostUrl = "https://uris2026restaurant.onrender.com/api/menu/";
        private readonly string localUrl = "https://localhost:7278/api/menu/";
        public async Task<MenuItemDTO> GetMenuItemById(int idMenuItem, string? token)
        {
            string stringUrl = hostUrl + idMenuItem.ToString();
            using HttpClient httpClient = new HttpClient();
            Uri url = new(stringUrl);

            if (token != null)
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            MenuItemDTO? menuItemDTO;
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                menuItemDTO = JsonConvert.DeserializeObject<MenuItemDTO>(responseContent);
            }
            else
                menuItemDTO = null;


            if (menuItemDTO == null)
                throw new Exception("Menu item not found!");

            return menuItemDTO;
        }
    }
}
