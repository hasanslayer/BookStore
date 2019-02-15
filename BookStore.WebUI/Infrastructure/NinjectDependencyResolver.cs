using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using BookStore.Domain.Abstract;
using BookStore.Domain.Concrete;
using BookStore.Domain.Entities;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Infrastructure.Concrete;

namespace BookStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings(); // using of dependency
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //mock.Setup(b => b.Books).Returns(
            //    new List<Book>
            //    {
            //        new Book {Title = "ASP.NET", Price = 50},
            //        new Book {Title = "SQL Server DB", Price = 90},
            //        new Book {Title = "Web Client", Price = 87}
            //    }
            //);
            EmailSettings emailSettings = new EmailSettings()
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IBookRepository>().To<EFBookRepository>();
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);// "settings" that passed in the constructor 
        }
    }
}