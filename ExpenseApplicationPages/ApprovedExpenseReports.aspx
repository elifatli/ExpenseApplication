<%@ Page Title="Approved Expense Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApprovedExpenseReports.aspx.cs" Inherits="ExpenseApplication.ExpenseApplicationPages.ApprovedExpenseReports" %>

<%@ Import Namespace="ExpenseApplication.Helpers" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <hr />
    <div runat="server" id="ErrorMessageBlock" class="alert alert-dismissible alert-danger" visible="false">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <asp:Label runat="server" ID="ExceptionLabel"></asp:Label>
    </div>
    <p class="text-info" style="font-size: 16px;">
        Please, select a date to view the expenses made by the employees.
    </p>
    <hr />
    <br />
    <div class="container">
        <div class="form-horizontal">
            <asp:Label runat="server" AssociatedControlID="TemplateDropdown" CssClass="col-lg-5 control-label" Text="Template"></asp:Label>
            <div class="col-lg-3">
                <asp:DropDownList runat="server" ID="TemplateDropdown" CssClass="form-control" OnSelectedIndexChanged="TemplateDropdown_SelectedIndexChanged" AutoPostBack="true"/>
            </div>
        </div>
        <br />
        <br />
        <br />
        <div class="row">
            <div class="col-md-offset-5">
                <asp:RadioButton ID="BtnMonthRadio" Text="Monthly" GroupName="ReportTypeGroup" runat="server" Font-Size="Large" CssClass="col-md-3"
                    AutoPostBack="true" OnCheckedChanged="BtnMonthRadio_CheckedChanged" />
                <asp:RadioButton ID="BtnYearRadio" Text="Annual" GroupName="ReportTypeGroup" runat="server" Font-Size="Large"
                    AutoPostBack="true" OnCheckedChanged="BtnYearRadio_CheckedChanged" />
            </div>
        </div>
        <br />
        <br />
        <div id="ReportFilterSection" class="form-horizontal" runat="server" visible="false">
            <div>
                <asp:Label runat="server" AssociatedControlID="ReportExpenseType" CssClass="col-lg-2 control-label" Text="Expense Type"></asp:Label>
                <div class="col-lg-2">
                    <asp:DropDownList runat="server" ID="ReportExpenseType" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ReportExpenseType" ValidationGroup="ReportGroup" CssClass="text-danger" ErrorMessage="Please select expense type." />
                </div>
            </div>
            <div runat="server" id="ReportMonthSection">
                <asp:Label runat="server" AssociatedControlID="ReportMonth" CssClass="col-lg-2 control-label" Text="Month"></asp:Label>
                <div class="col-lg-2">
                    <asp:DropDownList runat="server" ID="ReportMonth" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ReportMonth" ValidationGroup="ReportGroup" CssClass="text-danger" ErrorMessage="Please select month." />
                </div>
            </div>
            <div>
                <asp:Label runat="server" AssociatedControlID="ReportYear" CssClass="col-lg-2 control-label" Text="Year"></asp:Label>
                <div class="col-lg-2">
                    <asp:DropDownList runat="server" ID="ReportYear" CssClass="form-control" OnSelectedIndexChanged="ReportYear_SelectedIndexChanged" AutoPostBack="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ReportYear" ValidationGroup="ReportGroup" CssClass="text-danger" ErrorMessage="Please select year." />
                </div>
            </div>
            <br />
            <div class="col-lg-12">
                <asp:Button ID="DownloadReportButton" runat="server" OnClick="DownloadReportButton_Click" CssClass="btn btn-success buttonright" Text="Download" />
                <asp:Button ID="AddTemplate" runat="server" CssClass="btn btn-info buttonmargin" OnClick="AddTemplate_Click" Text="Add New Template" />
            </div>
        </div>
    </div>
    <%-- Add New Template Modal --%>
    <div class="modal" id="AddNewTemplateModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Add Template</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <fieldset>
                            <div class="row">
                                <div class="col-md-offset-3">
                                    <asp:RadioButton ID="TemplateMonthly" Text="Monthly" GroupName="TemplateExpenseGroup" runat="server" Font-Size="Large" CssClass="col-md-4"
                                        AutoPostBack="true" OnCheckedChanged="TemplateMonthly_CheckedChanged" />
                                    <asp:RadioButton ID="TemplateAnnual" Text="Annual" GroupName="TemplateExpenseGroup" runat="server" Font-Size="Large"
                                        AutoPostBack="true" OnCheckedChanged="TemplateAnnual_CheckedChanged" />
                                </div>
                                <div class="col-md-offset-3">
                                    <asp:CustomValidator ID="CustomValidator" runat="server" Display="Dynamic" ErrorMessage="Please select interval." CssClass="text-danger col-md-5"
                                        ValidationGroup="TemplateExpenseGroup" EnableClientScript="false"
                                        OnServerValidate="CustomValidator_ServerValidate"></asp:CustomValidator>
                                </div>
                            </div>
                            <br />
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="TemplateName" CssClass="col-lg-3 control-label" Text="Template Name"></asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" MaxLength="50" ID="TemplateName" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TemplateName" ValidationGroup="TemplateExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please enter template name." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="TemplateExpenseType" CssClass="col-lg-3 control-label" Text="Expense Type"></asp:Label>
                                <div class="col-lg-6">
                                    <asp:DropDownList runat="server" ID="TemplateExpenseType" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TemplateExpenseType" ValidationGroup="TemplateExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please select expense type." />
                                </div>
                            </div>
                            <div class="form-group required" runat="server" id="TemplateMonthSection">
                                <asp:Label runat="server" AssociatedControlID="TemplateMonth" CssClass="col-lg-3 control-label" Text="Month"></asp:Label>
                                <div class="col-lg-6">
                                    <asp:DropDownList runat="server" ID="TemplateMonth" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TemplateMonth" ValidationGroup="TemplateExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please select month." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="TemplateYear" CssClass="col-lg-3 control-label" Text="Year"></asp:Label>
                                <div class="col-lg-6">
                                    <asp:DropDownList runat="server" ID="TemplateYear" CssClass="form-control" OnSelectedIndexChanged="TemplateYear_SelectedIndexChanged" AutoPostBack="true" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TemplateYear" ValidationGroup="TemplateExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please select year." />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="AddTemplateButton" OnClick="AddTemplateButton_Click" ValidationGroup="TemplateExpenseGroup" CssClass="btn btn-info" Text="Add" />
                </div>
            </div>

        </div>
    </div>

</asp:Content>
