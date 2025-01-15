using System.Text.Json;
using finance.Models;
namespace finance.Services
{
    public class UserService
    {

        private const string UserFilePath = "Data/users.json";

        public async Task<List<User>> GetUsersAsync()
        {
            if (!File.Exists(UserFilePath))
                throw new FileNotFoundException("User data file not found.");

            var json = await File.ReadAllTextAsync(UserFilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var users = await GetUsersAsync();
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }



    }
}