﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AnnelidaLauncher.ViewModel;

namespace AnnelidaLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var vm = new AnnelidaLauncherViewModel();
            var v = new MainWindow();
            v.DataContext = vm;
            v.Show();
        }
    }
}
