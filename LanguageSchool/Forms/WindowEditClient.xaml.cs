using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
using LanguageSchool.Pages;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

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
            TBPhoneNumber.PreviewTextInput += new TextCompositionEventHandler(PhoneNumber_OnPreviewTextInput);

            editClient = client;
            CmbGender.ItemsSource = AppData.Ent.Gender.ToList();
            CmbGender.SelectedValuePath = "ID";
            CmbGender.DisplayMemberPath = "Name";

            TBLastName.Text = client.LastName;
            TBFirstName.Text = client.FirstName;
            TBPatronymic.Text = client.Patronymic;
            TBEmail.Text = client.Email;
            TBPhoneNumber.Text = client.Phone;
            Birthday.SelectedDate = client.Birthday;
            CmbGender.SelectedValue = client.Gender.ID;
        

            Photo.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(client.PhotoPath);


        }

        public WindowClient()
        {
            InitializeComponent();
            TBPhoneNumber.PreviewTextInput += new TextCompositionEventHandler(PhoneNumber_OnPreviewTextInput);
            CmbGender.ItemsSource = AppData.Ent.Gender.ToList();
            CmbGender.SelectedValuePath = "ID";
            CmbGender.DisplayMemberPath = "Name";
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {

            if (TBLastName.Text == "" || TBFirstName.Text == "" || TBPatronymic.Text == "" || TBEmail.Text == "" || TBPhoneNumber.Text == "" || Birthday.SelectedDate == null || CmbGender.SelectedItem == null )
            {
                MessageBox.Show("Заполните все поля");
            }

            else
            {
                string sLastName = TBLastName.Text;
                string sFirstName = TBFirstName.Text;
                string sPatronymic = TBPatronymic.Text;

                char[] LastNameArray = sLastName.ToCharArray();
                char[] FirstNameArray = sFirstName.ToCharArray();
                char[] PatronymicArray = sPatronymic.ToCharArray();

                if ((LastNameArray.Length > 50) | (FirstNameArray.Length > 50) | (PatronymicArray.Length > 50))
                {
                    MessageBox.Show("Ошибка! Вы ввели в поля ФИО больше 50 символов!");
                    return;
                }

                string sym = "!@#$%^+()";

                if (sLastName.IndexOfAny(sym.ToCharArray()) > -1 || sLastName.IndexOfAny(sym.ToCharArray()) > -1 || sPatronymic.IndexOfAny(sym.ToCharArray()) > -1)
                {
                    MessageBox.Show("В полях ФИО не допускается использовать символы!");
                    return;
                }

                try
                {

                    var eMailAddress = new MailAddress(TBEmail.Text);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Неверный формат Email");
                    return; // wrong format for email
                }


                if (editClient != null)
                {
                    editClient.LastName = TBLastName.Text;
                    editClient.FirstName = TBFirstName.Text;
                    editClient.Patronymic = TBPatronymic.Text;
                    editClient.Email = TBEmail.Text;
                    editClient.Phone = TBPhoneNumber.Text;
                    editClient.Birthday = Birthday.SelectedDate;
                    editClient.GenderCode = (int)CmbGender.SelectedValue;

                    AppData.Ent.SaveChanges();
                    MessageBox.Show("Успешно сохранено");
                    
                    this.Close();
                }

                else
                {
                    Client client = new Client()

                    {
                        LastName = TBLastName.Text,
                        FirstName = TBFirstName.Text,
                        Patronymic = TBPatronymic.Text,
                        Email = TBEmail.Text,
                        Phone = TBPhoneNumber.Text,
                        Birthday = Birthday.SelectedDate,
                        GenderCode = (int)CmbGender.SelectedValue,
                    };
                        AppData.Ent.Client.Add(client);
                        AppData.Ent.SaveChanges();
                        MessageBox.Show("Успешно сохранено");
                       
                        this.Close();
                    

                }

                
            }
            
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private static readonly Regex _regex = new Regex("[^0-9+()-]+");

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void PhoneNumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
           //WindowState = WindowState.Minimized;
        }

        private void BtnCloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


    
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
