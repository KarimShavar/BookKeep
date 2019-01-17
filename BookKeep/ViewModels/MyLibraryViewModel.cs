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
        public MyLibraryViewModel()
        {
            LibraryCollection = new ObservableCollection<BookModel>(GetLibrary());
            DeleteBookCommand = new RelayCommand<BookModel>(OnDeleteFromLibrary);
        }

        private void OnDeleteFromLibrary(BookModel obj)
        {
            if (obj == null) return;

            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDb(context);
                LibraryDb.DeleteBook(obj.BookId);
            }

            LibraryCollection.Remove(obj);
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
