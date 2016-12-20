using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Reflection;
using System.Security;
using System.Xml;

using MDM.Core;

namespace MDM.BusinessObjects.Workflow
{

    /// <summary>
    /// Specifies the Workflow Instance
    /// </summary>
    /// 
    [DataContract]
    public class WorkflowInstance : MDMObject
    {
        #region Fields

        /// <summary>
        /// field denoting Workflow Id of this workflow instance
        /// </summary>
        private Int32 _workflowId = 0;

        /// <summary>
        /// field denoting Workflow Version of this workflow instance
        /// </summary>
        private Int32 _workflowVersionId = 0;

        /// <summary>
        /// field denoting workflow name
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// field denoting workflow type
        /// </summary>
        private String _workflowType = String.Empty;

        /// <summary>
        /// Indicates the comments entered for the workflow
        /// </summary>
        private String _workflowComments = String.Empty;

        /// <summary>
        /// field denoting runtime instance id
        /// </summary>
        private String _runtimeInstanceId = String.Empty;

        /// <summary>
        /// field denoting Status of the workflow instance
        /// </summary>
        private String _status = String.Empty;

        /// <summary>
        /// field denoting currently acting user on this instance
        /// </summary>
        private String _actingUser = String.Empty;

        /// <summary>
        /// field denoting the Id of the service which has invoked this instance
        /// </summary>
        private Int32 _serviceId = 0;

        /// <summary>
        /// field denoting the type of the service which has invoked this instance
        /// </summary>
        private String _serviceType = String.Empty;

        /// <summary>
        /// field denoting the Id of the belonging group
        /// </summary>
        private Int32 _groupId = 0;

        /// <summary>
        /// field denoting whether the instance is ready for any action 
        /// </summary>
        private Boolean _isReadyForAction = false;

        /// <summary>
        /// field denoting whether instance is on escalation or not
        /// </summary>
        private Boolean _isOnEscalation = false;

        /// <summary>
        /// field denoting whether instance has escalation or not
        /// </summary>
        private Boolean _hasEscalation = false;

        /// <summary>
        /// field denoting Start Date of the instance
        /// </summary>
        private String _startDate = String.Empty;

        /// <summary>
        /// field denoting Last Updated Date of the instance
        /// </summary>
        private String _lastUpdatedDate = String.Empty;

        /// <summary>
        /// Represents the collection of WorkflowMDMObjects which contains 1 or more MDMObjectId and MDMObjectType
        /// </summary>
        private WorkflowMDMObjectCollection _workflowMDMObjects = new WorkflowMDMObjectCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public WorkflowInstance()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Workflow Instance</param>
        public WorkflowInstance(Int32 id)
            : base(id)
        {
            
        }

        /// <summary>
        /// Constructor with Object array 
        /// </summary>
        /// <param name="objectArray">Object array containing values for Workflow Instance. </param>
        public WorkflowInstance(object[] objectArray)
        {
            if (objectArray != null && objectArray.Count() > 0)
            {
                int intId = -1;
                if (objectArray[0] != null)
                    Int32.TryParse(objectArray[0].ToString(), out intId);

                this.Id = intId;

                if (objectArray[1] != null)
                    Int32.TryParse(objectArray[1].ToString(), out this._workflowVersionId);

                if (objectArray[2] != null)
                    this._runtimeInstanceId = objectArray[2].ToString();

                if (objectArray[5] != null)
                    this._status = objectArray[5].ToString();
            }
        }

