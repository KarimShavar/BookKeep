using BookKeep.Data;
using BookKeep.Models;
using BookKeep.ViewModels.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BookKeep.ViewModels
{
    public class MyLibraryViewModel: BaseViewModel
    {
        public RelayCommand<BookModel> DeleteBookCommand { get; private set; }
        private ObservableCollection<BookModel> _libraryCollection;
        public ObservableCollection<BookModel> LibraryCollection
        {
            get => _libraryCollection;
            set
            {
                _libraryCollection = value;
                OnPropertyChanged();
            }
        }
        public MyLibraryViewModel()
        {
            LibraryCollection = new ObservableCollection<BookModel>(GetLibrary());
            DeleteBookCommand = new RelayCommand<BookModel>(OnDelete);
        }

        public override void OnDelete(BookModel book)
        {
            base.OnDelete(book);
            LibraryCollection.Remove(book);
        }

        // Todo - Think how to avoid calling db when viewModel initialise / stop initializing viewmodel.
        private List<BookModel> GetLibrary()
        {
            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                return LibraryDb.GetAllBooksRead();
            }
        }
    }
}
