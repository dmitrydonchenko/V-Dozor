using DozorDatabaseLib;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dozor_live.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        #region Fields

        private ICommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private static ApplicationViewModel instance;

        #endregion

        public static ApplicationViewModel AppContext
        {
            get
            {
                if(instance == null)
                {
                    instance = new ApplicationViewModel();
                }
                return instance;
            }
        }

        private ApplicationViewModel()
        {          
            // Add available pages
            PageViewModels.Add(new DefaultViewModel());

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        #region Properties / Commands

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    if (!PageViewModels.Contains(value))
                        PageViewModels.Add(value);

                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            //if (!PageViewModels.Contains(viewModel))
            //   PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        #endregion
    }
}
