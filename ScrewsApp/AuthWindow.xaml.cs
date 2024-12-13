using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ScrewsApp.Class;

namespace ScrewsApp
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public AuthWindow()
        {
            InitializeComponent();
            App.AuthWindow = this;
            this.timer.Interval = TimeSpan.FromMinutes(1);
            this.timer.Tick += TokenRefreshTimer_Tick;
        }
        private async void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;

            if (login == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }

            await LoginUser(login, password);
        }
        private async Task LoginUser(string login, string password)
        {
            try
            {
                var loginData = new
                {
                    email = login,
                    password = password
                };

                using (HttpClient client = new HttpClient())
                {
                    var message = await client.PostAsJsonAsync($"https://localhost:7156/api/Auth/Login", loginData);
                    if (message.IsSuccessStatusCode)
                    {
                        var content = await message.Content.ReadAsStringAsync();
                        var token = JsonSerializer.Deserialize<Token>(content);

                        Properties.Settings.Default.AuthToken = token?.token;
                        Properties.Settings.Default.TokenEndTime = DateTime.Now.AddHours(1);
                        Properties.Settings.Default.Save();

                        this.timer.Start();

                        if(App.MainWindow == null)
                        {
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Hide();
                        }
                    }
                    else if (message.StatusCode.ToString() == "NotFound")
                        MessageBox.Show("Такого пользователя не существует");
                    else if (message.StatusCode.ToString() == "BadRequest")
                        MessageBox.Show("Неверные данные");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonReg_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegWindow regWindow = new RegWindow();
            regWindow.Show();
        }
        private async void TokenRefreshTimer_Tick(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.TokenEndTime <= DateTime.Now)
            {
                await LoginUser(textBoxLogin.Text,textBoxPassword.Password);
                MessageBox.Show("Токен обновлен");
            }
        }
    }
}
