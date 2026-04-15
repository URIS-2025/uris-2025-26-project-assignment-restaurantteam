using Newtonsoft.Json;
using OrderService.DTO.User;

namespace OrderService.Handlers.Links
{
    public class UserLink : IUserLink
    {

        public UserLink()
        {

        }
        public Task<bool> CheckUserId(int idUser)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetUserById(int idUser, string? token)
        {
            using HttpClient httpClient = new HttpClient();
            Uri url = new($"https://localhost:7276/api/users/{idUser}");

            if (token != null)
                httpClient.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            UserDTO? userDTO;
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                userDTO = JsonConvert.DeserializeObject<UserDTO>(responseContent);
            }
            else
                userDTO = null;


            if (userDTO == null)
                throw new Exception("User not found HERE!");

            return userDTO;
        }
    }
}
