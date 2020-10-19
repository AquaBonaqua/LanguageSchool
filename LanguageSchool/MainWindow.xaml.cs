using System.Windows;
using System.Windows.Input;
using LanguageSchool.Classes;
using LanguageSchool.Forms;
using LanguageSchool.Pages;

namespace LanguageSchool
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppData.Frame = MainFrame;
            AppData.Ent = new wsr_user_2Entities();
            AppData.Frame.Navigate(new PageClients());
        }


        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var isWindowOpen = false;

            foreach (Window w in Application.Current.Windows)
                if (w is WindowClient)
                {
                    isWindowOpen = true;
                    w.Activate();
                }

            if (!isWindowOpen) Application.Current.Shutdown();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}