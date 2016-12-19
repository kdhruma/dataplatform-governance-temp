using System;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Business Object helps to trace the tracked Activity Records
    /// </summary>
    [DataContract]
    public class TrackedActivityInfo : MDMObject, ITrackedActivityInfo
    {
        #region Fields

        /// <summary>
        /// Represents the Workflow Id
        /// </summary>
        private Int32 _workflowId = -1;

        /// <summary>
        /// Represents the Workflow short Name 
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Represents the Workflow long Name 
        /// </summary>
        private String _workflowLongName = String.Empty;

        /// <summary>
        /// Represents the Workflow Version Id
        /// </summary>
        private Int32 _workflowVersionId = 0;

        /// <summary>
        /// Represent the Workflow Version long Name 
        /// </summary>
        private String _workflowVersionName = String.Empty;

        /// <summary>
        /// Id of the running workflow which uniquely identifies running instance
        /// </summary>
        private String _runtimeInstanceId = String.Empty;

        /// <summary>
        /// Represents the collection of Ids and types of the objects participating in the workflow
        /// </summary>
        private WorkflowMDMObjectCollection _mdmObjectCollection = new WorkflowMDMObjectCollection();

        /// <summary>
        /// Represents WorkflowDataContext related properties in the format of XML string
        /// </summary>
        private String _extendedProperties = String.Empty;

        /// <summary>
        /// Represents the ID of the activity as per the Workflow Definition
        /// </summary>
        private String _workflowDefinitionActivityID = String.Empty;

        /// <summary>
        /// Unique Name of an activity
        /// </summary>
        private String _activityShortName = String.Empty;

        /// <summary>
        /// Name of the activity as displayed in the designer(Display Name)
        /// </summary>
        private String _activityLongName = String.Empty;

        /// <summary>
        /// Represents the activity running(executing) status
        /// </summary>
        private Boolean _isExecuting = false;

        /// <summary>
        /// Field for the Status of the Activity
        /// </summary>
        private String _status = String.Empty;

        /// <summary>
        /// Represent the PerformedAction by the Activity
        /// </summary>
        private String _performedAction = String.Empty;

        /// <summary>
        /// Id of the user who took the ownership of the activity and under whom the activity is currently waiting/running
        /// </summary>
        private Int32 _actingUserId = 0;

        /// <summary>
        /// Name of the user who took the ownership of the activity and under whom the activity is currently waiting/running
        /// </summary>
        private String _actingUser = String.Empty;

        /// <summary>
        /// Indicates Id of the user who performed the activity
        /// </summary>
        private Int32 _actedUserId = 0;

        /// <summary>
        /// Indicates the user who performed the activity
        /// </summary>
        private String _actedUser = String.Empty;

        /// <summary>
        /// Indicates mail address of the acting/acted user
        /// </summary>
        private String _userMailAddress = String.Empty;

        /// <summary>
        /// Indicates the comments entered for the workflow
        /// </summary>
        private String _workflowComments = String.Empty;

        /// <summary>
        /// Indicates the comments entered for an action
        /// </summary>
        private String _activityComments = String.Empty;

        /// <summary>
        /// Represents the variables and values in the scope of the activity in the format of Name:Value collection
        /// </summary>
        private String _variables = String.Empty;

        /// <summary>
        /// Represents the In and Out arguments of an activity in the format of Name:Value collection
        /// </summary>
        private String _arguments = String.Empty;

        /// <summary>
        /// Represents the custom data in the format of Name:Value collection
        /// </summary>
        private String _customData = String.Empty;

        /// <summary>
        /// Users to whom the activity has been assigned
        /// </summary>
        private String _assignedUsers = String.Empty;

        /// <summary>
        /// Roles to which the activity has been assigned
        /// </summary>
        private String _assignedRoles = String.Empty;

        /// <summary>
        /// This field indicates whether the activity is queued or not
        /// </summary>
        private AssignmentType _assignementType = AssignmentType.RoundRobin;

        /// <summary>
        /// Value used to sort the activities in a workflow while displaying on the UI
        /// </summary>
        private Int32 _sortOrder = 0;

        /// <summary>
        /// Flag which indicates the current activity is human activity or not
        /// </summary>
        private Boolean _isHumanActivity = false;

        /// <summary>
        /// The date time at which tracking record has been generated
        /// </summary>
        private String _eventDate = String.Empty;

        /// <summary>
        /// Field denoting previous activity short name.
        /// </summary>
        private String _previousActivityShortName = String.Empty;

        /// <summary>
        /// Field denoting previous activity short name.
        /// </summary>
        private String _previousActivityStartDateTime = String.Empty;

        /// <summary>
        /// Field denoting previous activity comment.
        /// </summary>
        private String _lastActivityComments = String.Empty;

        #endregion

        #region Properties

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
        /// Property denoting the Workflow Short Name 
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
        /// Property denoting the Workflow Long Name 
        /// </summary>
        [DataMember]
        public String WorkflowLongName
        {
            get
            {
                return this._workflowLongName;
            }
            set
            {
                this._workflowLongName = value;
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
        /// Property denoting the Workflow Version long Name 
        /// </summary>
        [DataMember]
        public String WorkflowVersionName
        {
            get
            {
                return this._workflowVersionName;
            }
            set
            {
                this._workflowVersionName = value;
            }
        }

        /// <summary>
        /// Property denoting the Id of the running workflow which uniquely identifies running instance
        /// </summary>
        [DataMember]
        public String RuntimeInstanceId
        {
            get
            {
                return _runtimeInstanceId;
            }
            set
            {
                _runtimeInstanceId = value;
            }
        }

        /// <summary>
        /// Property denoting the collection of Ids and types of the objects participating in the workflow
        /// </summary>
        [DataMember]
        public WorkflowMDMObjectCollection MDMObjectCollection
        {
            get
            {
                return this._mdmObjectCollection;
            }
            set
            {
                this._mdmObjectCollection = value;
            }
        }

        /// <summary>
        /// Property denoting the WorkflowDataContext related properties in the format of XML string
        /// </summary>
        [DataMember]
        public new String ExtendedProperties
        {
            get
            {
                return _extendedProperties;
            }
            set
            {
                _extendedProperties = value;
            }
        }

        /// <summary>
        /// Property denoting the ID of the activity as per the Workflow Definition
        /// </summary>
        [DataMember]
        public String WorkflowDefinitionActivityID
        {
            get
            {
                return this._workflowDefinitionActivityID;
            }
            set
            {
                this._workflowDefinitionActivityID = value;
            }
        }

        /// <summary>
        ///Property denoting the Unique Name of an activity
        /// </summary>
        [DataMember]
        public String ActivityShortName
        {
            get
            {
                return _activityShortName;
            }
            set
            {
                _activityShortName = value;
            }
        }

        /// <summary>
        /// Property denoting the Name of the activity as displayed in the designer(Display Name)
        /// </summary>
        [DataMember]
        public String ActivityLongName
        {
            get
            {
                return _activityLongName;
            }
            set
            {
                _activityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting whether the activity is Executing(Running) or not
        /// </summary>
        [DataMember]
        public Boolean IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                _isExecuting = value;
            }
        }

        /// <summary>
        /// Property denoting the Status of the Activity
        /// </summary>
        [DataMember]
        public String Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        /// <summary>
        /// Property denoting the PerformedAction by the Activity
        /// </summary>
        [DataMember]
        public String PerformedAction
        {
            get
            {
                return _performedAction;
            }
            set
            {
                _performedAction = value;
            }
        }

        /// <summary>
        /// Property denoting the Id of the User who took the ownership of the activity and under whom the activity is currently waiting/running
        /// </summary>
        [DataMember]
        public Int32 ActingUserId
        {
            get
            {
                return _actingUserId;
            }
            set
            {
                _actingUserId = value;
            }
        }

        /// <summary>
        /// Property denoting the name of the User who took the ownership of the activity and under whom the activity is currently waiting/running
        /// </summary>
        [DataMember]
        public String ActingUser
        {
            get
            {
                return _actingUser;
            }
            set
            {
                _actingUser = value;
            }
        }

        /// <summary>
        /// Property denoting the ID of the user who performed the activity
        /// </summary>
        [DataMember]
        public Int32 ActedUserId
        {
            get
            {
                return _actedUserId;
            }
            set
            {
                _actedUserId = value;
            }
        }

        /// <summary>
        /// Property denoting the user who performed the activity
        /// </summary>
        [DataMember]
        public String ActedUser
        {
            get
            {
                return _actedUser;
            }
            set
            {
                _actedUser = value;
            }
        }

        /// <summary>
        /// Property denoting mail address of the acting/acted user
        /// </summary>
        [DataMember]
        public String UserMailAddress
        {
            get
            {
                return _userMailAddress;
            }
            set
            {
                _userMailAddress = value;
            }
        }

        /// <summary>
        /// Property denoting the comments entered for an action. 
        /// </summary>
        [DataMember]
        public String ActivityComments
        {
            get
            {
                return _activityComments;
            }
            set
            {
                _activityComments = value;
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
        /// Property denoting the variables and values in the scope of the activity in the format of Name:Value collection
        /// </summary>
        [DataMember]
        public String Variables
        {
            get
            {
                return _variables;
            }
            set
            {
                _variables = value;
            }
        }

        /// <summary>
        /// Property denoting the In and Out arguments of an activity in the format of Name:Value collection
        /// </summary>
        [DataMember]
        public String Arguments
        {
            get
            {
                return _arguments;
            }
            set
            {
                _arguments = value;
            }
        }

        /// <summary>
        /// Property denoting the custom data in the format of Name:Value collection
        /// </summary>
        [DataMember]
        public String CustomData
        {
            get
            {
                return _customData;
            }
            set
            {
                _customData = value;
            }
        }

        /// <summary>
        ///Property denoting the Users to whom the activity has been assigned
        /// </summary>
        [DataMember]
        public String AssignedUsers
        {
            get
            {
                return _assignedUsers;
            }
            set
            {
                _assignedUsers = value;
            }
        }

        /// <summary>
        ///Property denoting the Roles to which the activity has been assigned
        /// </summary>
        [DataMember]
        public String AssignedRoles
        {
            get
            {
                return _assignedRoles;
            }
            set
            {
                _assignedRoles = value;
            }
        }

        /// <summary>
        /// Property denoting the type of the Assignment.
        /// </summary>
        [DataMember]
        public AssignmentType AssignementType
        {
            get
            {
                return _assignementType;
            }
            set
            {
                _assignementType = value;
            }
        }

        /// <summary>
        /// Property denoting the sort order of the activities. 
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                _sortOrder = value;
            }
        }

        /// <summary>
        ///Property denoting the Flag which indicates the current activity is human activity or not
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

        /// <summary>
        /// Property denoting the date time at which tracking record has been generated
        /// </summary>
        [DataMember]
        public String EventDate
        {
            get
            {
                return this._eventDate;
            }
            set
            {
                this._eventDate = value;
            }
        }

        /// <summary>
        /// Property denoting previous activity short name.
        /// </summary>
        [DataMember]
        public String PreviousActivityShortName
        {
            get
            {
                return this._previousActivityShortName;
            }
            set
            {
                this._previousActivityShortName = value;
            }
        }

        /// <summary>
        /// Property denoting previous activity commnet.
        /// </summary>
        [DataMember]
        public String LastActivityComments
        {
            get
            {
                return this._lastActivityComments;
            }
            set
            {
                this._lastActivityComments = value;
            }
        }

        /// <summary>
        /// Property denoting previous activity short name.
        /// </summary>
        [DataMember]
        public String PreviousActivityStartDateTime
        {
            get
            {
                return this._previousActivityStartDateTime;
            }
            set
            {
                this._previousActivityStartDateTime = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public TrackedActivityInfo()
        {
        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for ActivityTracking. </param>
        public TrackedActivityInfo(object[] objectArray)
        {
            if (objectArray[0] != null)
                this.WorkflowDefinitionActivityID = objectArray[0].ToString();

            if (objectArray[1] != null)
                this.ActivityShortName = objectArray[1].ToString();

            if (objectArray[2] != null)
                this.ActivityLongName = objectArray[2].ToString();

            if (objectArray[3] != null)
                this.Status = objectArray[3].ToString();

            if (objectArray[4] != null)
                this.ActedUser = objectArray[4].ToString();

            if (objectArray[5] != null)
                this.ActivityComments = objectArray[5].ToString();

            if (objectArray[6] != null)
                this.EventDate = objectArray[6].ToString();
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public TrackedActivityInfo(String valuesAsXml)
        {
            LoadTrackedActivityInfo(valuesAsXml);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <returns>Xml representation of the object</returns>
        public override String ToXml()
        {
            String propertyValuesFormat = String.Empty;

            propertyValuesFormat = "Id=\"{0}\" WorkflowVersionId=\"{1}\" WorkflowDefinitionActivityId=\"{2}\" ActivityShortName=\"{3}\" ActivityLongName=\"{4}\" RuntimeInstanceId=\"{5}\" ExtendedProperties=\"{6}\" Status=\"{7}\" ActingUserId=\"{8}\" ActedUserId=\"{9}\" ActivityComments=\"{10}\" Variables=\"{11}\" Arguments=\"{12}\" CustomData=\"{13}\" AssignedUsers=\"{14}\" AssignedRoles=\"{15}\" AssignementType=\"{16}\" SortOrder=\"{17}\" IsHumanActivity=\"{18}\" WorkflowId=\"{19}\" WorkflowName=\"{20}\" IsExecuting=\"{21}\" PerformedAction=\"{22}\" WorkflowVersionName=\"{23}\" ActedUser=\"{24}\" EventDate=\"{25}\" ActingUser=\"{26}\" UserMailAddress=\"{27}\" PreviousActivityShortName=\"{28}\" LastActivityComments=\"{29}\" WorkflowComments=\"{30}\"";

            String propertyValues = String.Format(propertyValuesFormat, this.Id, this.WorkflowVersionId, this.WorkflowDefinitionActivityID, this.ActivityShortName, this.ActivityLongName, this.RuntimeInstanceId, this.ExtendedProperties, this.Status, this.ActingUserId, this.ActedUserId, this.ActivityComments, this.Variables, this.Arguments, this.CustomData, this.AssignedUsers, this.AssignedRoles, (int)this.AssignementType, this.SortOrder, this.IsHumanActivity, this.WorkflowId, this.WorkflowName, this.IsExecuting, this.PerformedAction, this.WorkflowVersionName, this.ActedUser, this.EventDate, this.ActingUser, this.UserMailAddress, this.PreviousActivityShortName, this.LastActivityComments, this.WorkflowComments);

            String mdmObjectsCollectionString = String.Empty;

            if (this.MDMObjectCollection != null)
                mdmObjectsCollectionString = this.MDMObjectCollection.ToXml();

            String retXML = String.Format("<TrackedActivityInfo {0}>{1}</TrackedActivityInfo>", propertyValues, mdmObjectsCollectionString);

            return retXML;
        }

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <param name="activityDetailOnly">If it is false then return object and collection details else only object detail </param>
        /// <returns>Xml representation of the object</returns>
        public String ToXml(ObjectSerialization objectSerialization, bool activityDetailOnly)
        {
            String retXML = String.Empty;

            if (objectSerialization == ObjectSerialization.ProcessingOnly)
            {
                String propertyValuesFormat = String.Empty;

                propertyValuesFormat = "Id=\"{0}\" WorkflowVersionId=\"{1}\" WorkflowDefinitionActivityId=\"{2}\" ActivityShortName=\"{3}\" ActivityLongName=\"{4}\" RuntimeInstanceId=\"{5}\" ExtendedProperties=\"{6}\" Status=\"{7}\" ActingUserId=\"{8}\" ActedUserId=\"{9}\" ActivityComments=\"{10}\" Variables=\"{11}\" Arguments=\"{12}\" CustomData=\"{13}\" AssignedUsers=\"{14}\" AssignedRoles=\"{15}\" AssignementType=\"{16}\" SortOrder=\"{17}\" IsHumanActivity=\"{18}\" WorkflowId=\"{19}\" WorkflowName=\"{20}\" IsExecuting=\"{21}\" PerformedAction=\"{22}\" WorkflowVersionName=\"{23}\" ActedUser=\"{24}\" EventDate=\"{25}\" ActingUser=\"{26}\" UserMailAddress=\"{27}\" PreviousActivityShortName=\"{28}\" LastActivityComments=\"{29}\" WorkflowComments=\"{30}\"";

                String propertyValues = String.Format(propertyValuesFormat, this.Id, this.WorkflowVersionId, this.WorkflowDefinitionActivityID, this.ActivityShortName, this.ActivityLongName, this.RuntimeInstanceId, this.ExtendedProperties, this.Status, this.ActingUserId, this.ActedUserId, this.ActivityComments, this.Variables, this.Arguments, this.CustomData, this.AssignedUsers, this.AssignedRoles, (int)this.AssignementType, this.SortOrder, this.IsHumanActivity, this.WorkflowId, this.WorkflowName, this.IsExecuting, this.PerformedAction, this.WorkflowVersionName, this.ActedUser, this.EventDate, this.ActingUser, this.UserMailAddress, this.PreviousActivityShortName, this.LastActivityComments, this.WorkflowComments);

                if (!activityDetailOnly)
                {
                    String mdmObjectsCollectionString = String.Empty;

                    if (this.MDMObjectCollection != null)
                        mdmObjectsCollectionString = this.MDMObjectCollection.ToXml();

                    retXML = String.Format("<TrackedActivityInfo {0}>{1}</TrackedActivityInfo>", propertyValues, mdmObjectsCollectionString);
                }
                else
                {
                    retXML = String.Format("<TrackedActivityInfo {0}></TrackedActivityInfo>", propertyValues);
                }
            }

            return retXML;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ActivityTracking object which needs to be compared.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is TrackedActivityInfo)
            {
                TrackedActivityInfo objectToBeCompared = obj as TrackedActivityInfo;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (!this.MDMObjectCollection.Equals(objectToBeCompared.MDMObjectCollection))
                    return false;

                if (this.WorkflowVersionId != objectToBeCompared.WorkflowVersionId)
                    return false;

                if (this.WorkflowDefinitionActivityID != objectToBeCompared.WorkflowDefinitionActivityID)
                    return false;

                if (this.ActivityShortName != objectToBeCompared.ActivityShortName)
                    return false;

                if (this.ActivityLongName != objectToBeCompared.ActivityLongName)
                    return false;

                if (this.RuntimeInstanceId != objectToBeCompared.RuntimeInstanceId)
                    return false;

                if (this.ExtendedProperties != objectToBeCompared.ExtendedProperties)
                    return false;

                if (this.Status != objectToBeCompared.Status)
                    return false;

                if (this.ActingUserId != objectToBeCompared.ActingUserId)
                    return false;

                if (this.ActedUserId != objectToBeCompared.ActedUserId)
                    return false;

                if (this.ActivityComments != objectToBeCompared.ActivityComments)
                    return false;

                if (this.Variables != objectToBeCompared.Variables)
                    return false;

                if (this.Arguments != objectToBeCompared.Arguments)
                    return false;

                if (this.CustomData != objectToBeCompared.CustomData)
                    return false;

                if (this.AssignedUsers != objectToBeCompared.AssignedUsers)
                    return false;

                if (this.AssignedRoles != objectToBeCompared.AssignedRoles)
                    return false;

                if (this.AssignementType != objectToBeCompared.AssignementType)
                    return false;

                if (this.SortOrder != objectToBeCompared.SortOrder)
                    return false;

                if (this.IsHumanActivity != objectToBeCompared.IsHumanActivity)
                    return false;

                if (this.WorkflowId != objectToBeCompared.WorkflowId)
                    return false;

                if (this.WorkflowName != objectToBeCompared.WorkflowName)
                    return false;

                if (this.WorkflowLongName != objectToBeCompared.WorkflowLongName)
                    return false;

                if (this.IsExecuting != objectToBeCompared.IsExecuting)
                    return false;

                if (this.PerformedAction != objectToBeCompared.PerformedAction)
                    return false;

                if (this.WorkflowVersionName != objectToBeCompared.WorkflowVersionName)
                    return false;

                if (this.ActingUser != objectToBeCompared.ActingUser)
                    return false;

                if (this.ActedUser != objectToBeCompared.ActedUser)
                    return false;

                if (this.UserMailAddress != objectToBeCompared.UserMailAddress)
                    return false;

                if (this.EventDate != objectToBeCompared.EventDate)
                    return false;

                if (this.PreviousActivityShortName != objectToBeCompared.PreviousActivityShortName)
                    return false;

                if (this.LastActivityComments != objectToBeCompared.LastActivityComments)
                    return false;

                if (this.WorkflowComments != objectToBeCompared.WorkflowComments)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override int GetHashCode()
        {
            //TODO :: Implement GetHashCode for WorkflowMDMObjectCollection and include in the below statement
            return base.GetHashCode() ^ this.WorkflowVersionId.GetHashCode() ^ this.WorkflowDefinitionActivityID.GetHashCode() ^ this.ActivityShortName.GetHashCode() ^ this.ActivityLongName.GetHashCode() ^ this.RuntimeInstanceId.GetHashCode() ^ this.ExtendedProperties.GetHashCode() ^ this.Status.GetHashCode() ^ this.ActingUserId.GetHashCode() ^ this.ActedUserId.GetHashCode()
                ^ this.ActivityComments.GetHashCode() ^ this.Variables.GetHashCode() ^ this.Arguments.GetHashCode() ^ this.CustomData.GetHashCode() ^ this.AssignedUsers.GetHashCode() ^ this.AssignedRoles.GetHashCode() ^ this.AssignementType.GetHashCode() ^ this.SortOrder.GetHashCode() ^ this.IsHumanActivity.GetHashCode() ^ this.WorkflowId.GetHashCode() ^ this.WorkflowName.GetHashCode()
                ^ this.WorkflowLongName.GetHashCode() ^ this.IsExecuting.GetHashCode() ^ this.PerformedAction.GetHashCode() ^ this.WorkflowVersionName.GetHashCode() ^ this.ActedUser.GetHashCode() ^ this.ActingUser.GetHashCode() ^ this.UserMailAddress.GetHashCode() ^ this.EventDate.GetHashCode() ^ this.PreviousActivityShortName.GetHashCode() ^ this.LastActivityComments.GetHashCode() ^ this.WorkflowComments.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadTrackedActivityInfo(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "TrackedActivityInfo")
                    {
                        #region Read Tracked Activity Info Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("WorkflowVersionId"))
                            {
                                this.WorkflowVersionId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("WorkflowDefinitionActivityId"))
                            {
                                this.WorkflowDefinitionActivityID = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ActivityShortName"))
                            {
                                this.ActivityShortName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ActivityLongName"))
                            {
                                this.ActivityLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RuntimeInstanceId"))
                            {
                                this.RuntimeInstanceId = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ExtendedProperties"))
                            {
                                this.ExtendedProperties = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Status"))
                            {
                                this.Status = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ActingUserId"))
                            {
                                this.ActingUserId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("ActedUserId"))
                            {
                                this.ActedUserId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("Comments"))
                            {
                                this.ActivityComments = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ActivityComments"))
                            {
                                this.ActivityComments = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowComments"))
                            {
                                this.WorkflowComments = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Variables"))
                            {
                                this.Variables = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Arguments"))
                            {
                                this.Arguments = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CustomData"))
                            {
                                this.CustomData = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("AssignedUsers"))
                            {
                                this.AssignedUsers = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("AssignedRoles"))
                            {
                                this.AssignedRoles = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("AssignementType"))
                            {
                                AssignmentType assignmentType = AssignmentType.RoundRobin;
                                Enum.TryParse<AssignmentType>(reader.ReadContentAsString(), out assignmentType);

                                this.AssignementType = assignmentType;
                            }

                            if (reader.MoveToAttribute("SortOrder"))
                            {
                                this.SortOrder = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("IsHumanActivity"))
                            {
                                this.IsHumanActivity = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("WorkflowId"))
                            {
                                this.WorkflowId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("WorkflowName"))
                            {
                                this.WorkflowName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowLongName"))
                            {
                                this.WorkflowLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("IsExecuting"))
                            {
                                this.IsExecuting = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("PerformedAction"))
                            {
                                this.PerformedAction = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowVersionName"))
                            {
                                this.WorkflowVersionName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ActedUser"))
                            {
                                this.ActedUser = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("EventDate"))
                            {
                                this.EventDate = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ActingUser"))
                            {
                                this.ActingUser = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("UserMailAddress"))
                            {
                                this.UserMailAddress = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("PreviousActivityShortName"))
                            {
                                this.PreviousActivityShortName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LastActivityComments"))
                            {
                                this.LastActivityComments = reader.ReadContentAsString();
                            }
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

        #endregion
    }
}
