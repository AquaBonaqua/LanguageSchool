using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LanguageSchool.Classes;
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
        private IList<Client> myList = AppData.Ent.Client.ToList();
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

            UpdateTable();
        }


        public void FindInTable()
        {
            if (TbFind.Text == String.Empty)
            {
                UpdateTable();
            }

            else
            {
                ClientsGrid.ItemsSource = myList.Where(x => x.FirstName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) || x.LastName.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) || x.Patronymic.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) || x.Email.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase) || x.Phone.Contains(TbFind.Text, StringComparison.OrdinalIgnoreCase));
            }

        }


        public void UpdateTable()
        {
            var firstTable =
                PagedTable.SetPaging(myList,
                    numberOfRecPerPage); //Fill a DataTable with the First set based on the numberOfRecPerPage
            ClientsGrid.ItemsSource = firstTable.DefaultView; //Fill the dataGrid with the DataTable created previously

            if (NumberOfRecords.SelectedIndex == 3)
            {
                if (CmbGender.SelectedIndex != 2)

                {
                    ClientsGrid.ItemsSource = myList.Where(x => x.Gender.Name == (string) CmbGender.SelectedItem);
                    PageInfo.Content = "Показано " + myList.Count + " записей";
                }
                else
                {
                    ClientsGrid.ItemsSource = myList;
                    PageInfo.Content = "Показано " + myList.Count + " записей";
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
            FindInTable();
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