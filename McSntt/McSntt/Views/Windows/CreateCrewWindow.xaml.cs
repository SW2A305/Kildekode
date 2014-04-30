using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreatCrewWindow.xaml
    /// </summary>
    public partial class CreateCrewWindow : Window , INotifyPropertyChanged
    {
        public List<Person> _crewList = new List<Person>(); 


        public CreateCrewWindow()
        {
            InitializeComponent();

            using (var db = new McSntttContext())
            {
                db.SailClubMembers.Load();
                DataGridCollection = CollectionViewSource.GetDefaultView(db.SailClubMembers.Local);
                DataGridCollection.Filter = new Predicate<object>(Filter);

            }
        }

        public CreateCrewWindow(List<Person> CrewList )
        {
            InitializeComponent();
            _crewList = CrewList;

            CurrentCrewDataGrid.ItemsSource = _crewList;

            using (var db = new McSntttContext())
            {
                db.SailClubMembers.Load();
                DataGridCollection = CollectionViewSource.GetDefaultView(db.SailClubMembers.Local);
                DataGridCollection.Filter = new Predicate<object>(Filter);
                SearchBox.Text = "Søg efter medlemmer her";

            }
        }

        private void RefreshDatagrid(DataGrid Grid, List<Person> list )
        {
            Grid.ItemsSource = null;
            Grid.ItemsSource = list;
        }

        private ICollectionView _dataGridCollection;
        private string _filterString;

        public ICollectionView DataGridCollection
        {
            get { return _dataGridCollection; }
            set { _dataGridCollection = value; NotifyPropertyChanged("DataGridCollection"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string FilterString
        {
            get
            {
                return _filterString;
            }
            set
            {
                _filterString = value;
                NotifyPropertyChanged("FilterString");
                FilterCollection();
            }
        }

        private void FilterCollection()
        {
            if (_dataGridCollection != null)
            {
                _dataGridCollection.Refresh();
            }
        }
        public bool Filter(object obj)
        {
            var data = obj as SailClubMember;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(_filterString))
                {
                    // Sanitise input to lower
                    var lower = _filterString.ToLower();

                    // Check if either of the data points for the members match the filterstring
                    if (data.FirstName != null)
                        if (data.FirstName.ToLower().Contains(lower))
                            return true;

                    if (data.LastName != null)
                        if (data.LastName.ToLower().Contains(lower))
                            return true;

                    if (data.MemberId.ToString().Contains(lower))
                        return true;

                    // If none succeeds return false
                    return false;
                }
                return true;
            }
            return false;
        }

        private void SearchBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.MemberDataGrid.Focus();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {

            Person currentPerson = (Person) MemberDataGrid.SelectedItem;
            if (!_crewList.Contains(currentPerson))
            {
                _crewList.Add(currentPerson);
            }

            RefreshDatagrid(CurrentCrewDataGrid, _crewList);
        }

       private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            CreateCrewWindowName.Close();
        }

       private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
       {
           Person currentPerson = (Person)CurrentCrewDataGrid.SelectedItem;

           _crewList.Remove(currentPerson);

           RefreshDatagrid(CurrentCrewDataGrid, _crewList);
       }

       private void AddGuestButton_Click(object sender, RoutedEventArgs e)
       {

           if (Regex.IsMatch(FirstNameBox.Text, "^[A-ZÆØÅa-zæøå]*$") && FirstNameBox.Text != String.Empty)
           {
               if (Regex.IsMatch(LastNameBox.Text, "^[A-ZÆØÅa-zæøå]*$") && LastNameBox.Text != String.Empty)
               {
                   var p = new Person();
                   p.FirstName = FirstNameBox.Text;
                   p.LastName = LastNameBox.Text;
                   _crewList.Add(p);

                   RefreshDatagrid(CurrentCrewDataGrid, _crewList);

                   FirstNameBox.Clear();
                   LastNameBox.Clear();
               }  
           }
           else
           {
               MessageBox.Show("Ugyldigt navn. \nPrøv venligst igen");
           }
       }

        private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = String.Empty;

            /*Resfreshes the Datagrid when starting a new search */
            FilterString = String.Empty;
        }
    }
}
