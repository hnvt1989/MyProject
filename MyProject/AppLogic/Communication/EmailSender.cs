using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using MyProject.DAL;
using MyProject.Models.ViewModels;

namespace MyProject.AppLogic.Communication
{
    public class EmailSender
    {
        public EmailSender()
        {
            
        }

        //public static async Task<int> Send(OrderSummaryViewModel order)
        //{
        //    var body = "<p>Order number: {0} from customer {1} </p><p>Order Total:</p><p>{2}</p>";
        //    var message = new MailMessage();
        //    var email1 = "";
        //    var email2 = "";

        //    using (var context = new ShoppingCartContext())
        //    {
        //        email1 = context.AppSettings.Single(a => a.Code == "NotificationEmail1").Value;
        //        email2 = context.AppSettings.Single(a => a.Code == "NotificationEmail2").Value;
        //    }

        //    message.To.Add(new MailAddress(email1)); //send to company email
        //    message.To.Add(new MailAddress(email2)); //send to company email

        //    message.Subject = "An order has placed !!";
        //    message.Body = string.Format(body, order.OrderNumber, order.FullName, order.Total.ToString("N0"));
        //    message.IsBodyHtml = true;
        //    using (var smtp = new SmtpClient())
        //    {
        //        await smtp.SendMailAsync(message);
                
        //    }

        //    //send to customer email
        //    if (!string.IsNullOrEmpty(order.Email))
        //    {
        //        body = CreateEmailTemplate(order);
        //        message = new MailMessage();


        //        message.To.Add(new MailAddress(order.Email));

        //        message.Subject = "Cảm ơn bạn đã đặt hàng ở H.A Shop !";
        //        message.Body = body;
        //        message.IsBodyHtml = true;
        //        using (var smtp = new SmtpClient())
        //        {
        //            await smtp.SendMailAsync(message);

        //        }
        //    }
        //    return 1;
        //}

        public static string CreateEmailTemplate(OrderConfirmViewModel order, string orderNumber)
        {
            var person1_Name = "";
            var person1_Phone = "";

            var person2_Name = "";
            var person2_Phone = "";

            using (var context = new ShoppingCartContext())
            {
                var person1_n = context.AppSettings.First(a => a.Code == "ContactVietnam_Name_1");
                if (person1_n != null)
                {
                    person1_Name = person1_n.Value;
                }
                var person1_p = context.AppSettings.First(a => a.Code == "ContactVietnam_Phone_1");
                if (person1_p != null)
                {
                    person1_Phone = person1_p.Value;
                }

                var person2_n = context.AppSettings.First(a => a.Code == "ContactVietnam_Name_2");
                if (person2_n != null)
                {
                    person2_Name = person2_n.Value;
                }
                var person2_p = context.AppSettings.First(a => a.Code == "ContactVietnam_Phone_2");
                if (person2_p != null)
                {
                    person2_Phone = person2_p.Value;
                }
            }

            var text = "Tin nhắn từ: {0}. Nội dung: {1}. {2}";
            string contact = ".";
            if (!person1_Name.IsNullOrWhiteSpace() && !person1_Phone.IsNullOrWhiteSpace())
            {
                contact =  "Xin vui lòng liên lạc với chúng tôi để biết cách thanh toán tiền và nhận sản phẩm: " +
                              person1_Name + " ( " + person1_Phone + " ).";
                if (!person2_Name.IsNullOrWhiteSpace() && !person2_Phone.IsNullOrWhiteSpace())
                    contact += " Hoặc: " + person2_Name + " ( " + person2_Phone + " ).";

            }

            return string.Format(text, "J.A Shop", "Cảm ơn bạn đã đặt hàng ở J.A Shop. Số đơn đặt hàng của bạn là: " + orderNumber, contact);

        }

