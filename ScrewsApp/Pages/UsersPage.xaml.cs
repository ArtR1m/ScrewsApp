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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            SetListView();
        }
        public async void SetListView()
        {
            List<User> users = await User.GetAllUsers();
            listViewUsers.Items.Clear();
            foreach(User user in users)
            {
                listViewUsers.Items.Add(new UserControl1(user));
            }
        }
    }
}
