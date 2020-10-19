using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LanguageSchool.Classes;
using LanguageSchool.Forms;
using PagingWPFDataGrid;

namespace LanguageSchool.Pages

{
    
    /// <summary>
    ///     Логика взаимодействия для PageClients.xaml
    /// </summary>
    /// 
    public partial class PageClients : Page
    {
        private static readonly Paging PagedTable = new Paging();
        private IList<Client> myList;
        private int numberOfRecPerPage;


        public PageClients()
        {
            InitializeComponent();

            

            PagedTable.PageIndex = 1; //Sets the Initial Index to a default value

            int[] recordsToShow = {10, 50, 200}; //This Array can be any number groups

            foreach (var recordGroup in recordsToShow)
                NumberOfRecords.Items.Add(recordGroup); //Fill the ComboBox with the Array

            NumberOfRecords.Items.Add("Все");

            NumberOfRecords.SelectedItem = "Все"; //Initialize the ComboBox

            string[] gendersToShow = {"Male", "Female", "Любой"}; //This Array can be any number groups

            foreach (var genderGroup in gendersToShow)
                CmbGender.Items.Add(genderGroup); //Fill the ComboBox with the Array

            CmbGender.SelectedItem = "Любой";

            foreach (MainWindow window in Application.Current.Windows) window.TBTitle.Text = Title;

            UpdateTable();

           

        }


        public void FindInTable()
        {
            if (TbFind.Text == string.Empty)
                UpdateTable();

            else
                ClientsGrid.ItemsSource = myList.Where(x =>
                    x.FirstName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                    x.LastName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                    x.Patronymic.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                    x.Email.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                    x.Phone.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase));
        }


        public void UpdateTable()
        {
            myList = AppData.Ent.Client.ToList();

            var firstTable =
                PagedTable.SetPaging(myList,
                    numberOfRecPerPage); //Fill a DataTable with the First set based on the numberOfRecPerPage
            ClientsGrid.ItemsSource = firstTable.DefaultView; //Fill the dataGrid with the DataTable created previously



            if (NumberOfRecords.SelectedIndex == 3)
            {
                if (CmbGender.SelectedIndex != 2)

                {
                    if (TbFind.Text == string.Empty)
                    {
                        ClientsGrid.ItemsSource = myList.Where(x => x.Gender.Name == (string)CmbGender.SelectedItem);
                        PageInfo.Content = "Показано " + myList.Count + " записей";
                    }

                    else
                    {
                        ClientsGrid.ItemsSource = myList.Where(x => x.Gender.Name == (string)CmbGender.SelectedItem).Where(x =>
                            x.FirstName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.LastName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.Patronymic.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.Email.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.Phone.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase)); ;
                        PageInfo.Content = "Показано " + myList.Count + " записей";

                      
                    }


                  
                }
                else
                {
                    if (TbFind.Text == string.Empty)
                    {
                        ClientsGrid.ItemsSource = myList;
                        PageInfo.Content = "Показано " + myList.Count + " записей";
                    }

                    else
                    {
                        ClientsGrid.ItemsSource = myList.Where(x =>
                            x.FirstName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.LastName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.Patronymic.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.Email.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) ||
                            x.Phone.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase));
                        PageInfo.Content = "Показано " + myList.Count + " записей";


                    }


                    
                }
            }
            else
            {
                if (CmbGender.SelectedIndex == 2)

                {
                    
                    numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
                    myList = AppData.Ent.Client.ToList();
                    ClientsGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
                    PageInfo.Content = PageNumberDisplay();
                }
                else
                {
                    numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
                    myList = AppData.Ent.Client.Where(x => x.Gender.Name == (string) CmbGender.SelectedItem).ToList();
                    ClientsGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
                    PageInfo.Content = PageNumberDisplay();
                }
            }
        }

        public string PageNumberDisplay()
        {
            var pagedNumber = numberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (pagedNumber > myList.Count) pagedNumber = myList.Count;
            return
                "Показано " + pagedNumber + " записей из " +
                myList.Count; //This dramatically reduced the number of times I had to write this string statement
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            ClientsGrid.ItemsSource = PagedTable.Next(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Backwards_Click(object sender, RoutedEventArgs e)
        {
            ClientsGrid.ItemsSource = PagedTable.Previous(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTable();
        }

        private void CmbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTable();
        }

        private void TbFind_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTable();
        }

        private void CheckBirthDay_OnChecked(object sender, RoutedEventArgs e)
        {
            ClientsGrid.ItemsSource = AppData.Ent.Client.Where(x => x.Birthday.Value.Month == DateTime.Now.Month).ToList();
        }

        private void CheckBirthDay_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }


        private void BtnEditClient_OnClick(object sender, RoutedEventArgs e)
        {
            var isWindowOpen = false;

            foreach (Window w in Application.Current.Windows)
                if (w is WindowClient)
                {
                    isWindowOpen = true;
                    w.Activate();
                }

            if (!isWindowOpen)
            {
                var windowClient = new WindowClient((sender as Button).DataContext as Client);

                windowClient.Owner = Application.Current.MainWindow;
                windowClient.Show();
            }


           
        }

        private void BtnDeleteClient_OnClick(object sender, RoutedEventArgs e)
        {
            var client = (sender as Button).DataContext as Client;
           
            if (client.ClientService.Count > 0)
            {
                MessageBox.Show("Вы не можете удалить клиента с посещениями");
            }

            else
            {
                var s1 = string.Format(@"Удалить клиента {0} {1} {2} и всю информацию о его тегах?", client.LastName,client.FirstName,client.Patronymic);

                var result = MessageBox.Show(s1,
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    AppData.Ent.Client.Remove(client);
                    AppData.Ent.SaveChanges();
                    MessageBox.Show(string.Format(@"Клиент {0} {1} {2} и  информация о его тегах удалены", client.LastName, client.FirstName, client.Patronymic));
                    UpdateTable();


                }
            }
            
        }


        private void AddClient_OnClick(object sender, RoutedEventArgs e)
        {
            var windowClient = new WindowClient();

            windowClient.Owner = Application.Current.MainWindow;
            windowClient.Show();
        }

        private void BtnService_OnClick(object sender, RoutedEventArgs e)
        {
            AppData.Frame.Navigate(new PageServices((sender as Button).DataContext as Client));

           
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }



}