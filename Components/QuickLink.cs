using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Gafware.Modules.TabbedSearch.Components
{
    public class QuickLink : ContentItem
    {
        /// <summary>
        /// Id of link
        /// </summary>
        public int LinkId { get; set; }
        /// <summary>
        /// Link text
        /// </summary>
        public string LinkText { get; set; }
        /// <summary>
        /// URL of link
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Target window
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// Tab Module Id
        /// </summary>
        public int ModuleId { get; set; }
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

            LinkId = Null.SetNullInteger(dr["LinkId"]);
            LinkText = Null.SetNullString(dr["LinkText"]);
            Url = Null.SetNullString(dr["LinkUrl"]);
            Target = Null.SetNullString(dr["Target"]);
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
                return LinkId;
            }
            set
            {
                LinkId = value;
            }
        }
    }
}