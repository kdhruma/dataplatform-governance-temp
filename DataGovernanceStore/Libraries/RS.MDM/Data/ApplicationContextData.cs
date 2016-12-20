using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using SP = Riversand.StoredProcedures;
using MDM.BusinessObjects;
using MDM.Interfaces;
using MDM.Services;

namespace RS.MDM.Data
{
    
    /// <summary>
    /// Added by Shridhar Moorkhandi
    /// Helps to get data from the Database to populate Application context dropdowns(Organization, Catalog, NodeType etc.)
    /// </summary>
    public class ApplicationContextData
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the organizations
        /// </summary>
        /// <returns>Dictionary of organizations with Org Id as key and Org LongName as value</returns>
        public static Dictionary<int, string> GetAllOrganizations()
        {
            Int32 orgId = 0;
            DataTable dtOrganizations = null;
            Dictionary<int, string> orgDictionary = new Dictionary<int, string>();

            dtOrganizations = SP.Organization.GetUserVisibleOrgsDT("cfadmin", "cfadmin", 1, 0, 999, "LongName", "", "");

            if (dtOrganizations != null && dtOrganizations.Rows.Count > 0)
            {
                orgDictionary.Add(0, "--Select Organization--");

                foreach (DataRow dtRow in dtOrganizations.Rows)
                {
                    Int32.TryParse(dtRow["PK_Org"].ToString(), out orgId);

                    if (orgId > 0 && !orgDictionary.ContainsKey(orgId))
                        orgDictionary.Add(orgId, dtRow["LongName"].ToString());
                }
            }

            return orgDictionary;
        }

