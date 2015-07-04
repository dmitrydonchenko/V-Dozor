using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dozor_live.ViewModels
{
    class DefaultViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Поднесите карту к любому считывателю";
            }
        }
    }
}
