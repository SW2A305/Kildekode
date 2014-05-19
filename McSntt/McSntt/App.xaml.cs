﻿using System;
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
            // TODO: Uncomment below if reverting to SQLite.
            //DatabaseManager.InitializeDatabase();

            // TODO: Comment out if reverting to SQLite
            if (!MockToSqlite.RestoreMockData()) { DbSeedData.CreateSeedData(); }
        }

        private void App_OnExit(object sender, ExitEventArgs e) { MockToSqlite.PersistMockData(); }
    }
}
