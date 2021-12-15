using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using Gafware.Modules.TabbedSearch.Data;

namespace Gafware.Modules.TabbedSearch.Components
{
    public class TabbedSearchController
    {
        public const string CSS_TAG_INCLUDE_FORMAT = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />";
        public const string SCRIPT_TAG_INCLUDE_FORMAT = "<script language=\"javascript\" type=\"text/javascript\" src=\"{0}\"></script>";

        public static SearchTab GetTab(int tabId)
        {
            return CBO.FillObject<SearchTab>(DataProvider.Instance().GetTab(tabId));
        }

        public static List<SearchTab> GetTabs(int moduleId)
        {
            return CBO.FillCollection<SearchTab>(DataProvider.Instance().GetTabs(moduleId));
        }

        public static void DeleteTab(int searchTabId)
        {
            DataProvider.Instance().DeleteTab(searchTabId);
        }

        public static int SaveTab(SearchTab t, int tabId)
        {
            if (t.SearchTabId < 1)
            {
                t.SearchTabId = DataProvider.Instance().AddTab(t);
            }
            else
            {
                DataProvider.Instance().UpdateTab(t);
            }
            return t.SearchTabId;
        }

        public static QuickLink GetLink(int linkId)
        {
            return CBO.FillObject<QuickLink>(DataProvider.Instance().GetLink(linkId));
        }

        public static List<QuickLink> GetLinks(int moduleId)
        {
            return CBO.FillCollection<QuickLink>(DataProvider.Instance().GetLinks(moduleId));
        }

        public static void DeleteLink(int linkId)
        {
            DataProvider.Instance().DeleteLink(linkId);
        }

        public static int SaveLink(QuickLink l, int tabId)
        {
            if (l.LinkId < 1)
            {
                l.LinkId = DataProvider.Instance().AddLink(l);
            }
            else
            {
                DataProvider.Instance().UpdateLink(l);
            }
            return l.LinkId;
        }
    }
}