using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;

using RS.MDM;
using RS.MDM.Data;
using System.Runtime.Serialization;


namespace RS.MDM.Configuration.Data
{
    /// <summary>
    /// Provides functionality to get configuration objects from the database
    /// </summary>
    public sealed class DataSourceSet : DataSource
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DataSourceSet()
            : base()
        {
            //UpdateApplicationConfigXML(
            //    SqlInt32 FK_Application_ConfigType, 
            //    SqlString ShortName, 
            //    SqlString LongName, 
            //    SqlInt32 FK_Event_Source, 
            //    SqlInt32 FK_Event_Subscriber, 
            //    SqlInt32 FK_Org, 
            //    SqlInt32 FK_Catalog, 
            //    SqlInt32 FK_Category, 
            //    SqlInt32 FK_CNode, 
            //    SqlInt32 FK_Attribute, 
            //    SqlInt32 FK_NodeType, 
            //    SqlInt32 FK_RelationshipType, 
            //    SqlInt32 FK_Security_Role, 
            //    SqlInt32 FK_Security_user, 
            //    SqlString ConfigXML, 
            //    SqlString Description, 
            //    SqlString PreCondition, 
            //    SqlString PostCondition, 
            //    SqlString XSDSchema, 
            //    SqlString SampleXML, 
            //    SqlString loginUser, 
            //    SqlString userProgram, 
            //out SqlInt32 RETURN_VALUE)
            this.Assembly = "StoredProcedures.dll";
            this.TypeName = "Riversand.StoredProcedures.Events";
            this.MethodName = "UpdateApplicationConfigXML";
            this.MethodType = System.Reflection.BindingFlags.Static;

            //RS.MDM.Data.Parameter _parameter = new RS.MDM.Data.Parameter();
            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Application_ContextDefinition";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "ApplicationConfigTypeId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Application_ConfigParent";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "ApplicationConfigParentId";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "ShortName";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "ShortName";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "LongName";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "LongName";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Event_Source";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "EventSourceId";
            //_parameter.DefaultValue = "1";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Event_Subscriber";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "EventSubscriberId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Org";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "OrgId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Catalog";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "CatalogId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Category";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "CategoryId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_CNode";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "CNodeId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Attribute";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "AttributeId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_NodeType";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "NodeTypeId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_RelationshipType";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "RelationshipTypeId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Security_Role";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "SecurityRoleId";
            //_parameter.DataType = SqlType.SqlInt32;
            //_parameter.DefaultValue = "0";
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Security_user";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "SecurityUserId";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "ConfigXML";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "ConfigXML";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "Description";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "Description";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "PreCondition";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "PreCondition";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "PostCondition";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "PostCondition";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "XSDSchema";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "XSDSchema";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "SampleXML";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "SampleXML";
            //_parameter.DefaultValue = "";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "loginUser";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.SourceFieldName = "loginUser";
            //_parameter.DefaultValue = "Configuration Tool";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "userProgram";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.Direction = System.Data.ParameterDirection.Input;
            //_parameter.SourceFieldName = "userProgram";
            //_parameter.DefaultValue = "Configuration Tool";
            //_parameter.DataType = SqlType.SqlString;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "FK_Locale";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.Direction = System.Data.ParameterDirection.Input;
            //_parameter.SourceFieldName = "FK_Locale";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            //_parameter = new RS.MDM.Data.Parameter();
            //_parameter.Name = "RETURN_VALUE";
            //_parameter.ParameterSourceType = ParameterSourceType.QueryStringParameter;
            //_parameter.Direction = System.Data.ParameterDirection.Output;
            //_parameter.SourceFieldName = "RETURN_VALUE";
            //_parameter.DefaultValue = "0";
            //_parameter.DataType = SqlType.SqlInt32;
            //this.Parameters.Add(_parameter);

            this.RemoveVerb("Add Parameter"); 
        }

        #endregion

        #region Overrides
        /// <summary>
        /// Get a list of filtered properties that are displayed in the designer
        /// </summary>
        /// <param name="properties">Indicates a super set of properties</param>
        /// <returns>A list of filtered properties that are displayed in the designer</returns>
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
                        //case "Id":
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
            _treeNode.ImageKey = "DataSourceSet";
            _treeNode.SelectedImageKey = _treeNode.ImageKey;
            return _treeNode;
        }

        #endregion

    }
}
