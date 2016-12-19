using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Reflection;
using System.Security;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies the Workflow Activity
    /// </summary>
    [DataContract]
    public class WorkflowActivity : MDMObject, IWorkflowActivity
    {
        #region Fields

        /// <summary>
        /// field denoting Workflow id, in which, this activity is placed
        /// </summary>
        private Int32 _workflowId = 0;

        /// <summary>
        /// field denoting Workflow Version, in which, this activity is placed
        /// </summary>
        private Int32 _workflowVersionId = 0;

        /// <summary>
        /// field denoting the Metadata Attribute ID helps in identifying the activity as Main or Sub activity
        /// </summary>
        private Int32 _metadataAttributeId = 0;

        /// <summary>
        /// field denoting Assignment Type
        /// </summary>
        private AssignmentType _assignmentType = AssignmentType.RoundRobin;

        /// <summary>
        /// field denoting currently acting user id on this activity
        /// </summary>
        private Int32 _actingUserId = 0;

        /// <summary>
        /// field denoting currently acting user on this activity
        /// </summary>
        private String _actingUser = String.Empty;

        /// <summary>
        /// field denoting List of comma separated users, allowed for this Activity
        /// </summary>
        private String _allowedUsers = String.Empty;

        /// <summary>
        /// field denoting List of comma separated roles, allowed for this Activity
        /// </summary>
        private String _allowedRoles = String.Empty;

        /// <summary>
        /// field denoting Sort Sequence 
        /// </summary>
        private Int32 _sortOrder = 0;

        /// <summary>
        /// Represents Escalation context set up for this activity in the format of XML
        /// </summary>
        private String _escalationContext = String.Empty;

        /// <summary>
        /// Flag representing whether delegation is allowed for this activity or not
        /// </summary>
        private Boolean _delegationAllowed = true;

		/// <summary>
		/// field denoting whether system should show unassigned entities
		/// </summary>
		private Boolean _displayUnassignedEntities = true;

		/// <summary>
		/// field denoting whether system should show entities assigned to other users
		/// </summary>
		private Boolean _displayOtherUsersEntities = false;

        /// <summary>
        /// field denoting type of the workflow
        /// </summary>
        private String _workflowType = "WWF";

        /// <summary>
        /// field denoting state of the activity
        /// </summary>
        private String _state = String.Empty;

        /// <summary>
        /// field denoting escalation level of the activity
        /// </summary>
        private String _escalationLevel = String.Empty;

        /// <summary>
        /// field denoting Id of the object binding in the Tree node
        /// </summary>
        private Int32 _nodeValue = 0;

        /// <summary>
        /// field denoting start date of the activity
        /// </summary>
        private String _startDate = String.Empty;

        /// <summary>
        /// field denoting the possible actions for this activity
        /// </summary>
        private Collection<ActivityAction> _activityActionsCollection = new Collection<ActivityAction>();

        /// <summary>
        /// fied denoting the description of the workflow activity
        /// </summary>
        private String _description = String.Empty;

		/// <summary>
		/// Specifies if its  Human Activity
		/// </summary>
		private Boolean _isHumanActivity = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public WorkflowActivity()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an AppConfig Instance</param>
        public WorkflowActivity(Int32 id)
            : base(id)
        {
            
        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for Workflow Activity. </param>
        public WorkflowActivity(object[] objectArray)
        {
            int intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);

            this.Id = intId;

            if (objectArray[1] != null)
                this.Name = objectArray[1].ToString();

            if (objectArray[2] != null)
                this.LongName = objectArray[2].ToString();

            if (objectArray[3] != null)
                Int32.TryParse(objectArray[3].ToString(), out this._workflowVersionId);

            if (objectArray[4] != null)
                this._assignmentType = (AssignmentType)objectArray[4];

            if (objectArray[5] != null)
                this._allowedUsers = objectArray[5].ToString();

            if (objectArray[6] != null)
                this._allowedRoles = objectArray[6].ToString();

            if (objectArray[7] != null)
                Int32.TryParse(objectArray[7].ToString(), out this._sortOrder);

            //Description
            if (objectArray[8] != null)
                this.Description = objectArray[8].ToString();
        }

        /// <summary>
        ///  Constructor with workflow activity details in the form of xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates the workflow activity details in xml format </param>
        public WorkflowActivity(String valuesAsXml)
        {
            /*
             * Sample:
             * <Activity Id="" ShortName="" LongName="" WorkflowId="" WorkflowVersionId="" WorkflowType="">
               </Activity>
             */

            //Considered only some fields(as shown in the sample) for this constructor currently under usage keeping performance in mind
            //If other fields are required, include in the code

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.Name == "Activity")
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("ShortName"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("LongName"))
                                    {
                                        this.LongName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowId"))
                                    {
                                        this.WorkflowId = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("WorkflowVersionId"))
                                    {
                                        this.WorkflowVersionId = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("WorkflowType"))
                                    {
                                        this.WorkflowType = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Description"))
                                    {
                                        this.Description = reader.ReadContentAsString();
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
                return _workflowId;
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
        /// Property denoting the Metadata Attribute ID helps in identifying the activity as Main or Sub activity
        /// </summary>
        [DataMember]
        public Int32 MetadataAttributeId
        {
            get
            {
                return _metadataAttributeId;
            }
            set
            {
                _metadataAttributeId = value;
            }
        }

        /// <summary>
        /// Property denoting the Assignment Type
        /// </summary>
        [DataMember]
        public AssignmentType AssignmentType
        {
            get
            {
                return this._assignmentType;
            }
            set
            {
                this._assignmentType = value;
            }
        }

        /// <summary>
        /// Description of the workflow activity
        /// </summary>
        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Property denoting currently acting user id on this activity
        /// </summary>
        [DataMember]
        public Int32 ActingUserId
        {
            get
            {
                return this._actingUserId;
            }
            set
            {
                this._actingUserId = value;
            }
        }

        /// <summary>
        /// Property denoting currently acting user on this activity
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
        /// Property denoting the Assigned Users
        /// </summary>
        [DataMember]
        public String AllowedUsers
        {
            get
            {
                return this._allowedUsers;
            }
            set
            {
                this._allowedUsers = value;
            }
        }

        /// <summary>
        /// Property denoting the Assigned Roles
        /// </summary>
        [DataMember]
        public String AllowedRoles
        {
            get
            {
                return this._allowedRoles;
            }
            set
            {
                this._allowedRoles = value;
            }
        }

        /// <summary>
        /// Property denoting the Sort Order
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        {
            get
            {
                return this._sortOrder;
            }
            set
            {
                this._sortOrder = value;
            }
        }
        
        /// <summary>
        ///Property denoting the Escalation context set up for this activity in the format of XML
        /// </summary>
        [DataMember]
        public String EscalationContext
        {
            get
            {
                return this._escalationContext;
            }
            set
            {
                this._escalationContext = value;
            }
        }

        /// <summary>
        ///Property denoting the Flag representing whether delegation is allowed for this activity or not
        /// </summary>
        [DataMember]
        public Boolean DelegationAllowed
        {
            get
            {
                return this._delegationAllowed;
            }
            set
            {
                this._delegationAllowed = value;
            }
        }

		/// <summary>
		///Property denoting whether system should display unassigned entities
		/// </summary>
		[DataMember]
		public Boolean DisplayUnassignedEntities
		{
			get
			{
				return this._displayUnassignedEntities;
			}
			set
			{
				this._displayUnassignedEntities = value;
			}
		}

		/// <summary>
		///Property denoting whether system should display other users' entities
		/// </summary>
		[DataMember]
		public Boolean DisplayOtherUsersEntities
		{
			get
			{
				return this._displayOtherUsersEntities;
			}
			set
			{
				this._displayOtherUsersEntities = value;
			}
		}

        /// <summary>
        /// Property denoting type of the workflow
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
        /// Property denoting state of the activity
        /// </summary>
        [DataMember]
        public String State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }
	    
	    /// <summary>
        /// Property denoting escalation level of the activity
        /// </summary>
        [DataMember]
        public String EscalationLevel
        {
            get
            {
                return this._escalationLevel;
            }
            set
            {
                this._escalationLevel = value;
            }
        }

        /// <summary>
        /// Property denoting the Id of the object binding in the Tree node
        /// </summary>
        [DataMember]
        public Int32 NodeValue
        {
            get
            {
                return _nodeValue;
            }
            set
            {
                _nodeValue = value;
            }
        }

        /// <summary>
        /// Property denoting start date of the activity
        /// </summary>
        [DataMember]
        public String StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        /// <summary>
        /// Property denoting the Activity action collection
        /// </summary>
        [DataMember]
        public Collection<ActivityAction> ActivityActionsCollection
        {
            get
            {
                return this._activityActionsCollection;
            }
            set
            {
                this._activityActionsCollection = value;
            }
        }

		/// <summary>
		/// Specifies if its a Human activity
		/// </summary>
	    [DataMember]
	    public Boolean IsHumanActivity
	    {
		    get
		    {
			    return _isHumanActivity;
		    }
		    set
		    {
				_isHumanActivity = value;
		    }
	    }
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of object
        /// </summary>
        /// <returns>Xml representation of the object</returns>
        public String ToXML()
        {
			String propertyValuesFormat = "Id=\"{0}\" Name=\"{1}\" LongName=\"{2}\" WorkflowVersionId=\"{3}\" AssignmentType=\"{4}\" AllowedUsers=\"{5}\" AllowedRoles=\"{6}\" SortOrder=\"{7}\" EscalationContext=\"{8}\" DelegationAllowed=\"{9}\" DisplayUnassignedEntities=\"{10}\" DisplayOtherUsersEntities=\"{11}\" Action=\"{12}\" Description=\"{13}\" IsHumanActivity=\"{14}\"";

            String propertyValues = String.Format(propertyValuesFormat, this.Id, SecurityElement.Escape(this.Name), SecurityElement.Escape(this.LongName), this.WorkflowVersionId, (int)this.AssignmentType, this.AllowedUsers, this.AllowedRoles,
                                        this.SortOrder, this.EscalationContext, this.DelegationAllowed, this.DisplayUnassignedEntities, this.DisplayOtherUsersEntities, this.Action, this.Description,this.IsHumanActivity);

            String activityActionsString = String.Empty;

            foreach (ActivityAction activityAction in this.ActivityActionsCollection)
            {
                activityActionsString = String.Concat(activityActionsString, activityAction.ToXML());
            }

            String retXML = String.Format("<WorkflowActivity {0}><ActivityActions>{1}</ActivityActions></WorkflowActivity>", propertyValues, activityActionsString);

            return retXML;
        }

        /// <summary>
        /// Get Xml representation of activity based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Model</returns>
        public String ToXML(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                returnXml = this.ToXML();
            }
            else if (objectSerialization == ObjectSerialization.UIRender)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

	            xmlWriter.WriteString(ToXml());

                xmlWriter.Flush();

                //Get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return returnXml;
        }

        #endregion

    }
}
