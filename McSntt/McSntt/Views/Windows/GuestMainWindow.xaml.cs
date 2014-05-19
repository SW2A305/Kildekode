using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using McSntt.Views.UserControls;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        private readonly Login _login;

        public GuestMainWindow(Login loginWindow)
        {
            this._login = loginWindow;
            // Set the list as the current DataContext  
            this.InitializeComponent();
            this.EventsGrid.Children.Add(new EventsAdmin());
            this.BoatsGrid.Children.Add(new Boats());
            this.Closing += this.Window_Closing;
        }

        private int CurrentWindow { get; set; }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CurrentWindow != this.TopMenuTabControl.SelectedIndex)
            {
                this.CurrentWindow = this.TopMenuTabControl.SelectedIndex;

                this.BoatsGrid.Children.Clear();
                this.BoatsGrid.Children.Add(new Boats());
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this._login.IsVisible != true) { Application.Current.Shutdown(); }
        }

        private void LogOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            this._login.Show();
            this._login.UsernameBox.Text = String.Empty;
            this._login.PasswordBox.Password = String.Empty;
            this._login.StatusTextBlock.Text = String.Empty;
            this.Close();
        }
    }
}
