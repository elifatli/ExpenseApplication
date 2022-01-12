<%@ Page Title="My Expense Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyExpenseReports.aspx.cs" Inherits="ExpenseApplication.ExpenseApplicationPages.MyExpenseReports" %>

<%@ Import Namespace="ExpenseApplication.Helpers" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <hr />
    <div runat="server" id="ErrorMessageBlock" class="alert alert-dismissible alert-danger" visible="false">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <asp:Label runat="server" ID="ExceptionLabel"></asp:Label>
    </div>
    <p class="text-info" style="font-size: 16px;">
        Please, select a date to view the expenses you have made.
    </p>
    <hr />
    <br />
    <div class="container">
        <div class="row">
            <div class="col-md-offset-5">
                <asp:RadioButton ID="MonthRadioButton" Text="Monthly" GroupName="ReportTypeGroup" runat="server" Font-Size="Large" CssClass="col-md-3"
                    AutoPostBack="true" OnCheckedChanged="MonthRadioButton_CheckedChanged" />
                <asp:RadioButton ID="YearRadioButton" Text="Annual" GroupName="ReportTypeGroup" runat="server" Font-Size="Large" 
                    AutoPostBack="true" OnCheckedChanged="YearRadioButton_CheckedChanged" />
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
                    <asp:DropDownList runat="server" ID="ReportYear" CssClass="form-control" OnSelectedIndexChanged="ReportYear_SelectedIndexChanged" AutoPostBack="true"/>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ReportYear" ValidationGroup="ReportGroup" CssClass="text-danger" ErrorMessage="Please select year." />
                </div>
            </div>
           <br />
            <div class="col-lg-12">
                <asp:Button ID="DownloadReportButton" runat="server" OnClick="DownloadReportButton_Click" CssClass="btn btn-success buttonright" Text="Download" />
            </div>
        </div>
    </div>




</asp:Content>
