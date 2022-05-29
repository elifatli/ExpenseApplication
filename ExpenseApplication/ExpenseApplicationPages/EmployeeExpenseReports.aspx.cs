using ExpenseApplication.Base;
using ExpenseApplication.Helpers;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseApplication.ExpenseApplicationPages
{
    public partial class EmployeeExpenseReports : ExpenseApplicationBasePage
    {
        private static readonly string id = "ID";
        private static readonly string name = "Name";
        private static readonly string monthName = "MonthName";
        private static readonly string monthNumber = "MonthNumber";
        private static readonly string all = "All";
        private static List<ExpenseType> expenseTypeList;
        private static List<ReportTemplate> reportTemplateList;
        protected override void Page_Load(object sender, EventArgs e)
        {
            AccessType = Parameters.AccessEnum.CanPay;
            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                FillExpenseTypeDropdown();
                FillYearDropdown();
                FillMonthDropdown();
                BindTemplateDropdown();
            }
        }

        private void FillYearDropdown()
        {
            ReportYear.DataSource = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse();
            ReportYear.DataBind();

        }

        private void FillTemplateYearDropdown()
        {
            TemplateYear.DataSource = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse();
            TemplateYear.DataBind();
        }

        private void FillMonthDropdown()
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

        private void FillTemplateMonthDropdown()
        {
            if (TemplateYear.SelectedValue == DateTime.Now.Year.ToString())
            {

                TemplateMonth.DataSource = Enumerable.Range(1, DateTime.Now.Month).Select(a => new
                {
                    MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                    MonthNumber = a
                });

            }
            else
            {
                TemplateMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
                {
                    MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                    MonthNumber = a
                });
            }

            TemplateMonth.DataTextField = monthName;
            TemplateMonth.DataValueField = monthNumber;
            TemplateMonth.DataBind();

        }

        protected void FillExpenseTypeDropdown()
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

        protected void FillTemplateExpenseTypeDropdown()
        {
            try
            {
                expenseTypeList = DBHelper.GetExpenseTypes();
                TemplateExpenseType.DataSource = expenseTypeList;
                TemplateExpenseType.DataValueField = id;
                TemplateExpenseType.DataTextField = name;
                TemplateExpenseType.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void ReportYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillMonthDropdown();
        }

        protected void TemplateYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillTemplateMonthDropdown();
            DisplayTemplateModal();
        }

        protected void BtnMonthRadio_CheckedChanged(object sender, EventArgs e)
        {
            ReportFilterSection.Visible = true;
            ReportMonthSection.Visible = true;
        }

        protected void BtnYearRadio_CheckedChanged(object sender, EventArgs e)
        {
            ReportFilterSection.Visible = true;
            ReportMonthSection.Visible = false;
        }

        private void GetReportData()
        {
            var selectedDate = new DateTime(int.Parse(ReportYear.SelectedValue), int.Parse(ReportMonth.SelectedValue), 1);
            var reportData = DBHelper.GetAllStaffReportData(selectedDate, BtnYearRadio.Checked);
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
            Sheet.Cells["A2:G2"].Merge = true;
            Sheet.Cells["A2:G2"].RichText.Add("EXPENSE REPORTS").Bold = true;
            Sheet.Cells["A2:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            if (reportData.Count > 0)
            {
                Sheet.Cells["A4"].RichText.Add("Full Name").Bold = true;
                Sheet.Cells["B4"].RichText.Add("Type").Bold = true;
                Sheet.Cells["C4"].RichText.Add("Month").Bold = true;
                Sheet.Cells["D4"].RichText.Add("Year").Bold = true;
                Sheet.Cells["E4"].RichText.Add("Cost").Bold = true;
                Sheet.Cells["F4"].RichText.Add("Description").Bold = true;
                Sheet.Cells["G4"].RichText.Add("Form").Bold = true;
                int row = 5;
                foreach (var form in reportData)
                {
                    foreach (var item in form.ExpenseItems)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = $"{item.ExpenseForm.AspNetUser.Name} {item.ExpenseForm.AspNetUser.Surname}";
                        Sheet.Cells[string.Format("B{0}", row)].Value = item.ExpenseType1.Name;
                        Sheet.Cells[string.Format("C{0}", row)].Value = item.ExpenseDate.Month;
                        Sheet.Cells[string.Format("D{0}", row)].Value = item.ExpenseDate.Year;
                        Sheet.Cells[string.Format("E{0}", row)].Value = string.Format("{0:C}", item.Cost);
                        Sheet.Cells[string.Format("E{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        Sheet.Cells[string.Format("F{0}", row)].Value = item.Description;
                        Sheet.Cells[string.Format("G{0}", row)].Value = item.ExpenseForm.Description;
                        row++;

                    }
                }
                Sheet.Cells[string.Format("D{0}", row)].RichText.Add("Total").Bold = true;
                Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                Sheet.Cells[string.Format("E{0}", row)].RichText.Add(string.Format("{0:C}", reportData.SelectMany(x => x.ExpenseItems).Sum(x => x.Cost)).ToString()).Bold = true;
                Sheet.Cells[string.Format("E{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            else
            {
                Sheet.Cells["A3:G3"].Merge = true;
                Sheet.Cells["A4:G4"].Merge = true;
                Sheet.Cells["A4:G4"].RichText.Add("No data to display.").Bold = true;
                Sheet.Cells["A4:G4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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

        protected void AddTemplate_Click(object sender, EventArgs e)
        {
            FillTemplateYearDropdown();
            FillTemplateMonthDropdown();
            FillTemplateExpenseTypeDropdown();
            ClearDialogFields();
            DisplayTemplateModal();
        }
        protected void DisplayTemplateModal()
        {
            ClientScript.RegisterStartupScript(GetType(), "AddTemplateModalKey", "<script> $('#AddNewTemplateModal').modal('toggle');</script>");
        }
        protected void BindTemplateDropdown()
        {
            reportTemplateList = DBHelper.GetReportTemplateByUserID(Context.User.Identity.GetUserId());
            TemplateDropdown.DataSource = reportTemplateList;
            TemplateDropdown.DataValueField = id;
            TemplateDropdown.DataTextField = name;
            TemplateDropdown.DataBind();
            TemplateDropdown.Items.Insert(0, "Please select template");
        }
        protected void ClearDialogFields()
        {
            try
            {
                TemplateName.Text = string.Empty;
                TemplateExpenseType.SelectedIndex = 0;
                TemplateMonth.SelectedIndex = 0;
                TemplateYear.SelectedIndex = 0;
                TemplateMonthly.Checked = false;
                TemplateAnnual.Checked = false;
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }
        }
        protected void AddTemplateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    var reportTemplate = new ReportTemplate();
                    reportTemplate.Name = TemplateName.Text;
                    reportTemplate.ExpenseType = int.Parse(TemplateExpenseType.SelectedItem.Value);
                    reportTemplate.Month = int.Parse(TemplateMonth.SelectedValue);
                    reportTemplate.Year = int.Parse(TemplateYear.SelectedValue);
                    reportTemplate.IsMonthly = TemplateMonthly.Checked;
                    reportTemplate.UserID = Context.User.Identity.GetUserId();

                    using (var dbconnection = new Entities())
                    {
                        dbconnection.ReportTemplates.AddOrUpdate(reportTemplate);
                        dbconnection.SaveChanges();
                    }
                    BindTemplateDropdown();
                    ClearDialogFields();
                }
                else
                    DisplayTemplateModal();
            }
            catch (Exception ex)
            {
                ExceptionLabel.Text = ex.Message;
                ErrorMessageBlock.Visible = true;
            }
        }

        protected void TemplateMonthly_CheckedChanged(object sender, EventArgs e)
        {
            TemplateMonthSection.Visible = true;
            DisplayTemplateModal();
        }

        protected void TemplateAnnual_CheckedChanged(object sender, EventArgs e)
        {
            TemplateMonthSection.Visible = false;
            DisplayTemplateModal();
        }

        protected void CustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = TemplateMonthly.Checked || TemplateAnnual.Checked;
        }

        protected void TemplateDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TemplateDropdown.SelectedIndex != 0)
            {
                var templateID = int.Parse(TemplateDropdown.SelectedValue);
                var selectedTemplate = reportTemplateList.Where(x => x.ID == templateID).FirstOrDefault();
                if (selectedTemplate.IsMonthly.Value)
                {
                    BtnYearRadio.Checked = false;
                    BtnMonthRadio.Checked = true;
                    ReportMonthSection.Visible = true;
                }
                else
                {
                    BtnMonthRadio.Checked = false;
                    BtnYearRadio.Checked = true;
                    ReportMonthSection.Visible = false;
                }
                ReportExpenseType.ClearSelection();
                ReportExpenseType.Items.FindByValue(selectedTemplate.ExpenseType.ToString()).Selected = true;

                ReportMonth.ClearSelection();
                ReportMonth.Items.FindByValue(selectedTemplate.Month.ToString()).Selected = true;

                ReportYear.ClearSelection();
                ReportYear.Items.FindByValue(selectedTemplate.Year.ToString()).Selected = true;

                ReportFilterSection.Visible = true;
            }

        }
    }
}