using System.Configuration;
using System.Data;
using System.Windows;

namespace ScrewsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AuthWindow AuthWindow { get;set; }
        public static MainWindow MainWindow { get;set; }
        public static RegWindow RegWindow { get;set; }
    }

}
