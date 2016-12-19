using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Workflow MDMObject
    /// </summary>
    [DataContract]
    public class WorkflowMDMObject : MDMObject, IWorkflowMDMObject
    {
        #region Private fields

        /// <summary>
        /// Represents the Id of the object which is being passed to the workflow
        /// the object can be any of MDM objects
        /// </summary>
        private Int64 _mdmObjectId = 0;

        /// <summary>
        /// Represents the object being sent tp workflow.
        /// the object can be any valid MDM object.
        /// </summary>
        private String _mdmObjectType = String.Empty;

        /// <summary>
        /// Represents the entity name being sent to workflow.
        /// </summary>
        private String _mdmObjectName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private String _mdmObjectGUID = String.Empty;            

        #endregion Private fields

        #region Public Properties

        /// <summary>
        /// Property denoting the Id of the object which is being passed to the workflow
        /// the object can be any of MDM objects
        /// </summary>
        [DataMember]
        public Int64 MDMObjectId
        {
            get
            {
                return this._mdmObjectId;
            }
            set
            {
                this._mdmObjectId = value;
            }
        }

        /// <summary>
        /// Property denoting the object being sent tp workflow.
        /// The object can be any valid MDM object.
        /// </summary>
        [DataMember]
        public String MDMObjectType
        {
            get
            {
                return this._mdmObjectType;
            }
            set
            {
                this._mdmObjectType = value;
            }
        }

        /// <summary>
        /// Property denoting the entity name being sent to workflow.
        /// </summary>
        [DataMember]
        public String MDMObjectName
        {
            get
            {
                return this._mdmObjectName;
            }
            set
            {
                this._mdmObjectName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String MDMObjectGUID
        {
            get
            {
                return this._mdmObjectGUID;
            }
            set
            {
                this._mdmObjectGUID = value;
            }
        }

        #endregion Public Properties

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public WorkflowMDMObject()
            : base()
        {
        }

        /// <summary>
        /// Initializes workflow MDM object with provided Object Id and and Object Type
        /// </summary>
        /// <param name="mdmObjectId">Id of the MDM Object</param>
        /// <param name="mdmObjectType">Type of the MDM Object</param>
        public WorkflowMDMObject(Int64 mdmObjectId, String mdmObjectType)
            : base()
        {
            _mdmObjectId = mdmObjectId;
            _mdmObjectType = mdmObjectType;
        }

        /// <summary>
        /// Initializes workflow MDM object with provided Object GUId and and Object Type
        /// </summary>
        /// <param name="mdmObjectGUID">Indicates GUId of the MDM Object</param>
        /// <param name="mdmObjectType">Type of the MDM Object</param>
        public WorkflowMDMObject(String mdmObjectGUID, String mdmObjectType)
            : base()
        {
            _mdmObjectGUID = mdmObjectGUID;
            _mdmObjectType = mdmObjectType;
        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for MDM Object</param>
        public WorkflowMDMObject(object[] objectArray)
        {
            if (objectArray[0] != null)
                this.MDMObjectType = objectArray[0].ToString();

            Int64 intId = -1;
            if (objectArray[1] != null)
                Int64.TryParse(objectArray[1].ToString(), out intId);

            if (objectArray[2] != null)
                this.MDMObjectName = objectArray[2].ToString();

            this.MDMObjectId = intId;
        }

        /// <summary>
        ///  Constructor with object details in the form of xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates the object details in xml format </param>
        public WorkflowMDMObject(String valuesAsXml)
        {
            /*
             * Sample:
             * <WorkflowMDMObject Id="5" MDMObjectId="2" MDMObjectType="Entity" />
             */
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.Name == "WorkflowMDMObject")
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }

                                    //if (reader.MoveToAttribute("MDMObjectId"))
                                    //{
                                    //    this.MDMObjectId = reader.ReadContentAsInt();
                                    //}

                                    if (reader.MoveToAttribute("MDMObjectId"))
                                    {
                                        this.MDMObjectGUID = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("MDMObjectType"))
                                    {
                                        this.MDMObjectType = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("MDMObjectName"))
                                    {
                                        this.MDMObjectName = reader.ReadContentAsString();
                                    }
                                }
                            }
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
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ActivityTracking object which needs to be compared.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is WorkflowMDMObject)
            {
                WorkflowMDMObject objectToBeCompared = obj as WorkflowMDMObject;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.MDMObjectId != objectToBeCompared.MDMObjectId)
                    return false;

                if (this.MDMObjectType != objectToBeCompared.MDMObjectType)
                    return false;

                if (this.MDMObjectName != objectToBeCompared.MDMObjectName)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.MDMObjectId.GetHashCode() ^ this.MDMObjectType.GetHashCode() ^ this.MDMObjectName.GetHashCode();
        }

        /// <summary>
        /// Get Xml representation of Workflow Instance
        /// </summary>
        /// <returns>Xml representation of Workflow Instance</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<WorkflowMDMObject MDMObjectId = \"{0}\" MDMObjectType = \"{1}\" MDMObjectName = \"{2}\" Action=\"{3}\"/>";
            returnXml = String.Format(returnXml, this.MDMObjectId, this.MDMObjectType, this.MDMObjectName, this.Action);

            return returnXml;
        }

        #endregion Public Methods
    }
}
