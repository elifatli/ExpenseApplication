<%@ Page Title="Confirm Expense" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmExpense.aspx.cs" Inherits="ExpenseApplication.ExpenseApplicationPages.ConfirmExpense" %>

<%@ Import Namespace="ExpenseApplication.Helpers" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <hr />
    <div runat="server" id="ErrorMessageBlock" class="alert alert-dismissible alert-danger" visible="false">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <asp:Label runat="server" ID="ExceptionLabel"></asp:Label>
    </div>
    <p class="text-info" style="font-size:16px;">
        Please click on the confirm button to send your expenses for approval. If you want to make changes to your expenses, click back button.
    </p>
    <hr />
    <br />

    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmDescriptionLiteral" Text="Description" CssClass="col-md-1 control-label"></asp:Label>

            <div class="col-md-2 control-label" style="text-align:left!important">
                <asp:Literal ID="ConfirmDescriptionLiteral" runat="server" />
            </div>
        </div>

        <div>
            <asp:GridView ID="ConfirmGridview" runat="server" CssClass="table table-striped table-hover" GridLines="None"
                ShowHeaderWhenEmpty="true" ShowFooter="true" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <%# DBHelper.GetExpenseTypeByID((int)Eval("ExpenseType")).Name%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ExpenseDate" DataFormatString="{0:dd/MM/yyyy}"
                        HeaderText="Date" />
                    <asp:BoundField DataField="BillNumber"
                        HeaderText="Bill No" />
                    <asp:BoundField DataField="Description"
                        HeaderText="Description" />
                    <asp:BoundField DataField="Cost" DataFormatString="{0:C}" HeaderStyle-CssClass="cellAlignCenter" ItemStyle-CssClass="cellAlignRight" ItemStyle-Width="50"
                        HeaderText="Cost" />
                </Columns>
            </asp:GridView>
        </div>

        <div class="form-group">
            <div class="col-md-offset-10">
                <asp:Button runat="server" ID="ConfirmBack" Text="Back" OnClick="ConfirmBack_Click" CssClass="btn btn-warning" />
                <asp:Button runat="server" ID="ConfirmButton" Text="Confirm" OnClick="ConfirmButton_Click" CssClass="btn btn-success" />
            </div>
        </div>
    </div>


</asp:Content>
