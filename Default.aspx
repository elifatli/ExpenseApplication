<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExpenseApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h2>Welcome to Expense Application</h2>
        <p class="lead">Hello, <%: CurrentUser != null ? CurrentUser.Name : "Stranger"  %> !</p>

    </div>
    <%if (CurrentUser != null)
        { %>
    <div class="row">
        <div class="card col-md-4">
            <img class="card-img-top" src="/Content/img/addexpense.png" alt="Add Expense">
            <div class="card-body">
                <h4 class="card-title">Add Expense</h4>
                <p class="card-text">You can submit your expenses to manager approval.</p>
                <a href="~/ExpenseApplicationPages/AddExpense" runat="server" class="btn btn-primary card-btn-margin">GO!..</a>
            </div>
        </div>
        <div class="card col-md-4">
            <img class="card-img-top" src="/Content/img/ListExpense.jpg" alt="List Expense">
            <div class="card-body">
                <h4 class="card-title">List</h4>
                <p class="card-text">You can display the status of expenses and change the status of expenses when necessary.</p>
                <div class="btn-group">
                    <a href="#" class="btn btn-primary">SELECT!..</a>
                    <a href="#" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <%if (CurrentUser.Role.CanInsert)
                            { %>
                        <li><a runat="server" href="~/ExpenseApplicationPages/MyExpenses">My Expenses</a></li>
                        <%} %>

                        <%if (CurrentUser.Role.CanApprove)
                            { %>
                        <li><a runat="server" href="~/ExpenseApplicationPages/PendingApprovalExpenses">Approval Pending Expenses</a></li>
                        <%} %>

                        <%if (CurrentUser.Role.CanPay)
                            { %>
                        <li><a runat="server" href="~/ExpenseApplicationPages/PendingPaymentExpenses">Payment Pending Expenses</a></li>
                        <%} %>
                    </ul>
                </div>
            </div>
        </div>
        <div class="card col-md-4">
            <img class="card-img-top" src="/Content/img/ExpenseReport.jpg" alt="Report Expense">
            <div class="card-body">
                <h4 class="card-title">Report</h4>
                <p class="card-text">You can observe the required reporting on expenses.</p>
                <div class="btn-group card-btn-margin">
                    <a href="#" class="btn btn-primary">SELECT!..</a>
                    <a href="#" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <%if (CurrentUser.Role.CanInsert)
                            { %>
                        <li><a runat="server" href="~/ExpenseApplicationPages/MyExpenseReports">My Expense Reports</a></li>
                        <%} %>

                        <%if (CurrentUser.Role.CanApprove)
                            { %>
                        <li><a runat="server" href="~/ExpenseApplicationPages/ApprovedExpenseReports">Approved Expense Reports</a></li>
                        <%} %>

                        <%if (CurrentUser.Role.CanPay)
                            { %>
                        <li><a runat="server" href="~/ExpenseApplicationPages/EmployeeExpenseReports">Employee Expense Reports </a></li>
                        <%} %>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <%} %>

</asp:Content>
