using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Gafware.Modules.TabbedSearch.Components
{
    public class SearchTab : ContentItem
    {
        /// <summary>
        /// Id of tab
        /// </summary>
        public int SearchTabId { get; set; }
        /// <summary>
        /// Tab Name
        /// </summary>
        public string TabName { get; set; }
        /// <summary>
        /// Parameter Name
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// Search URL
        /// </summary>
        public string SearchUrl { get; set; }
        /// <summary>
        /// Auto Complete Parameter Name
        /// </summary>
        public string AutoCompleteParameterName { get; set; }
        /// <summary>
        /// Auto Complete URL
        /// </summary>
        public string AutoCompleteUrl { get; set; }
        /// <summary>
        /// Custom parameter name
        /// </summary>
        public string CustomName { get; set; }
        /// <summary>
        /// Custom parameter value
        /// </summary>
        public string CustomValue { get; set; }
        /// <summary>
        /// Target window
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// Parameterless URL query string
        /// </summary>
        public bool Parameterless  { get; set; }
        /// <summary>
        /// Tab Module Id
        /// </summary>
        public new int ModuleID { get; set; }
        /// <summary>
        /// Created by user Id
        /// </summary>
        public new int CreatedByUserID { get; set; }
        /// <summary>
        /// Last modified by user Id
        /// </summary>
        public new int LastModifiedByUserID { get; set; }
        /// <summary>
        /// Created on date
        /// </summary>
        public new DateTime CreatedOnDate { get; set; }
        /// <summary>
        /// Last modified date
        /// </summary>
        public new DateTime LastModifiedOnDate { get; set; }
        /// <summary>
        /// Portal Id
        /// </summary>
        public int PortalId { get; set; }

        public new string CreatedByUser
        {
            get
            {
                return CreatedByUserID != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, CreatedByUserID).DisplayName : String.Empty;
            }
        }

        public new string LastModifiedByUser
        {
            get
            {
                return LastModifiedByUserID != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, LastModifiedByUserID).DisplayName : String.Empty;
            }
        }

        public override void Fill(IDataReader dr)
        {
            //base.Fill(dr);

            SearchTabId = Null.SetNullInteger(dr["TabId"]);
            TabName = Null.SetNullString(dr["TabName"]);
            ParameterName = Null.SetNullString(dr["ParameterName"]);
            SearchUrl = Null.SetNullString(dr["SearchUrl"]);
            AutoCompleteParameterName = Null.SetNullString(dr["AutoCompleteParameterName"]);
            AutoCompleteUrl = Null.SetNullString(dr["AutoCompleteUrl"]);
            CustomName = Null.SetNullString(dr["CustomName"]);
            CustomValue = Null.SetNullString(dr["CustomValue"]);
            Target = Null.SetNullString(dr["Target"]);
            Parameterless = Null.SetNullBoolean(dr["Parameterless"]);
            ModuleID = Null.SetNullInteger(dr["ModuleId"]);
            CreatedByUserID = Null.SetNullInteger(dr["CreatedByUserId"]);
            LastModifiedByUserID = Null.SetNullInteger(dr["LastModifiedByUserId"]);
            CreatedOnDate = Null.SetNullDateTime(dr["CreatedOnDate"]);
            LastModifiedOnDate = Null.SetNullDateTime(dr["LastModifiedOnDate"]);
        }

        public override int KeyID
        {
            get
            {
                return SearchTabId;
            }
            set
            {
                SearchTabId = value;
            }
        }
    }
}