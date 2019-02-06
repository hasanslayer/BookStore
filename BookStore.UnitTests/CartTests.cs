using System;
using System.Linq;
using BookStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
