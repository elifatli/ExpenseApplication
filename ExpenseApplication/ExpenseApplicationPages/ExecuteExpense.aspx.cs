using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseApplication.Helpers;
using Microsoft.AspNet.Identity;
using ExpenseApplication.Base;

namespace ExpenseApplication.ExpenseApplicationPages
{
    public partial class ExecuteExpense : ExpenseApplicationBasePage
    {
        private static readonly string expenseFormKey = "ExpenseForm";
        protected override void Page_Load(object sender, EventArgs e)
        {
            AccessType = Parameters.AccessEnum.CanInsert;
            base.Page_Load(sender, e);

            var expenseForm = (ExpenseForm)Session[expenseFormKey];
            try
            {
                if (expenseForm.ID != 0)
                {
                    expenseForm.ExpenseItems = expenseForm.ExpenseItems.Select(c => { c.FormID = expenseForm.ID; return c; }).ToList();
                    expenseForm.RejectReason = string.Empty;
                }
                using (var dbconnection = new Entities())
                {
                    dbconnection.ExpenseForms.AddOrUpdate(expenseForm);
                    foreach (var item in expenseForm.ExpenseItems)
                    {
                        dbconnection.ExpenseItems.AddOrUpdate(item);
                    }
                    dbconnection.SaveChanges();
                }

                ExecuteSuccess.Visible = true;
            }
            catch (Exception ex)
            {
                ExecuteError.Visible = true;
            }
            Session[expenseFormKey] = null;
        }
    }
}