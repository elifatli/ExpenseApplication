using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.UI;
using System.Data.Entity;

namespace ExpenseApplication.Helpers
{
    public class DBHelper
    {
        public static List<ExpenseType> GetExpenseTypes()
        {
            using (var dbconnection = new Entities())
            {
                return dbconnection.ExpenseTypes.ToList();
            }
        }

        public static ExpenseType GetExpenseTypeByID(int id)
        {
            using (var dbconnection = new Entities())
            {
                return dbconnection.ExpenseTypes.Where(x => x.ID == id).FirstOrDefault();
            }
        }

        public static Status GetStatusByID(int id)
        {
            using (var dbconnection = new Entities())
            {
                return dbconnection.Status.Where(x => x.ID == id).FirstOrDefault();
            }
        }

        public static AspNetUser GetCurrentUserByID(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                using (var dbconnection = new Entities())
                {
                    return dbconnection.AspNetUsers.Include(y => y.Role).Where(x => x.Id == id).FirstOrDefault();
                }
            }
            return null;
        }

        public static List<ExpenseForm> GetExpenseFormListByUserID(string userID)
        {
            using (var dbconnection = new Entities())
            {

                return dbconnection.ExpenseForms.Include(u => u.ExpenseItems).Where(x => x.UserID == userID).OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<ExpenseForm> GetPendingExpenseFormListByManagerID(string userID)
        {
            using (var dbconnection = new Entities())
            {

                return dbconnection.ExpenseForms.Include(u => u.ExpenseItems).Include(y => y.AspNetUser)
                    .Where(x => x.AspNetUser.ManagerUserID == userID && x.Status == (int)Parameters.ExpenseStatusEnum.PendingManagerApproval).OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<ExpenseForm> GetPendingPaymentExpenseFormList()
        {
            using (var dbconnection = new Entities())
            {
                return dbconnection.ExpenseForms.Include(a => a.ExpenseItems).Include(x => x.AspNetUser)
                    .Where(y => y.Status == (int)Parameters.ExpenseStatusEnum.PendingAccountantPayment).OrderByDescending(x => x.ID).ToList();

            }
        }

        public static List<ExpenseForm> GetPersonalReportData(string userID, DateTime selectedDate, bool isAnnual)
        {

            using (var dbconnection = new Entities())
            {
                if (isAnnual)
                {
                    return dbconnection.ExpenseForms.Include(a => a.ExpenseItems.Select(i => i.ExpenseType1)).Include(x => x.AuditExpenseForms)
                    .Where(x => x.UserID == userID && x.Status == (int)Parameters.ExpenseStatusEnum.Paid &&
                    x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Year == selectedDate.Year).ToList();
                }
                else
                {

                    return dbconnection.ExpenseForms.Include(a => a.ExpenseItems.Select(i => i.ExpenseType1)).Include(x => x.AuditExpenseForms)
                         .Where(x => x.UserID == userID && x.Status == (int)Parameters.ExpenseStatusEnum.Paid &&
                         x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Year == selectedDate.Year &&
                         x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Month == selectedDate.Month).ToList();
                }
            }
        }

        public static List<ExpenseForm> GetStaffReportData(string userID, DateTime selectedDate, bool isAnnual)
        {

            using (var dbconnection = new Entities())
            {
                if (isAnnual)
                {
                    return dbconnection.ExpenseForms.Include(a => a.ExpenseItems.Select(i => i.ExpenseType1)).Include(x => x.AuditExpenseForms).Include(u => u.AspNetUser)
                    .Where(x => x.AspNetUser.ManagerUserID == userID && x.Status == (int)Parameters.ExpenseStatusEnum.Paid &&
                    x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Year == selectedDate.Year).ToList();
                }
                else
                {
                    return dbconnection.ExpenseForms.Include(a => a.ExpenseItems.Select(i => i.ExpenseType1)).Include(x => x.AuditExpenseForms).Include(u => u.AspNetUser)
                    .Where(x => x.AspNetUser.ManagerUserID == userID && x.Status == (int)Parameters.ExpenseStatusEnum.Paid &&
                         x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Year == selectedDate.Year &&
                         x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Month == selectedDate.Month).ToList();
                }
            }
        }

        public static List<ExpenseForm> GetAllStaffReportData(DateTime selectedDate, bool isAnnual)
        {

            using (var dbconnection = new Entities())
            {
                if (isAnnual)
                {
                    return dbconnection.ExpenseForms.Include(a => a.ExpenseItems.Select(i => i.ExpenseType1)).Include(x => x.AuditExpenseForms).Include(u => u.AspNetUser)
                    .Where(x => x.Status == (int)Parameters.ExpenseStatusEnum.Paid &&
                    x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Year == selectedDate.Year).ToList();
                }
                else
                {
                    return dbconnection.ExpenseForms.Include(a => a.ExpenseItems.Select(i => i.ExpenseType1)).Include(x => x.AuditExpenseForms).Include(u => u.AspNetUser)
                    .Where(x => x.Status == (int)Parameters.ExpenseStatusEnum.Paid &&
                         x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Year == selectedDate.Year &&
                         x.AuditExpenseForms.Where(a => a.Status == (int)Parameters.ExpenseStatusEnum.Paid).FirstOrDefault().ModifiedDate.Value.Month == selectedDate.Month).ToList();
                }
            }
        }

        public static List<ExpenseForm> GetExpensesForReminderMail()
        {
            using(var dbconnection =new Entities())
            {
                return dbconnection.ExpenseForms.Include(a => a.AuditExpenseForms).Include(z=>z.AspNetUser)
                    .Where(x => x.Status == (int)Parameters.ExpenseStatusEnum.PendingManagerApproval &&
                    DbFunctions.DiffHours(x.AuditExpenseForms.OrderByDescending(q => q.ID).FirstOrDefault().ModifiedDate, DateTime.Now) >=48).ToList();
            }
           
        }

        public static AspNetUser GetManagerByUserID(string id)
        {
            using (var dbconnection= new Entities())
            {
                return dbconnection.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public static void SetModifiedBySytem(long id)
        {
            using(var dbconnection=new Entities())
            {
               var expense=dbconnection.ExpenseForms.Where(x => x.ID == id).FirstOrDefault();
                expense.ModifiedBy = "System";
                dbconnection.SaveChanges();
            }
        }

        public static List<ReportTemplate> GetReportTemplateByUserID(string id)
        {
            using (var dbconnection = new Entities())
            {
                return dbconnection.ReportTemplates.Where(x => x.UserID == id).ToList();
            }
        }

    }
}