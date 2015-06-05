using dozor_live.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dozor_live.Models
{
    class MessageModel : ViewModelBase
    {
        #region properties

        private String messageText;
        public String MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                base.RaisePropertyChanged("MessageText");
            }
        }        

        #endregion

        #region constructor

        public MessageModel(String _MessageText)
        {
            MessageText = _MessageText;
        }
        #endregion
    }
}
