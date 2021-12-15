<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Gafware.Modules.TabbedSearch.View" %>
<div id="tabbedHeader">
    <div class="headerRow">
        <div id="headerSearch">
            <label for="qdummy" class="hiddenlabel">Search box</label>
            <input class="searchbox" type="text" name="q" id="qdummy" readonly="true" size="18" value="SEARCH"/>
            <input type="hidden" name="type" id="type" value="web"/>
            <input class="primaryButton" type="button" value="GO" alt="Search" id="sa" name="sa"/>
            <div id="tabbedSearch" style="display: none;">
                <div class="parbase search tabbedsearch">
                    <div class="tabbable tabs-stacked">
                        <ul id="ulTabs" runat="server" class="nav nav-tabs">
                            <asp:Repeater ID="rptTabs" runat="server">
                                <ItemTemplate>
                                    <li<%# GetTabClass(Container.ItemIndex) %>><a href='#<%# ModuleId %>_<%# (Container.ItemIndex + 1).ToString() %>' class='<%# GetTabAnchorClass(Container.ItemIndex) %>' data-index="<%# Container.ItemIndex.ToString() %>" data-toggle="tab"><%# DataBinder.Eval(Container.DataItem, "TabName").ToString() %></a></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="tab-content">
                            <asp:Repeater ID="rptContent" runat="server" OnItemDataBound="rptContent_ItemDataBound">
                                <ItemTemplate>
                                    <div <%# GetContentClass(Container.ItemIndex) %> id='<%# ModuleId %>_<%# (Container.ItemIndex + 1).ToString() %>'>
                                        <div class="headerSearch">
											<asp:Panel id="searchPanel" runat="server" CssClass="headerSearch" DefaultButton="lnkSearch">
												<label id="lblSearch" runat="server" class="hiddenlabel">Search box</label>
												<div class="ui-widget"><asp:TextBox CssClass="searchbox" id="searchField" runat="server" size="18" /></div>
												<input type="hidden" name="<%# DataBinder.Eval(Container.DataItem, "CustomName") %>" value='<%# DataBinder.Eval(Container.DataItem, "CustomValue") %>' />
												<asp:LinkButton ID="lnkSearch" runat="server" CssClass="primaryButton" ToolTip="Search" target='<%# DataBinder.Eval(Container.DataItem, "Target") %>' Text="Go" />
											</asp:Panel>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>  
                    </div>       
                    <div id="divQuickLinks" runat="server" class="tabbedsearch-tagcloud">
                        <div class="text parbase text_0">
                            <div class="tagcloud">      
                                <h3>Quick Links</h3>
                                <div class="text">
                                    <ul>
                                        <asp:Repeater ID="rptQuickLinks" runat="server">
                                            <ItemTemplate>
                                                <li><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' target='<%# DataBinder.Eval(Container.DataItem, "Target") %>'><%# DataBinder.Eval(Container.DataItem, "LinkText") %></a></li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>       
        </div>
    </div>
</div>
