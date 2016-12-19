using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using RS.MDM;
using RS.MDM.Data;

namespace RS.MDM.Configuration.Data
{
    /// <summary>
    /// Provides functionality to save a configuration object
    /// </summary>
    public class DataSourceGet : DataSource
    {

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataSourceGet()
            : base()
        {
            //GetApplicationConfigXML(
            //    SqlInt32 FK_Event_Source, 
            //    SqlInt32 FK_Event_Subscriber, 
            //    SqlInt32 FK_Security_Role, 
            //    SqlInt32 FK_Security_user, 
            //    SqlInt32 FK_Org, 
            //    SqlInt32 FK_Catalog, 
            //    SqlInt32 FK_Category, 
            //    SqlInt32 FK_CNode, 
            //    SqlInt32 FK_Attribute, 
            //    SqlInt32 FK_NodeType, 
            //    SqlInt32 FK_RelationshipType)

            this.Assembly = "StoredProcedures.dll";
            this.TypeName = "Riversand.StoredProcedures.Events";
            this.MethodName = "GetApplicationConfigXML";
            this.MethodType = System.Reflection.BindingFlags.Static;

            RS.MDM.Data.Parameter _parameter = new RS.MDM.Data.Parameter();
            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Source";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "EventSourceId";
            _parameter.DefaultValue = "1";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Subscriber";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "EventSubscriberId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_Role";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "SecurityRoleId";
            _parameter.DataType = SqlType.SqlInt32;
            _parameter.DefaultValue = "0";
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_user";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "SecurityUserId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Org";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "OrgId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Catalog";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "CatalogId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Category";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "CategoryId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_CNode";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "CNodeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Attribute";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "AttributeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_NodeType";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "NodeTypeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_RelationshipType";
            _parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            _parameter.SourceFieldName = "RelationshipTypeId";
            _parameter.DefaultValue = "0";
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            this.RemoveVerb("Add Parameter"); 
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="eventSourceId">Indicates an id of an event source</param>
        /// <param name="eventSubscriberId">Indicates an id of an event subscriber</param>
        /// <param name="securityRoleId">Indicates an id of the security role</param>
        /// <param name="securityUserId">Indicates an id of the security user</param>
        /// <param name="orgId">Indicates an id of an organization</param>
        /// <param name="catalogId">Indicates an id of a catalog</param>
        /// <param name="categoryId">Indicates an id of a category</param>
        /// <param name="cNodeId">Indicates an id of a cnode</param>
        /// <param name="attributeId">Indicates an id of an attribute</param>
        /// <param name="nodeTypeId">Indicates an id of a node type</param>
        /// <param name="relationshipTypeId">Indicates an id of relationship type</param>
        public DataSourceGet(int eventSourceId, int eventSubscriberId, int securityRoleId, int securityUserId, int orgId, int catalogId, int categoryId, int cNodeId, int attributeId, int nodeTypeId, int relationshipTypeId)
            : base()
        {
            //GetApplicationConfigXML(
            //    SqlInt32 FK_Event_Source, 
            //    SqlInt32 FK_Event_Subscriber, 
            //    SqlInt32 FK_Security_Role, 
            //    SqlInt32 FK_Security_user, 
            //    SqlInt32 FK_Org, 
            //    SqlInt32 FK_Catalog, 
            //    SqlInt32 FK_Category, 
            //    SqlInt32 FK_CNode, 
            //    SqlInt32 FK_Attribute, 
            //    SqlInt32 FK_NodeType, 
            //    SqlInt32 FK_RelationshipType)

            this.Assembly = "StoredProcedures.dll";
            this.TypeName = "Riversand.StoredProcedures.Events";
            this.MethodName = "GetApplicationConfigXML";
            this.MethodType = System.Reflection.BindingFlags.Static;

            RS.MDM.Data.Parameter _parameter = new RS.MDM.Data.Parameter();
            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Source";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "EventSourceId";
            _parameter.DefaultValue = eventSourceId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Event_Subscriber";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "EventSubscriberId";
            _parameter.DefaultValue = eventSubscriberId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_Role";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "SecurityRoleId";
            _parameter.DataType = SqlType.SqlInt32;
            _parameter.DefaultValue = securityRoleId.ToString();
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Security_user";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "SecurityUserId";
            _parameter.DefaultValue = securityUserId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Org";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "OrgId";
            _parameter.DefaultValue = orgId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Catalog";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "CatalogId";
            _parameter.DefaultValue = catalogId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Category";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "CategoryId";
            _parameter.DefaultValue = categoryId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_CNode";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "CNodeId";
            _parameter.DefaultValue = cNodeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_Attribute";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "AttributeId";
            _parameter.DefaultValue = attributeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_NodeType";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "NodeTypeId";
            _parameter.DefaultValue = nodeTypeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            _parameter = new RS.MDM.Data.Parameter();
            _parameter.Name = "FK_RelationshipType";
            _parameter.ParameterSourceType = ParameterSourceType.DefaultValue;
            _parameter.SourceFieldName = "RelationshipTypeId";
            _parameter.DefaultValue = relationshipTypeId.ToString();
            _parameter.DataType = SqlType.SqlInt32;
            this.Parameters.Add(_parameter);

            this.RemoveVerb("Add Parameter"); 

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an Event Source
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter EventSource
        {
            get
            {
                return this["FK_Event_Source"];
            }
            set
            {
                this["FK_Event_Source"] = value;
            }
        }

        /// <summary>
        /// Gets or sets an Event Subscriber
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter EventSubscriber
        {
            get
            {
                return this["FK_Event_Subscriber"];
            }
            set
            {
                this["FK_Event_Subscriber"] = value;
            }

        }

        /// <summary>
        /// Gets or sets a Security Role
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter SecurityRole
        {
            get
            {
                return this["FK_Security_Role"];
            }
            set
            {
                this["FK_Security_Role"] = value;
            }

        }

        /// <summary>
        /// Gets or sets a Security User
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter Securityuser
        {
            get
            {
                return this["FK_Security_user"];
            }
            set
            {
                this["FK_Security_user"] = value;
            }

        }

        /// <summary>
        /// Gets or sets an organization
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter Org
        {
            get
            {
                return this["FK_Org"];
            }
            set
            {
                this["FK_Org"] = value;
            }

        }

        /// <summary>
        /// Gets or sets a catalog
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter Catalog
        {
            get
            {
                return this["FK_Catalog"];
            }
            set
            {
                this["FK_Catalog"] = value;
            }

        }

        /// <summary>
        /// Gets or sets a category
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter Category
        {
            get
            {
                return this["FK_Category"];
            }
            set
            {
                this["FK_Category"] = value;
            }

        }

        /// <summary>
        /// Gets or sets a CNode
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter CNode
        {
            get
            {
                return this["FK_CNode"];
            }
            set
            {
                this["FK_CNode"] = value;
            }

        }

        /// <summary>
        /// Gets or sets an Attribute
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter Attribute
        {
            get
            {
                return this["FK_Attribute"];
            }
            set
            {
                this["FK_Attribute"] = value;
            }

        }

        /// <summary>
        /// Gets or sets a nodetype
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter NodeType
        {
            get
            {
                return this["FK_NodeType"];
            }
            set
            {
                this["FK_NodeType"] = value;
            }

        }

        /// <summary>
        /// Gets or sets a Relationship Type
        /// </summary>
        [Category("Application Configuration")]
        [Browsable(false)]
        [XmlIgnore()]
        public Parameter RelationshipType
        {
            get
            {
                return this["FK_RelationshipType"];
            }
            set
            {
                this["FK_RelationshipType"] = value;
            }

        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets a filtered list of properties that are displayed in the designer
        /// </summary>
        /// <param name="properties">Indicates a superset of properties</param>
        /// <returns>A filtered list of properties that are displayed in the designer</returns>
        protected override PropertyDescriptorCollection GetProperties(PropertyDescriptorCollection properties)
        {
            properties = base.GetProperties(properties);
            PropertyDescriptorCollection _props = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor _prop in properties)
            {
                if (_prop != null)
                {
                    switch (_prop.Name)
                    {
                        case "ObjectStatus":
                        case "InheritedParent":
                        case "Parent":
                        case "Id":
                        case "Description":
                        case "UniqueIdentifier":
                        case "Name":
                            continue;
                        default:
                            _props.Add(_prop);
                            break;
                    }
                }
            }
            return _props;
        }

        /// <summary>
        /// Get a tree node that reprents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();
            _treeNode.ImageKey = "DataSourceGet";
            _treeNode.SelectedImageKey = _treeNode.ImageKey;
            return _treeNode;
        }

        #endregion
    }
}
