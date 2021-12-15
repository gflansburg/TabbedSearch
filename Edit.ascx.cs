/*
' Copyright (c) 2021  Gafware
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/


using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Gafware.Modules.TabbedSearch.Components;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gafware.Modules.TabbedSearch
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditTabbedSearch class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from TabbedSearchModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : TabbedSearchModuleBase
    {
        private const string c_jqTransformKey = "jquery.plugin.jqtransform";

        private readonly INavigationManager _navigationManager;
        
        public Edit()
        {
            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BindData(true);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected string GetTargetName(string target)
        {
            switch (target)
            {
                case "_blank":
                    return "New Window";
                case "_self":
                    return "Same Window";
                case "_parent":
                    return "Parent Frameset";
                case "_top":
                    return "Full Body of Window";
            }
            return String.Empty;
        }

        protected string GetUrl(string url)
        {
            if (View.IsNumber(url))
            {
                return _navigationManager.NavigateURL(Convert.ToInt32(url));
            }
            return url;
        }

        private void BindData(bool loadForm)
        {
            BindData(loadForm, false);
            if (!IsPostBack)
            {
                gvQuickLinks.DataSource = TabbedSearchController.GetLinks(ModuleId);
                gvQuickLinks.DataBind();
                if (Settings.Contains("ShowQuickLinks"))
                {
                    rblQuickLinks.SelectedValue = Settings["ShowQuickLinks"].ToString();
                }
                gvQuickLinks.Visible = (rblQuickLinks.SelectedIndex == 0);
            }
        }
 
        private void BindData(bool loadForm, bool loadContent)
        {
            ImportPlugins();
            List<SearchTab> tabs = TabbedSearchController.GetTabs(ModuleId);
            if (tabs.Count > 0)
            {
                if (SelectedTab == -1)
                {
                    SelectedTab = 0;
                }
            }
            int i = 0;
            divTabs.Controls.Clear();
            foreach (SearchTab tab in tabs)
            {
                EditTab editTab = (EditTab)LoadControl("EditTab.ascx");
                editTab.ID = "EditTab" + i.ToString();
                editTab.Text = tab.TabName;
                editTab.Value = tab.SearchTabId.ToString();
                editTab.Index = i;
                editTab.Default = (i == DefaultTab);
                editTab.Selected = i == SelectedTab;
                editTab.Deleted += editTab_Deleted;
                editTab.Click += editTab_Click;
                editTab.SetDefault += editTab_SetDefault;
                divTabs.Controls.Add(editTab);
                if (loadForm)
                {
                    if (!String.IsNullOrEmpty(Request.Form[editTab.UniqueID + "$txtTabName"]))
                    {
                        editTab.Text = Request.Form[editTab.UniqueID + "$txtTabName"];
                    }
                }
                i++;
            }
            if (!IsPostBack || loadContent)
            {
                txtParameterName.Text = (tabs.Count > 0 ? tabs[SelectedTab].ParameterName : "Search");    
                ctlSearchUrl.Url = (tabs.Count > 0 ? tabs[SelectedTab].SearchUrl : String.Empty);
                txtAutoCompleteParameterName.Text = (tabs.Count > 0 ? tabs[SelectedTab].AutoCompleteParameterName : "q");
                txtAutoCompleteUrl.Text = (tabs.Count > 0 ? tabs[SelectedTab].AutoCompleteUrl : String.Empty);    
                txtCustomName.Text = (tabs.Count > 0 ? tabs[SelectedTab].CustomName : String.Empty);    
                txtCustomValue.Text = (tabs.Count > 0 ? tabs[SelectedTab].CustomValue : String.Empty);    
                lstTarget.SelectedValue = (tabs.Count > 0 ? tabs[SelectedTab].Target : "_self");    
                rblParameterless.SelectedValue = (tabs.Count > 0 ? tabs[SelectedTab].Parameterless.ToString() : "True");    
            }
            tabContent.Visible = (tabs.Count > 0);
        }

        private void ImportPlugins()
        {
            //load the plugin client scripts on every page load
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqTransformKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqTransformKey, String.Format(TabbedSearchController.SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jqTransform/jquery.jqtransform.js")), false);
            }
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl(String.Format(TabbedSearchController.CSS_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jqTransform/jqtransform.css"))));
        }

        void editTab_SetDefault(object sender, EditTab.EditTabEventArgs e)
        {
            DefaultTab = e.Index;
        }

        void editTab_Click(object sender, EditTab.EditTabEventArgs e)
        {
            if(SaveSettings())
            {
                SearchTab tab = TabbedSearchController.GetTab(Convert.ToInt32(e.Value));
                txtParameterName.Text = tab.ParameterName;
                ctlSearchUrl.Url = tab.SearchUrl;
                txtAutoCompleteParameterName.Text = tab.AutoCompleteParameterName;
                txtAutoCompleteUrl.Text = tab.AutoCompleteUrl;
                txtCustomName.Text = tab.CustomName;
                txtCustomValue.Text = tab.CustomValue;
                lstTarget.SelectedValue = tab.Target;
                rblParameterless.SelectedValue = tab.Parameterless.ToString();
                SelectedTab = e.Index;
                BindData(false);
            }
        }

        void editTab_Deleted(object sender, EditTab.EditTabEventArgs e)
        {
            TabbedSearchController.DeleteTab(Convert.ToInt32(e.Value));
            List<SearchTab> tabs = TabbedSearchController.GetTabs(ModuleId);
            if (SelectedTab >= tabs.Count)
            {
                SelectedTab = tabs.Count - 1;
            }
            if (DefaultTab >= tabs.Count)
            {
                DefaultTab = tabs.Count - 1;
            }
            BindData(false, true);
        }

        protected int SelectedTab
        {
            get
            {
                return (ViewState["SelectedTab_" + ModuleId.ToString()] != null
                    ? (int) ViewState["SelectedTab_" + ModuleId.ToString()]
                    : -1);
            }
            set { ViewState["SelectedTab_" + ModuleId.ToString()] = value; }
        }

        static public bool IsNumber(string str)
		{
			bool b = true;
			try
			{
				Convert.ToInt32( str );
			}
			catch
			{
				b = false;
			}
			return b;
		}

        protected void btnAddTab_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                List<SearchTab> tabs = TabbedSearchController.GetTabs(ModuleId);
                List<SearchTab> newTabs = tabs.FindAll(p => p.TabName.StartsWith("New Tab") && p.TabName.LastIndexOf(' ') > -1 && IsNumber(p.TabName.Substring(p.TabName.LastIndexOf(' ') + 1)));
                int tabId = 0;
                foreach (SearchTab tab in newTabs)
                {
                    int space = tab.TabName.LastIndexOf(' ');
                    if (space > -1)
                    {
                        string val = tab.TabName.Substring(space + 1);
                        if (IsNumber(val))
                        {
                            tabId = Math.Max(tabId, Convert.ToInt32(val));
                        }
                    }
                }
                tabId++;
                SearchTab newTab = new SearchTab();
                newTab.TabName = String.Format("New Tab {0}", tabId);
                newTab.ParameterName = "Search";
                newTab.Target = "_self";
                newTab.SearchUrl = String.Empty;
                newTab.AutoCompleteParameterName = "q";
                newTab.AutoCompleteUrl = String.Empty;
                newTab.Parameterless = true;
                newTab.ModuleID = ModuleId;
                newTab.PortalId = PortalId;
                newTab.CreatedByUserID = UserId;
                newTab.LastModifiedByUserID = UserId;
                TabbedSearchController.SaveTab(newTab, TabId);
                SelectedTab = tabs.Count;
                if (tabs.Count == 0)
                {
                    DefaultTab = 0;
                }
                BindData(false, true);
            }
        }

        private int DefaultTab
        {
            get
            {
                if (Settings.Contains("DefaultTab") && IsNumber(Settings["DefaultTab"].ToString()))
                {
                    return Convert.ToInt32(Settings["DefaultTab"].ToString());
                }
                return 0;
            }
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(ModuleId, "DefaultTab", value.ToString());
            }
        }

        private bool SaveSettings()
        {
            Page.Validate();
            if (Page.IsValid)
            {
                if (SelectedTab != -1)
                {
                    // Save Current Tab
                    SearchTab tab = TabbedSearchController.GetTab(Convert.ToInt32((divTabs.Controls[SelectedTab] as EditTab).Value));
                    if (tab != null)
                    {
                        tab.TabName = (divTabs.Controls[SelectedTab] as EditTab).Text;
                        tab.ParameterName = txtParameterName.Text;
                        tab.SearchUrl = ctlSearchUrl.Url;
                        tab.AutoCompleteParameterName = txtAutoCompleteParameterName.Text;
                        tab.AutoCompleteUrl = txtAutoCompleteUrl.Text;
                        tab.CustomName = txtCustomName.Text;
                        tab.CustomValue = txtCustomValue.Text;
                        tab.Target = lstTarget.SelectedValue;
                        tab.Parameterless = (rblParameterless.SelectedIndex == 0);
                        tab.LastModifiedByUserID = UserId;
                        TabbedSearchController.SaveTab(tab, TabId);

                        // Save Quick Link Settings
                        ModuleController modules = new ModuleController();
                        modules.DeleteTabModuleSetting(TabModuleId, "ShowQuickLinks");
                        modules.UpdateModuleSetting(ModuleId, "ShowQuickLinks", rblQuickLinks.SelectedValue);

                        if (gvQuickLinks.EditIndex != -1)
                        {
                            gvQuickLinks.UpdateRow(gvQuickLinks.EditIndex, false);
                        }
                        GridViewRow footerRow = gvQuickLinks.FooterRow;
                        if (footerRow != null)
                        {
                            ImageButton saveButton = footerRow.FindControl("saveButton2") as ImageButton;
                            if (saveButton != null)
                            {
                                if (saveButton.Visible)
                                {
                                    TextBox txtLinkText = footerRow.FindControl("txtLinkText2") as TextBox;
                                    DropDownList target = footerRow.FindControl("lstTarget3") as DropDownList;
                                    DotNetNuke.UI.UserControls.UrlControl ctlLinkUrl = footerRow.FindControl("ctlLinkUrl2") as DotNetNuke.UI.UserControls.UrlControl;
                                    QuickLink link = new QuickLink();
                                    link.LinkText = txtLinkText.Text;
                                    link.Target = target.SelectedValue;
                                    link.Url = ctlLinkUrl.Url;
                                    link.ModuleID = ModuleId;
                                    link.PortalId = PortalId;
                                    link.CreatedByUserID = UserId;
                                    link.LastModifiedByUserID = UserId;
                                    TabbedSearchController.SaveLink(link, TabId);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        protected void cmdCloseEdit_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                Response.Redirect(_navigationManager.NavigateURL());
            }
        }

        protected void cmdCancelEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(_navigationManager.NavigateURL());
        }

        protected void rblQuickLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvQuickLinks.Visible = (rblQuickLinks.SelectedIndex == 0);
        }

        protected void gvQuickLinks_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvQuickLinks.EditIndex = e.NewEditIndex;
            gvQuickLinks.DataSource = TabbedSearchController.GetLinks(ModuleId);
            gvQuickLinks.DataBind();
        }

        protected void gvQuickLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("New"))
                {
                    GridViewRow row = null;
                    if (e.CommandSource.GetType() == typeof(LinkButton))
                    {
                        LinkButton btnNew = e.CommandSource as LinkButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    else if (e.CommandSource.GetType() == typeof(ImageButton))
                    {
                        ImageButton btnNew = e.CommandSource as ImageButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    if (row == null)
                    {
                        return;
                    }
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = true;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton2") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton2") as ImageButton;
                    newButton.Visible = false;
                    saveButton.Visible = cancelButton.Visible = true;
                }
                else if (e.CommandName.Equals("Cancel"))
                {
                    GridViewRow row = null;
                    if (e.CommandSource.GetType() == typeof(LinkButton))
                    {
                        LinkButton btnNew = e.CommandSource as LinkButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    else if (e.CommandSource.GetType() == typeof(ImageButton))
                    {
                        ImageButton btnNew = e.CommandSource as ImageButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    if (row == null)
                    {
                        return;
                    }
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = false;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton2") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton2") as ImageButton;
                    newButton.Visible = true;
                    saveButton.Visible = cancelButton.Visible = false;
                }
                else if (e.CommandName.Equals("Insert"))
                {
                    GridViewRow row = null;
                    if (e.CommandSource.GetType() == typeof(LinkButton))
                    {
                        LinkButton btnNew = e.CommandSource as LinkButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    else if (e.CommandSource.GetType() == typeof(ImageButton))
                    {
                        ImageButton btnNew = e.CommandSource as ImageButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    if (row == null)
                    {
                        return;
                    }
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = false;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton2") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton2") as ImageButton;
                    newButton.Visible = true;
                    saveButton.Visible = cancelButton.Visible = false;
                    TextBox txtLinkText = row.FindControl("txtLinkText2") as TextBox;
                    DropDownList lstTarget = row.FindControl("lstTarget3") as DropDownList;
                    DotNetNuke.UI.UserControls.UrlControl ctlLinkUrl = row.FindControl("ctlLinkUrl2") as DotNetNuke.UI.UserControls.UrlControl;
                    QuickLink link = new QuickLink();
                    link.LinkText = txtLinkText.Text;
                    link.Target = lstTarget.SelectedValue;
                    link.Url = ctlLinkUrl.Url;
                    link.ModuleID = ModuleId;
                    link.PortalId = PortalId;
                    link.CreatedByUserID = UserId;
                    link.LastModifiedByUserID = UserId;
                    TabbedSearchController.SaveLink(link, TabId);
                    gvQuickLinks.EditIndex = -1;
                    gvQuickLinks.DataSource = TabbedSearchController.GetLinks(ModuleId);
                    gvQuickLinks.DataBind();
                }
            }
            catch (Exception)
            {

            }
        }

        protected void gvQuickLinks_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int index = gvQuickLinks.EditIndex;
            GridViewRow row = gvQuickLinks.Rows[index];
            TextBox txtLinkText = row.FindControl("txtLinkText") as TextBox;
            DropDownList lstTarget = row.FindControl("lstTarget2") as DropDownList;
            DotNetNuke.UI.UserControls.UrlControl ctlLinkUrl = row.FindControl("ctlLinkUrl") as DotNetNuke.UI.UserControls.UrlControl;
            List<QuickLink> links = TabbedSearchController.GetLinks(ModuleId);
            QuickLink link = links.Find(p => p.LinkId == (int)gvQuickLinks.DataKeys[index].Value);
            if (link != null)
            {
                link.LinkText = txtLinkText.Text;
                link.Target = lstTarget.SelectedValue;
                link.Url = ctlLinkUrl.Url;
                link.LastModifiedByUserID = UserId;
                TabbedSearchController.SaveLink(link, TabId);
            }
            gvQuickLinks.EditIndex = -1;
            gvQuickLinks.DataSource = links;
            gvQuickLinks.DataBind();
        }

        protected void gvQuickLinks_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvQuickLinks.EditIndex = -1;
            gvQuickLinks.DataSource = TabbedSearchController.GetLinks(ModuleId);
            gvQuickLinks.DataBind();
        }

        protected void gvQuickLinks_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                QuickLink link = (QuickLink)e.Row.DataItem;
                TextBox txtLinkText = e.Row.FindControl("txtLinkText") as TextBox;
                if (txtLinkText != null)
                {
                    ImageButton saveButton = e.Row.FindControl("saveButton") as ImageButton;
                    string js = "if ((event.which && event.which == 13) || "
                                + "(event.keyCode && event.keyCode == 13)) "
                                + "{" + Page.ClientScript.GetPostBackEventReference(saveButton, String.Empty) + ";return false;} "
                                + "else return true;";
                    txtLinkText.Attributes.Add("onkeydown", js);
                }
                DropDownList lstTarget = e.Row.FindControl("lstTarget2") as DropDownList;
                if (lstTarget != null)
                {
                    lstTarget.SelectedValue = link.Target;
                }
                DotNetNuke.UI.UserControls.UrlControl ctlLinkUrl = e.Row.FindControl("ctlLinkUrl") as DotNetNuke.UI.UserControls.UrlControl;
                if (ctlLinkUrl != null)
                {
                    ctlLinkUrl.Url = link.Url;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
            }
        }

        protected void gvQuickLinks_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int linkID = (int)gvQuickLinks.DataKeys[e.RowIndex].Value;
            TabbedSearchController.DeleteLink(linkID);
            gvQuickLinks.DataSource = TabbedSearchController.GetLinks(ModuleId);
            gvQuickLinks.DataBind();
        }
    }
}