        /// <summary>
        /// Constructor with Object array including all the properties. Written for workflow dashboard instance summary 
        /// </summary>
        /// <param name="objectArray">Object array containing values for Workflow Instance.</param>
        /// <param name="context">Indicates context XML</param>
        public WorkflowInstance(object[] objectArray, String context)
        {
            if (objectArray != null && objectArray.Count() > 0)
            {
                int intId = -1;
                if (objectArray[1] != null)
                    Int32.TryParse(objectArray[1].ToString(), out intId);
                this.WorkflowId = intId;

                intId = -1;
                if (objectArray[2] != null)
                    Int32.TryParse(objectArray[2].ToString(), out intId);
                this.WorkflowVersionId = intId;

                if (objectArray[3] != null)
                    this.WorkflowName = objectArray[3].ToString();

                if (objectArray[4] != null)
                    this.WorkflowType = objectArray[4].ToString();

                intId = -1;
                if (objectArray[5] != null)
                    Int32.TryParse(objectArray[5].ToString(), out intId);
                this.Id = intId;

                if (objectArray[6] != null)
                    this.RuntimeInstanceId = objectArray[6].ToString();

                if (objectArray[7] != null)
                    this.Status = objectArray[7].ToString();

                if (objectArray[8] != null)
                    this.ActingUser = objectArray[8].ToString();

                intId = -1;
                if (objectArray[9] != null)
                    Int32.TryParse(objectArray[9].ToString(), out intId);
                this.ServiceId = intId;

                if (objectArray[10] != null)
                    this.ServiceType = objectArray[10].ToString();

                intId = -1;
                if (objectArray[11] != null)
                    Int32.TryParse(objectArray[11].ToString(), out intId);
                this.GroupId = intId;

                Boolean flag = false;
                if (objectArray[12] != null)
                    Boolean.TryParse(objectArray[12].ToString(), out flag);
                this.IsOnEscalation = flag;

                flag = false;
                if (objectArray[13] != null)
                    Boolean.TryParse(objectArray[13].ToString(), out flag);
                this.HasEscalation = flag;

                if (objectArray[14] != null)
                    this.StartDate = objectArray[14].ToString();

                if (objectArray[15] != null)
                    this.LastUpdatedDate = objectArray[15].ToString();
            }
        }
        
