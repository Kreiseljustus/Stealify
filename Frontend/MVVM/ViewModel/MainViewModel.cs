using Frontend.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand LikedSongsViewCommand { get; set; }
        public RelayCommand DownloadViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public LikedSongsViewModel LikedSongsVM { get; set; }
        public DownloadViewModel DownloadVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            LikedSongsVM = new LikedSongsViewModel();
            DownloadVM = new DownloadViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => {
                CurrentView = HomeVM;
            });

            LikedSongsViewCommand = new RelayCommand(o => {
                CurrentView = LikedSongsVM;
            });

            DownloadViewCommand = new RelayCommand(o =>
            {
                CurrentView = DownloadVM;
            });
        }
    }
}
