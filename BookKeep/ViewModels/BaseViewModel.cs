using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookKeep.Annotations;
using BookKeep.Data;
using BookKeep.Helpers.Interfaces;
using BookKeep.Models;

namespace BookKeep.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BookModel> _libraryCollection;
        private ObservableCollection<BookModel> _wishlistCollection;
        private ObservableCollection<BookModel> _searchResults;

        public BookLibraryDB LibraryDb { get; set; }

        public ObservableCollection<BookModel> LibraryCollection
        {
            get { return _libraryCollection; }
            set
            {
                _libraryCollection = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BookModel> WishlistCollection
        {
            get { return _wishlistCollection; }
            set
            {
                _wishlistCollection = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BookModel> SearchResults
        {
            get { return _searchResults; }
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
