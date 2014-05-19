using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for CreatCrewWindow.xaml
    /// </summary>
    public partial class CreateCrewWindow : Window, INotifyPropertyChanged
    {
        private readonly ISailClubMemberDal sailClubMemberDal = DalLocator.SailClubMemberDal;
        public ICollection<Person> CrewList = new List<Person>();

        private ICollectionView _dataGridCollection;
        private string _filterString;

        public CreateCrewWindow(ICollection<Person> CrewList)
        {
            this.InitializeComponent();

            this.CrewList = CrewList;

            this.CurrentCrewDataGrid.ItemsSource = this.CrewList;

            this.DataGridCollection = CollectionViewSource.GetDefaultView(this.sailClubMemberDal.GetAll());
            this.DataGridCollection.Filter = this.Filter;
        }

        public ICollectionView DataGridCollection
        {
            get { return this._dataGridCollection; }
            set
            {
                this._dataGridCollection = value;
                this.NotifyPropertyChanged("DataGridCollection");
            }
        }

        public string FilterString
        {
            get { return this._filterString; }
            set
            {
                this._filterString = value;
                this.NotifyPropertyChanged("FilterString");
                this.FilterCollection();
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void RefreshDatagrid(DataGrid Grid, ICollection<Person> list)

        {
            Grid.ItemsSource = null;
            Grid.ItemsSource = list;
        }

        private void NotifyPropertyChanged(string property)
        {
            if (this.PropertyChanged != null) { this.PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }

        private void FilterCollection()
        {
            if (this._dataGridCollection != null) { this._dataGridCollection.Refresh(); }
        }

        public bool Filter(object obj)
        {
            var data = obj as SailClubMember;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(this._filterString))
                {
                    // Sanitise input to lower
                    string lower = this._filterString.ToLower();

                    if (this.CrewList.Contains(data)) { return false; }

                    // Check if either of the data points for the members match the filterstring
                    if (data.FirstName != null) { if (data.FirstName.ToLower().Contains(lower)) { return true; } }

                    if (data.LastName != null) { if (data.LastName.ToLower().Contains(lower)) { return true; } }

                    if (data.SailClubMemberId.ToString().Contains(lower)) { return true; }

                    // If none succeeds return false
                    return false;
                }
                return true;
            }
            return false;
        }

        private void SearchBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { this.MemberDataGrid.Focus(); }
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPerson = (SailClubMember) this.MemberDataGrid.SelectedItem;

            if (
                this.CrewList.Where(x => x is SailClubMember)
                    .Cast<SailClubMember>()
                    .All(x => x.SailClubMemberId != currentPerson.SailClubMemberId)) {
                        this.CrewList.Add(currentPerson);
                    }

            this.DataGridCollection.Filter = this.Filter;
            this.RefreshDatagrid(this.CurrentCrewDataGrid, this.CrewList);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) { this.Close(); }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPerson = (Person) this.CurrentCrewDataGrid.SelectedItem;

            this.CrewList.Remove(currentPerson);

            this.RefreshDatagrid(this.CurrentCrewDataGrid, this.CrewList);
        }

        private void AddGuestButton_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(this.FirstNameBox.Text, "^[A-ZÆØÅa-zæøå ]*$")
                && this.FirstNameBox.Text.Trim() != String.Empty)
            {
                if (Regex.IsMatch(this.LastNameBox.Text, "^[A-ZÆØÅa-zæøå ]*$")
                    && this.LastNameBox.Text.Trim() != String.Empty)
                {
                    var p = new Person();
                    p.FirstName = this.FirstNameBox.Text;
                    p.LastName = this.LastNameBox.Text;
                    p.BoatDriver = this.IsBoatDriver.IsChecked.GetValueOrDefault();
                    this.CrewList.Add(p);

                    this.RefreshDatagrid(this.CurrentCrewDataGrid, this.CrewList);

                    this.FirstNameBox.Clear();
                    this.LastNameBox.Clear();
                    this.IsBoatDriver.IsChecked = false;
                }
            }
            else
            {
                MessageBox.Show("Ugyldigt navn. \nPrøv venligst igen");
            }
        }

        private void resultDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.AddButton_OnClick(sender, e);
        }

        private void removeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                var currentPerson = (Person) this.CurrentCrewDataGrid.SelectedItem;

                this.CrewList.Remove(currentPerson);

                this.RefreshDatagrid(this.CurrentCrewDataGrid, this.CrewList);
            }
        }

        private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.SearchBox.Text = String.Empty;

            /*Resfreshes the Datagrid when starting a new search */
            this.FilterString = String.Empty;
        }
    }
}
