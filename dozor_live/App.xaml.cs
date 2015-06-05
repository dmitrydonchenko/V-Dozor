using dozor_live.Models;
using dozor_live.ViewModels;
using dozor_live.Views;
using DozorMediaLib.Video;
using DozorUsbLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace dozor_live
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private String currentRfid;
        private RfidReader rfidReader;
        WebcamCapture webcamCapture;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set startup Window
            MainWindow mainWindow = new MainWindow();
            ApplicationViewModel appContext = ApplicationViewModel.AppContext;
            mainWindow.DataContext = appContext;
            mainWindow.Show();

            // Set camera
            webcamCapture = new WebcamCapture();

            // Start Usb Service
            currentRfid = "";
            rfidReader = RfidReader.Instance;
            rfidReader.Rfid_Updated += new EventHandler<string>(RfidReceived);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            rfidReader.CloseDevices();
            base.OnExit(e);
        }

        private void RfidReceived(object sender, String rfid)
        {
            if (currentRfid != rfid)
            {
                currentRfid = rfid;                
                if(ApplicationViewModel.AppContext.CurrentPageViewModel.Name != "StudentsViewModel")
                {
                    StudentsViewModel svm = new StudentsViewModel(rfid, webcamCapture.Capture(), DateTime.Now);
                    ApplicationViewModel.AppContext.CurrentPageViewModel = svm;
                }
                else
                {
                    ((StudentsViewModel)ApplicationViewModel.AppContext.CurrentPageViewModel).AddNewStudent(rfid, webcamCapture.Capture(), DateTime.Now);
                }
            }
        }
    }
}
