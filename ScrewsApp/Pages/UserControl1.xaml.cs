using ScrewsApp.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для UserControl.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private User user;
        public UserControl1(User user)
        {
            InitializeComponent();
            SetValues(user);
            this.user = user;
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow updateWindow = new UpdateWindow(this.user, false);
            updateWindow.ShowDialog();
        }
        private void SetValues(User user)
        {
            labelId.Content = $"Id: {user.Id}";
            labelName.Content = $"Name: {user.Name}";
            labelEmail.Content = $"Email: {user.Email}";

            switch (user.Role)
            {
                case 1:
                    labelRole.Content = $"Роль: Администратор";
                    break;
                case 2:
                    labelRole.Content = $"Роль: Менеджер";
                    break;
                case 3:
                    labelRole.Content = $"Роль: Пользователь";
                    break;
            }
        }
    }
}
