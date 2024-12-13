using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace ScrewsApp.Class
{
    public class User
    {
        public User(int id,string name,string email,string password,int role)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.Role = role;
        }
        public User()
        {

        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int Role { get; set; }

        public static async Task<User> GetUser(int id)
        {
            string token = Properties.Settings.Default.AuthToken;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var message = await client.GetAsync($"https://localhost:7156/api/Users/GetUser?id={id}");
                    if (message.IsSuccessStatusCode)
                    {
                        var user = await message.Content.ReadFromJsonAsync<User>();
                        if(user != null) 
                            return user;
                    }
                    else
                        MessageBox.Show(message.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new User();
        }
        public static async Task<Boolean> UpdateUser(User user)
        {
            string token = Properties.Settings.Default.AuthToken;

            var updateData = new
            {
                name = user.Name,
                email = user.Email,
                password = user.Password,
                role = user.Role,
            };
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var message = await client.PutAsJsonAsync($"https://localhost:7156/api/Users/UpdateUser?id={user.Id}",updateData);
                    if (message.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Успешное редактирование");
                        return true;
                    }
                    else if (message.StatusCode.ToString() == "NotFound")
                        MessageBox.Show("Такого пользователя не существует");
                    else if (message.StatusCode.ToString() == "BadRequest")
                        MessageBox.Show("Этот email уже занят");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public static async Task<List<User>> GetAllUsers()
        {
            string token = Properties.Settings.Default.AuthToken;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var message = await client.GetAsync($"https://localhost:7156/api/Users/GetAllUsers");
                    if (message.IsSuccessStatusCode)
                    {
                        var users = await message.Content.ReadFromJsonAsync<List<User>>();
                        if (users != null)
                            return users;
                    }
                    else
                        MessageBox.Show(message.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new List<User>();
        }
    }
}
