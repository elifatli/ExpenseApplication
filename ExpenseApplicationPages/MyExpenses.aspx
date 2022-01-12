<%@ Page Title="My Expenses List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyExpenses.aspx.cs" Inherits="ExpenseApplication.ExpenseApplicationPages.MyExpenses" %>


<%@ Import Namespace="ExpenseApplication.Helpers" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <hr />
    <div runat="server" id="ErrorMessageBlock" class="alert alert-dismissible alert-danger" visible="false">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <asp:Label runat="server" ID="ExceptionLabel"></asp:Label>
    </div>
    <p class="text-info" style="font-size: 16px;">
        The expenses you have submitted are as below. You can resubmit rejected expenses.
    </p>
    <hr />
    <br />

    <div class="container">
        <div>
            <asp:GridView ID="MyExpensesGridview" runat="server" CssClass="table table-striped table-hover" GridLines="None" PageSize="5" AllowPaging="true" 
                EmptyDataText="You have not any submited expenses." EmptyDataRowStyle-HorizontalAlign="Center" 
                EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-CssClass="text-info"
                ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" OnPageIndexChanging="MyExpensesGridview_PageIndexChanging" PagerStyle-CssClass="gridViewPager">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="ExpenseFormID" Value='<%# Eval("ID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CreateDate" DataFormatString="{0:dd/MM/yyyy}"
                        HeaderText="Submit Date" />
                    <asp:BoundField DataField="Description"
                        HeaderText="Description" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# DBHelper.GetStatusByID((int)Eval("Status")).Description%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RejectReason"
                        HeaderText="Reject Reason" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="DetailButton" OnClick="DetailButton_Click"
                                CssClass="fa fa-file-text-o fa-2x text-primary" Font-Underline="false" CausesValidation="false"
                                aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Display Expense Items"
                                data-original-title="Tooltip on top"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="EditListButton" Visible='<%# ((int)Eval("Status"))==(int)Parameters.ExpenseStatusEnum.Repair %>' 
                                OnClick="EditListButton_Click"
                                CssClass="fa fa-pencil-square-o fa-2x text-primary"
                                Font-Underline="false" CausesValidation="false"
                                aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit Expense Items"
                                data-original-title="Tooltip on top"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Mode="Numeric" PageButtonCount="4" />

            </asp:GridView>
        </div>
    </div>

    <div class="modal" id="ExpenseDetailModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Expense Detail</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <fieldset>
                            <asp:GridView ID="DetailGridview" runat="server" CssClass="table table-striped table-hover" GridLines="None"
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

                        </fieldset>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>

    </div>


</asp:Content>
