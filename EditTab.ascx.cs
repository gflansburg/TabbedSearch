using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gafware.Modules.TabbedSearch
{
    public partial class EditTab : System.Web.UI.UserControl
    {
        public delegate void EditTabEventHandler(object sender, EditTabEventArgs e);
        public event EditTabEventHandler Deleted;
        public event EditTabEventHandler Click;
        public event EditTabEventHandler SetDefault;

        public int Index
        {
            get
            {
                return (ViewState["Index"] != null ? (int)ViewState["Index"] : 0);
            }
            set
            {
                ViewState["Index"] = value;
            }
        }

        public string Value
        {
            get
            {
                return (ViewState["Value"] != null ? ViewState["Value"].ToString() : String.Empty);
            }
            set
            {
                ViewState["Value"] = value;
            }
        }

        public string Text
        {
            get
            {
                return txtTabName.Text;
            }
            set
            {
                txtTabName.Text = lblTabName.Text = value;
            }
        }

        public bool Selected
        {
            get
            {
                return (ViewState["Selected"] != null ? (bool)ViewState["Selected"] : false);
            }
            set
            {
                ViewState["Selected"] = value;
                cellDelete.Visible = value;
                cellEdit.Visible = value;
                cellLabel.Visible = !value;
                cellDefault.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cellLabel.Attributes["onclick"] = this.Page.ClientScript.GetPostBackEventReference(lnkPostBack, String.Empty);
        }

        public bool Default
        {
            get
            {
                return defaultButton.ImageUrl.Contains("tick_checked.png");
            }
            set
            {
                if (value)
                {
                    defaultButton.ImageUrl = ResolveUrl("~/DesktopModules/Gafware/TabbedSearch/images/tick_checked.png");
                    defaultButton.Enabled = false;
                }
                else
                {
                    defaultButton.ImageUrl = ResolveUrl("~/DesktopModules/Gafware/TabbedSearch/images/tick_unchecked.png");
                    defaultButton.Enabled = true;
                }
            }
        }

        protected void deleteButton_Click(object sender, ImageClickEventArgs e)
        {
            if(Deleted != null)
            {
                Deleted(this, new EditTabEventArgs(this.Text, this.Value, this.Index));
            }
        }

        public class EditTabEventArgs : EventArgs
        {
            public string TabName { get; set; }
            public string Value { get; set; }
            public int Index { get; set; }

            public EditTabEventArgs(string tabName, string value, int index)
            {
                TabName = tabName;
                Value = value;
                Index = index;
            }
        }

        protected void lnkPostBack_Click(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(this, new EditTabEventArgs(this.Text, this.Value, this.Index));
            }
        }

        protected void defaultButton_Click(object sender, ImageClickEventArgs e)
        {
            Default = !Default;
            defaultButton.Enabled = !Default;
            if (SetDefault != null)
            {
                SetDefault(this, new EditTabEventArgs(this.Text, this.Value, this.Index));
            }
        }
    }
}