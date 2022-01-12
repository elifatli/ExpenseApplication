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
    public partial class AddExpense : ExpenseApplicationBasePage
    {
        private static readonly string maxAttributeKey = "max";
        private static readonly string expenseFormKey = "ExpenseForm";
        private static readonly string dateTimeKey = "yyyy-MM-dd";
        private static readonly string maxLengthKey = "maxlength";
        private static readonly string maxLengthValue = "150";
        private static readonly string id = "ID";
        private static readonly string name = "Name";
        private static List<ExpenseType> expenseTypeList;

        private ICollection<ExpenseItem> expenseItemList
        {
            get
            {
                return ((ExpenseForm)Session[expenseFormKey]).ExpenseItems;
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            AccessType = Parameters.AccessEnum.CanInsert;
            base.Page_Load(sender, e);
            try
            {
                if (!Page.IsPostBack)
                {

                    SetExpenseItemFormMaxLengths();
                    FillExpenseTypeDropdown();
                    CheckIfBackButtonClicked();
                    if (Session[expenseFormKey] == null)  //yeni bir liste ise yani back butonuna basılmamışsa
                    {
                        Session[expenseFormKey] = new ExpenseForm();
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }
        }

        private void CheckIfBackButtonClicked()
        {
            var expenseForm = (ExpenseForm)Session[expenseFormKey];
            if (expenseForm != null)
            {
                txtDescription.Text = expenseForm.Description;
            }
        }

        private void SetExpenseItemFormMaxLengths()
        {
            ExpenseDate.Attributes[maxAttributeKey] = DateTime.Now.ToString(dateTimeKey);
            EditExpenseDate.Attributes[maxAttributeKey] = DateTime.Now.ToString(dateTimeKey);
            Description.Attributes.Add(maxLengthKey, maxLengthValue);
            EditDescription.Attributes.Add(maxLengthKey, maxLengthValue);
        }

        protected void FillExpenseTypeDropdown()
        {
            try
            {
                expenseTypeList = DBHelper.GetExpenseTypes();

                ExpenseType.DataSource = expenseTypeList;
                ExpenseType.DataValueField = id;
                ExpenseType.DataTextField = name;
                ExpenseType.DataBind();

                EditExpenseDropdownList.DataSource = expenseTypeList;
                EditExpenseDropdownList.DataValueField = id;
                EditExpenseDropdownList.DataTextField = name;
                EditExpenseDropdownList.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void ExpenseItemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var expenseItem = new ExpenseItem();
                expenseItem.ExpenseType = int.Parse(ExpenseType.SelectedItem.Value);
                expenseItem.ExpenseDate = DateTime.Parse(ExpenseDate.Text);
                expenseItem.BillNumber = BillNumber.Text;
                expenseItem.Description = Description.Text;
                expenseItem.Cost = decimal.Parse(Cost.Text);
                expenseItemList.Add(expenseItem);
                ClearDialogFields();
                BindGridview();
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }



        }

        protected void BindGridview()
        {
            try
            {
                AddExpenseGridview.DataSource = expenseItemList;
                AddExpenseGridview.DataBind();
                AddExpenseGridview.HeaderRow.TableSection = TableRowSection.TableHeader;
                if (expenseItemList.Where(x => x.IsDeleted == false).ToList().Count > 0)
                {
                    AddExpenseGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex+1].Text = "Total";
                    AddExpenseGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex+1].Font.Bold = true;
                    AddExpenseGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex+1].HorizontalAlign = HorizontalAlign.Right;
                    AddExpenseGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex+1].Text = string.Format("{0:C}", expenseItemList.Where(x => x.IsDeleted == false).ToList().Sum(item => item.Cost));
                    AddExpenseGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex+1].Font.Bold = true;

                }
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }
        }
        protected void ClearDialogFields()
        {
            try
            {
                ExpenseType.SelectedIndex = 0;
                ExpenseDate.Text = string.Empty;
                BillNumber.Text = string.Empty;
                Description.Text = string.Empty;
                Cost.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }


        }

        protected void DeleteExpenseItem(int index)
        {
            var selectedItem = expenseItemList.ElementAt(index);

            if (selectedItem.ID == 0)
            {
                expenseItemList.Remove(selectedItem);
            }
            else
            {
                selectedItem.IsDeleted = true;
            }
        }

        protected void Send_Click(object sender, EventArgs e)
        {
            try
            {
                if (expenseItemList.Where(x => x.IsDeleted == false).ToList().Count > 0)
                {
                    var expenseForm = (ExpenseForm)Session[expenseFormKey];
                    expenseForm.UserID = Context.User.Identity.GetUserId();
                    if (expenseForm.ID == 0)
                    {
                        expenseForm.CreateDate = DateTime.Now;
                    }
                    expenseForm.Description = txtDescription.Text;
                    expenseForm.Status = (int)Parameters.ExpenseStatusEnum.PendingManagerApproval;
                    expenseForm.ModifiedBy = Context.User.Identity.GetUserName();
                    Session[expenseFormKey] = expenseForm;
                    Response.Redirect("~/ExpenseApplicationPages/ConfirmExpense.aspx");
                }
                else
                {
                    throw new Exception("Please add at least one expense");
                }



            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }

        }

        protected void AddExpenseGridview_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindGridview();
                }
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }


        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            //Get rowindex
            int rowindex = gvr.RowIndex;

            DeleteExpenseItem(rowindex);
            BindGridview();
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //Get rowindex
            int rowindex = gvr.RowIndex;
            SelectedRowIndex.Value = rowindex.ToString();

            ExpenseItem expenseToEdit = expenseItemList.ElementAt(rowindex);
            EditExpenseDropdownList.ClearSelection();
            EditExpenseDropdownList.Items.FindByValue(expenseToEdit.ExpenseType.ToString()).Selected = true;
            EditExpenseDate.Text = expenseToEdit.ExpenseDate.ToString(dateTimeKey);
            EditBillNumber.Text = expenseToEdit.BillNumber.ToString();
            EditDescription.Text = expenseToEdit.Description;
            EditCost.Text = expenseToEdit.Cost.ToString();

            ClientScript.RegisterStartupScript(GetType(), "EditExpenseModalKey", "<script> $('#ExpenseItemEditModal').modal('toggle');</script>");

        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            var rowIndex = Convert.ToInt32(SelectedRowIndex.Value);
            var expenseToEdit = expenseItemList.ElementAt(rowIndex);
            expenseToEdit.ExpenseType = int.Parse(EditExpenseDropdownList.SelectedItem.Value);
            expenseToEdit.ExpenseDate = DateTime.Parse(EditExpenseDate.Text);
            expenseToEdit.BillNumber = EditBillNumber.Text;
            expenseToEdit.Description = EditDescription.Text;
            expenseToEdit.Cost = decimal.Parse(EditCost.Text);
            BindGridview();
        }

        protected void AddExpenseGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.FindControl("IsDeleted")!=null && bool.Parse((e.Row.FindControl("IsDeleted") as HiddenField).Value))
            {
                e.Row.Visible = false;
            }
        }
    }
}