        public static async Task<int> SendMail(string orderNumber, OrderConfirmViewModel order)
        {
            var HOST = "";
            var FROM = "";
            var TO = "";
            var SMTP_USERNAME = "";
            var SMTP_PASSWORD = "";
            var TO2 = "";
            using (var context = new ShoppingCartContext())
            {
                FROM = context.AppSettings.Single(a => a.Code == "EmailFrom_EmailAddress").Value;
                SMTP_USERNAME = context.AppSettings.Single(a => a.Code == "EmailFrom_UserName").Value;
                SMTP_PASSWORD = context.AppSettings.Single(a => a.Code == "EmailFrom_Password").Value;
                HOST = context.AppSettings.Single(a => a.Code == "EmailFrom_Host").Value;// Amazon SES SMTP host name. This example uses the US West (Oregon) region.
                TO = context.AppSettings.Single(a => a.Code == "NotificationEmail1").Value;
                TO2 = context.AppSettings.Single(a => a.Code == "NotificationEmail2").Value;
            }


            var SUBJECT = "An order has placed !";
            var BODY = string.Format("Order number: {0} from customer {1} . Order Total: {2}", orderNumber, order.CheckOutInfo.Name, order.CartViewModel.CartTotal.ToString("N0"));

            //const String FROM = "SENDER@EXAMPLE.COM";   // Replace with your "From" address. This address must be verified.
            //const String TO = "RECIPIENT@EXAMPLE.COM";  // Replace with a "To" address. If your account is still in the
            // sandbox, this address must be verified.


            //const String BODY = "This email was sent through the Amazon SES SMTP interface by using C#.";

            // Supply your SMTP credentials below. Note that your SMTP credentials are different from your AWS credentials.
            //const String SMTP_USERNAME = "YOUR_SMTP_USERNAME";  // Replace with your SMTP username. 
            //const String SMTP_PASSWORD = "YOUR_SMTP_PASSWORD";  // Replace with your SMTP password.



            // Port we will connect to on the Amazon SES SMTP endpoint. We are choosing port 587 because we will use
            // STARTTLS to encrypt the connection.
            const int PORT = 587;

            // Create an SMTP client with the specified host name and port.
            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Create a network credential with your SMTP user name and password.
                client.Credentials = new System.Net.NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Use SSL when accessing Amazon SES. The SMTP session will begin on an unencrypted connection, and then 
                // the client will issue a STARTTLS command to upgrade to an encrypted connection using SSL.
                client.EnableSsl = true;

                
                // Send the email. 
                try
                {
                    //Console.WriteLine("Attempting to send an email through the Amazon SES SMTP interface...");
                    await client.SendMailAsync(FROM, TO, SUBJECT, BODY);
                    

                    //send to 2nd email:
                    await client.SendMailAsync(FROM, TO2, SUBJECT, BODY);

                    if (!string.IsNullOrEmpty(order.CheckOutInfo.Email))
                    {
                        BODY = CreateEmailTemplate(order, orderNumber);
                        TO = order.CheckOutInfo.Email;

                        SUBJECT = "Cảm ơn bạn đã đặt hàng ở H.A Shop !";
                        //message.Body = body;
                        //message.IsBodyHtml = true;
                        //using (var smtp = new SmtpClient())
                        //{
                        //    await smtp.SendMailAsync(message);

                        //}

                        await client.SendMailAsync(FROM, TO, SUBJECT, BODY);
                    }

                }
                catch (Exception ex)
                {
                    //Console.WriteLine("The email was not sent.");
                    //Console.WriteLine("Error message: " + ex.Message);
                }
            }


            //if (!string.IsNullOrEmpty(order.CheckOutInfo.Email))
            //{
            //    BODY = CreateEmailTemplate(order, orderNumber);
            //    TO = order.CheckOutInfo.Email;

            //    SUBJECT = "Cảm ơn bạn đã đặt hàng ở H.A Shop !";
            //    //message.Body = body;
            //    //message.IsBodyHtml = true;
            //    using (var smtp = new SmtpClient())
            //    {
            //        await smtp.SendMailAsync(message);

            //    }
            //}

            return 1;
            //Console.Write("Press any key to continue...");
            //Console.ReadKey();
        }
    }
}