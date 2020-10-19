using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LanguageSchool.Classes;

namespace LanguageSchool.Pages
{
    /// <summary>
    ///     Логика взаимодействия для PageServices.xaml
    /// </summary>
    public partial class PageServices : Page
    {
        public int clientid;

        public PageServices(Client client)
        {
            InitializeComponent();

            clientid = client.ID;
            NameLabel.Content = string.Format(client.LastName + " " + client.FirstName + " " + client.Patronymic);
            foreach (MainWindow window in Application.Current.Windows) window.TBTitle.Text = Title;


            UpdateTable();
        }


        public void UpdateTable()
        {
            ServicesGrid.ItemsSource = AppData.Ent.ClientService.Where(x => x.ClientID == clientid).ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AppData.Frame.GoBack();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private void BtnFiles_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}