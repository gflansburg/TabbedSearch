<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditTab.ascx.cs" Inherits="Gafware.Modules.TabbedSearch.EditTab" %>
<table class="EditTab">
    <tr>
        <td id="cellDelete" runat="server"><asp:ImageButton ID="deleteButton" Width="16px" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/DeleteIcon1_16px.gif" AlternateText="Delete Tab" ToolTip="Delete Tab" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete this tab?');" OnClick="deleteButton_Click" /></td>
        <td id="cellDefault" runat="server"><asp:ImageButton ID="defaultButton" Width="16px" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/tick_unchecked.png" AlternateText="Default Tab" ToolTip="Default Tab" CommandName="Default" Text="Default" OnClick="defaultButton_Click" /></td>
        <td id="cellEdit" runat="server"><asp:TextBox id="txtTabName" Width="165px" MaxLength="100" runat="server"></asp:TextBox></td>
        <td id="cellLabel" runat="server" class="EditTabLabel"><asp:Label id="lblTabName" CssClass="EditTabSpan" runat="server"></asp:Label></td>
    </tr>
</table>
<asp:LinkButton ID="lnkPostBack" runat="server" OnClick="lnkPostBack_Click" />