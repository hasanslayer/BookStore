using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Html_Helper;
using BookStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
            {
                new Book() {ISBN = 1, Title = "Book1"},
                new Book() {ISBN = 2, Title = "Book2"},
                new Book() {ISBN = 3, Title = "Book3"},
                new Book() {ISBN = 4, Title = "Book4"},
                new Book() {ISBN = 5, Title = "Book5"}
            });

            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            IEnumerable<Book> resultBooks = (IEnumerable<Book>)controller.List(1).Model;

            //Assert
            Book[] bookArray = resultBooks.ToArray();
            Assert.IsTrue(bookArray.Length == 3);
            Assert.AreEqual(bookArray[0].Title, "Book1");
            Assert.AreEqual(bookArray[1].Title, "Book2");
            Assert.AreEqual(bookArray[2].Title, "Book3");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 14,
                ItemsPerPage = 5
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            String expectedResult = "<a class=\"btn btn-default\" href=\"Page1\">1</a>"
                                   + "<a class=\"btn btn-default btn-primary selected\" href=\"Page2\">2</a>"
                                   + "<a class=\"btn btn-default\" href=\"Page3\">3</a>";

            //Act
            String result = myHelper.PageLinks(pagingInfo, pageUrlDelegate).ToString();

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
