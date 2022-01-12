using ExpenseApplication.Base;
using ExpenseApplication.Helpers;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseApplication.ExpenseApplicationPages
{
    public partial class MyExpenseReports : ExpenseApplicationBasePage
    {
        private static readonly string id = "ID";
        private static readonly string name = "Name";
        private static readonly string monthName = "MonthName";
        private static readonly string monthNumber = "MonthNumber";
        private static readonly string all = "All";
        private static List<ExpenseType> expenseTypeList;
        protected override void Page_Load(object sender, EventArgs e)
        {

            AccessType = Parameters.AccessEnum.CanInsert;
            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                FillExpenseTypeDropdownList();
                FillYearDropdownList();
                FillMonthDropdownList();
            }
           
        }

        private void FillYearDropdownList()
        {
            ReportYear.DataSource = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse();
            ReportYear.DataBind();

        }

        private void FillMonthDropdownList()
        {
            if (ReportYear.SelectedValue == DateTime.Now.Year.ToString())
            {

                ReportMonth.DataSource = Enumerable.Range(1, DateTime.Now.Month).Select(a => new
                {
                    MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                    MonthNumber = a
                });

            }
            else
            {
                ReportMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
                {
                    MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                    MonthNumber = a
                });
            }

            ReportMonth.DataTextField = monthName;
            ReportMonth.DataValueField = monthNumber;
            ReportMonth.DataBind();
        }

        protected void FillExpenseTypeDropdownList()
        {
            try
            {
                expenseTypeList = DBHelper.GetExpenseTypes();
                ReportExpenseType.DataSource = expenseTypeList;
                ReportExpenseType.DataValueField = id;
                ReportExpenseType.DataTextField = name;
                ReportExpenseType.DataBind();
                ReportExpenseType.Items.Insert(0, all);
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void ReportYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillMonthDropdownList();
        }

        protected void MonthRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ReportFilterSection.Visible = true;
            ReportMonthSection.Visible = true;
        }

        protected void YearRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ReportFilterSection.Visible = true;
            ReportMonthSection.Visible = false;
        }

        private void GetReportData()
        {
            var selectedDate = new DateTime(int.Parse(ReportYear.SelectedValue), int.Parse(ReportMonth.SelectedValue), 1);
            var userID = Context.User.Identity.GetUserId();
            var reportData=DBHelper.GetPersonalReportData(userID, selectedDate, YearRadioButton.Checked);
            if (ReportExpenseType.SelectedValue != all)
            {
                foreach (var expense in reportData)
                {
                    expense.ExpenseItems = expense.ExpenseItems.Where(x => x.ExpenseType == int.Parse(ReportExpenseType.SelectedValue)).ToList();
                }
            }
            DownloadExcel(reportData);
        }

        public void DownloadExcel(List<ExpenseForm> reportData)
        {
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A2:F2"].Merge = true;
            Sheet.Cells["A2:F2"].RichText.Add("EXPENSE REPORTS").Bold = true;
            Sheet.Cells["A2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            if (reportData.Count > 0)
            {
                Sheet.Cells["A4"].RichText.Add("Type").Bold = true;
                Sheet.Cells["B4"].RichText.Add("Month").Bold = true;
                Sheet.Cells["C4"].RichText.Add("Year").Bold = true;
                Sheet.Cells["D4"].RichText.Add("Cost").Bold = true;
                Sheet.Cells["E4"].RichText.Add("Description").Bold = true;
                Sheet.Cells["F4"].RichText.Add("Form").Bold = true;
                int row = 5;
                foreach (var form in reportData)
                {
                    foreach (var item in form.ExpenseItems)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = item.ExpenseType1.Name;
                        Sheet.Cells[string.Format("B{0}", row)].Value = item.ExpenseDate.Month;
                        Sheet.Cells[string.Format("C{0}", row)].Value = item.ExpenseDate.Year;
                        Sheet.Cells[string.Format("D{0}", row)].Value = string.Format("{0:C}", item.Cost);
                        Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        Sheet.Cells[string.Format("E{0}", row)].Value = item.Description;
                        Sheet.Cells[string.Format("F{0}", row)].Value = item.ExpenseForm.Description;
                        row++;

                    }
                }
                Sheet.Cells[string.Format("C{0}", row)].RichText.Add("Total").Bold = true;
                Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                Sheet.Cells[string.Format("D{0}", row)].RichText.Add(string.Format("{0:C}", reportData.SelectMany(x => x.ExpenseItems).Sum(x => x.Cost)).ToString()).Bold = true;
                Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            else
            {
                Sheet.Cells["A3:F3"].Merge = true;
                Sheet.Cells["A4:F4"].Merge = true;
                Sheet.Cells["A4:F4"].RichText.Add("No data to display.").Bold = true;
                Sheet.Cells["A4:F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }

        protected void DownloadReportButton_Click(object sender, EventArgs e)
        {
            GetReportData();
        }
    }
}