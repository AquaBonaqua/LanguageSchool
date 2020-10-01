using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LanguageSchool.Classes;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var index = int.Parse(((Button) e.Source).Uid);

            GridCursor.Margin = new Thickness(5 + 198 * index, 0, 0, 0);

            switch (index)
            {
                case 0:
                    AppData.Frame.Navigate(new PageClients());
                    break;

                case 1:
                    GridMain.Background = Brushes.Beige;
                    break;

                case 2:
                    GridMain.Background = Brushes.CadetBlue;
                    break;

                case 3:
                    GridMain.Background = Brushes.DarkBlue;
                    break;

                case 4:
                    GridMain.Background = Brushes.Firebrick;
                    break;

                case 5:
                    GridMain.Background = Brushes.Gainsboro;
                    break;

                case 6:
                    GridMain.Background = Brushes.HotPink;
                    break;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}