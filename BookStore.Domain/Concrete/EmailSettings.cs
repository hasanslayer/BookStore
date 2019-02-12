using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "reciver1@example.com, reciever2@example.com";
        public string MailFromAddress = "sender@example.com";
        public bool UseSsl = true;
        public string Username = "sender@example.com";
        public string Password = "password";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"E:\orders_bookstore_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder msgBody = new StringBuilder()
                    .AppendLine("A new Order has been submitted")
                    .AppendLine("----------")
                    .AppendLine("Books: ");

                foreach (var line in cart.CartLines)
                {
                    var subtotal = line.Book.Price * line.Quantity;
                    msgBody.AppendFormat("{0} x {1} (subtotal: {2:C})", line.Quantity, line.Book.Title, subtotal);
                }

                msgBody.AppendFormat("Total order value : {0:C}", cart.ComputerTotalValue())
                    .AppendLine("----------")
                    .AppendLine("Ship to :")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2)
                    .AppendLine(shippingDetails.State)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("----------")
                    .AppendFormat("Gift Wrap :{0} ", shippingDetails.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAddress,
                    "New order submitted",
                    msgBody.ToString()
                );

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch(Exception e)
                {
                    Debug.Print(e.Message);
                }
               


            }
        }
    }
}
