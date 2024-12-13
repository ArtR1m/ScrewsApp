using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using ScrewsApp.Class;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScrewsApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User user = new User();
        public ProfilePage()
        {
            InitializeComponent();
            SetValues();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow updateWindow = new UpdateWindow(this.user,true);
            updateWindow.ShowDialog();
        }
        public async void SetValues()
        {
            string token = Properties.Settings.Default.AuthToken;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var jwtId = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            
            User user = await User.GetUser(Convert.ToInt32(jwtId));
            this.user = user;

            labelId.Content = $"Id: {user.Id.ToString()}";
            labelName.Content = $"Name: {user.Name}";
            labelEmail.Content = $"Email: {user.Email}";
        }

    }
}