        /// <summary>
        /// Gets all the catalogs under the selected Organization
        /// </summary>
        /// <param name="orgId"> Organization Id for which Catalogs are required</param>
        /// <returns>Dictionary of Catalogs with Catalog Id as key and Catalog LongName as value</returns>
        public static Dictionary<int, string> GetCatalogsByOrganization(Int32 orgId)
        {
            Int32 catalogId = 0;
            String catalogName = String.Empty;
            String xmlCatalogs = String.Empty;
            Dictionary<int, string> catalogDictionary = new Dictionary<int, string>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlCatalogs = SP.Catalog.GetCatalogPermissionsByOrg(null, "cfadmin", orgId, 1, 0, 999, "LongName", "ShortName", "", 0, false, false, true, false, true, false);

            xmlDoc.LoadXml(xmlCatalogs);

            XmlNode root = xmlDoc.FirstChild;

            if (root != null && root.ChildNodes.Count > 0)
            {
                catalogDictionary.Add(0, "--Select Catalog--");

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Attributes["PK_Catalog"] != null)
                        Int32.TryParse(node.Attributes["PK_Catalog"].Value.ToString(), out catalogId);

                    if (catalogId < 1)
                        continue;

                    if (node.Attributes["LongName"] != null)
                        catalogName = node.Attributes["LongName"].Value.ToString();

                    if(!catalogDictionary.ContainsKey(catalogId))
                        catalogDictionary.Add(catalogId, catalogName);
                }
            }

            return catalogDictionary;
        }

        /// <summary>
        /// Gets all categories by catalog
        /// </summary>
        /// <returns>Dictionary of categories with CNode Id as key and Category LongName as value</returns>
        public static Dictionary<int, string> GetAllCategoriesByCatalog(Int32 catalogId)
        {
            Int32 categoryId = 0;
            DataTable dtCategories = null;
            Dictionary<int, string> categoryDictionary = new Dictionary<int, string>();

            dtCategories = SP.Catalog.GetAllCategories(catalogId, 1, "ALL", String.Empty);

            categoryDictionary.Add(0, "--Select Category--");

            if (dtCategories != null && dtCategories.Rows.Count > 0)
            {
                foreach (DataRow dtRow in dtCategories.Rows)
                {
                    Int32.TryParse(dtRow["PK_CNode"].ToString(), out categoryId);

                    if (categoryId > 0 && !categoryDictionary.ContainsKey(categoryId))
                        categoryDictionary.Add(categoryId, dtRow["LongName"].ToString());
                }
            }

            return categoryDictionary;
        }

        /// <summary>
        /// Gets all the EntityTypes
        /// </summary>
        /// <returns>Dictionary of entity types with EntityType Id as key and EntityType LongName as value</returns>
        public static Dictionary<int, string> GetAllEntityTypes()
        {
            Dictionary<int, string> entityTypeDictionary = new Dictionary<int, string>();

            var dataModelService = new DataModelService();
            IEntityTypeCollection entityTypes = dataModelService.GetAllEntityTypes(new CallerContext(global::MDM.Core.MDMCenterApplication.PIM, global::MDM.Core.MDMCenterModules.Modeling));

            if (entityTypes != null && entityTypes.Any())
            {
                entityTypeDictionary.Add(0, "--Select NodeType--");

                foreach (EntityType et in entityTypes)
                {
                    if (et.Id > 0 && !entityTypeDictionary.ContainsKey(et.Id))
                        entityTypeDictionary.Add(et.Id, et.LongName.ToString());
                }
            }

            return entityTypeDictionary;

        }

        /// <summary>
        /// Gets all the RelationshipTypes
        /// </summary>
        /// <returns>Dictionary of relationship types with RelationshipType Id as key and RelationshipType LongName as value</returns>
        public static Dictionary<int, string> GetAllRelationshipTypes()
        {
            Int32 relationshipTypeId = 0;
            DataTable dtRelationshipTypes = null;
            Dictionary<int, string> relationshipTypeDictionary = new Dictionary<int, string>();

            dtRelationshipTypes = SP.Administration.GetAllRelationshipType();

            if (dtRelationshipTypes != null && dtRelationshipTypes.Rows.Count > 0)
            {
                relationshipTypeDictionary.Add(0, "--Select RelationshipType--");

                foreach (DataRow dtRow in dtRelationshipTypes.Rows)
                {
                    Int32.TryParse(dtRow["PK_RelationshipType"].ToString(), out relationshipTypeId);

                    if (relationshipTypeId > 0 && !relationshipTypeDictionary.ContainsKey(relationshipTypeId))
                        relationshipTypeDictionary.Add(relationshipTypeId, dtRow["LongName"].ToString());
                }
            }

            return relationshipTypeDictionary;
        }

        /// <summary>
        /// Gets all the attributes
        /// </summary>
        /// <returns>Dictionary of attributes with attribute Id as key and attribute LongName as value</returns>
        public static Dictionary<int, string> GetAllAttributes()
        {
            Int32 attributeId = 0;
            DataTable dtAttributes = null;
            Dictionary<int, string> attributeDictionary = new Dictionary<int, string>();

            dtAttributes = SP.Attribute.GetAllAttributes();

            if (dtAttributes != null && dtAttributes.Rows.Count > 0)
            {
                attributeDictionary.Add(0, "--Select Attribute--");

                foreach (DataRow dtRow in dtAttributes.Rows)
                {
                    Int32.TryParse(dtRow["PK_Attribute"].ToString(), out attributeId);

                    if (attributeId > 0 && !attributeDictionary.ContainsKey(attributeId))
                        attributeDictionary.Add(attributeId, dtRow["LongName"].ToString());
                }
            }

            return attributeDictionary;
        }

        /// <summary>
        /// Gets all the Locales
        /// </summary>
        /// <returns>Dictionary of Locales with Locale Id as key and Locale LongName as value</returns>
        public static Dictionary<int, string> GetAllLocale()
        {
            Int32 localeId = 0;
            String localeName = String.Empty;
            String xmlLocales = String.Empty;
            Dictionary<int, string> localeDictionary = new Dictionary<int, string>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlLocales = SP.Language.GetLocales(0, 0, 999, "LongName", "", "", "cfadmin");

            xmlDoc.LoadXml(xmlLocales);

            XmlNode root = xmlDoc.FirstChild;

            if (root != null && root.ChildNodes.Count > 0)
            {
                localeDictionary.Add(0, "--Select Locale--");

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Attributes["PK_Locale"] != null)
                        Int32.TryParse(node.Attributes["PK_Locale"].Value.ToString(), out localeId);

                    if (localeId < 1)
                        continue;

                    if (node.Attributes["LongName"] != null)
                        localeName = node.Attributes["LongName"].Value.ToString();

                    if(!localeDictionary.ContainsKey(localeId))
                        localeDictionary.Add(localeId, localeName);
                }
            }

            return localeDictionary;
        }

        /// <summary>
        /// Gets all the Roles
        /// </summary>
        /// <returns>Dictionary of Roles with Role Id as key and Role LongName as value</returns>
        public static Dictionary<int, string> GetAllRoles()
        {
            Int32 roleId = 0;
            String roleName = String.Empty;
            String xmlRoles = String.Empty;
            Dictionary<int, string> roleDictionary = new Dictionary<int, string>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlRoles = SP.Security.GetRoles(0, 0, "N", 0, 999, "LongName", "ShortName", "", "cfadmin", false);

            xmlDoc.LoadXml(xmlRoles);

            XmlNode root = xmlDoc.FirstChild;

            if (root != null && root.ChildNodes.Count > 0)
            {
                roleDictionary.Add(0, "--Select Role--");

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Attributes["PK_Security_Role"] != null)
                        Int32.TryParse(node.Attributes["PK_Security_Role"].Value.ToString(), out roleId);

                    if (roleId < 1)
                        continue;

                    if (node.Attributes["LongName"] != null)
                        roleName = node.Attributes["LongName"].Value.ToString();

                    if(!roleDictionary.ContainsKey(roleId))
                        roleDictionary.Add(roleId, roleName);
                }
            }

            return roleDictionary;
        }
        
        /// <summary>
        /// Gets all the Users 
        /// </summary>
        /// <returns>Dictionary of Users with User Id as key and User LongName as value</returns>
        public static Dictionary<int, string> GetAllUsers()
        {
            Int32 userId = 0;
            String userName = String.Empty;
            String xmlUsers = String.Empty;
            Dictionary<int, string> userDictionary = new Dictionary<int, string>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlUsers = SP.Security.GetUsers(0, 0, 0, 999, "Login", "", "", "cfadmin");

            xmlDoc.LoadXml(xmlUsers);

            XmlNode root = xmlDoc.FirstChild;

            if (root != null && root.ChildNodes.Count > 0)
            {
                userDictionary.Add(0, "--Select User--");

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Attributes["PK_Security_User"] != null)
                        Int32.TryParse(node.Attributes["PK_Security_User"].Value.ToString(), out userId);

                    if (userId < 1)
                        continue;

                    if (node.Attributes["Login"] != null)
                        userName = node.Attributes["Login"].Value.ToString();

                    if(!userDictionary.ContainsKey(userId))
                        userDictionary.Add(userId, userName);
                }
            }

            return userDictionary;
        }

        /// <summary>
        /// Tests whether connection is possible with the provided connection string
        /// </summary>
        /// <param name="connectionString">Connection String to be tested</param>
        public static void TestConnection(string connectionString)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
            }
            catch
            {
                throw new Exception("Failed to connect to database. The server was not found or not accessible. Verify connection details.");
            }
        }

        #endregion
    }
}
