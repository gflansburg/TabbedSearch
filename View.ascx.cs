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
using System.Collections.Generic;
using System.Web.UI;
using Gafware.Modules.TabbedSearch.Components;
using System.Web.UI.WebControls;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gafware.Modules.TabbedSearch
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from TabbedSearchModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : TabbedSearchModuleBase, IActionable
    {
        private readonly INavigationManager _navigationManager;

        public View()
        {
            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }

        protected string CreateClassName
        {
            get
            {
                return Guid.NewGuid().ToString().Replace("-", String.Empty);
            }
        }

		public void LoadHeader()
		{
			/*if (this.Page.Header.FindControl("BootstrapScript") == null)
			{
				Literal literal = new Literal()
				{
					ID = "BootstrapScript"
				};
                string str = string.Concat("<script src=\"", ControlPath, "bootstrap/js/bootstrap.min.js\" type=\"text/javascript\"></script>");
                literal.Text = str;
                this.Page.Header.Controls.Add(literal);
			}
			if (this.Page.Header.FindControl("BootstrapCss") == null)
			{
				Literal literal = new Literal()
				{
					ID = "BootstrapCss"
				};
                string str = string.Concat("<link href=\"", ControlPath, "bootstrap/css/bootstrap.min.css\" rel=\"stylesheet\" type=\"text/css\"/>");
                literal.Text = str;
                this.Page.Header.Controls.AddAt(0, literal);
			}*/

            if (this.Page.Header.FindControl("JQueryCSS") == null)
            {
                Literal literal = new Literal()
                {
                    ID = "JQueryCSS"
                };
                string str = string.Concat("<link href=\"", ControlPath, "Style/jquery-ui.min.css\" rel=\"stylesheet\" type=\"text/css\"/>");
                str += string.Concat("<link href=\"", ControlPath, "Style/jquery-ui.structure.min.css\" rel=\"stylesheet\" type=\"text/css\"/>");
                str += string.Concat("<link href=\"", ControlPath, "Style/jquery-ui.theme.min.css\" rel=\"stylesheet\" type=\"text/css\"/>");
                literal.Text = str;
                this.Page.Header.Controls.Add(literal);
            }

            Literal litJQuery = new Literal()
            {
                ID = "jQuery"
            };
            litJQuery.Text = "<script>!window.jQuery && document.write(unescape('%3Cscript src=\"" + ControlPath + "Scripts/jquery-3.2.1.min.js\"%3E%3C/script%3E'))</script>";
            this.Page.Header.Controls.Add(litJQuery);

            Literal litJQueryUI = new Literal()
            {
                ID = "jQueryUI"
            };
            litJQueryUI.Text = "<script>!window.jQuery.ui && document.write(unescape('%3Cscript src=\"" + ControlPath + "Scripts/jquery-ui.min.js\"%3E%3C/script%3E'))</script>";
            this.Page.Header.Controls.Add(litJQueryUI);

            if (this.Page.Header.FindControl("ComponentScriptTabbedSearch") == null)
            {
                Literal literal = new Literal()
                {
                    ID = "ComponentScriptTabbedSearch"
                };
                string str = String.Empty;
                str = string.Concat(str, "<script src=\"", ControlPath, "Scripts/jquery.outside.js\" type=\"text/javascript\"></script>");
                str = string.Concat(str, "<script src=\"", ControlPath, "Scripts/jquery.tabbable.js\" type=\"text/javascript\"></script>");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("<script type=\"text/javascript\">");
                sb.AppendLine("  function handleTabbedSearchEvent(event) {");
                sb.AppendLine("    event.stopImmediatePropagation();");
                sb.AppendLine("    event.preventDefault();");
                sb.AppendLine("    if ($('#tabbedSearch').is(':hidden')) {");
                sb.AppendLine("      setTimeout(function () {");
                sb.AppendLine("        $('#tabbedSearch').show();");
                sb.AppendLine("        setTimeout(function () {");
                sb.AppendLine("          $('#tabbedHeader div.active input.searchbox').focus();");
                sb.AppendLine("        }, 300);");
                sb.AppendLine("      }, 200);");
                sb.AppendLine("    }");
                sb.AppendLine("  }");
                //if (!IsPostBack)
                {
                    sb.AppendLine("$(document).ready(function() {");
                }
                sb.AppendLine("  $('#tabbedSearch').hide();");
                sb.AppendLine("  Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {");
                sb.AppendLine("    $('#tabbedSearch').hide();");
                sb.AppendLine("  });");
                sb.AppendLine("  $('#qdummy').on('focus', function (event) {");
                sb.AppendLine("    event.stopImmediatePropagation();");
                sb.AppendLine("    event.preventDefault();");
                sb.AppendLine("    if ($('#tabbedSearch').is(':visible')) {");
                sb.AppendLine("      $.tabPrev();");
                sb.AppendLine("    } else {");
                sb.AppendLine("      handleTabbedSearchEvent(event);");
                sb.AppendLine("    };");
                sb.AppendLine("  });");
                sb.AppendLine("  $('#qdummy').on('click', function (event) {");
                sb.AppendLine("    event.stopImmediatePropagation();");
                sb.AppendLine("    event.preventDefault();");
                sb.AppendLine("    handleTabbedSearchEvent(event);");
                sb.AppendLine("  });");
                sb.AppendLine("  $('#tabbedSearch').bind('clickoutside', function (event) {");
                sb.AppendLine("    if ($('#tabbedSearch').is(':visible')) {");
                sb.AppendLine("      $('#tabbedSearch').hide();");
                sb.AppendLine("    }");
                sb.AppendLine("  });");
                sb.AppendLine("  $('#tabbedHeader').bind('focusoutside', function (event) {");
                sb.AppendLine("    if ($('#tabbedSearch').is(':visible')) {");
                sb.AppendLine("      $('#tabbedSearch').hide();");
                sb.AppendLine("    }");
                sb.AppendLine("  });");

                /*sb.AppendLine("  var tabPressed = false;");
                sb.AppendLine("  var shiftPressed = false;");
                sb.AppendLine("  $(window).keydown(function (e) {");
                sb.AppendLine("    var event = e || window.event;");
                sb.AppendLine("    var keyCode = event.keyCode || event.which;");
                sb.AppendLine("    if([9,16].indexOf(keyCode) > -1) {");
                sb.AppendLine("      if(keyCode == 9) tabPressed = true;");
                sb.AppendLine("      if(keyCode == 16) shiftPressed = true;");
                sb.AppendLine("    }");
                sb.AppendLine("  });");
                sb.AppendLine("  $(window).keyup(function (e) {");
                sb.AppendLine("    var event = e || window.event;");
                sb.AppendLine("    var keyCode = event.keyCode || event.which;");
                sb.AppendLine("    if([9,16].indexOf(keyCode) > -1) {");
                sb.AppendLine("      if(keyCode == 9) tabPressed = false;");
                sb.AppendLine("      if(keyCode == 16) shiftPressed = false;");
                sb.AppendLine("    }");
                sb.AppendLine("  });");*/

                sb.AppendLine("  var firstTab = $('#tabbedSearch .anchorTab:first');");
                sb.AppendLine("  var lastTab = $('#tabbedSearch .anchorTab:last');");
                sb.AppendLine("  var tabs = $('#tabbedSearch .anchorTab');");
                sb.AppendLine("  tabs.each(function() {");
                sb.AppendLine("    $(this).on('keydown', function (e) {");
                sb.AppendLine("      var event = e || window.event;");
                sb.AppendLine("      var keyCode = event.keyCode || event.which;");
                sb.AppendLine("      if([32, 37, 38, 39, 40].indexOf(keyCode) > -1) {");
                sb.AppendLine("        event.preventDefault();");
                sb.AppendLine("        var index = parseInt($(this).attr('data-index'), 10);");
				sb.AppendLine("        var first = $(this).hasClass('first');");
				sb.AppendLine("        var last = $(this).hasClass('last');");
                sb.AppendLine("        if(keyCode == 37) {");
                sb.AppendLine("          if(!first) {");
                sb.AppendLine("            $.tabPrev();");
                sb.AppendLine("          } else {");
                sb.AppendLine("            lastTab.focus();");
                sb.AppendLine("          }");
                sb.AppendLine("        } else if(keyCode == 39) {");
                sb.AppendLine("          if(!last) {");
                sb.AppendLine("            $.tabNext();");
                sb.AppendLine("          } else {");
                sb.AppendLine("            firstTab.focus();");
                sb.AppendLine("          }");
                sb.AppendLine("        }else if(keyCode == 40 || keyCode == 32) {");
                sb.AppendLine("          if(keyCode ==32) {");
                sb.AppendLine("            $('.nav-tabs a[href=\"#" + ModuleId + "_' + (index + 1) + '\"]').tab('show');");
                sb.AppendLine("          }");
                sb.AppendLine("          var current = $('#tabbedSearch li.active')");
                sb.AppendLine("          index = parseInt($(current).find('a').attr('data-index'), 10);");
                sb.AppendLine("          $('#" + rptContent.ClientID + "_searchField_' + index).focus();");
                sb.AppendLine("        }");
                sb.AppendLine("      }");
                sb.AppendLine("    });");
                sb.AppendLine("  });");
                sb.AppendLine("  $('#tabbedSearch .anchorTab').on('keyup', function (e) {");
                sb.AppendLine("    var event = e || window.event;");
                sb.AppendLine("    var keyCode = event.keyCode || event.which;");
                sb.AppendLine("    if([32, 37, 38, 39, 40].indexOf(keyCode) > -1) {");
                sb.AppendLine("      event.preventDefault();");
                sb.AppendLine("    }");
                sb.AppendLine("  });");
                
                //if (!IsPostBack)
                {
                    sb.AppendLine("});");
                }
                sb.AppendLine("</script>");
                str = string.Concat(str, sb.ToString());
                literal.Text = str;
                this.Page.Header.Controls.Add(literal);
            }
		}

        protected int SelectedTab
        {
            get
            {
                return (ViewState["SelectedTab_" + ModuleId.ToString()] != null
                    ? (int) ViewState["SelectedTab_" + ModuleId.ToString()]
                    : 0);
            }
            set { ViewState["SelectedTab_" + ModuleId.ToString()] = value; }
        }

        public int DefaultTab
        {
            get
            {
                if (Settings.Contains("DefaultTab"))
                {
                    return Convert.ToInt32(Settings["DefaultTab"].ToString());
                }
                return 0;
            }
        }

        public bool ShowQuickLinks
        {
            get
            {
                if (Settings.Contains("ShowQuickLinks"))
                {
                    return Convert.ToBoolean(Settings["ShowQuickLinks"].ToString());
                }
                return true;
            }
        }

        protected string GetTabAnchorClass(int index)
        {
            return "anchorTab tabbable" + (index == 0 ? " first" : String.Empty) + (index == TabCount - 1 ? " last" : String.Empty);
        }

        protected string GetTabClass(int index)
        {
            return (index == SelectedTab ? " class=\"active\"" : String.Empty);
        }

        protected string GetContentClass(int index)
        {
            return "class=\"tab-pane" + (index == SelectedTab ? " active" : String.Empty) + "\"";
        }

        protected int TabCount
        {
            get
            {
                if (ViewState["TabCount"] == null)
                {
                    List<SearchTab> tabs = TabbedSearchController.GetTabs(ModuleId);
                    ViewState["TabCount"] = tabs.Count;
                }
                return (int)ViewState["TabCount"];
            }
            set
            {
                ViewState["TabCount"] = value;
            }
        }

        private void LoadTabs()
        {
            List<SearchTab> tabs = TabbedSearchController.GetTabs(ModuleId);
            if (!IsPostBack)
            {
                SelectedTab = DefaultTab;
                rptTabs.DataSource = rptContent.DataSource = tabs;
                rptTabs.DataBind();
                rptContent.DataBind();
            }
        }

        private void LoadQuickLinks()
        {
            if (!IsPostBack)
            {
                List<QuickLink> links = TabbedSearchController.GetLinks(ModuleId);
                rptQuickLinks.DataSource = links;
                rptQuickLinks.DataBind();
                divQuickLinks.Visible = (ShowQuickLinks && links.Count > 0);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadHeader();
                LoadTabs();
                LoadQuickLinks();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
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

        protected void rptContent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SearchTab tab = (SearchTab)e.Item.DataItem;
                LinkButton lnk = (LinkButton)e.Item.FindControl("lnkSearch");
                TextBox search = (TextBox)e.Item.FindControl("searchField");
                System.Web.UI.HtmlControls.HtmlGenericControl lbl = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("lblSearch");
                lbl.Attributes["for"] = search.ClientID;
                string url = "'";
                if (IsNumber(tab.SearchUrl))
                {
                    url += _navigationManager.NavigateURL(Convert.ToInt32(tab.SearchUrl));
                }
                else
                {
                    url += tab.SearchUrl;
                }
                if (tab.Parameterless)
                {
                    url += "/" + tab.ParameterName + "/' + $('#" + search.ClientID + "').val()";
                    if (!String.IsNullOrEmpty(tab.CustomName) && !String.IsNullOrEmpty(tab.CustomValue))
                    {
                        url += " + '/" + tab.CustomName + "/" + System.Web.HttpUtility.UrlEncode(tab.CustomValue) + "'";
                    }
                }
                else
                {
                    url += "?" + tab.ParameterName + "=' + $('#" + search.ClientID + "').val()";
                    if (!String.IsNullOrEmpty(tab.CustomName) && !String.IsNullOrEmpty(tab.CustomValue))
                    {
                        url += " + '&" + tab.CustomName + "=" + System.Web.HttpUtility.UrlEncode(tab.CustomValue) + "'";
                    }
                }
                lnk.Attributes["onclick"] = String.Format("this.href={0};", url);
                if (!String.IsNullOrEmpty(tab.AutoCompleteUrl))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("$(\"#" + lnk.ClientID + "\").click(function(event) {");
                    sb.AppendLine("  var link = $(this);");
                    sb.AppendLine("  var target = link.attr(\"target\");");
                    sb.AppendLine("  if($.trim(target).length > 0) {");
                    sb.AppendLine("    window.open(link.attr(\"href\"), target);");
                    sb.AppendLine("  } else {");
                    sb.AppendLine("     window.location = link.attr(\"href\");");
                    sb.AppendLine("  }");
                    sb.AppendLine("  event.preventDefault();");
                    sb.AppendLine("});");
                    sb.AppendLine("function initAutoComplete" + tab.SearchTabId.ToString() + "() {");
                    /*sb.AppendLine("  $('#" + search.ClientID + "').autocomplete({");
                    if (tab.AutoCompleteUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || tab.AutoCompleteUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        sb.AppendLine("    source: function( request, response ) {");
                        sb.AppendLine("      $.ajax({");
                        sb.AppendLine("        url: \"" + tab.AutoCompleteUrl + "\",");
                        sb.AppendLine("        dataType: \"jsonp\",");
                        sb.AppendLine("        data: {");
                        sb.AppendLine("          " + tab.AutoCompleteParameterName + ": request.term");
                        sb.AppendLine("        },");
                        sb.AppendLine("        success: function( data ) {");
                        sb.AppendLine("          response( data );");
                        sb.AppendLine("        }");
                        sb.AppendLine("      });");
                        sb.AppendLine("    },");
                        sb.AppendLine("    minLength: 3,");
                    }
                    else
                    {
                        sb.AppendLine("    source: \"" + tab.AutoCompleteUrl + "\",");
                        sb.AppendLine("    minLength: 1,");
                    }
                    sb.AppendLine("    select: function(event, ui) {");
                    sb.AppendLine("      $('#" + search.ClientID + "').val(ui.item.label);");
                    sb.AppendLine("      $('#" + lnk.ClientID + "').attr('href', " + url + ");");
                    sb.AppendLine("      $('#" + lnk.ClientID + "').click();");
                    sb.AppendLine("    },");
                    sb.AppendLine("    open: function() {");
                    sb.AppendLine("      $(this).removeClass(\"ui-corner-all\").addClass(\"ui-corner-top\");");
                    sb.AppendLine("    },");
                    sb.AppendLine("    close: function() {");
                    sb.AppendLine("      $(this).removeClass(\"ui-corner-top\").addClass(\"ui-corner-all\");");
                    sb.AppendLine("    }");
                    sb.AppendLine("  }).keyup(function(e) {");
                    sb.AppendLine("    if(e.which === 13) {");
                    sb.AppendLine("      $('#" + search.ClientID + "').autocomplete('close');");
                    sb.AppendLine("      $('#" + lnk.ClientID + "').attr('href', " + url + ");");
                    sb.AppendLine("      $('#" + lnk.ClientID + "').click();");
                    sb.AppendLine("    }");
                    sb.AppendLine("  });");
                    sb.AppendLine("  $(\".ui-autocomplete\").wrap('<div class=\"tabbedAutoComplete\" />');"); */
                    sb.AppendLine("}");
                    sb.AppendLine("$(document).ready(function() {");
                    sb.AppendLine("  initAutoComplete" + tab.SearchTabId.ToString() + "();");
                    sb.AppendLine("  Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {");
                    sb.AppendLine("     initAutoComplete" + tab.SearchTabId.ToString() + "();");
                    sb.AppendLine("  });");
                    sb.AppendLine("});");
                    Page.ClientScript.RegisterStartupScript(GetType(), "AutoComplete" + tab.SearchTabId.ToString(), sb.ToString(), true);
                }
            }
        }
    }
}