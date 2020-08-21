<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AddEditUpBlock.aspx.cs" Inherits="PBALBS.AddEditUpBlock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
        function Validation(field) {
            var arr = field.parentElement.parentElement.getElementsByTagName("input");
            if (arr.length == 1) {
                if (arr[0].type == 'text') {
                    if (arr[0].value == '') {
                        alert('Block Name Required');
                        arr[0].focus();
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }

        }

        function Exchange_Validation() {
            var count = Count_Checkbox_Checked();
            if (count != 2) {
                alert('Exchange Can Only Be Done Within 2 Items. Please Verify.');
                return false;
            }
            else { return true; }
        }
        function Checkbox_Check_Validation(checkbox) {
            var count = Count_Checkbox_Checked();
            if (checkbox.checked == true) {
                if (count > 2) {
                    checkbox.checked = false;
                    alert('Exchange Can Only Be Done Within 2 Items. Please Uncheck 1 Item.');
                }
            }
        }
        function Count_Checkbox_Checked() {
            var gv = document.getElementById('<%=dgvBlock.ClientID %>');
            var arr = gv.getElementsByTagName('input');
            var count = 0;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].type == 'checkbox') {
                    if (arr[i].checked == true) {
                        count += 1;
                    }
                }
            }

            return count;
        }
</script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="Script1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UP1" runat="server">
    <ContentTemplate>
    <table width="60%" align="center">
        <tr>
            <td align="right">
                <asp:Button ID="btnExchange" runat="server" CssClass="button" Text="Exchange" 
                    OnClientClick="javascript:return Exchange_Validation();" 
                    onclick="btnExchange_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="dgvBlock" runat="server" Width="100%" AllowPaging="false" 
                    AutoGenerateColumns="false" GridLines="none" BorderColor="#1398ED" 
                    BorderStyle="Solid" BorderWidth="1px"
                    DataKeyNames="BlockId,DistrictId" onrowcommand="dgvBlock_RowCommand" 
                    onrowdatabound="dgvBlock_RowDataBound" onrowdeleting="dgvBlock_RowDeleting">
                 <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkSelect" runat="server" onclick="javascript:Checkbox_Check_Validation(this);" /> 
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                   
                     
                     <asp:TemplateField HeaderText="Block">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBlock" runat="server" Width="250px" CssClass="textbox" Text='<%#Bind("BlockName") %>' ToolTip='<%#Bind("BlockName") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                     
                     <asp:TemplateField HeaderText="Potato Type" >
                        <ItemTemplate>
                            <asp:DropDownList ID="dd2PotatoType" runat="server" CssClass="dropdown" DataValueField="PotatoTypeId" DataTextField="PotatoTypeName"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>


                    
                    <asp:TemplateField HeaderText="District">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="dropdown" DataValueField="DistrictId" DataTextField="DistrictName"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="AddUpdate" OnClientClick="Javascript:return Validation(this);"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                     
                 </Columns>
                 <HeaderStyle CssClass="HeaderStyle" />
                 <RowStyle CssClass="RowStyle" />
                 <AlternatingRowStyle CssClass="AltRowStyle" />
                 </asp:GridView>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
