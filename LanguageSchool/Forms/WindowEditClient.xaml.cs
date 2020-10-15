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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LanguageSchool.Classes;

namespace LanguageSchool.Forms
{
    /// <summary>
    /// Логика взаимодействия для WindowClient.xaml
    /// </summary>
    public partial class WindowClient : Window
    {
        Client editClient;


        public WindowClient(Client client)
        {
            InitializeComponent();

            editClient = client;
            CmbGender.ItemsSource = AppData.Ent.Gender.ToList();
            CmbGender.SelectedValuePath = "ID";
            CmbGender.DisplayMemberPath = "Name";

            LastName.Text = client.LastName;
            FirstName.Text = client.FirstName;
            Patronymic.Text = client.Patronymic;
            Email.Text = client.Email;
            PhoneNumber.Text = client.Phone;
            Birthday.SelectedDate = client.Birthday;
            CmbGender.SelectedValue = client.Gender.ID;
        

            Photo.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(client.PhotoPath);


        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            editClient.LastName = LastName.Text;
            editClient.FirstName = FirstName.Text;
            editClient.Patronymic = Patronymic.Text;
            editClient.Email = Email.Text;
            editClient.Phone = PhoneNumber.Text;
            editClient.Birthday = Birthday.SelectedDate;
            editClient.Gender.ID = (int)CmbGender.SelectedValue;

            AppData.Ent.SaveChanges();
            MessageBox.Show("Успешно сохранено");
            AppData.Frame.Refresh();
            this.Close();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
