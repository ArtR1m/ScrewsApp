using ScrewsApp.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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

namespace ScrewsApp
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
            App.RegWindow = this;
        }

        private async void buttonReg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string name = textBoxName.Text;
            string password = textBoxPassword.Password;

            if(login == string.Empty || name == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }

            await RegUser(login, name, password);
        }

        private async Task RegUser(string login,string name,string password)
        {
            try
            {
                var regData = new
                {
                    name = name,
                    email = login,
                    password = password
                };

                using(HttpClient  client = new HttpClient())
                {
                    var message = await client.PostAsJsonAsync($"https://localhost:7156/api/Auth/Register", regData);

                    if(message.IsSuccessStatusCode)
                    {
                        var content = await message.Content.ReadAsStringAsync();
                        var loginResponse = JsonSerializer.Deserialize<Token>(content);

                        Properties.Settings.Default.AuthToken = loginResponse?.token;
                        Properties.Settings.Default.TokenEndTime = DateTime.Now.AddHours(1);
                        Properties.Settings.Default.Save();

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Hide();
                    }
                    else if (message.StatusCode.ToString() == "BadRequest")
                        MessageBox.Show("Этот Email уже занят");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(App.AuthWindow != null)
                App.AuthWindow.Show();
        }
    }
}
