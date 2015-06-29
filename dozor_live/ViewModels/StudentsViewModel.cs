using dozor_live.Models;
using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorMediaLib.Video;
using DozorUsbLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dozor_live.ViewModels
{
    class StudentsViewModel : ViewModelBase, IPageViewModel
    {
        #region Properties

        public string Name
        {
            get
            {
                return "StudentsViewModel";
            }
        }

        private StudentModel[] students;
        public StudentModel[] Students
        {
            get
            {
                return students;
            }
            set
            {
                if (students != value)
                {
                    students = value;
                    RaisePropertyChanged("Students");
                }
            }
        }

        private StudentModel student1;
        public StudentModel Student1
        {
            get
            {
                return student1;
            }
            set
            {
                if (student1 != value)
                {
                    student1 = value;
                    RaisePropertyChanged("Student1");
                }
            }
        }

        private StudentModel student2;
        public StudentModel Student2
        {
            get
            {
                return student2;
            }
            set
            {
                if (student2 != value)
                {
                    student2 = value;
                    RaisePropertyChanged("Student2");
                }
            }
        }

        private BitmapImage snapshot1;
        public BitmapImage Snapshot1
        {
            get { return snapshot1; }
            set
            {
                snapshot1 = value;
                base.RaisePropertyChanged("Snapshot1");
            }
        }

        private BitmapImage snapshot2;
        public BitmapImage Snapshot2
        {
            get { return snapshot2; }
            set
            {
                snapshot2 = value;
                base.RaisePropertyChanged("Snapshot2");
            }
        }

        private ICommand _goBackCommand;

        private DateTime currentDateTime;
        private Student currentStudent;

        private Dictionary<Int32, Int32> receivers;

        #endregion

        public StudentsViewModel(RfidReaderEventArgs args, Bitmap snapshot, DateTime dateTime)
        {
            Students = new StudentModel[2];
            receivers = new Dictionary<int, int>();
            AddNewStudent(args, snapshot, dateTime);                  
        }        

        public void AddNewStudent(RfidReaderEventArgs args, Bitmap snapshot, DateTime dateTime)
        {
            if(!receivers.ContainsKey(args.ReceiverId))
                receivers[args.ReceiverId] = receivers.Count;
            DozorDatabase dozorDatabase = DozorDatabase.Instance;

            // Getting student from db
            currentStudent = dozorDatabase.GetStudentByRfid(args.Rfid);
            BitmapImage bitmapSource = ConvertToBitmapImage(snapshot);
            bitmapSource.Freeze();            
            currentDateTime = dateTime;

            // Create and insert attendance to db
            Attendance attendance = new Attendance();
            attendance.DATETIME = DateTime.Now;
            attendance.STUDENT_ID = currentStudent.ID;
            attendance.SNAPSHOT = WebcamCapture.ImageToByteArray(snapshot);

            // define direction
            IEnumerable<Attendance> todayAttendancies = dozorDatabase.GetStudentAttendanciesByDate(currentStudent.ID, dateTime);
            if(todayAttendancies.Count() % 2 == 0)
            {
                attendance.IS_IN = true;
            }
            else
            {
                attendance.IS_IN = false;
            }            
            
            dozorDatabase.InsertAttendance(attendance);
            SetStudents(attendance.IS_IN, bitmapSource, receivers[args.ReceiverId]);
        }

        private void SetStudents(Boolean isIn, BitmapImage bitmapSource, int studentIndex)
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;

            int curStudentIndex;
            curStudentIndex = studentIndex;
            if (studentIndex == 0)
                Snapshot1 = bitmapSource;
            else
                Snapshot2 = bitmapSource;

            // Setting StudentModel           
            Students[curStudentIndex] = new StudentModel(currentStudent.FIRST_NAME);
            Students[curStudentIndex].AttendanceDateTime = currentDateTime;
            Students[curStudentIndex].AttendanceIsIn = isIn;
            IEnumerable<Message> studentMessages = dozorDatabase.GetStudentMessages(currentStudent.ID, currentStudent.GRADE_ID);

            // Setting MessagesModel
            List<MessageModel> messagesModel = new List<MessageModel>();
            foreach (Message m in studentMessages)
            {
                if((m.MESSAGE_SHOW_DIRECTION == 1 && Students[curStudentIndex].AttendanceIsIn) || 
                   (m.MESSAGE_SHOW_DIRECTION == 2 && !Students[curStudentIndex].AttendanceIsIn) ||
                    m.MESSAGE_SHOW_DIRECTION == 0)
                {
                    MessageModel curMessage = new MessageModel(m.MESSAGE_TEXT, m.MESSAGE_PRIORITY);
                    messagesModel.Add(curMessage);
                }        
            }
            messagesModel.Sort((x, y) => Math.Min(x.MessagePriority, y.MessagePriority));
            Students[curStudentIndex].Messages = messagesModel;            

            Student1 = Students[0];
            Student2 = Students[1];
        }

        private BitmapImage ConvertToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private BitmapSource loadBitmap(Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return bs;
        }

        #region commands

        public void GoToDefaultView()
        {
            DefaultViewModel dvm = new DefaultViewModel();
            ApplicationViewModel.AppContext.CurrentPageViewModel = dvm;
        }

        public ICommand GoToDefaultViewCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(
                        param => GoToDefaultView()
                    );
                }
                return _goBackCommand;
            }
        }

        #endregion
    }
}
