<%@ Page Title="Add Expense" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddExpense.aspx.cs" Inherits="ExpenseApplication.ExpenseApplicationPages.AddExpense" %>

<%@ Import Namespace="ExpenseApplication.Helpers" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <hr />
    <div runat="server" id="ErrorMessageBlock" class="alert alert-dismissible alert-danger" visible="false">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <asp:Label runat="server" ID="ExceptionLabel"></asp:Label>
    </div>

    <p class="text-info" style="font-size: 16px;">
        Please enter your expenses and click continue button to go on.
    </p>
    <hr />
    <br />
    <div class="form-horizontal">
        <div class="form-group required">
            <asp:Label runat="server" AssociatedControlID="txtDescription" Text="Description" CssClass="col-md-1 control-label"></asp:Label>
            <div class="col-md-11 ">
                <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control"
                    placeholder="Enter your overall expenses description" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ExpenseFormGroup"
                    ControlToValidate="txtDescription" CssClass="text-danger" ErrorMessage="Please enter description." />
            </div>
        </div>

        <div>
            <asp:GridView ID="AddExpenseGridview" runat="server" CssClass="table table-striped table-hover" GridLines="None"
                EmptyDataText="You have not any additional expenses." EmptyDataRowStyle-HorizontalAlign="Center"
                EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-CssClass="text-info" OnRowDataBound="AddExpenseGridview_RowDataBound"
                ShowHeaderWhenEmpty="true" ShowFooter="true" OnLoad="AddExpenseGridview_Load" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="IsDeleted" Value='<%# Eval("IsDeleted") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
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
                    <asp:TemplateField ItemStyle-CssClass="cellAlignCenter">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="DeleteButton" OnClick="DeleteButton_Click"
                                Font-Underline="false" CssClass="fa fa-trash-o fa-2x text-primary" CausesValidation="false"
                                aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Delete"
                                data-original-title="Tooltip on top"></asp:LinkButton>
                            <%--<div style="display:inline-block" data-toggle="modal" data-target="#ExpenseItemEditModal" >--%>
                            <asp:LinkButton runat="server" ID="EditButton" OnClick="EditButton_Click" Font-Underline="false"
                                CausesValidation="false" CssClass="fa fa-pencil-square-o fa-2x text-primary"
                                aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit" data-original-title="Tooltip on top"></asp:LinkButton>
                            <%--</div>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div class="form-group">
            <div class="col-md-offset-10">
                <button type="button" class="btn btn-info" data-toggle="modal" data-target="#ExpenseItemModal">
                    Add Expense
                </button>
                <asp:Button runat="server" ID="Send" Text="Continue" ValidationGroup="ExpenseFormGroup" OnClick="Send_Click" CssClass="btn btn-success" />
            </div>
        </div>
    </div>


    <%-- Expense Item Modal --%>
    <div class="modal" id="ExpenseItemModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Add New Expense</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <fieldset>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="ExpenseType" CssClass="col-lg-3 control-label">Expense Type</asp:Label>
                                <div class="col-lg-6">
                                    <asp:DropDownList runat="server" ID="ExpenseType" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ExpenseType" ValidationGroup="AddExpenseGroup" CssClass="text-danger" ErrorMessage="Please select expense type." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="ExpenseDate" CssClass="col-lg-3 control-label">Expense Date</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" TextMode="Date" ID="ExpenseDate" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ExpenseDate" ValidationGroup="AddExpenseGroup" CssClass="text-danger" ErrorMessage="Please select expense date." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="BillNumber" CssClass="col-lg-3 control-label">Bill Number</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="BillNumber" onkeypress="return AlphaNumeric(event)" MaxLength="8" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="BillNumber" ValidationGroup="AddExpenseGroup" CssClass="text-danger" ErrorMessage="Please enter expense bill number." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="Description" CssClass="col-lg-3 control-label">Description</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="Description" MaxLength="150" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Description" ValidationGroup="AddExpenseGroup" CssClass="text-danger" ErrorMessage="Please enter explanatory expense description." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="Cost" CssClass="col-lg-3 control-label">Cost</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="Cost" MaxLength="10" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Cost" ValidationGroup="AddExpenseGroup" CssClass="text-danger" ErrorMessage="Please enter expense cost." />
                                </div>
                            </div>

                        </fieldset>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="ExpenseItemAdd" ValidationGroup="AddExpenseGroup" CssClass="btn btn-info" Text="Add" OnClick="ExpenseItemAdd_Click" />
                </div>
            </div>

        </div>
    </div>

    <%-- Modal for Edit --%>

    <div class="modal" id="ExpenseItemEditModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Edit Expense</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <fieldset>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="EditExpenseDropdownList" CssClass="col-lg-3 control-label">Expense Type</asp:Label>
                                <div class="col-lg-6">
                                    <asp:DropDownList runat="server" ID="EditExpenseDropdownList" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EditExpenseDropdownList" ValidationGroup="EditExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please select expense type." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="EditExpenseDate" CssClass="col-lg-3 control-label">Expense Date</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" TextMode="Date" ID="EditExpenseDate" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EditExpenseDate" ValidationGroup="EditExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please select expense date." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="EditBillNumber" CssClass="col-lg-3 control-label">Bill Number</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="EditBillNumber" onkeypress="return AlphaNumeric(event)" MaxLength="8" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EditBillNumber" ValidationGroup="EditExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please enter expense bill number." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="EditDescription" CssClass="col-lg-3 control-label">Description</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="EditDescription" MaxLength="150" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EditDescription" ValidationGroup="EditExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please enter explanatory expense description." />
                                </div>
                            </div>
                            <div class="form-group required">
                                <asp:Label runat="server" AssociatedControlID="EditCost" CssClass="col-lg-3 control-label">Cost</asp:Label>
                                <div class="col-lg-6">
                                    <asp:TextBox runat="server" ID="EditCost" MaxLength="10" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EditCost" ValidationGroup="EditExpenseGroup" CssClass="text-danger"
                                        ErrorMessage="Please enter expense cost." />
                                </div>
                            </div>
                            <asp:HiddenField ID="SelectedRowIndex" runat="server" />
                        </fieldset>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="UpdateButton" ValidationGroup="EditExpenseGroup" CssClass="btn btn-info" Text="Edit" OnClick="UpdateButton_Click" />
                </div>
            </div>

        </div>
    </div>




    <script>

        $(function () {
            $('#<%:Cost.ClientID%>').maskMoney();
            $('#<%:EditCost.ClientID%>').maskMoney();
        });
    </script>


</asp:Content>
