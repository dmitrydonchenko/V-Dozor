using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorUsbLib
{
    public class RfidReaderEventArgs : EventArgs
    {
        public String Rfid { get; internal set; }
        public Int32 ReceiverId { get; internal set; } 

        public RfidReaderEventArgs(String rfid, int receiverId)
        {
            Rfid = rfid;
            ReceiverId = receiverId;
        }
    }
}
