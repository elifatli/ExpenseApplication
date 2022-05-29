<%@ Page Title="Execute Expense" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExecuteExpense.aspx.cs" Inherits="ExpenseApplication.ExpenseApplicationPages.ExecuteExpense" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <hr />

    <div class="form-horizontal" id="ExecuteSuccess" runat="server" visible="false">
        <div class="form-group">
            <div class="col-md-1 control-label ">
                <i class="fa fa-check-circle-o fa-4x text-success" aria-hidden="true"></i>
            </div>
            <div style="font-size:25px;">
                <p class="col-md-11 control-label text-success" style="text-align:left!important">
                    Your transaction has been successfully submitted to administrator approval.
                </p>
            </div>
        </div>
    </div>

    <div class="form-horizontal" id="ExecuteError" runat="server" visible="false">
        <div class="form-group">
            <div class="col-md-1 control-label ">
                <i class="fa fa-times-circle-o fa-4x text-danger" aria-hidden="true"></i>
            </div>
            <div style="font-size:25px;">
                <p class="col-md-11 control-label text-danger" style="text-align:left!important">
                    Your transaction has failed, please try again.
                </p>
            </div>
        </div>
    </div>

</asp:Content>
