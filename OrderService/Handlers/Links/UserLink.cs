using Newtonsoft.Json;
using OrderService.DTO.User;

namespace OrderService.Handlers.Links
{
    public class UserLink : IUserLink
    {
        private readonly string hostUrl = "https://accountservice-py1t.onrender.com/api/users/";
        private readonly string localUrl = "https://localhost:7276/api/users/";
        public UserLink()
        {

        }
        public Task<bool> CheckUserId(int idUser)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetUserById(int idUser, string? token)
        {
            string stringUrl = hostUrl+idUser.ToString();
            using HttpClient httpClient = new HttpClient();
            Uri url = new(stringUrl);

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
