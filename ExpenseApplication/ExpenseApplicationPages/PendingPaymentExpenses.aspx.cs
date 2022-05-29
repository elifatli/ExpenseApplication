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

    public partial class PendingPaymentExpenses : ExpenseApplicationBasePage
    {
        private static readonly string expenseFormKey = "AccountantExpenseFormList";
        private ICollection<ExpenseForm> expenseFormList
        {
            get
            {
                return (List<ExpenseForm>)Session[expenseFormKey];
            }
            set
            {
                Session[expenseFormKey] = value;
            }
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            AccessType = Parameters.AccessEnum.CanPay;
            base.Page_Load(sender, e);
            BindPaymentExpensesGridview();
        }

        protected void BindPaymentExpensesGridview()
        {
            try
            {
                var userID = Context.User.Identity.GetUserId();
                expenseFormList = DBHelper.GetPendingPaymentExpenseFormList();
                PaymentExpensesGridview.DataSource = expenseFormList;
                PaymentExpensesGridview.DataBind();
                PaymentExpensesGridview.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.ToString();
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void PaymentExpensesGridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaymentExpensesGridview.PageIndex = e.NewPageIndex;
            PaymentExpensesGridview.DataSource = expenseFormList;
            PaymentExpensesGridview.DataBind();
        }

        protected void DisplayPaymentExpenseItemsModal()
        {
            var selectedFormID = Convert.ToInt32(SelectedRowID.Value);
            ExpenseForm selectedExpenseForm = expenseFormList.Where(x => x.ID == selectedFormID).FirstOrDefault();
            PaymentDetailGridview.DataSource = selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList();
            PaymentDetailGridview.DataBind();
            if (selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList().Count > 0)
            {
                PaymentDetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Text = "Total";
                PaymentDetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Font.Bold = true;
                PaymentDetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].HorizontalAlign = HorizontalAlign.Right;
                PaymentDetailGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Text = string.Format("{0:C}", selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList().Sum(item => item.Cost));
                PaymentDetailGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Font.Bold = true;

            }
            DisplayExpenseModalDefault();
            ClientScript.RegisterStartupScript(GetType(), "DetailExpenseModalKey", "<script> $('#PaymentExpenseDetailModal').modal('toggle');</script>");
        }

        protected void DisplayExpenseModalDefault()
        {
            FormGridview.Visible = true;
            ExecuteSuccess.Visible = false;
            ExecuteError.Visible = false;
        }

        protected void DetailIcon_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            DisplayActionForDialog(gvr, false);
        }

        protected void PayIcon_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            DisplayActionForDialog(gvr, true);
        }

        private void DisplayActionForDialog(GridViewRow gvr, bool showPayButton)
        {
            var expenseFormHiddenfield = gvr.FindControl("ExpenseFormID") as HiddenField;
            int formID = int.Parse(expenseFormHiddenfield.Value);
            SelectedRowID.Value = formID.ToString();
            DisplayPaymentExpenseItemsModal();
            PayButton.Visible = showPayButton;
        }

        private void PaymentExpenseStatusChange(int status)
        {
            try
            {
                var selectedFormID = Convert.ToInt32(SelectedRowID.Value);
                using (var dbconnection = new Entities())
                {
                    var selectedExpenseForm = dbconnection.ExpenseForms.Where(x => x.ID == selectedFormID).FirstOrDefault();
                    selectedExpenseForm.Status = status;
                    selectedExpenseForm.ModifiedBy = Context.User.Identity.GetUserName();
                    dbconnection.SaveChanges();
                    MailHelper.SendMailForPaidExpense(selectedExpenseForm);
                }
                BindPaymentExpensesGridview();
                PayButton.Visible = false;
                FormGridview.Visible = false;
                ExecuteSuccess.Visible = true;
            }
            catch (Exception ex)
            {
                ExecuteError.Visible = true;
            }
            ClientScript.RegisterStartupScript(GetType(), "DetailExpenseModalKey", "<script> $('#PaymentExpenseDetailModal').modal('toggle');</script>");
        }

        protected void PayButton_Click(object sender, EventArgs e)
        {
            PaymentExpenseStatusChange ((int) Parameters.ExpenseStatusEnum.Paid);
           
        }
    }
}