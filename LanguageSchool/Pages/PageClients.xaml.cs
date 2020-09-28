﻿using LanguageSchool.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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



        ObservableCollection<Client> clients = new ObservableCollection<Client>(AppData.Ent.Client);

        public PageClients()
        {
            InitializeComponent();


            UpdateTable();



        }



        public void UpdateTable()
        {


            ClientsGrid.ItemsSource = clients;
   

        }



        

}
}
