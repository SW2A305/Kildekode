using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using McSntt.DataAbstractionLayer.Sqlite;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DatabaseManager.InitializeDatabase();

            var boatDal = new BoatSqliteDal();
            var boats = boatDal.GetAll();

            foreach (var boat in boats)
            {
                MessageBox.Show("Boat: " + boat.NickName + " (" + boat.BoatId + ")");
            }

            this.Shutdown();
        }
    }
}
