using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDbManagement.ViewModels
{
    public class DefaultViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Выберите действие в меню";
            }
        }
    }
}
