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
    public partial class ConfirmExpense : ExpenseApplicationBasePage
    {
        private static readonly string expenseFormKey = "ExpenseForm";
        protected override void Page_Load(object sender, EventArgs e)
        {
            AccessType = Parameters.AccessEnum.CanInsert;
            base.Page_Load(sender, e);
            try
            {
                ConfirmDescriptionLiteral.Text = ((ExpenseForm)Session[expenseFormKey]).Description;
                BindGridview();
            }
            catch
            {
                Response.Redirect("~/ExpenseApplicationPages/AddExpense.aspx");
            }
        }


        protected void BindGridview()
        {
            try
            {
                ConfirmGridview.DataSource = ((ExpenseForm)Session[expenseFormKey]).ExpenseItems.Where(x=>x.IsDeleted==false).ToList();
                ConfirmGridview.DataBind();
                ConfirmGridview.HeaderRow.TableSection = TableRowSection.TableHeader;
                if (((ExpenseForm)Session[expenseFormKey]).ExpenseItems.Where(x => x.IsDeleted == false).ToList().Count > 0)
                {
                    ConfirmGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Text = "Total";
                    ConfirmGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].Font.Bold = true;
                    ConfirmGridview.FooterRow.Cells[Parameters.GridviewSumTextIndex].HorizontalAlign = HorizontalAlign.Right;
                    ConfirmGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Text = string.Format("{0:C}", ((ExpenseForm)Session[expenseFormKey]).ExpenseItems.Where(x => x.IsDeleted == false).ToList().Sum(item => item.Cost));
                    ConfirmGridview.FooterRow.Cells[Parameters.GridviewSumValueIndex].Font.Bold = true;

                }
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.ToString();
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void ConfirmBack_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/ExpenseApplicationPages/AddExpense.aspx");
        }

        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExpenseApplicationPages/ExecuteExpense.aspx");
        }
    }

}