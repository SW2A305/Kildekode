using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using McSntt.DataAbstractionLayer.Sqlite;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DatabaseManager.InitializeDatabase();
        }
    }
}
