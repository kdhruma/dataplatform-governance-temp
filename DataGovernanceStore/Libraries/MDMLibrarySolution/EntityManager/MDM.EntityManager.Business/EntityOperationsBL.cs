using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Transactions;
using System.Xml;

namespace MDM.EntityManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Data;
    using MDM.ExceptionManager;
    using MDM.Utility;
    using Attribute = MDM.BusinessObjects.Attribute;
    using MDMBOW = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Specifies entity manager
    /// </summary>
    public class EntityOperationsBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// 
        /// </summary>
        private EntityOperationsDA _entityOperationsTempDA = new EntityOperationsDA();

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public EntityOperationsBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Constructor which takes the security principal if its already present in the system 
        /// </summary>
        /// <param name="securityPrincipalInstance">The user specific security principal of this instance</param>
        public EntityOperationsBL(SecurityPrincipal securityPrincipalInstance)
        {
            _securityPrincipal = securityPrincipalInstance;
            SecurityPrincipalHelper.ValidateSecurityPrincipal(_securityPrincipal);
        }

        #endregion

        #region Methods

        /// <summary>
        /// It returns the Catalog, Organization, and the CNodes impacted by a change in a core attribute
        /// </summary>
        /// <param name="localeId">Indicates the locale Id </param>
        /// <param name="cnodeId">Indicates the Cnode Id </param>
        /// <param name="catalogId">Indicates the Catalog Id </param>
        /// <param name="attributeId">Indicates the Attribute Id </param>
        /// <param name="toTotalImpacted">Returns the Total Impacted Items </param>
        /// <param name="totalAffected">Returns Total Affected Items </param>
        /// <param name="maxReturnCount">Maximum Return counts </param>
        /// <returns>XML representing affected entity detail</returns>
        public string GetImpactedEntities(Int32 localeId, Int64 cnodeId, Int32 catalogId, Int32 attributeId, ref Int32 toTotalImpacted, ref Int32 totalAffected, ref Int32 maxReturnCount)
        {
            String impactedEntities = String.Empty;
            try
            {
                impactedEntities = _entityOperationsTempDA.GetImpactedEntities(localeId, cnodeId, catalogId, attributeId, ref toTotalImpacted, ref  totalAffected, ref  maxReturnCount);
            }
            finally
            {

            }

            return impactedEntities;
        }

        #endregion

        #region MDL

        #region Get MDL

        /// <summary>
        /// Gets list of Entities for a given list of Entity Ids, ContainerId, CategoryId combination
        /// </summary>
        /// <param name="entityIdList">list of entity ids to search for their mdls</param>
        /// <param name="containerId">container id of the entity to be searched</param>
        /// <param name="categoryId">category id of the entity to be searched</param>
        /// <returns>List of entity objects</returns>
        public Collection<Entity> GetMDLsByIdList(String entityIdList, Int64 categoryId, Int32 containerId)
        {
            Collection<Entity> entityList = new Collection<Entity>();
            entityList = _entityOperationsTempDA.GetMDLsByIdList(entityIdList, categoryId, containerId);
            return entityList;
        }

        /// <summary>
        /// Get all MDL containers for a given CNode.
        /// </summary>
        /// <param name="cnodeID">CNode of which MDLs are to be found</param>
        /// <param name="catalogIDList">If catalog ID List is specified then will return the CNode of given containers only</param>
        /// <param name="delimiter">Delimiter for the catalogIDList</param>
        /// <param name="userName">Login user Name </param>
        /// <param name="returnSelf">If true then return itself in the result. If false then returns only other containers</param>
        /// <returns>xML having MDLed entity detail</returns>
        public String GetMDL(Int64 cnodeID, String catalogIDList, String delimiter, String userName, Boolean returnSelf)
        {
            return _entityOperationsTempDA.GetMDL(cnodeID, catalogIDList, delimiter, userName, returnSelf);
        }

        /// <summary>
        /// Get all MDL containers for a given CNode.
        /// </summary>
        /// <param name="cnodeID">CNode of which MDLs are to be found</param>
        /// <param name="catalogIDList">If catalog ID List is specified then will return the CNode of given containers only</param>
        /// <param name="delimiter">Delimiter for the catalogIDList</param>
        /// <param name="returnSelf">If true then return itself in the result. If false then returns only other containers</param>
        /// <returns>Entity objects representing the MDLs of specified CNode</returns>
        public Collection<Entity> GetMDL(Int64 cnodeID, String catalogIDList, String delimiter, Boolean returnSelf)
        {
            Collection<Entity> lstContainers = null;
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            String containers = _entityOperationsTempDA.GetMDL(cnodeID, catalogIDList, delimiter, userName, returnSelf);

            if (!String.IsNullOrEmpty(containers))
            {
                XmlTextReader xmlReader = new XmlTextReader(containers, XmlNodeType.Element, null);

                try
                {
                    lstContainers = new Collection<Entity>();
                    while (xmlReader.Read())
                    {
                        if (xmlReader.Name == "MDL")
                        {
                            if (xmlReader.IsStartElement())
                            {
                                if (xmlReader.HasAttributes)
                                {
                                    Entity container = new Entity();
                                    xmlReader.MoveToAttribute("PK_CNode");
                                    container.Id = xmlReader.ReadContentAsInt();

                                    xmlReader.MoveToAttribute("CatalogID");
                                    container.ContainerId = xmlReader.ReadContentAsInt();

                                    xmlReader.MoveToAttribute("CatalogName");
                                    container.ContainerLongName = xmlReader.ReadContentAsString();

                                    xmlReader.MoveToAttribute("CategoryID");
                                    container.CategoryId = xmlReader.ReadContentAsInt();

                                    xmlReader.MoveToAttribute("CategoryName");
                                    container.CategoryName = xmlReader.ReadContentAsString();

                                    xmlReader.MoveToAttribute("CategoryLongName");
                                    container.CategoryLongName = xmlReader.ReadContentAsString();

                                    xmlReader.MoveToAttribute("LNPath");
                                    container.Path = xmlReader.ReadContentAsString();

                                    xmlReader.MoveToAttribute("OrgId");
                                    container.OrganizationName = xmlReader.ReadContentAsString();

                                    xmlReader.MoveToAttribute("OrgName");
                                    container.OrganizationName = xmlReader.ReadContentAsString();

                                    lstContainers.Add(container);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (xmlReader != null)
                        xmlReader.Close();
                }
            }

            return lstContainers;
        }

        /// <summary>
        /// Get all MDL containers for a given CNode.
        /// </summary>
        /// <param name="cnodeID">CNode of which MDLs are to be found</param>
        /// <param name="dataConfigXML">Configuration for Panel Data</param>
        /// <param name="catalogIDList">If catalog ID List is specified then will return the CNode of given containers only</param>
        /// <param name="delimiter">Delimiter for the catalogIDList</param>
        /// <param name="returnSelf">If true then return itself in the result. If false then returns only other containers</param>
        /// <returns>Entity objects representing the MDLs of specified CNode</returns>
        public Collection<Entity> GetMDLBasedOnConfig(Int64 cnodeID, System.Data.SqlTypes.SqlString dataConfigXML, String catalogIDList, String delimiter, Boolean returnSelf)
        {
            Collection<Entity> mdlDetails = null;

            String userName = String.Empty;
            Int32 userLocaleId = 61;
            SecurityPrincipal securityPrinciple = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();

            if (securityPrinciple != null)
            {
                userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                userLocaleId = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserLocaleId;
            }

            mdlDetails = _entityOperationsTempDA.GetMDLBasedOnConfig(cnodeID, dataConfigXML.ToString(), catalogIDList, delimiter, userName, userLocaleId, returnSelf);

            return mdlDetails;
        }

        /// <summary>
        /// Returns the value of specified Common/Technical attribute values for given CNode
        /// The stored procedure returns the Catalog Extract in XML.    
        /// The called SP checks that the ID and ORG supplied actually exist in the database.    
        /// If no match is found, the Catalog requested is eliminated from the return. (No error is also returned from called SP)
        /// </summary>
        /// <param name="extSystemID">Extended System ID. Default value = 1</param>
        /// <param name="inputXML">XML specifying CNode for which value is to be fetched. Sample input xml is as follow:
        /// <para>
        /// &lt;xml id="ComponentsXml"&gt;
        ///         &lt;ArrayOfGenericElement&gt;
        ///             &lt;GenericElement TCode="PRHG01" ItemNumber="0001"&gt;
        ///                 &lt;UKey ObjectType="Catalog" ExternalID="" InternalID="17"/&gt;
        ///                 &lt;KeyAttributes&gt;
        ///                     &lt;KeyAttr ObjectType="Organization" Name="Organization" ExternalID="" InternalID="101"/&gt;
        ///                     &lt;KeyAttr ObjectType="CatalogNode" Name="CNodeParent" ExternalID="" InternalID="298"/&gt;
        ///                 &lt;/KeyAttributes&gt;
        ///                 &lt;ValueAttributes&gt;
        ///                     &lt;ValueAttr ObjectType="CNode" Name="CNode" ExternalID="" InternalID="1435683"/&gt;
        ///                 &lt;/ValueAttributes&gt;
        ///             &lt;/GenericElement&gt;
        ///         &lt;/ArrayOfGenericElement&gt;
        ///     &lt;/xml&gt;
        /// </para>
        /// </param>
        /// <param name="coreAttrList">List of comma separated Common attributes </param>
        /// <param name="techAttrList">List of comma separated Technical attributes </param>
        /// <param name="localeID">Locale of the values being saved</param>
        /// <param name="programName">The name of program which is calling the method</param>
        /// <param name="includeInheritedValue">Whether to include inherited value or not</param>
        /// <param name="computeInheritedValues">Whether to include inherited value by doing real time inheritance walk or take from denorm tables</param>
        /// <returns>XML containing attribute values. The sample output is as follow:
        /// <para>
        ///      &lt;Data&gt;
        ///         &lt;Catalogs&gt;
        ///         &lt;Catalog ItemNumber="1" TCode="PRHG01"&gt;
        ///         &lt;UKey ObjectType="Catalog" InternalID="17" ExternalID="" /&gt;
        ///         &lt;KeyAttributes&gt;
        ///             &lt;KeyAttr ObjectType="Organization" Name="Organization" InternalID="101" ExternalID="" /&gt;
        ///         &lt;/KeyAttributes&gt;
        ///         &lt;ValueAttributes&gt;
        ///             &lt;ValueAttr Name="ShortName" Value="Merchandising Catalog" /&gt;
        ///             &lt;ValueAttr Name="LongName" Value="Merchandising Catalog" /&gt;
        ///             &lt;ValueAttr Name="DefaultFlag" Value="Y" /&gt;
        ///             &lt;ValueAttr Name="ValidFrom" Value="May 18 2010 12:03PM" /&gt;
        ///             &lt;ValueAttr Name="ValidTo" Value="May 18 2012 12:03PM" /&gt;
        ///         &lt;/ValueAttributes&gt;
        ///         &lt;CNodes&gt;
        ///             &lt;CNode TCode="PHNA01" ItemNumber="1"&gt;
        ///                 &lt;UKey ObjectType="Catalog" InternalID="1435683" ExternalID="" /&gt;
        ///                 &lt;KeyAttributes&gt;
        ///                     &lt;KeyAttr ObjectType="Catalog" Name="Catalog" ExternalID="" InternalID="17" /&gt;
        ///                     &lt;KeyAttr ObjectType="CatalogNode" Name="ParentNode" ExternalID="" InternalID="298" /&gt;
        ///                     &lt;KeyAttr ObjectType="NodeType" Name="NodeType" ExternalID="" InternalID="7" /&gt;
        ///                     &lt;KeyAttr ObjectType="BranchLevel" Name="BranchLevel" ExternalID="" InternalID="2" /&gt;
        ///                 &lt;/KeyAttributes&gt;
        ///                 &lt;ValueAttributes&gt;
        ///                     &lt;ValueAttr Name="ShortName" Value="J1266" /&gt;
        ///                     &lt;ValueAttr Name="LongName" Value="j1266_test" /&gt;
        ///                     &lt;ValueAttr Name="SKU" Value="J1266" /&gt;
        ///                     &lt;ValueAttr Name="ComponentType" Value="SKU" /&gt;
        ///                     &lt;ValueAttr Name="EffectiveFrom" Value="Jul 11 2003 12:00AM" /&gt;
        ///                     &lt;ValueAttr Name="EffectiveTo" Value="Apr  6 2109  1:41AM" /&gt;
        ///                     &lt;ValueAttr Name="Sequence" Value="1" /&gt;
        ///                 &lt;/ValueAttributes&gt;
        ///                 &lt;CoreAttributes&gt;
        ///                     &lt;CoreAttr ID="3018" Name="Backorder Eligible" Value="True" ValueKey="0" hasUOM="N" DefaultUOM="" AllowableUOM="" AttributeAllowvalues="" Locale="en_WW" Parent="3027" SRCFlag="O" IsReadOnly="0" /&gt;
        ///                     &lt;CoreAttr ID="3014" Name="Buyer ID" Value="ann.heacock" ValueKey="0" hasUOM="N" DefaultUOM="" AllowableUOM="" AttributeAllowvalues="" Locale="en_WW" Parent="3010" SRCFlag="O" IsReadOnly="0" /&gt;
        ///                     &lt;CoreAttr ID="3008" Name="Pattern" Value="Starlite" ValueKey="0" hasUOM="N" DefaultUOM="" AllowableUOM="" AttributeAllowvalues="" Locale="en_WW" Parent="3010" SRCFlag="O" IsReadOnly="0" /&gt;
        ///                     &lt;CoreAttr ID="3015" Name="Coordinator ID" Value="virginia.johnson" ValueKey="0" hasUOM="N" DefaultUOM="" AllowableUOM="" AttributeAllowvalues="" Locale="en_WW" Parent="3010" SRCFlag="O" IsReadOnly="0" /&gt;
        ///                     &lt;CoreAttr ID="3017" Name="Item Class" Value="DEFAULT" ValueKey="0" hasUOM="N" DefaultUOM="" AllowableUOM="" AttributeAllowvalues="" Locale="en_WW" Parent="3010" SRCFlag="O" IsReadOnly="0" /&gt;
        ///                 &lt;/CoreAttributes&gt;
        ///             &lt;/CNode&gt;
        ///         &lt;/CNodes&gt;
        ///     &lt;/Catalog&gt;
        /// &lt;/Catalogs&gt;
        /// &lt;/Data&gt;
        /// </para>
        /// </returns>
        public String GetAttributeValuesForMDLs(Int32 extSystemID, String inputXML, String coreAttrList, String techAttrList, Int32 localeID, String programName, Boolean includeInheritedValue, Boolean computeInheritedValues)
        {
            return _entityOperationsTempDA.GetMDLsAttributeValues(extSystemID, inputXML, coreAttrList, techAttrList, localeID, programName, includeInheritedValue, computeInheritedValues);
        }

        #endregion

        #region Process MDL

        /// <summary>
        /// Process of MDL
        /// </summary>
        /// <param name="mdlXml">MDL xml Value</param>
        /// <param name="userLogin">User Login</param>
        public void ProcessMDL(String mdlXml, String userLogin)
        {
            _entityOperationsTempDA.ProcessMDL(mdlXml, userLogin);
        }

        #endregion

        #endregion

        #region Get Core Attributes

        #endregion

        #region Get Parent

        /// <summary>
        /// Get Parent
        /// </summary>
        /// <param name="cnodeId">Indicates the Cnode Id of an Entity</param>
        /// <param name="cnodeParentId">Indicates the Cnode Parent Id of an Entity</param>
        /// <param name="containerId">Indicates the Catalog Id of an Entity</param>
        /// <param name="bitUseDraftTax">bitUseDraftTax</param>
        /// <returns>GetParent as Xml String</returns>
        public String GetParent(Int64 cnodeId, Int64 cnodeParentId, Int32 containerId, Boolean bitUseDraftTax, Int32 dataLocale)
        {
            return _entityOperationsTempDA.GetParent(cnodeId, cnodeParentId, containerId, bitUseDraftTax, dataLocale);
        }

        /// <summary>
        /// Get Parent
        /// </summary>
        /// <param name="cnodeId">Indicates the Cnode Id of an Entity</param>
        /// <param name="cnodeParentId">Indicates the Cnode Parent Id of an Entity</param>
        /// <param name="containerId">Indicates the Catalog Id of an Entity</param>
        /// <returns>Gets the List of parent Entities.</returns>
        public List<Entity> GetParent(Int64 cnodeId, Int64 cnodeParentId, Int32 containerId, Int32 dataLocale)
        {
            List<Entity> parentEntities = new List<Entity>();
            if (cnodeId > 0)
            {
                String ParentXML = String.Empty;

                ParentXML = _entityOperationsTempDA.GetParent(cnodeId, cnodeParentId, containerId, false, dataLocale);
                if (!String.IsNullOrEmpty(ParentXML))
                {
                    Int64 currentCNodeID = 0;
                    String currentCNodeLongName = String.Empty;
                    Int32 entityTypeId = 0;
                    XmlTextReader xmlReader = new XmlTextReader(ParentXML, XmlNodeType.Element, null);

                    try
                    {
                        while (xmlReader.Read())
                        {
                            if (xmlReader.Name == "CNode")
                            {
                                Entity entity = new Entity();

                                xmlReader.MoveToAttribute("CNodeID");
                                currentCNodeID = xmlReader.ReadContentAsLong();
                                entity.Id = currentCNodeID;

                                xmlReader.MoveToAttribute("CNodeLongName");
                                currentCNodeLongName = xmlReader.ReadContentAsString();
                                entity.LongName = currentCNodeLongName;

                                xmlReader.MoveToAttribute("FK_NodeType");
                                entityTypeId = xmlReader.ReadContentAsInt();
                                entity.EntityTypeId = entityTypeId;

                                parentEntities.Add(entity);
                            }
                        }
                    }
                    finally
                    {
                        if (xmlReader != null)
                            xmlReader.Close();
                    }
                }
            }
            return parentEntities;
        }

        #endregion

        #region Get Nodetype

        /// <summary>
        /// Get child Node Type based on parentEntityId or ParentEntityTypeId
        /// </summary>
        /// <param name="parentEntityId">Specifies the id of parentEntity</param>
        /// <param name="parentEntityTypeId">Specifies the id of parentEntity Type</param>
        /// <param name="application">Name of the application which is perfoming action</param>
        /// <param name="module">Name of the module which is perfoming action</param>
        /// <returns>Collection of EntityTypes</returns>
        /// <exception cref="ArgumentException">Thrown if parentEntityId is less than 0</exception>
        /// <exception cref="ArgumentException">Thrown if ParentEntityTypeId is less than 0</exception>
        public EntityTypeCollection GetEntityNodeType(Int64 parentEntityId, Int32 parentEntityTypeId, MDMCenterApplication application, MDMCenterModules module)
        {
            #region ParameterValidation

            if (parentEntityId <= 0)
            {
                throw new ArgumentException("EntityId must be greater than 0");
            }

            if (parentEntityTypeId <= 0)
            {
                throw new ArgumentException("ParentEntityTypeId must be greater than 0");
            }

            #endregion

            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            EntityOperationsDA entityOperationsDA = new EntityOperationsDA();

            return entityOperationsDA.GetEntityNodeType(parentEntityId, parentEntityTypeId, command);
        }

        #endregion

        #region Move

        /// <summary>
        /// Move Catalog/Category/Entity node to a different Node
        /// </summary>
        /// <param name="fromCatalogId">Specifies from which Catalog,item Should be Moved</param>
        /// <param name="fromEntityId">Specifies from which Entity,item Should be Moved</param>
        /// <param name="toCatalogId">Specifies To which Catalog item Should be Moved</param>
        /// <param name="toEntityId">Specifies To which Entity item Should be Moved</param>
        /// <param name="userName">Specifies UserId</param>
        /// <param name="programName">Specifies Program Name</param>
        /// <param name="application">Specifies in Which Application it is using</param>
        /// <param name="module">Specifies module of an Application</param>
        /// <returns>1 if Move is successful</returns>
        public Int32 Move(Int32 fromCatalogId, Int64 fromEntityId, Int32 toCatalogId, Int64 toEntityId, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            #region ParameterValidation

            if (fromCatalogId <= 0)
            {
                throw new ArgumentException("CatalogId from where you want to move must be greater then 0");
            }

            if (fromEntityId <= 0)
            {
                throw new ArgumentException("EntityId from where you want to move must be greater then 0");
            }

            if (toCatalogId <= 0)
            {
                throw new ArgumentException("CatalogId must be greater then 0");
            }

            //if ToEntityId is NULL,Item is placed directly under catalog root
            //UserId and ProgramName can be Null.

            #endregion

            String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Update);

            EntityOperationsDA entityOperationsDA = new EntityOperationsDA();

            return entityOperationsDA.Move(fromCatalogId, fromEntityId, toCatalogId, toEntityId, loginUser, programName, command);
        }

        #endregion

        #region Get Affected Info

        /// <summary>
        /// Get Color coding information for a list of entities of a list of attributes
        /// </summary>
        /// <param name="entityIdList">comma separeted list of entity id</param>
        /// <param name="attributeIdList">comma separeted list of attribute id</param>
        /// <param name="containerId">container id for list of entities, because it can be Category</param>
        /// <returns>Xml which returns color code information based on affected node results for the entities</returns>
        public String GetAffectedInfo(Collection<Int64> entityIds, String attributeIdList, Int32 containerId, Int32 currentDataLocaleId, Int32 systemDataLocaleId)
        {
            //Result Xml:
            //<Entities>  
            // <Entity Id="10">  
            //  <Attribute Id="4052" Flag="B">  
            //  <Attribute Id="4053" Flag="G">  
            //  <Attribute Id="4054" Flag="R">  
            // </Entity>  
            // <Entity Id="330">  
            //  <Attribute Id="4052" Flag="R">  
            //  <Attribute Id="4053" Flag="G">  
            //  <Attribute Id="4054" Flag="R">  
            // </Entity>  
            //</Entities> 
            EntityOperationsDA entityDataManager = new EntityOperationsDA();
            return entityDataManager.GetAffectedInfo(entityIds, attributeIdList, containerId, currentDataLocaleId, systemDataLocaleId);
        }

        #endregion

        #region Process Attribute Exception

        /// <summary>
        ///  Attribute Exception  process
        /// </summary>
        /// <param name="cnodeId">Indicates the Cnode Id </param>
        /// <param name="attributeId">Indicates the Cnode Id </param>
        /// <param name="srcCatalog">Indicates the source catalog</param>
        /// <param name="startDate">Indicates the Start Date</param>
        /// <param name="endDate">Indicates the End Date</param>
        /// <param name="userLogin">Indicates the User Login</param>
        public void ProcessAttributeException(Int64 cnodeId, Int32 attributeId, Int32 srcCatalog, DateTime startDate, DateTime endDate, String userLogin)
        {
            _entityOperationsTempDA.ProcessAttributeException(cnodeId, attributeId, srcCatalog, startDate, endDate, userLogin);
        }

        #endregion

        #region Get Lookup Values

        /// <summary>
        /// Get Lookup Values
        /// </summary>
        /// <param name="cnodeId">Indicates the Cnode Id</param>
        /// <param name="attributeId">Indicates the Attribute Id</param>
        /// <param name="returnLookupColumns">Return Lookup Columns</param>
        /// <param name="delimiter">passed delimiter</param>
        /// <returns>Lookup values as string.</returns>
        public String GetLookupValues(Int64 cnodeId, Int32 attributeId, String returnLookupColumns, String delimiter)
        {
            return _entityOperationsTempDA.GetLookupValues(cnodeId, attributeId, returnLookupColumns, delimiter);
        }

        #endregion

        #region Workflow

        /// <summary>
        /// Gets the workflow tasks list for the requested cNodeID and userID
        /// </summary>
        /// <param name="cNodeID">Requested cNodeID</param>
        /// <param name="userID">Requested userID</param>
        /// <returns>Collection of Workflow Activities.</returns>
        public static Collection<MDMBOW.WorkflowActivity> GetWorkflowTasks(Int64 cNodeID, Int32 userID)
        {
            Collection<MDMBOW.WorkflowActivity> workflowActivityCollection = null;

            EntityOperationsDA entityDA = new EntityOperationsDA();
            workflowActivityCollection = entityDA.GetWorkflowTasks(cNodeID, userID);

            return workflowActivityCollection;
        }

        /// <summary>
        /// Gets the workflow status as xml.
        /// </summary>
        /// <param name="entityIDList">List of entity IDs</param>
        /// <param name="activityID">Activity ID</param>
        /// <param name="workflowType">Type of workflow</param>
        /// <param name="userID">Requested userId.</param>
        /// <returns>Workflow status as Xml Document</returns>
        public String GetWorkflowStatusXml(String entityIDList, Int32 activityID, String workflowType, Int32 userID)
        {
            EntityOperationsDA entityDataManager = new EntityOperationsDA();
            String strWorkflow = entityDataManager.GetWorkflowStatus(entityIDList, activityID, workflowType, userID);
            return strWorkflow;
        }

        #endregion

        #region Reclassify

        /// <summary>
        /// Reclassifies entities given in input xml
        /// </summary>
        /// <param name="dataXml">Xml containing list of entity Ids to be reclassified with destination container ids..</param>
        /// <param name="userName">UserName who is doing the reclassification</param>
        /// <param name="isCategoryReclassify">True if it is CategoryReclassify otherwise EntityReclassify as false</param>
        /// <returns>Datatable</returns>
        public DataTable Reclassify(String dataXml, String userName, Boolean isCategoryReclassify)
        {
            return Reclassify(dataXml, "MDM.EntityManager.Business.EntityOpeationBL.Reclassify", userName, isCategoryReclassify);
        }

        /// <summary>
        /// Reclassifies entities given in input xml
        /// </summary>
        /// <param name="dataXml">Xml containing list of entity Ids to be reclassified with destination container ids..</param>
        /// <param name="programName">Program wgich is requesting for the reclassification</param>
        /// <param name="userName">UserName who is doing the reclassification</param>
        /// <param name="isCategoryReclassify">True if it is CategoryReclassify otherwise EntityReclassify as false</param>
        /// <returns>Datatable</returns>
        public DataTable Reclassify(String dataXml, String programName, String userName, Boolean isCategoryReclassify)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Business.EntityOpeationBL.Reclassify", false);

            DataTable dataTable = new DataTable();
            EntityCollection entityCollection = new EntityCollection();

            dataTable = _entityOperationsTempDA.Reclassify(dataXml, userName, programName);

            #region Remove Entity from Cache

            if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
            {
                DataRow[] dataResult = dataTable.Select("Error=False");

                if (dataResult.Length > 0)
                {
                    entityCollection = PrepareEntityCollectionFromReclassifyXml(dataXml, isCategoryReclassify);
                    EntityCacheInvalidateContextCollection entityCacheInvalidateContextCollection = new EntityCacheInvalidateContextCollection();
                    HashSet<Int32> hierarchyIds = new HashSet<Int32>();

                    foreach (DataRow dr in dataResult)
                    {
                        Int64 entityId = ValueTypeHelper.Int64TryParse(dr["FK_Cnode"].ToString(), 0);
                        Int32 hierarchyId = ValueTypeHelper.Int32TryParse(dr["FK_Catalog"].ToString(), 0);

                        if (hierarchyId > 0 && isCategoryReclassify)
                        {
                            hierarchyIds.Add(hierarchyId);
                        }

                        if (entityCollection.Contains(entityId))
                        {
                            Entity entity = entityCollection[entityId];

                            EntityCacheInvalidateContext entityCacheInvalidateContext = new EntityCacheInvalidateContext();
                            entityCacheInvalidateContext.EntityId = entity.Id;
                            entityCacheInvalidateContext.ContainerId = entity.ContainerId;

                            entityCacheInvalidateContextCollection.Add(entityCacheInvalidateContext);
                        }
                    }

                    if (entityCacheInvalidateContextCollection != null && entityCacheInvalidateContextCollection.Count > 0)
                    {
                        EntityBL entityManager = new EntityBL();
                        CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

                        entityManager.RemoveEntityCache(entityCacheInvalidateContextCollection, callerContext);
                    }

                    if (hierarchyIds.Count > 0)
                    {
                        CategoryBufferManager categoryBufferManager = new CategoryBufferManager();

                        foreach (Int32 hierarchyId in hierarchyIds)
                        {
                            categoryBufferManager.RemoveBaseCategories(hierarchyId);
                        }
                    }
                }
            }

            #endregion

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.Business.EntityOpeationBL.Reclassify");

            return dataTable;
        }

        #endregion Reclassify

        #region Legacy Get Entity Attributes directly from DB

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeID"></param>
        /// <param name="customerID"></param>
        /// <param name="CNodeID"></param>
        /// <param name="CNodeParentID"></param>
        /// <param name="attrGroupID"></param>
        /// <param name="catalogID"></param>
        /// <param name="orgID"></param>
        /// <param name="user"></param>
        /// <param name="backupLocaleID"></param>
        /// <param name="viewPath"></param>
        /// <param name="useDraftTaxonomy"></param>
        /// <param name="encodeCollections"></param>
        /// <param name="attrIDList"></param>
        /// <returns></returns>
        public string GetCoreAttrXml(int localeID, int customerID, Int64 CNodeID, Int64 CNodeParentID, int attrGroupID, int catalogID, int orgID, string user, int backupLocaleID, string viewPath, bool useDraftTaxonomy, bool encodeCollections, string attrIDList)
        {
            //Changed by Vishal for bug#678. dt. 28-Mar-2008.
            //Call overload method with a ShowAtCreation as 0. 
            return GetCoreAttrXml(localeID, customerID, CNodeID, CNodeParentID, attrGroupID, catalogID, orgID, user, backupLocaleID, viewPath, useDraftTaxonomy, encodeCollections, attrIDList, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeID"></param>
        /// <param name="customerID"></param>
        /// <param name="CNodeID"></param>
        /// <param name="CNodeParentID"></param>
        /// <param name="attrGroupID"></param>
        /// <param name="catalogID"></param>
        /// <param name="orgID"></param>
        /// <param name="user"></param>
        /// <param name="backupLocaleID"></param>
        /// <param name="viewPath"></param>
        /// <param name="useDraftTaxonomy"></param>
        /// <param name="encodeCollections"></param>
        /// <param name="attrIDList"></param>
        /// <param name="showAtCreation"></param>
        /// <returns></returns>
        public string GetCoreAttrXml(int localeID, int customerID, Int64 CNodeID, Int64 CNodeParentID, int attrGroupID, int catalogID, int orgID, string user, int backupLocaleID, string viewPath, bool useDraftTaxonomy, bool encodeCollections, string attrIDList, bool showAtCreation)
        {
            string contextXML = Riversand.StoredProcedures.Catalog.GetCoreAttrByGroup(
                localeID, customerID, CNodeID, CNodeParentID < 0 ? SqlInt64.Null : CNodeParentID,
                attrGroupID, catalogID, orgID, user,
                backupLocaleID < 0 ? SqlInt32.Null : backupLocaleID, viewPath, useDraftTaxonomy, showAtCreation, attrIDList);

            if (encodeCollections) //&& contextXML != "<CoreAttributes ID=\"1\"/>")
                contextXML = EncodeCollectionXml(contextXML, "CoreAttributes", "CoreAttr");

            return contextXML;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CNodeID"></param>
        /// <param name="CNodeParentID"></param>
        /// <param name="catalogID"></param>
        /// <param name="attrGroupID"></param>
        /// <param name="localeID"></param>
        /// <param name="customerID"></param>
        /// <param name="user"></param>
        /// <param name="backupLocaleID"></param>
        /// <param name="viewPath"></param>
        /// <param name="useDraftTaxonomy"></param>
        /// <param name="encodeCollections"></param>
        /// <param name="attrIDList"></param>
        /// <returns></returns>
        public string GetTechAttrXml(int CNodeID, int CNodeParentID, int catalogID, int attrGroupID, int localeID, int customerID, string user, int backupLocaleID, string viewPath, bool useDraftTaxonomy, bool encodeCollections, string attrIDList)
        {
            string contextXML = Riversand.StoredProcedures.Catalog.GetTechAttr(
                CNodeID, CNodeParentID < 0 ? SqlInt32.Null : CNodeParentID, catalogID,
                attrGroupID, localeID, customerID, user,
                backupLocaleID < 0 ? SqlInt32.Null : backupLocaleID, viewPath, useDraftTaxonomy, attrIDList);

            if (encodeCollections) //&& contextXML != "<TechAttributes ID=\"1\"/>")
                contextXML = EncodeCollectionXml(contextXML, "TechAttributes", "TechAttribute");

            return contextXML;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rootTag"></param>
        /// <param name="attrTag"></param>
        /// <returns></returns>
        private static string EncodeCollectionXml(string source, string rootTag, string attrTag)
        {
            String xml = String.Empty;
            XmlTextReader xmlRdr = new XmlTextReader(source, XmlNodeType.Element, null);
            StringWriter strWtr = new StringWriter();
            XmlTextWriter xmlWtr = new XmlTextWriter(strWtr);

            /* Read and copy data from xmlRdr to xmlWtr
                 When xmlRdr <attrTag> has collection children, <CollectionXml ValSource=""> 
                 then encode and put in appropriate val attribute.
              <rootTag>
                  <attrTag>
                      <CollectionXml>
                          <SimpleAttribute/>
                      </CollectionXml>
                  </attrTag>
              </rootTag>			  
            */
            string ValSource = string.Empty;
            Hashtable values = new Hashtable();
            Hashtable valStrWriters = new Hashtable();
            Hashtable valWriters = new Hashtable();
            bool OpenCollectionXml = false;

            while (xmlRdr.Read())
            {
                //values.Clear();
                if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == rootTag)
                {
                    xmlWtr.WriteStartElement(rootTag);
                    while (xmlRdr.MoveToNextAttribute())
                    {
                        xmlWtr.WriteAttributeString(xmlRdr.Name, xmlRdr.Value);
                    }
                    xmlRdr.MoveToElement();
                    if (xmlRdr.IsEmptyElement)
                        xmlWtr.WriteEndElement(); //</rootTag>
                }
                else if (xmlRdr.NodeType == XmlNodeType.EndElement && xmlRdr.Name == rootTag)
                {
                    xmlWtr.WriteEndElement(); //</attrTag>
                }
                else if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == attrTag)
                {
                    xmlWtr.WriteStartElement(attrTag);

                    while (xmlRdr.MoveToNextAttribute())
                    {
                        if (xmlRdr.Name == "Val" || xmlRdr.Name == "InheritVal" || xmlRdr.Name == "MasterVal")
                            values[xmlRdr.Name] = xmlRdr.Value;
                        else
                            xmlWtr.WriteAttributeString(xmlRdr.Name, xmlRdr.Value);
                    }
                    xmlRdr.MoveToElement();
                    if (xmlRdr.IsEmptyElement)
                    {
                        xmlWtr.WriteAttributeString("Val", values["Val"] as string);
                        xmlWtr.WriteAttributeString("InheritVal", values["InheritVal"] as string);
                        xmlWtr.WriteAttributeString("MasterVal", values["MasterVal"] as string);
                        xmlWtr.WriteEndElement(); //</attrTag>
                        values.Clear();
                    }
                }
                else if (xmlRdr.NodeType == XmlNodeType.EndElement && xmlRdr.Name == attrTag)
                {
                    xmlWtr.WriteAttributeString("Val", values["Val"] as string);
                    xmlWtr.WriteAttributeString("InheritVal", values["InheritVal"] as string);
                    xmlWtr.WriteAttributeString("MasterVal", values["MasterVal"] as string);
                    xmlWtr.WriteEndElement(); //</attrTag>
                    values.Clear();
                }
                else if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == "CollectionXml")
                {
                    xmlRdr.MoveToAttribute("ValSource");
                    ValSource = xmlRdr.Value;
                    xmlRdr.MoveToElement();
                    if (xmlRdr.IsEmptyElement)
                    {
                        values[ValSource] = string.Empty;
                    }
                }
                else if (xmlRdr.NodeType == XmlNodeType.EndElement && xmlRdr.Name == "CollectionXml")
                {
                    if (valWriters.Count > 0)
                    {
                        XmlTextWriter xmlWtrVal = valWriters[ValSource] as XmlTextWriter;
                        if (xmlWtrVal != null)
                            xmlWtrVal.WriteEndElement(); //</CollectionXml>
                    }
                    if (valStrWriters.Count > 0)
                    {
                        values[ValSource] = (valStrWriters[ValSource] as StringWriter).ToString();
                    }
                    OpenCollectionXml = false;
                }
                else if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == "SimpleAttribute")
                {
                    XmlTextWriter xmlWtrVal;
                    if (!OpenCollectionXml)
                    {
                        OpenCollectionXml = true;
                        StringWriter strWtrVal = new StringWriter();
                        valStrWriters[ValSource] = strWtrVal;
                        xmlWtrVal = new XmlTextWriter(strWtrVal);
                        valWriters[ValSource] = xmlWtrVal;
                        xmlWtrVal.WriteStartElement("CollectionXml");
                    }
                    else
                    {
                        xmlWtrVal = valWriters[ValSource] as XmlTextWriter;
                    }
                    //Placed a condition here for a check for "xmlWtrVal" in case if it will be null.
                    if (valWriters.Count > 0)
                    {
                        if (xmlWtrVal != null)
                        {
                            xmlWtrVal.WriteStartElement("SimpleAttribute");
                            while (xmlRdr.MoveToNextAttribute())
                            {
                                xmlWtrVal.WriteAttributeString(xmlRdr.Name, xmlRdr.Value);
                            }
                            xmlWtrVal.WriteEndElement(); //</SimpleAttribute>
                        }
                    }
                }
            }

            xml = strWtr.ToString();

            xmlWtr.Close();
            xmlRdr.Close();
            strWtr.Close();

            return xml;
        }

        #endregion

        #region Get Entity Types with Level

        /// <summary>
        /// Gets entity variants level and entity types based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity variant level and entity type to be fetched</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns collection of key value pair with key as variant level and value as entity type</returns>
        public Dictionary<Int32, EntityType> GetEntityVariantLevels(Int64 entityId, CallerContext callerContext)
        {
            #region Initial SetUp
            
            EntityType entityType = null;
            EntityTypeBL entityTypeManager = new EntityTypeBL();
            Dictionary<Int32, EntityType> entityTypeToVariantLevelMappings = null; // key : level and value : EntityType 

            #endregion Initial SetUp

            #region Diagnostics & tracing

            var diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion Diagnostics & tracing

            try
            {
                Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = GetEntityVariantLevel(entityId, callerContext); // key : entityTypeId | value : variant level

                if (entityTypeIdToVariantLevelMappings != null && entityTypeIdToVariantLevelMappings.Count > 0)
                {
                    Collection<Int32> entityTypeIdList = new Collection<Int32>();
                    foreach (KeyValuePair<Int32, Int32> keyValuePair in entityTypeIdToVariantLevelMappings)
                    {
                        entityTypeIdList.Add(keyValuePair.Key);
                    }

                    // Get all entity types in one shot.
                    EntityTypeCollection entityTypes = entityTypeManager.GetEntityTypesByIds(entityTypeIdList);

                    if (entityTypes != null && entityTypes.Count > 0)
                    {
                        entityTypeToVariantLevelMappings = new Dictionary<Int32, EntityType>();

                        foreach (KeyValuePair<Int32, Int32> keyValuePair in entityTypeIdToVariantLevelMappings)
                        {
                            entityTypeToVariantLevelMappings.TryGetValue(keyValuePair.Value, out entityType);
                            if (entityType == null)
                            {
                                entityType = entityTypes.Get(keyValuePair.Key);
                                entityTypeToVariantLevelMappings.Add(keyValuePair.Value, entityType); // key : variant level | value : entity type
                            }
                        }  
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("GetEntityVariantLevelWithEntityType is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return entityTypeToVariantLevelMappings;
        }

        /// <summary>
        /// Gets entity variants level based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity variant level to be fetched</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns collection of key value pair with key as entity type id and value as variant level</returns>
        public Dictionary<Int32, Int32> GetEntityVariantLevel(Int64 entityId, CallerContext callerContext)
        {
            #region Initial Setup

            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = null;

            #endregion Initial Setup

            #region Diagnostics & tracing

            var diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("GetEntityVariantLevel is started.");
            }

            #endregion Diagnostics & tracing

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                EntityOperationsDA entityOperationsDA = new EntityOperationsDA();

                entityTypeIdToVariantLevelMappings = entityOperationsDA.GetEntityVariantLevel(entityId, command);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("GetEntityVariantLevel is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return entityTypeIdToVariantLevelMappings;
        }
       
        #endregion

        #region Private Methods

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private EntityCollection PrepareEntityCollectionFromReclassifyXml(String valuesAsXml, Boolean isCategoryReclassify)
        {
            EntityCollection entityCollection = new EntityCollection();

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                        {
                            Entity entity = new Entity();

                            #region Read Reclssify Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    entity.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    entity.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (isCategoryReclassify)
                                    entity.EntityTypeId = 6;

                                entityCollection.Add(entity);

                            }

                            #endregion
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }

            return entityCollection;
        }

        #region Event Management

        /// <summary>
        /// Event to be fired when an Child entity is updating.
        /// </summary>
        /// <param name="entity">Instance of the child entity that is being updated.</param>
        /// <param name="entityOperationResult">Parameter that holds the output message, error message or other data which results during the operation.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>True if the event executes successfully.</returns>
        public Boolean SKUUpdating(Entity entity, EntityOperationResult entityOperationResult, MDMCenterApplication application, MDMCenterModules module)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                if (entity == null)
                    throw new ArgumentNullException("Entity", "Entity is null");

                #region Prepare Entity Operation Result

                if (entityOperationResult == null)
                    entityOperationResult = new EntityOperationResult(entity.Id, entity.LongName);

                if (entity.Attributes != null)
                {
                    foreach (Attribute attribute in entity.Attributes)
                    {
                        AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attribute.Id, attribute.Name, attribute.LongName, attribute.AttributeModelType, attribute.Locale);

                        entityOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }

                EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();
                entityOperationResults.Add(entityOperationResult);

                #endregion

                #region Step 1: Perform data validations

                //TODO:: Call to validation service for required attributes, data type validations, etc

                #endregion

                #region Step 2: Trigger EntityUpdating event

                OnEntityUpdating(entity, entityOperationResults, application, module);

                #endregion

                //Trigger EntityUpdate if no error was logged for EntityUpdating event
                if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    //TODO:: Enhance in future
                    #region Step 3: Performs entity updates


                    #endregion

                    //Commit the transaction if there is no error logged in OperationResult object.
                    if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                    {
                        transactionScope.Complete();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Event that is fired after a child entity is updated.
        /// </summary>
        /// <param name="entity">Instance of the child entity that is updated.</param>
        /// <param name="entityOperationResult">Parameter that holds the output message, error message or other data which results during the operation.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>True if the event is executed successfully.</returns>
        public Boolean SKUUpdated(Entity entity, EntityOperationResult entityOperationResult, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean bIncludeUpdatedInTransaction = true;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                if (entity == null)
                    throw new ArgumentNullException("Entity", "Entity is null");

                #region Prepare Entity Operation Result

                if (entityOperationResult == null)
                    entityOperationResult = new EntityOperationResult(entity.Id, entity.LongName);

                if (entity.Attributes != null)
                {
                    foreach (Attribute attribute in entity.Attributes)
                    {
                        AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attribute.Id, attribute.Name, attribute.LongName, attribute.AttributeModelType, attribute.Locale);

                        entityOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }

                EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();
                entityOperationResults.Add(entityOperationResult);

                #endregion

                //Read the AppConfig to decide whether EntityUpdated is to be included in the transaction or not.
                String includeUpdatedInTransaction = "false";
                try
                {
                    includeUpdatedInTransaction = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.EntityManager.IncludeUpdated-CreatedEventInTransaction");
                }
                catch (Exception ex)
                {
                    ExceptionHandler handler = new ExceptionHandler(ex);
                }
                Boolean.TryParse(includeUpdatedInTransaction, out bIncludeUpdatedInTransaction);

                #region Step 1: Trigger EntityUpdated event

                OnEntityUpdated(entity, entityOperationResults, application, module);

                // If SKUUpdated is not in the transaction then read the errors from OperationResult,
                // which are logged in SKUUpdated event, convert it in Information and clear the errors.

                // This is done because when SKUUpdated is not in transaction, the transaction is to be committed even if 
                // there are any errors in SKUUpdated event. But the end user should be notified that 
                // Entity has been updated but there was some error in SKUUpdated.
                if (bIncludeUpdatedInTransaction == false)
                {
                    if (entityOperationResult.HasError == true)
                    {
                        entityOperationResult.Informations.Add(new Information("", "Updated successfully but some errors occurred in Updated event."));

                        foreach (Error err in entityOperationResult.Errors)
                            entityOperationResult.Informations.Add(new Information(err.ErrorCode, err.ErrorMessage));

                        entityOperationResult.Errors.Clear();

                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }

                #endregion

                //Commit the transaction if there is no error logged in OperationResult object.
                if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    transactionScope.Complete();
                }
            }

            return true;
        }

        private void OnEntityCreating(Entity entity, EntityOperationResultCollection entityOperationResults, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext();
            callerContext.Application = application;
            callerContext.Module = module;
            callerContext.MDMPublisher = MDMPublisher.Unknown;

            var eventArgs = new EntityEventArgs(new EntityCollection() { entity }, new EntityBL(), entityOperationResults, _securityPrincipal.CurrentUserId, callerContext);
            EntityEventManager.Instance.OnEntityCreating(eventArgs);
        }

        private void OnEntityCreated(Entity entity, EntityOperationResultCollection entityOperationResults, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext();
            callerContext.Application = application;
            callerContext.Module = module;
            callerContext.MDMPublisher = MDMPublisher.Unknown;

            var eventArgs = new EntityEventArgs(new EntityCollection() { entity }, new EntityBL(), entityOperationResults, _securityPrincipal.CurrentUserId, callerContext);

            EntityEventManager.Instance.OnEntityCreated(eventArgs);
        }

        private void OnEntityUpdating(Entity entity, EntityOperationResultCollection entityOperationResults, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext();
            callerContext.Application = application;
            callerContext.Module = module;
            callerContext.MDMPublisher = MDMPublisher.Unknown;

            var eventArgs = new EntityEventArgs(new EntityCollection() { entity }, new EntityBL(), entityOperationResults, _securityPrincipal.CurrentUserId, callerContext);

            EntityEventManager.Instance.OnEntityUpdating(eventArgs);
        }

        private void OnEntityUpdated(Entity entity, EntityOperationResultCollection entityOperationResults, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext();
            callerContext.Application = application;
            callerContext.Module = module;
            callerContext.MDMPublisher = MDMPublisher.Unknown;

            var eventArgs = new EntityEventArgs(new EntityCollection() { entity }, new EntityBL(), entityOperationResults, _securityPrincipal.CurrentUserId, callerContext);

            EntityEventManager.Instance.OnEntityUpdated(eventArgs);
        }


        private void OnDataQualityValidationEvent(Entity entity, EntityOperationResultCollection entityOperationResults, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext();
            callerContext.Application = application;
            callerContext.Module = module;
            callerContext.MDMPublisher = MDMPublisher.Unknown;

            EntityEventArgs eventArgs = new EntityEventArgs(new EntityCollection
            {
                entity
            }, new EntityBL(), entityOperationResults, _securityPrincipal.CurrentUserId, callerContext);
            EntityEventManager.Instance.OnDataQualityValidationEvent(eventArgs);
        }
        #endregion

        #endregion
    }
}