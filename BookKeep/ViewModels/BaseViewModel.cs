using BookKeep.Data;
using BookKeep.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookKeep.Properties;

namespace BookKeep.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public BookLibraryDb LibraryDb { get; set; }

        public virtual void OnDelete(BookModel book)
        {
            if (book == null) return;

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                LibraryDb.DeleteBook(book.BookId);
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
