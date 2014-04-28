﻿using System;
using System.Collections.Generic;
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
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class studyTeacher : UserControl
    {
        public studyTeacher()
        {
            InitializeComponent();
            editTeamGrid.IsEnabled = false;
            teamDropdown.ItemsSource = ab;
            teamDropdown.DisplayMemberPath = "Name";
            teamDropdown.SelectedValuePath = "Name";
        }

        // Test list undtil datebase implementation
        List<Team> ab = new List<Team>();
        
        
        


        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            var team = new Team { Name = teamName.Text };
            ab.Add(team);
            
        }

        private void editTeam_Checked(object sender, RoutedEventArgs e)
        {
            editTeamGrid.IsEnabled = true;
        }

        private void editTeam_Unchecked(object sender, RoutedEventArgs e)
        {
            editTeamGrid.IsEnabled = false;
        }

        private void newTeam_Click(object sender, RoutedEventArgs e)
        {
            teamName.Text = string.Empty;
            memberSearch.Text = string.Empty;
            level1.IsChecked = false;
            level2.IsChecked = false;
        }


        

    }
}
