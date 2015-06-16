using ARDevGroup.VotumHardware.Core;
using ARDevGroup.VotumHardware.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorUsbLib
{
    /// <summary>
    /// Singleton class for RFID cards reading 
    /// </summary>
    public class RfidReader 
    {
        private static RfidReader instance;
        private HardwareViewModel rcon = HardwareViewModel.HardwareController;

        /// <summary>
        /// Rfid update event
        /// </summary>
        public event EventHandler<String> Rfid_Updated;

        private RfidReader()
        {
            rcon = HardwareViewModel.HardwareController;

            //Command's waiting counter 0.2 sec * 5 = 1 sec. 
            //By default = 5
            ARDevGroup.VotumHardware.Core.UsbDeviceReciever.HidWaitMaxCounter = 5;

            //New interactive mode
            rcon.DevicesList.HidReadKind = ARDevGroup.VotumHardware.Core.UsbDeviceReciever.HidReadKindEnum.InteractiveMode;
            rcon.DevicesList.OpenDevices();
            foreach (DeviceRecieverBase device in rcon.DevicesList.DevicesList)
            {
                device.DataRecieved += new EventHandler<DeviceDataArgs>(DevicesList_DataRecieved);
            }
            //rcon.DevicesList.DataRecieved += new EventHandler<DeviceDataArgs>(DevicesList_DataRecieved);
        }        

        /// <summary>
        /// Returns an instance of the RfidReader class(creates new instance if it doesn't already exist)
        /// </summary>
        public static RfidReader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RfidReader();
                }
                return instance;
            }
        }

        /// <summary>
        /// Closes all devices
        /// </summary>
        public void CloseDevices()
        {
            rcon.DevicesList.CloseDevices();
        }

        protected virtual void onRfidUpdated(String rfid)
        {
            if(Rfid_Updated != null)
            {
                Rfid_Updated(this, rfid);
            }                
        }

        private void DevicesList_DataRecieved(object sender, DeviceDataArgs e)
        {
            onRfidUpdated(e.Data.ButtonsCodes);
        }
    }
}
