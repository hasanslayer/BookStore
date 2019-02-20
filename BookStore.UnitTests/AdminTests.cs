using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]
            {
                new Book() {ISBN = 1,Title = "Book1"},
                new Book() {ISBN = 2,Title = "Book2"},
                new Book() {ISBN = 3,Title = "Book3"},
                new Book() {ISBN = 4,Title = "Book4"},
            });
            AdminController target = new AdminController(mock.Object);

            //Act
            Book[] result = ((IEnumerable<Book>)target.Index().ViewData.Model).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 4);
            Assert.AreEqual("Book1", result[0].Title);
            Assert.AreEqual("Book2", result[1].Title);
            Assert.AreEqual("Book3", result[2].Title);
            Assert.AreEqual("Book4", result[3].Title);
        }

        [TestMethod]
        public void Can_Edit_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
            {
                new Book() {ISBN = 1, Title = "Book1"},
                new Book() {ISBN = 2, Title = "Book2"},
                new Book() {ISBN = 3, Title = "Book3"},
            });

            AdminController target = new AdminController(mock.Object);

            //Act
            Book book1 = target.Edit(1).ViewData.Model as Book;
            Book book2 = target.Edit(2).ViewData.Model as Book;
            Book book3 = target.Edit(3).ViewData.Model as Book;

            //Assert
            Assert.AreEqual("Book1", book1.Title);
            Assert.AreEqual(2, book2.ISBN);
            Assert.AreEqual("Book3", book3.Title);
        }

        [TestMethod]
        public void Cannot_Edit_NonExist_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
            {
                new Book() {ISBN = 1, Title = "Book1"},
                new Book() {ISBN = 2, Title = "Book2"},
                new Book() {ISBN = 3, Title = "Book3"},
            });

            AdminController target = new AdminController(mock.Object);

            //Act
            Book book4 = target.Edit(4).ViewData.Model as Book;

            //Assert
            Assert.IsNull(book4);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book() { Title = "Test Book" };

            //Act
            ActionResult result = target.Edit(book);

            //Assert
            mock.Verify(b => b.SaveBook(book));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Save_Invalid_Changes()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book { Title = "Test Book" };
            target.ModelState.AddModelError("error", "error");

            //Act
            ActionResult result = target.Edit(book);

            //Assert
            mock.Verify(b => b.SaveBook(It.IsAny<Book>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_valid_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            Book book1 = new Book() { ISBN = 1, Title = "Test book 1" };
            mock.Setup(b => b.Books).Returns(new Book[]
            {
                new Book() {ISBN = 2,Title = "Test book 2"},
                new Book() {ISBN = 3,Title = "Test book 3"},
                book1
            });
            AdminController target = new AdminController(mock.Object);

            //Act
            target.Delete(book1.ISBN);

            //Assert
            mock.Verify(b => b.DeleteBook(book1.ISBN));
        }
    }
}
