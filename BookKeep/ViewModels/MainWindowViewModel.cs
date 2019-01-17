using BookKeep.ViewModels.Commands;
using System;

namespace BookKeep.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        private readonly SearchViewModel _searchViewModel = new SearchViewModel();

        public RelayCommand<string> SearchCommand { get; private set; }
        public RelayCommand<string> NavigationCommand { get; private set; }

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
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
            CurrentViewModel = _searchViewModel;    // Making sure display changes on search
        }

        // Todo - Think of how to make this not new up every time command executes.
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
