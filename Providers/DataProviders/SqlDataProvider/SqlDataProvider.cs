/*
' Copyright (c) 2021 Gafware
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
using System.Data;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace Gafware.Modules.TabbedSearch.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// 
    /// This concreted data provider class provides the implementation of the abstract methods 
    /// from data dataprovider.cs
    /// 
    /// In most cases you will only modify the Public methods region below.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "TabbedSearch_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        // used to prefect your database objects (stored procedures, tables, views, etc)
        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        public override System.Data.IDataReader GetTab(int searchTabId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetTab", new SqlParameter("@TabId", searchTabId));
        }

        public override System.Data.IDataReader GetTabs(int moduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetTabs", new SqlParameter("@ModuleId", moduleId));
        }

        public override void DeleteTab(int searchTabId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteTab", new SqlParameter("@TabId", searchTabId));
        }

        public override int AddTab(Components.SearchTab t)
        {
             return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, NamePrefix + "AddTab",
                new SqlParameter("@TabName", t.TabName),
                new SqlParameter("@ParameterName", t.ParameterName),
                new SqlParameter("@SearchUrl", t.SearchUrl),
                new SqlParameter("@AutoCompleteParameterName", t.AutoCompleteParameterName),
                new SqlParameter("@AutoCompleteUrl", t.AutoCompleteUrl),
                new SqlParameter("@CustomName", t.CustomName),
                new SqlParameter("@CustomValue", t.CustomValue),
                new SqlParameter("@Target", t.Target),
                new SqlParameter("@Parameterless", t.Parameterless),
                new SqlParameter("@ModuleId", t.ModuleID),
                new SqlParameter("@CreatedByUserId", t.CreatedByUserID)));
        }

        public override void UpdateTab(Components.SearchTab t)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateTab", 
                new SqlParameter("@TabId", t.SearchTabId),
                new SqlParameter("@TabName", t.TabName),
                new SqlParameter("@ParameterName", t.ParameterName),
                new SqlParameter("@SearchUrl", t.SearchUrl),
                new SqlParameter("@AutoCompleteParameterName", t.AutoCompleteParameterName),
                new SqlParameter("@AutoCompleteUrl", t.AutoCompleteUrl),
                new SqlParameter("@CustomName", t.CustomName),
                new SqlParameter("@CustomValue", t.CustomValue),
                new SqlParameter("@Target", t.Target),
                new SqlParameter("@Parameterless", t.Parameterless),
	            new SqlParameter("@ModuleId", t.ModuleID),
	            new SqlParameter("@LastModifiedByUserId", t.LastModifiedByUserID));
        }

        public override System.Data.IDataReader GetLink(int linkId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetLink", new SqlParameter("@LinkId", linkId));
        }

        public override System.Data.IDataReader GetLinks(int moduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetLinks", new SqlParameter("@ModuleId", moduleId));
        }

        public override void DeleteLink(int linkId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteLink", new SqlParameter("@LinkId", linkId));
        }

        public override int AddLink(Components.QuickLink l)
        {
             return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, NamePrefix + "AddLink",
                new SqlParameter("@LinkText", l.LinkText),
                new SqlParameter("@LinkUrl", l.Url),
                new SqlParameter("@Target", l.Target),
                new SqlParameter("@ModuleId", l.ModuleID),
                new SqlParameter("@CreatedByUserId", l.CreatedByUserID)));
        }

        public override void UpdateLink(Components.QuickLink l)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateLink", 
                new SqlParameter("@LinkId", l.LinkId),
                new SqlParameter("@LinkText", l.LinkText),
                new SqlParameter("@LinkUrl", l.Url),
                new SqlParameter("@Target", l.Target),
	            new SqlParameter("@ModuleId", l.ModuleID),
	            new SqlParameter("@LastModifiedByUserId", l.LastModifiedByUserID));
        }

        #endregion

    }

}