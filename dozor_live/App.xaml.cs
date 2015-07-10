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
using System.IO;
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

        private DateTime lastReadTime;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create Database instance "D:\\Votum\\DozorDatabase\\Students.fdbB;"
            string appDataFolder = Environment.GetFolderPath
                      (Environment.SpecialFolder.CommonApplicationData);
            string dvAppDataFolder = appDataFolder + @"\DozorDatabase";
            AppDomain.CurrentDomain.SetData("DataDirectory", dvAppDataFolder);

            if (!Directory.Exists(dvAppDataFolder))
                Directory.CreateDirectory(dvAppDataFolder);
            String databasePath = dvAppDataFolder + @"\Students.fdb";
            FbConnectionStringBuilder connectString = new FbConnectionStringBuilder();
            connectString.Database = databasePath;
            //connectString.Database = "D:\\Votum\\DozorDatabase\\Students.fdb";
            connectString.Dialect = 3;
            connectString.UserID = "SYSDBA";
            connectString.Password = "masterkey";
            connectString.Charset = "win1251";
            DozorDatabase.CreateInstance(connectString.ConnectionString);  

            // Generate messages
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            String [] courses = new String [5]{ "Математика", "Русский язык", "Физика", "История", "Физ-ра" };

            DateTime now = DateTime.Now;
            // Delete old messages
            var messages = dozorDatabase.GetAllMessages();
            if (messages == null)
                return;
            foreach (Message message in messages)
            {
                if (message.EXPIRATION_DATETIME < DateTime.Now)
                {
                    dozorDatabase.DeleteMessageById(message.ID);
                }
            }
            messages = dozorDatabase.GetMessagesByDate(now);
            Boolean hasLessonMessage = false;
            foreach(Message message in messages)
            {
                if(message.MESSAGE_PRIORITY == 3)
                {
                    hasLessonMessage = true;
                    break;
                }
            }
            Random rand = new Random();
            if(messages.Count() == 0 || !hasLessonMessage)
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
                        DateTime expirationDateTime = DateTime.Now;
                        expirationDateTime.AddHours(23 - expirationDateTime.Hour);
                        expirationDateTime.AddMinutes(59 - expirationDateTime.Minute);
                        message.EXPIRATION_DATETIME = expirationDateTime;
                        message.STUDENT_ID = student.ID;
                        message.MESSAGE_PRIORITY = 3;
                        message.MESSAGE_SHOW_DIRECTION = 1;
                        dozorDatabase.InsertMessage(message);
                    }
                }
            }

            // Set camera
            webcamCapture = new WebcamCapture();

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
            if(currentRfid == args.Rfid)
            {
                if((DateTime.Now - lastReadTime).TotalSeconds < 3)
                {
                    return;
                }
            }
            currentRfid = args.Rfid;
            lastReadTime = DateTime.Now;
            if (ApplicationViewModel.AppContext.CurrentPageViewModel.Name != "StudentsViewModel")
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
