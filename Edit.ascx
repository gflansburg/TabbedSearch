<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Gafware.Modules.TabbedSearch.Edit" %>
<%@ Register Assembly="Gafware.TabbedSearch" Namespace="Gafware.Modules.TabbedSearch" TagPrefix="cc1" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/UrlControl.ascx" %>
<div class="dnnEditTabs dnnForm dnnClear">
    <h2 id="dnnSitePanel-TabSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%= LocalizeString("TabSettings") %></a></h2>
    <fieldset class="tabs-fieldset">
	    <div style="display: inline-block; width: 640px; border: 1px solid #ddd; vertical-align: top; padding-right: 5px;" id="tabContent" runat="server">
            <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%= LocalizeString("BasicSettings") %></a></h2>
            <fieldset class="tabs-fieldset">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblParameterName" runat="server" ResourceKey="lblParameterName" ControlName="txtParameterName" Suffix=":" /> 
                    <asp:TextBox ID="txtParameterName" runat="server" MaxLength="20" Text="q" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="SearchTabs" />
                    <asp:RequiredFieldValidator ID="rfvParameterName" runat="server" ControlToValidate="txtParameterName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="SearchTabs" ErrorMessage="Please enter the parameter name for your search query." />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblSearchUrl" runat="server" ResourceKey="lblSearchUrl" ControlName="ctlSearchUrl" Suffix=":" /> 
                    <dnn:Url ID="ctlSearchUrl" runat="server" Required="true" ShowDatabase="false" ShowFiles="false"  ShowImages="false" ShowLog="false" ShowNewWindow="false" ShowNone="false" ShowSecure="false" ShowTabs="true" ShowTrack="false" ShowUpLoad="false" ShowUrls="true" ShowUsers="false" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblTarget" runat="server" ResourceKey="lblTarget" ControlName="lstTarget" Suffix=":" /> 
                    <asp:DropDownList ID="lstTarget" runat="server" ValidationGroup="SearchTabs">
                        <asp:ListItem Text="New Window" Value="_blank" />
                        <asp:ListItem Text="Same Window" Value="_self" Selected="True" />
                        <asp:ListItem Text="Parent Frameset" Value="_parent" />
                        <asp:ListItem Text="Full Body of Window" Value="_top" />
                    </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblParameterless" runat="server" ResourceKey="lblParameterless" ControlName="rblParameterless" Suffix=":" /> 
                    <asp:RadioButtonList ID="rblParameterless" runat="server" RepeatDirection="Horizontal" ValidationGroup="SearchTabs">
                        <asp:ListItem Text="Yes" Selected="True" Value="True"></asp:ListItem>
                        <asp:ListItem Text="No" Value="False"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </fieldset>
            <h2 id="dnnSitePanel-AutoCompleteSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%= LocalizeString("AutoCompleteSettings") %></a></h2>
            <fieldset class="tabs-fieldset">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblAutoCompleteParameterName" runat="server" ResourceKey="lblAutoCompleteParameterName" ControlName="txtAutoCompleteParameterName" Suffix=":" /> 
                    <asp:TextBox ID="txtAutoCompleteParameterName" runat="server" MaxLength="20" ValidationGroup="SearchTabs" CssClass="NormalTextBox" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblAutoCompleteUrl" runat="server" ResourceKey="lblAutoCompleteUrl" ControlName="txtAutoCompleteUrl" Suffix=":" /> 
                    <asp:TextBox ID="txtAutoCompleteUrl" runat="server" MaxLength="255" ValidationGroup="SearchTabs" CssClass="NormalTextBox" />
                    <asp:RegularExpressionValidator ID="revAutoCompleteUrl" runat="server" ErrorMessage="Please enter a properly formated URL." ValidationExpression="^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(#(.*))?" ControlToValidate="txtAutoCompleteUrl" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="SearchTabs"></asp:RegularExpressionValidator>
                </div>
            </fieldset>
            <h2 id="dnnSitePanel-CustomSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%= LocalizeString("CustomSettings") %></a></h2>
            <fieldset class="tabs-fieldset">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCustomName" runat="server" ResourceKey="lblCustomName" ControlName="txtCustomName" Suffix=":" /> 
                    <asp:TextBox ID="txtCustomName" runat="server" MaxLength="20" ValidationGroup="SearchTabs" CssClass="NormalTextBox" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCustomValue" runat="server" ResourceKey="lblCustomValue" ControlName="txtCustomValue" Suffix=":" /> 
                    <asp:TextBox ID="txtCustomValue" runat="server" MaxLength="50" ValidationGroup="SearchTabs" CssClass="NormalTextBox" />
                </div>
            </fieldset>
	    </div>
	    <div style="display: inline-block; width: 220px; vertical-align: top; margin-left: -4px;">
		    <div class="TabNames" id="divTabs" runat="server" style="width: 220px; overflow: hidden;" />
		    <div class="AddNewTabLink">
			    <asp:LinkButton ID="btnAddTab" runat="server" Text="Add New Tab" CssClass="dnnSecondaryAction" ValidationGroup="SearchTabs" CausesValidation="true" OnClick="btnAddTab_Click" />
		    </div>
	    </div>
	</fieldset>
    <h2 id="dnnSitePanel-LinkSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%= LocalizeString("LinkSettings") %></a></h2>
    <fieldset class="tabs-fieldset">
        <div class="dnnFormItem">
            <dnn:Label ID="lblQuickLinks" runat="server" ResourceKey="lblQuickLinks" ControlName="rblQuickLinks" Suffix=":" /> 
            <asp:RadioButtonList ID="rblQuickLinks" runat="server" RepeatDirection="Horizontal" ValidationGroup="SearchTabs" AutoPostBack="true" OnSelectedIndexChanged="rblQuickLinks_SelectedIndexChanged">
                <asp:ListItem Text="Yes" Selected="True" Value="True"></asp:ListItem>
                <asp:ListItem Text="No" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <cc1:GridViewExtended ID="gvQuickLinks" runat="server" CssClass="QuickLinks" AutoGenerateColumns="False" DataKeyNames="LinkId" OnRowEditing="gvQuickLinks_RowEditing" RowStyle-BackColor="#eeeeee" ShowFooterWhenEmpty="True" ShowHeaderWhenEmpty="True"
            RowStyle-Height="18" HeaderStyle-Height="30" GridLines="None" Font-Names="Arial" Font-Size="Small" CellPadding="4" ShowFooter="True" OnRowCommand="gvQuickLinks_RowCommand" ForeColor="#333333" OnRowUpdating="gvQuickLinks_RowUpdating" OnRowCancelingEdit="gvQuickLinks_RowCancelingEdit" OnRowDataBound="gvQuickLinks_RowDataBound" OnRowDeleting="gvQuickLinks_RowDeleting">
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle VerticalAlign="Top" Font-Names="Arial" Font-Size="Small" BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle Font-Names="Arial" Font-Size="Small" BackColor="#FFFFFF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Font-Size="Small" Font-Names="Arial" ForeColor="White" Font-Bold="True" />
            <AlternatingRowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"> 
                    <ItemTemplate>
                        <asp:ImageButton ID="editButton" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/EditIcon1_16px.gif" AlternateText="Edit Link" ToolTip="Edit Link" CommandName="Edit" Text="Edit" CausesValidation="false" /> 
                        <asp:ImageButton ID="deleteButton" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/DeleteIcon1_16px.gif" AlternateText="Delete Link" ToolTip="Delete Link" CommandName="Delete" Text="Delete" CausesValidation="false" OnClientClick="return confirm('Are you sure you wish to delete this link?')" /> 
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:ImageButton ID="saveButton" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/save.gif" AlternateText="Save Link" ToolTip="Save Link" CommandName="Update" Text="Update" ValidationGroup="QuickLinks" CausesValidation="true" /> 
                        <asp:ImageButton ID="cancelButton" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/DeleteIcon1_16px.gif" AlternateText="Cancel Edit" ToolTip="Cancel Edit" CommandName="Cancel" Text="Cancel" CausesValidation="false" /> 
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="newButton" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/NewIcon1_16px.gif" AlternateText="New Link" ToolTip="New Link" CommandName="New" Text="New" /> 
                        <asp:ImageButton ID="saveButton2" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/save.gif" AlternateText="Save Link" ToolTip="Save Link" CommandName="Insert" Text="Insert" ValidationGroup="QuickLinks" CausesValidation="true" Visible="false" /> 
                        <asp:ImageButton ID="cancelButton2" runat="server" ImageUrl="~/DesktopModules/Gafware/TabbedSearch/images/DeleteIcon1_16px.gif" AlternateText="Cancel Edit" ToolTip="Cancel Edit" CommandName="Cancel" Text="Cancel" CausesValidation="false" Visible="false" /> 
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Text" ItemStyle-Wrap="false" ItemStyle-Width="200px">
                    <ItemTemplate>
                        <%# Eval("LinkText") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLinkText" runat="server" Text='<%# Eval("LinkText") %>' MaxLength="100" Width="100%" ValidationGroup="QuickLinks"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtLinkText2" runat="server" MaxLength="100" Width="100%" ValidationGroup="QuickLinks"></asp:TextBox>
                    </FooterTemplate>
                    <ItemStyle Wrap="False" Width="200px"></ItemStyle>
                    <HeaderStyle Wrap="False" Width="200px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Url" ItemStyle-Width="400px">
                    <ItemTemplate>
                        <%# GetUrl(Eval("Url").ToString()) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <dnn:Url ID="ctlLinkUrl" runat="server" Required="true" ShowDatabase="false" ShowFiles="false"  ShowImages="false" ShowLog="false" ShowNewWindow="false" ShowNone="false" ShowSecure="false" ShowTabs="true" ShowTrack="false" ShowUpLoad="false" ShowUrls="true" ShowUsers="false" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <dnn:Url ID="ctlLinkUrl2" runat="server" Required="true" ShowDatabase="false" ShowFiles="false"  ShowImages="false" ShowLog="false" ShowNewWindow="false" ShowNone="false" ShowSecure="false" ShowTabs="true" ShowTrack="false" ShowUpLoad="false" ShowUrls="true" ShowUsers="false" />
                    </FooterTemplate>
                    <ItemStyle Width="400px"></ItemStyle>
                    <HeaderStyle Width="400px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Target" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# GetTargetName(Eval("Target").ToString()) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="lstTarget2" runat="server" ValidationGroup="QuickLinks">
                            <asp:ListItem Text="New Window" Value="_blank" />
                            <asp:ListItem Text="Same Window" Value="_self" />
                            <asp:ListItem Text="Parent Frameset" Value="_parent" />
                            <asp:ListItem Text="Full Body of Window" Value="_top" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="lstTarget3" runat="server" ValidationGroup="QuickLinks">
                            <asp:ListItem Text="New Window" Value="_blank" />
                            <asp:ListItem Text="Same Window" Value="_self" Selected="True" />
                            <asp:ListItem Text="Parent Frameset" Value="_parent" />
                            <asp:ListItem Text="Full Body of Window" Value="_top" />
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                    <HeaderStyle Wrap="False"></HeaderStyle>
                </asp:TemplateField>
            </Columns>
            <sortedascendingcellstyle backcolor="#F5F7FB" />
            <sortedascendingheaderstyle backcolor="#6D95E1" />
            <sorteddescendingcellstyle backcolor="#E9EBEF" />
            <sorteddescendingheaderstyle backcolor="#4870BE" />
        </cc1:GridViewExtended>
    </fieldset>
    <div id="divCommands" class="dnnClear commands">
        <ol id="olCommands" class="twitter-list">
            <li class="tabs-listitem">
			    <asp:LinkButton ID="cmdCancelEdit" runat="server" Text="Cancel" CssClass="dnnSecondaryAction" CausesValidation="false" OnClick="cmdCancelEdit_Click" />
            </li>
            <li class="tabs-listitem">
                <asp:LinkButton ID="cmdCloseEdit" runat="server" Text="Save &amp; Close" ValidationGroup="SearchTabs" CausesValidation="true" OnClick="cmdCloseEdit_Click" CssClass="dnnPrimaryAction" />
            </li>
        </ol>
    </div>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/

    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('.dnnEditTabs').dnnPanels();
            jQuery('select, input:text').jqTransform();
        }

        $(document).ready(function () {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnSiteSettings();
            });
        });

    }(jQuery, window.Sys));
    /*]]>*/
</script>