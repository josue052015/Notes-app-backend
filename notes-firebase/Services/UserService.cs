using notes_firebase.Models;
using System.Text.Json;
using System.Text;
using notes_firebase.DTOs;
using AutoMapper;

namespace notes_firebase.Services
{
    public class UserService
    {
        static string firebaseDatabaseUrl = "https://notes-19150-default-rtdb.firebaseio.com/";
        static string firebaseDatabaseDocument = "User";
        static readonly HttpClient client = new HttpClient();
        private readonly IMapper _mapper;

        public UserService(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public async Task<User> GetUserFromDB(string id)
        {
            string url = $"{firebaseDatabaseUrl}" +
                       $"{firebaseDatabaseDocument}/" +
                       $"{id}.json";

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content != null || content != "null")
                {
                    return JsonSerializer.Deserialize<User>(content);
                }
            }
            return null;
        }

        public async Task<UserDTO> GetUserById(string id)
        {
            var user = await GetUserFromDB(id);
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO> AddUser(User user)
        {
            user.Id = Guid.NewGuid();
            user.IsDeleted = false;
            string noteJsonString = JsonSerializer.Serialize(user);
            var payload = new StringContent(noteJsonString, Encoding.UTF8, "application/json");
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{user.Id}.json";
            var httpResponse = await client.PutAsync(url, payload);
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDTO>(content);
            }
            return null;
        }
        public async Task<UserDTO> EditUser(UserDTO user)
        {
            User dbUser = await GetUserFromDB(user.Id.ToString());
            _mapper.Map(user, dbUser);
            string noteJsonString = JsonSerializer.Serialize(dbUser);
            var payload = new StringContent(noteJsonString, Encoding.UTF8, "application/json");
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{user.Id}.json";
            var httpResponse = await client.PutAsync(url, payload);
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                User userInformation = JsonSerializer.Deserialize<User>(content);
                return _mapper.Map<UserDTO>(userInformation);
            }
            return null;
        }
        public async Task<string> DeleteUser(string id)
        {
            string url = $"{firebaseDatabaseUrl}" +
                     $"{firebaseDatabaseDocument}/" +
                     $"{id}.json";
            var httpResponse = await client.DeleteAsync(url);

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
