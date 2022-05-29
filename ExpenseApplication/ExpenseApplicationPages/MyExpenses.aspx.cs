using ExpenseApplication.Base;
using ExpenseApplication.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseApplication.ExpenseApplicationPages
{
    public partial class MyExpenses : ExpenseApplicationBasePage
    {
        private static readonly string expenseFormListKey = "ExpenseFormList";
        private static readonly string expenseFormKey = "ExpenseForm";

        private ICollection<ExpenseForm> expenseFormList
        {
            get
            {
                return (List<ExpenseForm>)Session[expenseFormListKey];
            }
            set
            {
                Session[expenseFormListKey] = value;
            }
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            AccessType = Parameters.AccessEnum.CanInsert;
            base.Page_Load(sender, e);

            var userID = Context.User.Identity.GetUserId();
            expenseFormList = DBHelper.GetExpenseFormListByUserID(userID);
            BindGridview();
        }

        protected void BindGridview()
        {
            try
            {

                MyExpensesGridview.DataSource = expenseFormList;
                MyExpensesGridview.DataBind();
                MyExpensesGridview.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.ToString();
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void DetailButton_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            var expenseFormHiddenfield = gvr.FindControl("ExpenseFormID") as HiddenField;
            int formID = int.Parse(expenseFormHiddenfield.Value);
          
            ExpenseForm selectedExpenseForm = expenseFormList.Where(x=>x.ID== formID).FirstOrDefault();
            DetailGridview.DataSource = selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList();
            DetailGridview.DataBind();
            if (selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList().Count > 0)
            {
                DetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Text = "Total";
                DetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Font.Bold = true;
                DetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].HorizontalAlign = HorizontalAlign.Right;
                DetailGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Text = string.Format("{0:C}", selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList().Sum(item => item.Cost));
                DetailGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Font.Bold = true;

            }
            ClientScript.RegisterStartupScript(GetType(), "DetailExpenseModalKey", "<script> $('#ExpenseDetailModal').modal('toggle');</script>");
        }

        protected void MyExpensesGridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            MyExpensesGridview.PageIndex = e.NewPageIndex;
            MyExpensesGridview.DataSource = expenseFormList;
            MyExpensesGridview.DataBind();
        }

        protected void EditListButton_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            var expenseFormHiddenfield = gvr.FindControl("ExpenseFormID") as HiddenField;
            int formID = int.Parse(expenseFormHiddenfield.Value);

            ExpenseForm selectedExpenseForm = expenseFormList.Where(x => x.ID == formID).FirstOrDefault();
            Session[expenseFormKey] = selectedExpenseForm;
            Response.Redirect("~/ExpenseApplicationPages/AddExpense.aspx");
        }
    }
}