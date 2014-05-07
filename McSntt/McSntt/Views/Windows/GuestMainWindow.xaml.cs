﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.UserControls;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        private readonly Login _login;
        public GuestMainWindow(Login loginWindow)
        {
            _login = loginWindow;
            // Set the list as the current DataContext  
            InitializeComponent();
            EventsGrid.Children.Add(new EventsAdmin());
            BoatsGrid.Children.Add(new Boats());
            Closing += Window_Closing;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_login.IsVisible != true)
                Application.Current.Shutdown();
        }

        private void LogOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            _login.Show();
            _login.UsernameBox.Text = String.Empty;
            _login.PasswordBox.Password = String.Empty;
            _login.StatusTextBlock.Text = String.Empty;
            this.Close();
        }
    }
}