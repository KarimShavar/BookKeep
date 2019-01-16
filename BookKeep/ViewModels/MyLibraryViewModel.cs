using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookKeep.Data;
using BookKeep.Models;

namespace BookKeep.ViewModels
{
    public class MyLibraryViewModel: BaseViewModel
    {
        public MyLibraryViewModel()
        {
            LibraryCollection = new ObservableCollection<BookModel>(GetLibrary());
        }

        private List<BookModel> GetLibrary()
        {
            using (var context = new BookContext())
            {
                LibraryDb = new BookLibraryDB(context);
                return LibraryDb.GetAllBooksRead();
            }
        }
    }
}
