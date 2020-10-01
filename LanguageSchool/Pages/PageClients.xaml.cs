using LanguageSchool.Classes;
using PagingWPFDataGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

namespace LanguageSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageClients.xaml
    /// </summary>
    public partial class PageClients : Page
    {



   

        private int numberOfRecPerPage;
        static Paging PagedTable = new Paging();
        IList<Client> myList = AppData.Ent.Client.ToList();


        public PageClients()
        {
            InitializeComponent();

            PagedTable.PageIndex = 1; //Sets the Initial Index to a default value

            int[] RecordsToShow = { 10, 50, 200 }; //This Array can be any number groups



            foreach (int RecordGroup in RecordsToShow)
            {
                NumberOfRecords.Items.Add(RecordGroup); //Fill the ComboBox with the Array
            }

            NumberOfRecords.Items.Add("All");

            NumberOfRecords.SelectedItem = "All"; //Initialize the ComboBox

            string[] GendersToShow = { "Male", "Female", "All" }; //This Array can be any number groups

            foreach (string GenderGroup in GendersToShow)
            {
                CmbGender.Items.Add(GenderGroup); //Fill the ComboBox with the Array
            }
            
            CmbGender.SelectedItem = "All";
            
            UpdateTable();
        }



        public void UpdateTable()
        {
            DataTable firstTable = PagedTable.SetPaging(myList, numberOfRecPerPage); //Fill a DataTable with the First set based on the numberOfRecPerPage
            ClientsGrid.ItemsSource = firstTable.DefaultView; //Fill the dataGrid with the DataTable created previously

            if ((NumberOfRecords.SelectedItem == "All") && (CmbGender.SelectedItem == "All"))
            {
                ClientsGrid.ItemsSource = myList;
                PageInfo.Content = "Показано " + myList.Count + " записей";
            }

            else
            {
                numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
                if ((CmbGender.SelectedIndex == 0) || (CmbGender.SelectedIndex == 1))
                {
                    myList = AppData.Ent.Client.Where(x => x.GenderCode == CmbGender.SelectedIndex.ToString()).ToList();
                    ClientsGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
                    PageInfo.Content = PageNumberDisplay();

                }
                else
                {
                    ClientsGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
                    PageInfo.Content = PageNumberDisplay();
                }
          
             
            }

           
        }

        public string PageNumberDisplay()
        {
            int PagedNumber = numberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > myList.Count)
            {
                PagedNumber = myList.Count;
            }
            return "Показано " + PagedNumber + " записей из " + myList.Count; //This dramatically reduced the number of times I had to write this string statement
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
    }
}
