using ScrewsApp.Class;
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

namespace ScrewsApp
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private User user;
        private bool isProfile;
        public UpdateWindow(User user,bool isProfile)
        {
            InitializeComponent();
            this.user = user;
            this.isProfile = isProfile;
            SetValues(user.Role);
            SetComboBoxStatus(isProfile);
        }

        private async void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string name = textBoxName.Text;
            string password = textBoxPassword.Password;

            if (login == string.Empty || name == string.Empty || comboBoxRole.SelectedItem == null)
            {
                MessageBox.Show("Все поля кроме пароля обязаны быть заполнены");
                return;
            }
            if(await User.UpdateUser(new User(this.user.Id, name, login, password, comboBoxRole.SelectedIndex + 1)))
            {
                if(isProfile)
                    App.MainWindow.SetProfilePage();
                else
                    App.MainWindow.SetUsersPage();
                this.Close();
            }
        }
        private void SetValues(int idRole)
        {
            textBoxLogin.Text = user.Email;
            textBoxName.Text = user.Name;

            comboBoxRole.Items.Add("Администратор");
            comboBoxRole.Items.Add("Менеджер");
            comboBoxRole.Items.Add("Пользователь");

            switch(idRole)
            {
                case 1:
                    comboBoxRole.SelectedIndex = 0;
                    comboBoxRole.IsEnabled = false;
                    break;
                case 2:
                    comboBoxRole.SelectedIndex = 1;
                    break;
                case 3:
                    comboBoxRole.SelectedIndex = 2;
                    break;
            }
        }
        private void SetComboBoxStatus(bool isProfile)
        {
            string token = Properties.Settings.Default.AuthToken;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var jwtRole = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            int idRole = Convert.ToInt32(jwtRole);

            if (isProfile)
                idRole = 3;

            switch(idRole)
            {
                case 2:
                    comboBoxRole.IsEnabled = false;
                    break;
                case 3:
                    comboBoxRole.IsEnabled = false;
                    comboBoxRole.Opacity = 0;
                    labelRole.Opacity = 0;
                    break;
            }
        }
    }
}
