using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.UserControls;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for MemberMainWindow.xaml
    /// </summary>
    public partial class MemberMainWindow : Window
    {
        private readonly Login _login;
        public MemberMainWindow(Login loginWindow)
        {
            _login = loginWindow;
            // Set the list as the current DataContext  
            InitializeComponent();
            FrontPageGrid.Children.Add(new FrontPage());
            MembersGrid.Children.Add(new Members());
            EventsGrid.Children.Add(new EventsAdmin());
            BoatsGrid.Children.Add(new Boats());
            Closing += Window_Closing;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentWindow != TopMenuTabControl.SelectedIndex)
            {
                CurrentWindow = TopMenuTabControl.SelectedIndex;

                FrontPageGrid.Children.Clear();
                FrontPageGrid.Children.Add(new FrontPage());

                BoatsGrid.Children.Clear();
                BoatsGrid.Children.Add(new Boats());
            }
        }

        private int CurrentWindow { get; set; }
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
