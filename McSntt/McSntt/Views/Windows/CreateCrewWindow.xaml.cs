using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for CreatCrewWindow.xaml
    /// </summary>
    public partial class CreateCrewWindow : Window, INotifyPropertyChanged
    {
        public IList<Person> CrewList = new List<Person>();

        private ICollectionView _dataGridCollection;
        private string _filterString;

        public CreateCrewWindow(IList<Person> CrewList)
        {
            InitializeComponent();

            this.CrewList = CrewList;

            CurrentCrewDataGrid.ItemsSource = this.CrewList;

            var db = new SailClubMemberEfDal();
            DataGridCollection = CollectionViewSource.GetDefaultView(db.GetAll());
            DataGridCollection.Filter = Filter;
        }

        public ICollectionView DataGridCollection
        {
            get { return _dataGridCollection; }
            set
            {
                _dataGridCollection = value;
                NotifyPropertyChanged("DataGridCollection");
            }
        }

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                NotifyPropertyChanged("FilterString");
                FilterCollection();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RefreshDatagrid(DataGrid Grid, IList<Person> list)

        {
            Grid.ItemsSource = null;
            Grid.ItemsSource = list;
        }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
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
                    string lower = _filterString.ToLower();

                    if (CrewList.Contains(data))
                        return false;

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
                MemberDataGrid.Focus();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPerson = (SailClubMember) MemberDataGrid.SelectedItem;

            if (
                CrewList.Where(x => x is SailClubMember)
                    .Cast<SailClubMember>()
                    .All(x => x.MemberId != currentPerson.MemberId))
                CrewList.Add(currentPerson);

            DataGridCollection.Filter = Filter; 
            RefreshDatagrid(CurrentCrewDataGrid, CrewList);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            CreateCrewWindowName.Close();
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPerson = (Person) CurrentCrewDataGrid.SelectedItem;

            CrewList.Remove(currentPerson);

            RefreshDatagrid(CurrentCrewDataGrid, CrewList);
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
                    CrewList.Add(p);

                    RefreshDatagrid(CurrentCrewDataGrid, CrewList);

                    FirstNameBox.Clear();
                    LastNameBox.Clear();
                }
            }
            else
            {
                MessageBox.Show("Ugyldigt navn. \nPrøv venligst igen");
            }
        }

        private void resultDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddButton_OnClick(sender, e);
        }

        private void removeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                var currentPerson = (Person) CurrentCrewDataGrid.SelectedItem;

                CrewList.Remove(currentPerson);

                RefreshDatagrid(CurrentCrewDataGrid, CrewList);
            }
        }
    }
}