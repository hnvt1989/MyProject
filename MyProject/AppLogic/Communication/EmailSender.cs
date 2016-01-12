using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using MyProject.DAL;
using MyProject.Models.ViewModels;

namespace MyProject.AppLogic.Communication
{
    public class EmailSender
    {
        public EmailSender()
        {
            
        }

        public static async Task<int> Send(OrderSummaryViewModel order)
        {
            var body = "<p>Order number: {0} from customer {1} </p><p>Order Total:</p><p>{2}</p>";
            var message = new MailMessage();
            var email1 = "";
            var email2 = "";

            using (var context = new ShoppingCartContext())
            {
                email1 = context.AppSettings.Single(a => a.Code == "NotificationEmail1").Value;
                email2 = context.AppSettings.Single(a => a.Code == "NotificationEmail2").Value;
            }

            message.To.Add(new MailAddress(email1)); //send to company email
            message.To.Add(new MailAddress(email2)); //send to company email

            message.Subject = "An order has placed !!";
            message.Body = string.Format(body, order.OrderNumber, order.FullName, order.Total.ToString("N0"));
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
                
            }

            //send to customer email
            if (!string.IsNullOrEmpty(order.Email))
            {
                body = CreateEmailTemplate(order);
                message = new MailMessage();


                message.To.Add(new MailAddress(order.Email));

                message.Subject = "Cảm ơn bạn đã đặt hàng ở H.A Shop !";
                message.Body = body;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);

                }
            }
            return 1;
        }

        public static string CreateEmailTemplate(OrderSummaryViewModel order)
        {

            var text = "<p>Tin nhắn từ: {0} </p><p>Nội dung:</p><p>{1}</p><p>{2}</p>";
            return string.Format(text, "H.A Shop",
                "Cảm ơn bạn đã đặt hàng ở H.A Shop. Số đơn đặt hàng của bạn là: " + order.OrderNumber, "Xin vui lòng liên lạc với chúng tôi qua số điện thoại :");
        }
    }
}