        /// <summary>
        ///  Constructor with workflow Instance details in the form of xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates the workflow Instance details in xml format </param>
        public WorkflowInstance(String valuesAsXml)
        {
            /*
             * Sample:
             * <WorkflowInstance RuntimeInstanceId="4dd68c6b-8853-4ff8-9197-3e644436d4bc" MDMObjectId="2" 
             *      MDMObjectType="Entity" WorkflowVersionId="2" Status="Idle" />
             */

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    this.WorkflowMDMObjects = new WorkflowMDMObjectCollection();

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowInstance")
                        {
                            #region Instance detail

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("InstanceId"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("RuntimeInstanceId"))
                                {
                                    this.RuntimeInstanceId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WorkflowVersionId"))
                                {
                                    this.WorkflowVersionId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Status"))
                                {
                                    this.Status = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IsReadyForAction"))
                                {
                                    this.IsReadyForAction = reader.ReadContentAsBoolean();
                                }

                                if (reader.MoveToAttribute("WorkflowComments"))
                                {
                                    this.WorkflowComments = reader.ReadContentAsString();
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowMDMObject")
                        {
                            #region Read Workflow MDM Object detail

                            String mdmObjectXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(mdmObjectXml))
                            {
                                WorkflowMDMObject workflowMDMObject = new WorkflowMDMObject(mdmObjectXml);
                                this.WorkflowMDMObjects.Add(workflowMDMObject);
                            }

                            #endregion Read attributes
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
        }

        #endregion

        #region Property

        /// <summary>
        /// Property denoting the Workflow Id
        /// </summary>
        [DataMember]
        public Int32 WorkflowId
        {
            get
            {
                return this._workflowId;
            }
            set
            {
                this._workflowId = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Version Id
        /// </summary>
        [DataMember]
        public Int32 WorkflowVersionId
        {
            get
            {
                return this._workflowVersionId;
            }
            set
            {
                this._workflowVersionId = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Name
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get
            {
                return this._workflowName;
            }
            set
            {
                this._workflowName = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Type
        /// </summary>
        [DataMember]
        public String WorkflowType
        {
            get
            {
                return this._workflowType;
            }
            set
            {
                this._workflowType = value;
            }
        }

        /// <summary>
        /// Property denoting the comments entered for the Workflow.
        /// </summary>
        [DataMember]
        public String WorkflowComments
        {
            get
            {
                return _workflowComments;
            }
            set
            {
                _workflowComments = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Runtime Instance Id
        /// </summary>
        [DataMember]
        public String RuntimeInstanceId
        {
            get
            {
                return this._runtimeInstanceId;
            }
            set
            {
                this._runtimeInstanceId = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Instance Status
        /// </summary>
        [DataMember]
        public String Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        /// <summary>
        /// Property denoting currently acting user on this instance
        /// </summary>
        [DataMember]
        public String ActingUser
        {
            get
            {
                return this._actingUser;
            }
            set
            {
                this._actingUser = value;
            }
        }

        /// <summary>
        /// Property denoting the Id of the service which has invoked this instance
        /// </summary>
        [DataMember]
        public Int32 ServiceId
        {
            get
            {
                return this._serviceId;
            }
            set
            {
                this._serviceId = value;
            }
        }

        /// <summary>
        /// Property denoting the type of the service which has invoked this instance
        /// </summary>
        [DataMember]
        public String ServiceType
        {
            get
            {
                return this._serviceType;
            }
            set
            {
                this._serviceType = value;
            }
        }

        /// <summary>
        /// Property denoting the Id of the belonging group
        /// </summary>
        [DataMember]
        public Int32 GroupId
        {
            get
            {
                return this._groupId;
            }
            set
            {
                this._groupId = value;
            }
        }

        /// <summary>
        /// Property denoting whether the instance is ready for any action 
        /// </summary>
        [DataMember]
        public Boolean IsReadyForAction
        {
            get
            {
                return this._isReadyForAction;
            }
            set
            {
                this._isReadyForAction = value;
            }
        }

        /// <summary>
        /// Property denoting whether instance is on escalation or not
        /// </summary>
        [DataMember]
        public Boolean IsOnEscalation
        {
            get
            {
                return _isOnEscalation;
            }
            set
            {
                _isOnEscalation = value;
            }
        }

        /// <summary>
        /// Property denoting whether instance has escalation or not
        /// </summary>
        [DataMember]
        public Boolean HasEscalation
        {
            get
            {
                return _hasEscalation;
            }
            set
            {
                _hasEscalation = value;
            }
        }

        /// <summary>
        /// Property denoting Start Date of the instance
        /// </summary>
        [DataMember]
        public String StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }

        /// <summary>
        /// Property denoting Last Updated Date of the instance
        /// </summary>
        [DataMember]
        public String LastUpdatedDate
        {
            get
            {
                return _lastUpdatedDate;
            }
            set
            {
                _lastUpdatedDate = value;
            }
        }

        /// <summary>
        /// denoting the collection of WorkflowMDMObjects which contains 1 or more MDMObjectId and MDMObjectType
        /// </summary>
        [DataMember]
        public WorkflowMDMObjectCollection WorkflowMDMObjects
        {
            get
            {
                return this._workflowMDMObjects;
            }
            set
            {
                this._workflowMDMObjects = value;
            }
        }

        ///// <summary>
        ///// Name property does not make any sense for WorkflowInstance, so not implementing
        ///// </summary>
        //public override string Name
        //{
        //    get
        //    {
        //        throw new NotImplementedException("Name property is not implemented for WorkflowInstance");
        //    }
        //    set
        //    {
        //        throw new NotImplementedException("Name property is not implemented for WorkflowInstance");
        //    }
        //}

        ///// <summary>
        ///// LongName property does not make any sense for WorkflowInstance, so not implementing
        ///// </summary>
        //public override string LongName
        //{
        //    get
        //    {
        //        throw new NotImplementedException("LongName property is not implemented for WorkflowInstance");
        //    }
        //    set
        //    {
        //        throw new NotImplementedException("LongName property is not implemented for WorkflowInstance");
        //    }
        //}
        
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of object
        /// </summary>
        /// <returns>Xml representation of the object</returns>
        public String ToXML()
        {
            String xml = string.Empty;
            String mdmObjectString = String.Empty;

            xml = "<WorkflowInstance Id=\"{0}\" WorkflowVersionId=\"{1}\" RuntimeInstanceId=\"{2}\" Status=\"{3}\" WorkflowComments=\"{4}\" Action=\"{5}\">{6}</WorkflowInstance>";

            if (this.WorkflowMDMObjects != null && this.WorkflowMDMObjects.Count > 0)
            {
                mdmObjectString = this.WorkflowMDMObjects.ToXml();
            }
            String retXML = string.Format(xml, this.Id, this.WorkflowVersionId, this.RuntimeInstanceId, this.Status, this.WorkflowComments, this.Action, mdmObjectString);

            return retXML;
        }

        #endregion Methods

    }
}
