using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
