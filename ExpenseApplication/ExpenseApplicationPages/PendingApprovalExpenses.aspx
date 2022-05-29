<%@ Page Title="Approval Pending Expenses List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PendingApprovalExpenses.aspx.cs" Inherits="ExpenseApplication.ExpenseApplicationPages.PendingApprovalExpenses" %>


<%@ Import Namespace="ExpenseApplication.Helpers" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <hr />
    <div runat="server" id="ErrorMessageBlock" class="alert alert-dismissible alert-danger" visible="false">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <asp:Label runat="server" ID="ExceptionLabel"></asp:Label>
    </div>
    <p class="text-info" style="font-size: 16px;">
        Review, approve or deny expenses.
    </p>
    <hr />
    <br />

    <div class="container">
        <div>
            <asp:GridView ID="ApprovalExpensesGridview" runat="server" CssClass="table table-striped table-hover" GridLines="None" PageSize="5" AllowPaging="true"
                EmptyDataText="You have not any pending approval expenses." EmptyDataRowStyle-HorizontalAlign="Center"
                EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-CssClass="text-info"
                ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" OnPageIndexChanging="ApprovalExpensesGridview_PageIndexChanging" PagerStyle-CssClass="gridViewPager">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="ExpenseFormID" Value='<%# Eval("ID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Person">
                        <ItemTemplate>
                            <%# $"{(Eval("AspNetUser") as ExpenseApplication.AspNetUser).Name} {(Eval("AspNetUser") as ExpenseApplication.AspNetUser).Surname}" %>
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
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="DetailButton" OnClick="DetailButton_Click"
                                CssClass="fa fa-file-text-o fa-2x text-primary" Font-Underline="false" CausesValidation="false"
                                aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Display Expense Items"
                                data-original-title="Tooltip on top"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="ApproveButton" OnClick="GridviewApproveButton_Click"
                                CssClass="fa fa-check-circle-o fa-2x text-primary" Font-Underline="false" CausesValidation="false"
                                aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Approve Expenses"
                                data-original-title="Tooltip on top"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="RejectButton" OnClick="GridviewRejectButton_Click"
                                CssClass="fa fa-times-circle-o fa-2x text-primary"
                                Font-Underline="false" CausesValidation="false"
                                aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Reject Expenses"
                                data-original-title="Tooltip on top"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Mode="Numeric" PageButtonCount="4" />

            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="SelectedRowID" runat="server" />
    <%-- Expense Items Modal --%>
    <div class="modal" id="ApproveExpenseDetailModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Expense Detail</h4>
                </div>
                <div class="modal-body">
                    <div runat="server" id="FormGridview" class="form-horizontal">
                        <fieldset>
                            <asp:GridView ID="ApproveDetailGridview" runat="server" CssClass="table table-striped table-hover" GridLines="None"
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
                            <div runat="server" id="txtRejectReason" visible="false" class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="RejectReason" Text="Reject Reason" CssClass="col-lg-3 control-label"></asp:Label>
                                <div class="col-lg-8">
                                    <asp:TextBox runat="server" ID="RejectReason" MaxLength="150" TextMode="MultiLine" CssClass="form-control"
                                        placeholder="Enter your reason for refusal and press the required button to continue." />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="RejectReason" ValidationGroup="RejectExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please enter explanatory expense description." />
                                </div>
                            </div>

                        </fieldset>
                    </div>
                    <div class="form-horizontal" id="ExecuteSuccess" runat="server" visible="false">
                        <div class="form-group">
                            <div class="col-md-1 control-label ">
                                <i class="fa fa-check-circle-o fa-4x text-success" aria-hidden="true"></i>
                            </div>
                            <div style="font-size: 25px;">
                                <p class="col-md-11 control-label text-success" style="text-align: left!important">
                                    Your transaction has been successfully completed.
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="form-horizontal" id="ExecuteError" runat="server" visible="false">
                        <div class="form-group">
                            <div class="col-md-1 control-label ">
                                <i class="fa fa-times-circle-o fa-4x text-danger" aria-hidden="true"></i>
                            </div>
                            <div style="font-size: 25px;">
                                <p class="col-md-11 control-label text-danger" style="text-align: left!important">
                                    Your transaction has failed, please try again later.
                                </p>
                            </div>
                        </div>
                    </div>
            </div>

            <div class="modal-footer">
                <button runat="server" id="CloseButton" type="button" class="btn btn-warning" data-dismiss="modal">Close</button>
                <asp:Button runat="server" ID="ApprovalButton" OnClick="ApprovalButton_Click" CssClass="btn btn-success" Text="Approve" />
                <asp:Button runat="server" ID="RepairButton" ValidationGroup="RejectExpenseGroup" OnClick="RepairButton_Click" CssClass="btn btn-info" Text="Send Repair" />
                <asp:Button runat="server" ID="RejectButton" ValidationGroup="RejectExpenseGroup" OnClick="RejectButton_Click" CssClass="btn btn-danger" Text="Reject" />
            </div>
        </div>

    </div>

    </div>
</asp:Content>
