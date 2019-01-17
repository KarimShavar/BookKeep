using BookKeep.Data;
using BookKeep.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookKeep.Properties;

namespace BookKeep.ViewModels
{
    /// <summary>
    /// Allows accessing Collections from base. Solves a lot of issues with switching views.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BookModel> _libraryCollection;
        private ObservableCollection<BookModel> _wishlistCollection;
        private ObservableCollection<BookModel> _searchResults;

        public BookLibraryDb LibraryDb { get; set; }

        public ObservableCollection<BookModel> LibraryCollection
        {
            get => _libraryCollection;
            set
            {
                _libraryCollection = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BookModel> WishlistCollection
        {
            get => _wishlistCollection;
            set
            {
                _wishlistCollection = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BookModel> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// That implementation allows using OnPropertyChanged in derived class without
        /// specifying name of object. DRY
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
