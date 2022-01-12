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
    public partial class PendingApprovalExpenses : ExpenseApplicationBasePage
    {
        private static readonly string maxLengthKey = "maxlength";
        private static readonly string maxLengthValue = "150";
        private static readonly string expenseFormKey = "ManagerExpenseFormList";

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
            AccessType = Parameters.AccessEnum.CanApprove;
            base.Page_Load(sender, e);
            RejectReason.Attributes.Add(maxLengthKey, maxLengthValue);
            BindApprovalExpensesGridview();
        }

        protected void BindApprovalExpensesGridview()
        {
            try
            {
                var userID = Context.User.Identity.GetUserId();
                expenseFormList = DBHelper.GetPendingExpenseFormListByManagerID(userID);
                ApprovalExpensesGridview.DataSource = expenseFormList;
                ApprovalExpensesGridview.DataBind();
                ApprovalExpensesGridview.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.ToString();
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void ApprovalExpensesGridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ApprovalExpensesGridview.PageIndex = e.NewPageIndex;
            ApprovalExpensesGridview.DataSource = expenseFormList;
            ApprovalExpensesGridview.DataBind();
        }

        protected void DisplayExpenseItemsModal()
        {
            var selectedFormID = Convert.ToInt32(SelectedRowID.Value);
            ExpenseForm selectedExpenseForm = expenseFormList.Where(x => x.ID == selectedFormID).FirstOrDefault();
            ApproveDetailGridview.DataSource = selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList();
            ApproveDetailGridview.DataBind();
            if (selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList().Count > 0)
            {
                ApproveDetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Text = "Total";
                ApproveDetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Font.Bold = true;
                ApproveDetailGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].HorizontalAlign = HorizontalAlign.Right;
                ApproveDetailGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Text = string.Format("{0:C}", selectedExpenseForm.ExpenseItems.Where(x => x.IsDeleted == false).ToList().Sum(item => item.Cost));
                ApproveDetailGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Font.Bold = true;

            }
            DisplayModalDefault();
            ClientScript.RegisterStartupScript(GetType(), "DetailExpenseModalKey", "<script> $('#ApproveExpenseDetailModal').modal('toggle');</script>");
        }

        protected void DisplayModalDefault()
        {
            FormGridview.Visible = true;
            ExecuteSuccess.Visible = false;
            ExecuteError.Visible = false;
        }
        protected void DetailButton_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            DisplayActionPopup(gvr,false,false,false,false);
        }

        protected void GridviewApproveButton_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            DisplayActionPopup(gvr, false, true, false, false);
        }

        protected void GridviewRejectButton_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            DisplayActionPopup(gvr,true,false,true,true);
        }


        private void DisplayActionPopup(GridViewRow gvr, bool showRejectReason, bool showApprovalButton, bool showRepairButton, bool showRejectButton)
        {
            var expenseFormHiddenfield = gvr.FindControl("ExpenseFormID") as HiddenField;
            int formID = int.Parse(expenseFormHiddenfield.Value);
            SelectedRowID.Value = formID.ToString();
            DisplayExpenseItemsModal();
            txtRejectReason.Visible = showRejectReason;
            ApprovalButton.Visible = showApprovalButton;
            RepairButton.Visible = showRepairButton;
            RejectButton.Visible = showRejectButton;
        }

        private void ExpenseStatusChange(int status)
        {
            try
            {
                var selectedFormID = Convert.ToInt32(SelectedRowID.Value);
                using (var dbconnection = new Entities())
                {
                    var selectedExpenseForm = dbconnection.ExpenseForms.Where(x => x.ID == selectedFormID).FirstOrDefault();
                    selectedExpenseForm.Status = status;
                    if (status != (int)Parameters.ExpenseStatusEnum.PendingAccountantPayment)
                    {
                        selectedExpenseForm.RejectReason = RejectReason.Text;
                    }
                    selectedExpenseForm.ModifiedBy = Context.User.Identity.GetUserName();
                    dbconnection.SaveChanges();
                }
                BindApprovalExpensesGridview();
                RepairButton.Visible = false;
                RejectButton.Visible = false;
                RejectReason.Text = string.Empty;
                ApprovalButton.Visible = false;
                FormGridview.Visible = false;
                ExecuteSuccess.Visible = true;
            }
            catch(Exception ex)
            {
                ExecuteError.Visible = true;
            }
            ClientScript.RegisterStartupScript(GetType(), "DetailExpenseModalKey", "<script> $('#ApproveExpenseDetailModal').modal('toggle');</script>");
        }
        protected void RejectButton_Click(object sender, EventArgs e)
        {
            ExpenseStatusChange((int)Parameters.ExpenseStatusEnum.Rejected);
        }

        protected void RepairButton_Click(object sender, EventArgs e)
        {
            ExpenseStatusChange((int)Parameters.ExpenseStatusEnum.Repair);
        }

        protected void ApprovalButton_Click(object sender, EventArgs e)
        {
            ExpenseStatusChange((int)Parameters.ExpenseStatusEnum.PendingAccountantPayment);
        }
    }
}