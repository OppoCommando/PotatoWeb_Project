<%@ Page Title="Add Edit UP PotatoRate" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AddEditUPPotatoRate.aspx.cs" Inherits="PBALBS.AddEditUPPotatoRate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script runat="server">

    
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="Script1" runat="server">
   </asp:ToolkitScriptManager>
   
   <asp:UpdatePanel ID="UP1" runat="server" >
        <ContentTemplate>
            <%--<table width="95%" align="center">
                <tr>
                    <td align="center" style="font-family:Tahoma; font-weight:8px; color:Red; font-weight:bold;">
                        If any of the rate is 0, then it is not mandatory to fill all the textbox with 000. If you leave any textbox blank, then that rate will be considered as 0.
                    </td>
                </tr>
            </table>--%>
            <table width="45%" align="center">
                <tr id="trMsg" runat="server">
                    <td align="center">
                        <b>Rate Updated Successfully</b>
                    </td>
                </tr>
                
                <tr>
                    <td align="center">
                        <table width="90%" align="center">
                            <tr>
                                <td align="left" width="40%"><b>Select a Date:</b></td>
                                <td align="left" width="10%">
                                    <asp:DropDownList ID="ddlDay" runat="server" CssClass="dropdown">
                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                        <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                        <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                        <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                        <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                        <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                        <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                        <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                        <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                        <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                        <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                        <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                        <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                        <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                        <asp:ListItem Value="26" Text="26"></asp:ListItem>
                                        <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                        <asp:ListItem Value="28" Text="28"></asp:ListItem>
                                        <asp:ListItem Value="29" Text="29"></asp:ListItem>
                                        <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                        <asp:ListItem Value="31" Text="31"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left" width="10%">
                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown">
                                        <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left" width="10%">
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown">
                                        <asp:ListItem Value="2012" Text="2012"></asp:ListItem>
                                        <asp:ListItem Value="2013" Text="2013"></asp:ListItem>
                                        <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                                        <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                                        <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                                        <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                                        <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                                        <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                                        <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="10%" align="left">
                                    <asp:Button ID="btnGetPrice" runat="server" CssClass="button" Text="Get Price" 
                                        onclick="btnGetPrice_Click" />
                                </td>
                                <td width="20%"></td>
                            </tr>
                             <tr>
                                <td align="left" width="40%"><b>Potato Type:</b></td>
                                <td align="left" width="10%">
                                   <asp:DropDownList ID="dd2PotatoType" runat="server" CssClass="dropdown" DataValueField="PotatoTypeId" DataTextField="PotatoTypeName"></asp:DropDownList>
                                </td>
                                
                            </tr>


                        </table>
                    </td>
                </tr>

             




                
                <tr>
                    <td align="center">
                        <table width="100%" align="center">
                          <tr>
                           <td align="center" width="90%">  
                        <asp:GridView ID="dgvBlock" runat="server" AutoGenerateColumns="false" Width="80%" AllowPaging="false"
                         GridLines="none" BorderColor="#1398ED" 
                        BorderStyle="Solid" BorderWidth="1px" DataKeyNames="BlockId">
                            <Columns>
                                <asp:BoundField DataField="BlockName" HeaderText="Block" />
                               
                                <asp:TemplateField HeaderText="Low" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBond" runat="server" CssClass="textbox" Width="50px" MaxLength="4" Text='<%#Bind("Bond") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftb1" runat="server" TargetControlID="txtBond" FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="High" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAvg" runat="server" CssClass="textbox" Width="50px" MaxLength="4" Text='<%#Bind("Avg") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftb2" runat="server" TargetControlID="txtAvg" FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dala" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDala" runat="server" CssClass="textbox" Width="50px" MaxLength="4" Text='<%#Bind("Dala") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftb3" runat="server" TargetControlID="txtDala" FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="HeaderStyle" />
                            <RowStyle CssClass="RowStyle" />
                            <AlternatingRowStyle CssClass="AltRowStyle" />
                        </asp:GridView>
                       </td>
                       <td width="10%" align="center">
                            <asp:Button ID="btnPartialSave" runat="server" CssClass="button" 
                                Text="Save Now" Height="100px" onclick="btnPartialSave_Click" />
                       </td> 
                       </tr>
                      </table> 
                    </td>
                </tr>
                <tr><td><br /></td></tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" 
                            onclick="btnSave_Click" />
                        &nbsp;
                        <asp:Button ID="btnSaveAndSend" runat="server" CssClass="button" 
                            Text="Save & Send SMS" onclick="btnSaveAndSend_Click" />
                        &nbsp;
                        <asp:Button ID="btnReset" runat="server" CssClass="button" Text="Cancel" 
                            onclick="btnReset_Click" />
                        
                    </td>
                </tr>
            </table>
        </ContentTemplate>
   </asp:UpdatePanel> 
</asp:Content>
