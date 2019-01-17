using BookKeep.Data;
using BookKeep.Models;
using BookKeep.Tests.Data.AsyncMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeep.Tests.Data
{
    [TestClass()]
    public class BookLibraryDbTests
    {
        /// <summary>
        /// Test data
        /// </summary>
        public List<BookModel> Data = new List<BookModel>()
        {
            new BookModel()
            {
                BookId = 9999,
                Title = "Hobbit",
                Author = "J.R.R Tolkien",
                Description = "In a hole in the ground there lived a hobbit. " +
                              "Not a nasty, dirty, wet hole, filled with the ends of worms and an oozy smell, " +
                              "nor yet a dry, bare, sandy hole with nothing in it to sit down on or to eat: " +
                              "it was a hobbit-hole, and that means comfort.",
                ImageUrl = "https://images.gr-assets.com/books/1372847500l/5907.jpg",
                IsRead = true

            },
            new BookModel()
            {
                BookId = 10000,
                Title = "The Final Empire, Mistborn",
                Author = "Brandon Sanderson",
                Description = "In a world where ash falls from the sky, and mist dominates the night, an evil cloaks the land and stifles all life. " +
                              "The future of the empire rests on the shoulders of a troublemaker and his young apprentice. " +
                              "Together, can they fill the world with color once more?",
                ImageUrl = "https://images.gr-assets.com/books/1480717416l/68428.jpg",
                IsRead = false
            }
        };
    

        [TestCategory("AccessViaContext")]
        [TestMethod()]
        public void AddBook_SavesBook_ViaContextTest()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var mockContext = new Mock<BookContext>();
            var library = new BookLibraryDb(mockContext.Object);

            //--Act
            mockContext.Setup(m => m.Books).Returns(mockSet.Object);
            library.AddBook(new BookModel()
            {
                BookId = 10000,
                Title = "The Final Empire, Mistborn",
                Author = "Brandon Sanderson",
                Description = "In a world where ash falls from the sky, and mist dominates the night, an evil cloaks the land and stifles all life. " +
                              "The future of the empire rests on the shoulders of a troublemaker and his young apprentice. " +
                              "Together, can they fill the world with color once more?",
                ImageUrl = "https://images.gr-assets.com/books/1480717416l/68428.jpg",
                IsRead = false
            });

            //--Assert
            mockSet.Verify(m=> m.Add(It.IsAny<BookModel>()), Times.Once);
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);
        }

        [TestCategory("AccessViaContext")]
        [TestMethod]
        public void DeleteBook_DeletesBook_ViaContextTest()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var data = Data.AsQueryable();
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<BookContext>();
            var library = new BookLibraryDb(mockContext.Object);

            //--Act
            mockContext.Setup(m => m.Books).Returns(mockSet.Object);
            library.DeleteBook(9999);

            //--Assert
            mockSet.Verify(m=> m.Remove(It.IsAny<BookModel>()),Times.Once);
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);
        }

        [TestCategory("AccessViaContext")]
        [TestMethod]
        public void UpdateBook_UpdateBook_ViaContextTest()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var data = Data.AsQueryable();
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<BookContext>();
            var library = new BookLibraryDb(mockContext.Object);

            //--Act
            mockContext.Setup(m => m.Books).Returns(mockSet.Object);
            var updatedModel = (new BookModel()
            {
                BookId = 9999,
                Title = "Hobbit - Here and back",
                Author = "J.R.R Tolkien",
                Description = "In a hole in the ground there lived a hobbit. " +
                              "Not a nasty, dirty, wet hole, filled with the ends of worms and an oozy smell, " +
                              "nor yet a dry, bare, sandy hole with nothing in it to sit down on or to eat: " +
                              "it was a hobbit-hole, and that means comfort.",
                ImageUrl = "https://images.gr-assets.com/books/1372847500l/5907.jpg",
                IsRead = true

            });
            library.UpdateBook(updatedModel);

            //--Assert
            Assert.AreEqual(updatedModel.Title, mockSet.Object.FirstOrDefault(b=>b.BookId == updatedModel.BookId)?.Title);
        }

        [TestCategory("AccessViaContext")]
        [TestMethod]
        public void GetBook_ReturnsBookByBookId_ViaContextTests()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var data = Data.AsQueryable();
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<BookContext>();
            var library = new BookLibraryDb(mockContext.Object);

            var expected = Data[0]; // Hobbit

            //-- Act
            mockContext.Setup(m => m.Books).Returns(mockSet.Object);
            var actual = library.GetBook(9999);

            //--Assert
            Assert.AreEqual(expected.Author, actual.Author);
        }

        [TestCategory("AccessViaContext")]
        [TestMethod]
        public void GetAllBooksRead_ReturnsAllBooks_IsRead()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var dataAsQueryable = Data.AsQueryable();
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Provider).Returns(dataAsQueryable.Provider);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Expression).Returns(dataAsQueryable.Expression);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.ElementType).Returns(dataAsQueryable.ElementType);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.GetEnumerator()).Returns(dataAsQueryable.GetEnumerator());
            var mockContext = new Mock<BookContext>();
            var library = new BookLibraryDb(mockContext.Object);

            //-- Act
            mockContext.Setup(m => m.Books).Returns(mockSet.Object);
            var actual = library.GetAllBooksRead();

            //--Assert
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Count == 1);
        }

        [TestCategory("AccessViaContext")]
        [TestMethod]
        public void GetAllBooksUnRead_ReturnsAllBooks_NotRead()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var dataAsQueryable = Data.AsQueryable();
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Provider).Returns(dataAsQueryable.Provider);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Expression).Returns(dataAsQueryable.Expression);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.ElementType).Returns(dataAsQueryable.ElementType);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.GetEnumerator()).Returns(dataAsQueryable.GetEnumerator());
            var mockContext = new Mock<BookContext>();
            var library = new BookLibraryDb(mockContext.Object);

            //-- Act
            mockContext.Setup(m => m.Books).Returns(mockSet.Object);
            var actual = library.GetAllBooksRead();

            //--Assert
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Count == 1);
        }

        [TestCategory("AccessViaContext")]
        [TestMethod]
        public async Task GetAllBooksAsync_ReturnsAllBooksAsyncTest()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var dataAsQueryable = Data.AsQueryable();

            mockSet.As<IDbAsyncEnumerable<BookModel>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<BookModel>(dataAsQueryable.GetEnumerator()));

            mockSet.As<IQueryable<BookModel>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<BookModel>(dataAsQueryable.Provider));

            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Expression).Returns(dataAsQueryable.Expression);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.ElementType).Returns(dataAsQueryable.ElementType);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.GetEnumerator()).Returns(dataAsQueryable.GetEnumerator());

            var mockContext = new Mock<BookContext>();
            mockContext.Setup(c => c.Books).Returns(mockSet.Object);

            //--Act
            var library = new BookLibraryDb(mockContext.Object);
            var books = await library.GetAllBooksAsync();

            //--Assert
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("J.R.R Tolkien", books[0].Author);
            Assert.AreEqual("Brandon Sanderson", books[1].Author);
        }

        [TestCategory("AccessViaContext")]
        [TestMethod]
        public void MarkBookAsRead_MarkAsReadByBookId_ViaContextTest()
        {
            //--Arrange
            var mockSet = new Mock<DbSet<BookModel>>();
            var data = Data.AsQueryable();
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<BookModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<BookContext>();
            var library = new BookLibraryDb(mockContext.Object);


            //-- Act
            mockContext.Setup(m => m.Books).Returns(mockSet.Object);
            library.MarkBookAsRead(9999);

            //--Assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsTrue(library.GetBook(9999).IsRead);
        }

    }
}