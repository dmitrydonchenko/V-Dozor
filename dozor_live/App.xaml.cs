using dozor_live.Models;
using dozor_live.ViewModels;
using dozor_live.Views;
using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorMediaLib.Video;
using DozorUsbLib;
using FirebirdSql.Data.FirebirdClient;
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

            // Create Database instance "D:\\Votum\\DozorDatabase\\Students.fdbB;"
            FbConnectionStringBuilder connectString = new FbConnectionStringBuilder();
            connectString.Database = "D:\\Votum\\DozorDatabase\\Students.fdb";
            connectString.Dialect = 3;
            connectString.UserID = "SYSDBA";
            connectString.Password = "masterkey";
            connectString.Charset = "win1251";
            DozorDatabase.CreateInstance(connectString.ConnectionString);

            // Set camera
            webcamCapture = new WebcamCapture();

            // Generate messages
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            String [] courses = new String [5]{ "Математика", "Русский язык", "Физика", "История", "Физ-ра" };

            DateTime now = DateTime.Now;
            var messages = dozorDatabase.GetMessagesByDate(now);
            Random rand = new Random();
            if(messages.Count() == 0)
            {
                var grades = dozorDatabase.GetAllGrades();
                foreach (Grade grade in grades)
                {
                    int index = rand.Next(0, 5);
                    int classroom = rand.Next(1, 100);                    
                    var students = dozorDatabase.GetStudentsByGrade(grade.ID);
                    foreach(Student student in students)
                    {
                        Message message = new Message();
                        message.MESSAGE_TEXT = "1-й урок - " + courses[index] + " в каб. №" + classroom.ToString();
                        message.DATETIME = DateTime.Now;
                        message.STUDENT_ID = student.ID;
                        message.MESSAGE_PRIORITY = 3;
                        dozorDatabase.InsertMessage(message);
                    }
                }
            }            

            // Start Usb Service
            currentRfid = "";
            rfidReader = RfidReader.Instance;
            rfidReader.Rfid_Updated += new EventHandler<RfidReaderEventArgs>(RfidReceived);

            // Set startup Window
            MainWindow mainWindow = new MainWindow();
            ApplicationViewModel appContext = ApplicationViewModel.AppContext;
            mainWindow.DataContext = appContext;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            rfidReader.CloseDevices();
            base.OnExit(e);
        }

        private void RfidReceived(object sender, RfidReaderEventArgs args)
        {
            if (currentRfid != args.Rfid)
            {
                currentRfid = args.Rfid;                
                if(ApplicationViewModel.AppContext.CurrentPageViewModel.Name != "StudentsViewModel")
                {
                    StudentsViewModel svm = new StudentsViewModel(args, webcamCapture.Capture(), DateTime.Now);
                    ApplicationViewModel.AppContext.CurrentPageViewModel = svm;
                }
                else
                {
                    ((StudentsViewModel)ApplicationViewModel.AppContext.CurrentPageViewModel).AddNewStudent(args, webcamCapture.Capture(), DateTime.Now);
                }
            }
        }
    }
}
