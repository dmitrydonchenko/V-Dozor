using DozorDbManagement.ViewModels;
using DozorUsbLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DozorDbManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set startup Window
            MainWindow mainWindow = new MainWindow();
            ApplicationViewModel appContext = ApplicationViewModel.AppContext;
            mainWindow.DataContext = appContext;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            RfidReader rfidReader = RfidReader.Instance;
            rfidReader.CloseDevices();
            base.OnExit(e);
        }
    }
}
