using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LanguageSchool.Classes;

namespace LanguageSchool.Forms
{
    /// <summary>
    ///     Логика взаимодействия для WindowClient.xaml
    /// </summary>
    public partial class WindowClient : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9+()-]+");
        private readonly Client editClient;

        public WindowClient(Client client)
        {
            InitializeComponent();
            TBPhoneNumber.PreviewTextInput += PhoneNumber_OnPreviewTextInput;
            NameLabel.Content = string.Format(client.LastName + " " + client.FirstName + " " + client.Patronymic);
            editClient = client;
            CmbGender.ItemsSource = AppData.Ent.Gender.ToList();
            CmbTags.ItemsSource = AppData.Ent.Tag.ToList();
            CmbTags.SelectedValuePath = "ID";
            CmbTags.DisplayMemberPath = "Title";

            CmbGender.SelectedValuePath = "ID";
            CmbGender.DisplayMemberPath = "Name";

            TBLastName.Text = client.LastName;
            TBFirstName.Text = client.FirstName;
            TBPatronymic.Text = client.Patronymic;
            TBEmail.Text = client.Email;
            TBPhoneNumber.Text = client.Phone;
            Birthday.SelectedDate = client.Birthday;
            CmbGender.SelectedValue = client.Gender.ID;


            Photo.Source = (ImageSource) new ImageSourceConverter().ConvertFromString(client.PhotoPath);
           
            UpdateTable();
        }

        public WindowClient()
        {
            InitializeComponent();
            TBPhoneNumber.PreviewTextInput += PhoneNumber_OnPreviewTextInput;
            CmbGender.ItemsSource = AppData.Ent.Gender.ToList();
            CmbGender.SelectedValuePath = "ID";
            CmbGender.DisplayMemberPath = "Name";
            ButtonAddTag.IsEnabled = false;
            BtnSave.Content = "Добавить";
        }

        public void UpdateTable()
        {
            TagsGrid.ItemsSource = AppData.Ent.TagOfClient.Where(x => x.ClientID == editClient.ID).Select(z => z.Tag).ToList();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (TBLastName.Text == "" || TBFirstName.Text == "" || TBPatronymic.Text == "" || TBEmail.Text == "" ||
                TBPhoneNumber.Text == "" || Birthday.SelectedDate == null || CmbGender.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля");
            }

            else
            {
                var sLastName = TBLastName.Text;
                var sFirstName = TBFirstName.Text;
                var sPatronymic = TBPatronymic.Text;

                var LastNameArray = sLastName.ToCharArray();
                var FirstNameArray = sFirstName.ToCharArray();
                var PatronymicArray = sPatronymic.ToCharArray();

                if ((LastNameArray.Length > 50) | (FirstNameArray.Length > 50) | (PatronymicArray.Length > 50))
                {
                    MessageBox.Show("Ошибка! Вы ввели в поля ФИО больше 50 символов!");
                    return;
                }

                var sym = "!@#$%^+()";

                if (sLastName.IndexOfAny(sym.ToCharArray()) > -1 || sLastName.IndexOfAny(sym.ToCharArray()) > -1 ||
                    sPatronymic.IndexOfAny(sym.ToCharArray()) > -1)
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
                    var s1 = "Сохранить изменения?";

                    var result = MessageBox.Show(s1,
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        editClient.LastName = TBLastName.Text;
                        editClient.FirstName = TBFirstName.Text;
                        editClient.Patronymic = TBPatronymic.Text;
                        editClient.Email = TBEmail.Text;
                        editClient.Phone = TBPhoneNumber.Text;
                        editClient.Birthday = Birthday.SelectedDate;
                        editClient.GenderCode = (int) CmbGender.SelectedValue;

                        AppData.Ent.SaveChanges();
                        MessageBox.Show("Изменения сохранены.");

                        Close();
                    }
                }

                else
                {
                    var s1 = "Добавить клиента?";

                    var result = MessageBox.Show(s1,
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var client = new Client
                        {
                            LastName = TBLastName.Text,
                            FirstName = TBFirstName.Text,
                            Patronymic = TBPatronymic.Text,
                            Email = TBEmail.Text,
                            Phone = TBPhoneNumber.Text,
                            Birthday = Birthday.SelectedDate,
                            GenderCode = (int) CmbGender.SelectedValue
                        };
                        AppData.Ent.Client.Add(client);
                        AppData.Ent.SaveChanges();
                        MessageBox.Show("Клиент добавлен.");
                    }
                }
            }
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        public void CloseWindow()
        {
            var s1 = "Закрыть окно? Все несохранённые данные будут утеряны";

            var result = MessageBox.Show(s1,
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) Close();
        }

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
            CloseWindow();
        }


        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnDeleteTag_OnClick(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button).DataContext as TagOfClient;

            var s1 = "Удалить тег?";

            var result = MessageBox.Show(s1,
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AppData.Ent.TagOfClient.Remove(tag);
                AppData.Ent.SaveChanges();
                MessageBox.Show("Тег удалён");
                UpdateTable();
            }
        }

        private void ButtonAddTag_Click(object sender, RoutedEventArgs e)
        {
            var s1 = "Добавить тег?";

            var result = MessageBox.Show(s1,
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var tagOfClient = new TagOfClient
                    {
                        ClientID = editClient.ID,
                        TagID = (int) CmbTags.SelectedValue
                    }
                    ;

                AppData.Ent.TagOfClient.Add(tagOfClient);

                try
                {
                    AppData.Ent.SaveChanges();
                    MessageBox.Show("Тег добавлен.");

                    UpdateTable();
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка");
                }
            }
        }
    }
}