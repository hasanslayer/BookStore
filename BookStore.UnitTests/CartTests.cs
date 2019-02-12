using System;
using System.Linq;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Products()
        {

            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "ASP.NET" };
            Book book2 = new Book { ISBN = 2, Title = "Oracle" };

            //Act
            Cart cart = new Cart();
            cart.AddItem(book1);
            cart.AddItem(book2, 3);

            CartLine[] result = cart.CartLines.ToArray();

            //Assert
            Assert.AreEqual(result[0].Book, book1);
            Assert.AreEqual(result[1].Book, book2);



        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Products()
        {

            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "ASP.NET" };
            Book book2 = new Book { ISBN = 2, Title = "Oracle" };

            //Act
            Cart cart = new Cart();
            cart.AddItem(book1);
            cart.AddItem(book2, 3);
            cart.AddItem(book1, 5);

            CartLine[] result = cart.CartLines.OrderBy(cl => cl.Book.ISBN).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity, 3);
        }

        [TestMethod]
        public void Can_Remove_Product()
        {

            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "ASP.NET" };
            Book book2 = new Book { ISBN = 2, Title = "Oracle" };
            Book book3 = new Book { ISBN = 3, Title = "C#" };

            //Act
            Cart cart = new Cart();
            cart.AddItem(book1);
            cart.AddItem(book2, 3);
            cart.AddItem(book3, 5);
            cart.AddItem(book2, 1);

            cart.RemoveItem(book2);

            //Assert
            Assert.AreEqual(cart.CartLines.Count(cl => cl.Book == book2), 0);
            Assert.AreEqual(cart.CartLines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {

            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "ASP.NET", Price = 100 };
            Book book2 = new Book { ISBN = 2, Title = "Oracle", Price = 50 };
            Book book3 = new Book { ISBN = 3, Title = "C#", Price = 70 };

            //Act
            Cart cart = new Cart();
            cart.AddItem(book1);
            cart.AddItem(book2, 2);
            cart.AddItem(book3);

            decimal result = cart.ComputerTotalValue();

            //Assert
            Assert.AreEqual(result, 270);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {

            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "ASP.NET" };
            Book book2 = new Book { ISBN = 2, Title = "Oracle" };
            Book book3 = new Book { ISBN = 3, Title = "C#" };

            //Act
            Cart cart = new Cart();
            cart.AddItem(book1);
            cart.AddItem(book2, 3);
            cart.AddItem(book3, 5);
            cart.AddItem(book2, 1);
            cart.RemoveItem(book2);
            cart.Clear();

            //Assert
            Assert.AreEqual(cart.CartLines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                new Book[]
                {
                    new Book() {ISBN = 1,Title = "ASP.NET MVC",Specialization = "Programming"}
                }.AsQueryable()
                );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);

            //Act
            target.AddToChart(cart, 1, null);
            //RedirectToRouteResult result = target.AddToChart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual(cart.CartLines.ToArray()[0].Book.Title, "ASP.NET MVC");

        }

        public void Adding_Book_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                new Book[]
                {
                    new Book() {ISBN = 1,Title = "ASP.NET MVC",Specialization = "Programming"}
                }.AsQueryable()
            );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);

            //Act
            RedirectToRouteResult result = target.AddToChart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");

        }

        [TestMethod]
        public void Can_View_Cart_Content()
        {
            //Arrange
            Cart cart = new Cart();
            CartController target = new CartController(null, null);

            //Act
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            //Assert
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Arragne
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController targer = new CartController(null, mock.Object);

            //Act
            ViewResult result = targer.Checkout(cart, shippingDetails);

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Arragne
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController targer = new CartController(null, mock.Object);
            targer.ModelState.AddModelError("error", "error");

            //Act
            ViewResult result = targer.Checkout(cart, shippingDetails);

            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cann_Checkout_And_Submit_Order()
        {
            //Arragne
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController targer = new CartController(null, mock.Object);

            //Act
            ViewResult result = targer.Checkout(cart, shippingDetails);

            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
