using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BookKeep.ViewModels.Commands;

namespace BookKeep.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        private SearchViewModel _searchViewModel = new SearchViewModel();

        public RelayCommand<string> SearchCommand { get; private set; }
        public RelayCommand<string> NavigationCommand { get; private set; }

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainWindowViewModel()
        {
            NavigationCommand = new RelayCommand<string>(OnNav);
            SearchCommand = new RelayCommand<string>(OnSearch);
        }

        private async void OnSearch(string obj)
        {
            await _searchViewModel.SearchBooksAsync(obj);
            CurrentViewModel = _searchViewModel;
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "library":
                    CurrentViewModel = new MyLibraryViewModel();
                    break;
                case "wishlist":
                    CurrentViewModel = new WishlistViewModel();
                    break;
                default:
                    throw new NullReferenceException("Invalid viewModel");
                    
            }
        }
    }
}
