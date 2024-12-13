using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ScrewsApp.Pages;

namespace ScrewsApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.MainWindow = this;
            SetProfilePage();
            SetValues();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (App.RegWindow != null)
                App.RegWindow.Close();
            App.AuthWindow.Close();
        }
        private void SetValues()
        {
            string token = Properties.Settings.Default.AuthToken;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var jwtId = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var jwtRole = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

            string role = "";

            switch (jwtRole)
            {
                case "1":
                    role = "Администратор";
                    break;
                case "2":
                    role = "Менеджер";
                    break;
                case "3":
                    role = "Пользователь";
                    buttonUsers.IsEnabled = false;
                    buttonUsers.Opacity = 0;
                    break;
            }

            labelId.Content = $"ID: {jwtId}";
            labelRole.Content = $"Роль: {role}";
        }
        public void SetProfilePage()
        {
            MainFrame.Content = new ProfilePage();
            buttonProfile.IsEnabled = false;
            buttonUsers.IsEnabled = true;
        }
        public void SetUsersPage()
        {
            MainFrame.Content = new UsersPage();
            buttonProfile.IsEnabled = true;
            buttonUsers.IsEnabled = false;
        }

        private void buttonUsers_Click(object sender, RoutedEventArgs e)
        {
            SetUsersPage();
        }

        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            SetProfilePage();
        }
    }
}
