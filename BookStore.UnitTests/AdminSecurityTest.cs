using System;
using System.Web.Mvc;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTest
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //Arrange 
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            LoginViewModel model = new LoginViewModel()
            {
                Username = "admin",
                Password = "secret"
            };
            AccountController target = new AccountController(mock.Object);
            //Act
            ActionResult result = target.Login(model, "/MyUrl");
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Can_Login_With_Invalid_Credentials()
        {
            //Arrange 
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("userx", "passwordx")).Returns(false);
            LoginViewModel model = new LoginViewModel()
            {
                Username = "userx",
                Password = "passwordx"
            };
            AccountController target = new AccountController(mock.Object);
            //Act
            ActionResult result = target.Login(model, "/MyUrl");
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));// return to the same page
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
