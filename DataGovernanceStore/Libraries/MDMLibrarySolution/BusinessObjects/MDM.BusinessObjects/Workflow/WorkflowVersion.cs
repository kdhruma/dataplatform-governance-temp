using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Reflection;
using System.Security;
using System.Activities.Tracking;

using MDM.Core;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{

    /// <summary>
    /// Specifies the Workflow Version
    /// </summary>
    [DataContract]
    public class WorkflowVersion : MDMObject
    {
        #region Fields

        /// <summary>
        /// field denoting Workflow of this version
        /// </summary>
        private Int32 _workflowId = -1;

        /// <summary>
        /// field denoting Workflow short name of this version
        /// </summary>
        private String _workflowShortName = String.Empty;

        /// <summary>
        /// field denoting Workflow name of this version
        /// </summary>
        private String _workflowLongName = String.Empty;

        /// <summary>
        /// field denoting Version Name
        /// </summary>
        private String _versionName = String.Empty;

        /// <summary>
        /// field denoting Version Number
        /// </summary>
        private Int32 _versionNumber = 1;

        /// <summary>
        /// field denoting Version Type. Draft or Published version
        /// </summary>
        private String _versionType = String.Empty;

        /// <summary>
        /// field denoting Workflow Definition for this version, xaml code
        /// </summary>
        private String _workflowDefinition = String.Empty;

        /// <summary>
        /// field denoting Workflow Definition activity object for this version
        /// </summary>
        private Activity _workflowDefinitionActivity = null;

        /// <summary>
        /// field denoting Tracking Profile for this version, xml code
        /// </summary>
        private String _trackingProfile = String.Empty;

        /// <summary>
        /// field denoting Tracking Profile Object for this version.
        /// </summary>
        private TrackingProfile _trackingProfileObject = null;

        /// <summary>
        /// field denoting comments on why user created this version of workflow
        /// </summary>
        private String _comments = String.Empty;

        /// <summary>
        /// field denoting whether workflow definition is a draft(incomplete) or completed 
        /// </summary>
        private Boolean _isDraft = false;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public WorkflowVersion()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an AppConfig Instance</param>
        public WorkflowVersion(Int32 id)
            : base(id)
        {

        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for Workflow Version. </param>
        public WorkflowVersion(object[] objectArray)
        {
            if (objectArray != null && objectArray.Count() > 0)
            {
                Int32 id = 0;
                if (objectArray[0] != null)
                {
                    Int32.TryParse(objectArray[0].ToString(), out id);
                    this.WorkflowId = id;
                }

                if (objectArray[1] != null)
                    this.WorkflowShortName = objectArray[1].ToString();

                if (objectArray[2] != null)
                    this.WorkflowLongName = objectArray[2].ToString();

                id = 0;
                if (objectArray[3] != null)
                {
                    Int32.TryParse(objectArray[3].ToString(), out id);
                    this.Id = id;
                }

                if (objectArray[4] != null)
                    this.VersionName = objectArray[4].ToString();

                Int32 versionNumber = 0;
                if (objectArray[5] != null)
                {
                    Int32.TryParse(objectArray[5].ToString(), out versionNumber);
                    this.VersionNumber = versionNumber;
                }

                if (objectArray[6] != null)
                    this.WorkflowDefinition = objectArray[6].ToString();

                if (objectArray[7] != null)
                    this.TrackingProfile = objectArray[7].ToString();

                if (objectArray[8] != null)
                    this.Comments = objectArray[8].ToString();

                Boolean isDraft = false;
                if (objectArray[9] != null)
                {
                    Boolean.TryParse(objectArray[9].ToString(), out isDraft);
                    this.IsDraft = isDraft;
                }

                if (objectArray[10] != null)
                    this.VersionType = objectArray[10].ToString();
            }
        }

        /// <summary>
        /// Constructor with Workflow version details as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML containing value for WorkflowVersion object</param>
        /// <example>
        /// Sample XML:
        /// <para>
        ///     &lt;Version VersionId="2" VersionName="TestWF v1" VersionNumber="1" WorkflowId="15" WorkflowLongName="TestWF" Comments="Added for testing"&gt;
        ///         &lt;WorkflowDefinition&gt;
        ///             &lt;Activity xmlns="http://schemas.microsoft.com/netfx/2009/xaml/activities" xmlns:a="clr-namespace:ActivityLibrary;assembly=ActivityLibrary" xmlns:av="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" xmlns:mv="clr-namespace:Microsoft.VisualBasic;assembly=System" xmlns:mva="clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities" xmlns:s="clr-namespace:System;assembly=mscorlib" xmlns:s1="clr-namespace:System;assembly=System" xmlns:s2="clr-namespace:System;assembly=System.Xml" xmlns:s3="clr-namespace:System;assembly=System.Core" xmlns:s4="clr-namespace:System;assembly=System.ServiceModel" xmlns:sa="clr-namespace:System.Activities;assembly=System.Activities" xmlns:sad="clr-namespace:System.Activities.Debugger;assembly=System.Activities" xmlns:sap="http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation" xmlns:scg="clr-namespace:System.Collections.Generic;assembly=System" xmlns:scg1="clr-namespace:System.Collections.Generic;assembly=System.ServiceModel" xmlns:scg2="clr-namespace:System.Collections.Generic;assembly=System.Core" xmlns:scg3="clr-namespace:System.Collections.Generic;assembly=mscorlib" xmlns:sd="clr-namespace:System.Data;assembly=System.Data" xmlns:sl="clr-namespace:System.Linq;assembly=System.Core" xmlns:st="clr-namespace:System.Text;assembly=mscorlib" xmlns:this="clr-namespace:Persistence" xmlns:ua="clr-namespace:UIPersistence_Workflow.Activities" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" mc:Ignorable="sap" x:Class="Persistence.UIPersistence_Workflow" this:UIPersistence_Workflow.arg_AttributeID="0"&gt;
        ///                ......
        ///             &lt;/Activity&gt;
        ///         &lt;/WorkflowDefinition&gt;
        ///         &lt;TrackingProfile&gt;
        ///         &lt;/TrackingProfile&gt;
        ///     &lt;/Version&gt;
        /// </para>
        /// </example>
        public WorkflowVersion(String valuesAsXml)
        {
            /*
             * Sample:
             * <Version VersionId="2" VersionName="TestWF v1" VersionNumber="1" WorkflowId="15" WorkflowLongName="TestWF" Comments="Added for testing">
                  <WorkflowDefinition>
                    <Activity xmlns="http://schemas.microsoft.com/netfx/2009/xaml/activities" xmlns:a="clr-namespace:ActivityLibrary;assembly=ActivityLibrary" xmlns:av="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" xmlns:mv="clr-namespace:Microsoft.VisualBasic;assembly=System" xmlns:mva="clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities" xmlns:s="clr-namespace:System;assembly=mscorlib" xmlns:s1="clr-namespace:System;assembly=System" xmlns:s2="clr-namespace:System;assembly=System.Xml" xmlns:s3="clr-namespace:System;assembly=System.Core" xmlns:s4="clr-namespace:System;assembly=System.ServiceModel" xmlns:sa="clr-namespace:System.Activities;assembly=System.Activities" xmlns:sad="clr-namespace:System.Activities.Debugger;assembly=System.Activities" xmlns:sap="http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation" xmlns:scg="clr-namespace:System.Collections.Generic;assembly=System" xmlns:scg1="clr-namespace:System.Collections.Generic;assembly=System.ServiceModel" xmlns:scg2="clr-namespace:System.Collections.Generic;assembly=System.Core" xmlns:scg3="clr-namespace:System.Collections.Generic;assembly=mscorlib" xmlns:sd="clr-namespace:System.Data;assembly=System.Data" xmlns:sl="clr-namespace:System.Linq;assembly=System.Core" xmlns:st="clr-namespace:System.Text;assembly=mscorlib" xmlns:this="clr-namespace:Persistence" xmlns:ua="clr-namespace:UIPersistence_Workflow.Activities" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" mc:Ignorable="sap" x:Class="Persistence.UIPersistence_Workflow" this:UIPersistence_Workflow.arg_AttributeID="0">
                      ......
                    </Activity>
                  </WorkflowDefinition>
                  <TrackingProfile>
                  </TrackingProfile>
                </Version>
             */
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.Name == "Version")
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("VersionId"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("VersionName"))
                                    {
                                        this.VersionName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("VersionNumber"))
                                    {
                                        this.VersionNumber = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("WorkflowId"))
                                    {
                                        this.WorkflowId = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("WorkflowName"))
                                    {
                                        this.WorkflowShortName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowLongName"))
                                    {
                                        this.WorkflowLongName = reader.ReadContentAsString();
                                    }
                                }
                            }
                        }
                        else if (reader.Name == "WorkflowDefinition")
                        {
                            this.WorkflowDefinition = reader.ReadInnerXml();

                            //calling the activity to Load the workflow activity that can be cached as part of version object
                            _workflowDefinitionActivity = CreateActivityFromXamlString(this.WorkflowDefinition);
                        }
                        
                        if (reader.Name == "TrackingProfile")
                        {
                            this.TrackingProfile = reader.ReadInnerXml();
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
        /// Property denoting Workflow name of this version
        /// </summary>
        [DataMember]
        public String WorkflowShortName
        {
            get
            {
                return this._workflowShortName;
            }
            set
            {
                this._workflowShortName = value;
            }
        }

        /// <summary>
        /// Property denoting Workflow name of the version
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
        /// Property denoting the Version Name
        /// </summary>
        [DataMember]
        public String VersionName
        {
            get
            {
                return this._versionName;
            }
            set
            {
                this._versionName = value;
            }
        }

        /// <summary>
        /// Property denoting the Version Number
        /// </summary>
        [DataMember]
        public Int32 VersionNumber
        {
            get
            {
                return this._versionNumber;
            }
            set
            {
                this._versionNumber = value;
            }
        }

        /// <summary>
        /// Property denoting the Version Type. It will be Draft or Published version
        /// </summary>
        [DataMember]
        public String VersionType
        {
            get
            {
                return this._versionType;
            }
            set
            {
                this._versionType = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Definition
        /// </summary>
        [DataMember]
        public String WorkflowDefinition
        {
            get
            {
                return this._workflowDefinition;
            }
            set
            {
                this._workflowDefinition = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //[DataMember] NOT TRANSFERABLE
        public Activity WorkflowDefinitionActivity
        {
            get
            {
                if (_workflowDefinitionActivity == null)
                {
                    _workflowDefinitionActivity = CreateActivityFromXamlString(this.WorkflowDefinition);
                }

                return _workflowDefinitionActivity;
            }
        }

        /// <summary>
        /// Property denoting the Tracking Profile
        /// </summary>
        [DataMember]
        public String TrackingProfile
        {
            get
            {
                return this._trackingProfile;
            }
            set
            {
                this._trackingProfile = value;
            }
        }

        /// <summary>
        /// Property denoting the Tracking Profile object
        /// </summary>
        [DataMember]
        public TrackingProfile TrackingProfileObject
        {
            get
            {
                return this._trackingProfileObject;
            }
            set
            {
                this._trackingProfileObject = value;
            }
        }

        /// <summary>
        /// Property denoting the Comments
        /// </summary>
        [DataMember]
        public String Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
            }
        }

        /// <summary>
        /// field denoting whether workflow definition is a draft(incomplete) or completed 
        /// </summary>
        [DataMember]
        public Boolean IsDraft
        {
            get
            {
                return this._isDraft;
            }
            set
            {
                this._isDraft = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Workflow version
        /// </summary>
        /// <returns>Xml representation of Workflow version</returns>
        public String ToXML()
        {
            String propertyValuesFormat = String.Empty;

            propertyValuesFormat = "Id=\"{0}\" WorkflowId=\"{1}\" WorkflowLongName=\"{2}\" VersionName=\"{3}\" VersionNumber=\"{4}\" Comments=\"{5}\" IsDraft=\"{6}\" Action=\"{7}\"";

            String propertyValues = String.Format(propertyValuesFormat, this.Id, this.WorkflowId, this.WorkflowLongName, SecurityElement.Escape(this.VersionName), this.VersionNumber, SecurityElement.Escape(this.Comments), this.IsDraft, this.Action);

            String retXML = String.Format("<WorkflowVersion {0}><WorkflowDefinition>{1}</WorkflowDefinition><TrackingProfile>{2}</TrackingProfile></WorkflowVersion>", propertyValues, this.WorkflowDefinition, this.TrackingProfile);

            return retXML;
        }

        /// <summary>
        /// Create instance of Activity class from the String xml value of .xaml file
        /// </summary>
        /// <param name="xamlDefinitionOfActivity">Xml string containing Workflow definition</param>
        /// <returns>Activity Object representation of specified string value of xml</returns>
        private Activity CreateActivityFromXamlString(String xamlDefinitionOfActivity)
        {
            StringReader stringReader = new StringReader(xamlDefinitionOfActivity);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);
            Activity workflow = ActivityXamlServices.Load(xmlReader);

            xmlReader.Close();
            stringReader.Close();

            return workflow;
        }

        #endregion
    }
}
