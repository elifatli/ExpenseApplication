using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ExpenseApplication.Helpers
{
    public class MailHelper
    {
        public static void SendMailForPaidExpense(ExpenseForm expenseForm)
        {
            
            var fromAddress = new MailAddress("yourmail@gmail.com", "Expense App");
            var toAddress = new MailAddress(expenseForm.AspNetUser.Email, $" {expenseForm.AspNetUser.Name} {expenseForm.AspNetUser.Surname}");
            const string fromPassword = "your_password";
            const string subject = "Masraf Ödenmesi";
            string body = $"Sayın {expenseForm.AspNetUser.Name} {expenseForm.AspNetUser.Surname}, \n {expenseForm.Description} harcamanız onaylanmıştır. \n Bilgilerinize sunarız.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public static void SendReminderMailForManager(ExpenseForm expenseForm)
        {

            var fromAddress = new MailAddress("yourmail@gmail.com", "Expense App");
            var manager = DBHelper.GetManagerByUserID(expenseForm.AspNetUser.ManagerUserID);
            var toAddress = new MailAddress(manager.Email, $" {manager.Name} {manager.Surname}");
            const string fromPassword = "your_password";
            const string subject = "Masraf Onayı";
            string body = $"Sayın {manager.Name} {manager.Surname}, \n {expenseForm.Description} isimli harcamayı hala onaylamadınız. \n Bilgilerinize sunarız.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}