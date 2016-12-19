using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Globalization;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Interfaces.Exports;
    using MDM.Core;

    /// <summary>
    /// Specifies the syndication export profile data object
    /// </summary>
    [DataContract]
    [KnownType(typeof(ProfileSettingCollection))]
    [KnownType(typeof(Notification))]
    [KnownType(typeof(ScopeSpecification))]
    [KnownType(typeof(OutputSpecification))]
    [KnownType(typeof(ExecutionSpecification))]
    public class EntityExportSyndicationProfileData : EntityExportProfileData, IEntityExportSyndicationProfileData
    {
        #region Fields

        /// <summary>
        /// Field specifying syndication export profile data ruleset id 
        /// </summary>
        private Int32 _ruleSetId = -1;

        /// <summary>
        /// Field specifying collection of profile settings
        /// </summary>
        private ProfileSettingCollection _profileSettings = new ProfileSettingCollection();

        /// <summary>
        /// Field specifying notification
        /// </summary>
        private Notification _notification = new Notification();

        /// <summary>
        /// Field specifying ScopeSpecification
        /// </summary>
        private ScopeSpecification _scopeSpecification = new ScopeSpecification();

        /// <summary>
        /// Field specifying outputspecification
        /// </summary>
        private OutputSpecification _outputSpecification = new OutputSpecification();

        /// <summary>
        /// Field specifying executionspecification
        /// </summary>
        private ExecutionSpecification _executionSpecification = new ExecutionSpecification();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies syndication export profile data ruleset id 
        /// </summary>
        [DataMember]
        public Int32 RuleSetId
        {
            get
            {
                return _ruleSetId;
            }
            set
            {
                _ruleSetId = value;
            }
        }

        /// <summary>
        /// Property specifies collection of profile settings
        /// </summary>
        [DataMember]
        public ProfileSettingCollection ProfileSettings
        {
            get
            {
                return _profileSettings;
            }
            set
            {
                _profileSettings = value;
            }
        }

        /// <summary>
        /// Property specifies notification
        /// </summary>
        [DataMember]
        public Notification Notification
        {
            get
            {
                return _notification;
            }
            set
            {
                _notification = value;
            }
        }

        /// <summary>
        /// Property specifies scopespecification
        /// </summary>
        [DataMember]
        public ScopeSpecification ScopeSpecification
        {
            get
            {
                return _scopeSpecification;
            }
            set
            {
                _scopeSpecification = value;
            }
        }

        /// <summary>
        /// Property specifies outputSpecification
        /// </summary>
        [DataMember]
        public OutputSpecification OutputSpecification
        {
            get
            {
                return _outputSpecification;
            }
            set
            {
                _outputSpecification = value;
            }
        }

        /// <summary>
        /// Property specifies executionSpecification
        /// </summary>
        [DataMember]
        public ExecutionSpecification ExecutionSpecification
        {
            get
            {
                return _executionSpecification;
            }
            set
            {
                _executionSpecification = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes syndication exportprofiledata object with default parameters
        /// </summary>
        public EntityExportSyndicationProfileData() : base() { }

        /// <summary>
        ///  Initializes syndication exportprofiledata object with specified parameters
        /// </summary>
        /// <param name="id">Id of syndication exportprofiledata</param>
        /// <param name="name">Name of syndication exportprofiledata</param>
        /// <param name="ruleSetId">Rulesetid of syndication exportprofiledata</param>
        public EntityExportSyndicationProfileData(Int32 id, String name, Int32 ruleSetId)
        {
            this.Id = id;
            this.Name = name;
            this.RuleSetId = ruleSetId;
        }        
         
        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public EntityExportSyndicationProfileData(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadEntityExportSyndicationProfileData(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static EntityExportSyndicationProfileData GetDefault()
        {

            EntityExportSyndicationProfileData profileData = new EntityExportSyndicationProfileData(); 
            profileData.ProfileSettings.Add(new ProfileSetting { Name = ExportProfileConstants.DESCRIPTION, Value = "" });
            profileData.ProfileSettings.Add(new ProfileSetting { Name = ExportProfileConstants.APPROVEDCOPY, Value = "false" });
            profileData.ProfileSettings.Add(new ProfileSetting { Name = ExportProfileConstants.SUBSCRIBERQUEUELABEL, Value = String.Empty });
            profileData.ProfileSettings.Add(new ProfileSetting { Name = ExportProfileConstants.IS_CATEGORY_EXPORT, Value = String.Empty });

            profileData.ScopeSpecification.MDMObjectGroups = GetScopeSpecificationMDMObjectGroup();

            profileData.ScopeSpecification.AddContentSetting(ExportProfileConstants.INCLUDEVARIANTMODE, ExportEntityVariantMode.Unknown.ToString());
            profileData.ScopeSpecification.AddContentSetting(ExportProfileConstants.INCLUDEEXTENSIONMODE, ExportEntityExtensionMode.Unknown.ToString());
            profileData.ScopeSpecification.AddContentSetting(ExportProfileConstants.INCLUDEINHERITABLEATTRIBUTES, "false"); 

            profileData.Notification.EmailNotifications.Add(new EmailNotification { Action = ExportJobStatus.Begin });
            profileData.Notification.EmailNotifications.Add(new EmailNotification { Action = ExportJobStatus.Complete });
            profileData.Notification.EmailNotifications.Add(new EmailNotification { Action = ExportJobStatus.Success });
            profileData.Notification.EmailNotifications.Add(new EmailNotification { Action = ExportJobStatus.Failure });

            profileData.Notification.NotificationSettings.Add(new NotificationSetting { Name = ExportProfileConstants.SENDONLYIFITEMCOUNTISMORETHANZERO, Value = Boolean.FalseString });

            profileData.OutputSpecification.OutputDataSpecification.MDMObjectGroups = GetOutPutMDMObjectGroup();

            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.APPLYEXPORTMASKTOLOOKUPATTRIBUTE, Value = "true" });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.EXPORTFILESPLITTYPE, Value = ExportFileSplitType.NoSplit.ToString() });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.ATTRIBUTEHEADERFORMAT, Value = "" });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.INCLUDECATEGORYLONGNAMEPATH, Value = Boolean.FalseString });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.SORTENTITIESBY, Value = ExportEntityGroupBy.EntityTypeTogether.ToString() });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.SORTATTRIBUTESBY, Value = ExportAttributeSortOrder.SortOrderThenByAlphabetical.ToString() });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.INCLUDEBUSINESSCONDITIONS, Value = Boolean.FalseString });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.INCLUDEENTITYSTATES, Value = Boolean.FalseString });
            profileData.OutputSpecification.DataFormatter.DataFormatterSettings.Add(new DataFormatterSetting { Name = ExportProfileConstants.BATCHSIZE, Value = "100" });

            profileData.ExecutionSpecification.ExecutionSettings.Add(new ExecutionSetting { Name = ExportProfileConstants.EXECUTIONMODE, Value = Core.ExportExecutionMode.Full.ToString() });
            profileData.ExecutionSpecification.ExecutionSettings.Add(new ExecutionSetting { Name = ExportProfileConstants.LABEL, Value = String.Empty });

            return profileData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MDMObjectGroupCollection GetOutPutMDMObjectGroup()
        {
            MDMObjectGroupCollection mdmObjectGroups = new MDMObjectGroupCollection();

            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.CommonAttribute });
            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.CategoryAttribute }); 
            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.SystemAttribute });
            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.RelationshipType });
            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.RelationshipAttribute }); 
            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.Locale });

            return mdmObjectGroups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MDMObjectGroupCollection GetScopeSpecificationMDMObjectGroup()
        {
            MDMObjectGroupCollection mdmObjectGroups = new MDMObjectGroupCollection();
            
            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.EntityType });
            mdmObjectGroups.Add(new MDMObjectGroup { ObjectType = Core.ObjectType.Catalog });

            return mdmObjectGroups;
        }

        /// <summary>
        /// Represents syndication exportprofiledata in Xml format
        /// </summary>
        /// <returns>String representation of current syndication exportprofiledata object</returns>
        public override String ToXml()
        {
            String syndicationExportProfileDataXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // syndication exportprofiledata Item node start
            xmlWriter.WriteStartElement("SyndicationExportProfileData");

            #region write SyndicationExportProfileData properties for full SyndicationExportProfileData xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name.ToString());
            xmlWriter.WriteAttributeString("RuleSetId", this.RuleSetId.ToString());

            #endregion  write SyndicationExportProfileData properties for full SyndicationExportProfileData xml

            #region write profile settings for Full profileSettings Xml

            if (this.ProfileSettings != null)
                xmlWriter.WriteRaw(this.ProfileSettings.ToXml());

            #endregion write profile settings for Full profileSettings Xml

            #region write notification for Full notification Xml

            if (this.Notification != null)
                xmlWriter.WriteRaw(this.Notification.ToXml());

            #endregion write notification for Full notification Xml

            #region write scopespecification for Full scopespecification Xml

            if (this.ScopeSpecification != null)
                xmlWriter.WriteRaw(this.ScopeSpecification.ToXml());

            #endregion write scopespecification for Full scopespecification Xml

            #region write outputSpecification for Full outputSpecification Xml

            if (this.OutputSpecification != null)
                xmlWriter.WriteRaw(this.OutputSpecification.ToXml());

            #endregion write outputSpecification for Full outputSpecification Xml

            #region write executionSpecification for Full executionSpecification Xml

            if (this.ExecutionSpecification != null)
                xmlWriter.WriteRaw(this.ExecutionSpecification.ToXml());

            #endregion write executionSpecification for Full executionSpecification Xml

            // syndication exportprofiledata Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            syndicationExportProfileDataXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return syndicationExportProfileDataXml;
        }

        /// <summary>
        /// Represents syndication exportprofiledata in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current syndication exportprofiledata object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String syndicationExportProfileDataXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                // syndication exportprofiledata Item node start
                xmlWriter.WriteStartElement("SyndicationExportProfileData");

                #region write SyndicationExportProfileData properties for full SyndicationExportProfileData xml

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name.ToString());
                xmlWriter.WriteAttributeString("RuleSetId", this.RuleSetId.ToString());

                #endregion  write SyndicationExportProfileData properties for full SyndicationExportProfileData xml

                #region write profile settings for Full profileSettings Xml

                if (this.ProfileSettings != null)
                    xmlWriter.WriteRaw(this.ProfileSettings.ToXml(objectSerialization));

                #endregion write profile settings for Full profileSettings Xml

                #region write notification for Full notification Xml

                if (this.Notification != null)
                    xmlWriter.WriteRaw(this.Notification.ToXml(objectSerialization));

                #endregion write notification for Full notification Xml

                #region write scopespecification for Full scopespecification Xml

                if (this.ScopeSpecification != null)
                    xmlWriter.WriteRaw(this.ScopeSpecification.ToXml(objectSerialization));

                #endregion write scopespecification for Full scopespecification Xml

                #region write outputSpecification for Full outputSpecification Xml

                if (this.OutputSpecification != null)
                    xmlWriter.WriteRaw(this.OutputSpecification.ToXml(objectSerialization));

                #endregion write outputSpecification for Full outputSpecification Xml

                #region write executionSpecification for Full executionSpecification Xml

                if (this.ExecutionSpecification != null)
                    xmlWriter.WriteRaw(this.ExecutionSpecification.ToXml(objectSerialization));

                #endregion write executionSpecification for Full executionSpecification Xml

                // syndication exportprofiledata Item node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                syndicationExportProfileDataXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            else
            {
                syndicationExportProfileDataXml = this.ToXml();
            }

            return syndicationExportProfileDataXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is EntityExportSyndicationProfileData)
                {
                    EntityExportSyndicationProfileData objectToBeCompared = obj as EntityExportSyndicationProfileData;

                    if (!this.RuleSetId.Equals(objectToBeCompared.RuleSetId))
                        return false;

                    if (!this.ProfileSettings.Equals(objectToBeCompared.ProfileSettings))
                        return false;

                    if (!this.Notification.Equals(objectToBeCompared.Notification))
                        return false;

                    if (!this.ScopeSpecification.Equals(objectToBeCompared.ScopeSpecification))
                        return false;

                    if (!this.OutputSpecification.Equals(objectToBeCompared.OutputSpecification))
                        return false;

                    if (!this.ExecutionSpecification.Equals(objectToBeCompared.ExecutionSpecification))
                        return false;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode = base.GetHashCode() ^ this.RuleSetId.GetHashCode() ^ this.ProfileSettings.GetHashCode() ^ this.Notification.GetHashCode()
                        ^ this.ScopeSpecification.GetHashCode() ^ this.OutputSpecification.GetHashCode() ^ this.ExecutionSpecification.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the syndication exportprofiledata with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadEntityExportSyndicationProfileData(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <SyndicationExportProfileData Id="123" Name="Export Product Master" RuleSetId="">
	                <ProfileSettings>
		                <ProfileSetting Name="BatchSize" Value="10" />
		                <ProfileSetting Name="Localor" Value="" />
		                <ProfileSetting Name="LabelName" Value="" />
		                <ProfileSetting Name="ProfileGroup" Value="1" />
		                <ProfileSetting Name="UseInheritedValues" Value="1" />
	                </ProfileSettings>

	                <Notification>
		                <EmailNotifications>
			                <EmailNotification Action="OnBegin" Emails="" />
			                <EmailNotification Action="OnComplete" Emails="" />
			                <EmailNotification Action="OnFailure" Emails="" />
			                <EmailNotification Action="OnSuccess" Emails="" />
		                </EmailNotifications>
	                </Notification>

	                <ScopeSpecification>
		                <ExportScopes>
			                <ExportScope ObjectType="Container" ObjectId="" ObjectUniqueIdentifier="Product Master" Include="" IsRecursive="false">
				                <SearchAttributeRules>
					                <SearchAttributeRule AttributeId="" Operator="" Value="" />
					                <SearchAttributeRule AttributeId="" Operator="" Value="" />
				                </SearchAttributeRules>
				                <ExportScopes>
					                <ExportScope ObjectType="Category" ObjectId="" ObjectUniqueIdentifier="Apparel" Include="" IsRecursive="false">
						                <SearchAttributeRules />
					                </ExportScope>
					                <ExportScope ObjectType="Entity" ObjectId="" ObjectUniqueIdentifier="P1121" Include="" IsRecursive="false">
						                <SearchAttributeRules />
					                </ExportScope>
				                </ExportScopes>
			                </ExportScope>
		                </ExportScopes>
	                </ScopeSpecification>

	                <OutputSpecification>
		                <DataFormatters>
			                <DataFormatter Id="" Name="" Type="" AttributeColumnHeaderFormat="" ApplyExportMaskToLookupAttribute="" CategoryPathType=""></DataFormatter>
		                </DataFormatters>
		                <DataSubscribers>
			                <DataSubscriber Id="" Name="" Location="" FileName=""></DataSubscriber>
		                </DataSubscribers>
		                <OutputDataSpecification>
			                <MDMObjectGroups>
				                <MDMObjectGroup ObjectType="CommonAtttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects>
						                <MDMObject Id="" Locator="" Include="" MappedName="" />
						                <MDMObject Id="" Locator="" Include="" MappedName="" />
					                </MDMObjects>
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="CategoryAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="SystemAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="WorkflowAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="EntityType" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="RelationshipType" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="RelationshipAttributes" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="Locale" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
			                </MDMObjectGroups>
		                </OutputDataSpecification>
	                </OutputSpecification>
	
	                <ExecutionSpecification>
		                <ExecutionSettings>
			                <ExecutionSetting Name="ExecutionType" Value="" /> <!-- Possible values are "Full" or "Delta". Legacy property name is "Type" -->
			                <ExecutionSetting Name="FirstTimeAsFull" Value="" />
			                <ExecutionSetting Name="FromTime" Value="" />
			                <ExecutionSetting Name="Label" Value="" />
			                <ExecutionSetting Name="StartWithAllCommonAttributes" Value="" />
			                <ExecutionSetting Name="StartWithAllCategoryAttributes" Value="" />
			                <ExecutionSetting Name="StartWithAllSystemAttributes" Value="" />
			                <ExecutionSetting Name="StartWithAllWorkflowAttributes" Value="" />
		                </ExecutionSettings>
		                <TriggeringDataSpecification>
			                <MDMObjectGroups>
				                <MDMObjectGroup ObjectType="EntityMetadata" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects>
						                <MDMObject Id="" Locator="" Include="" MappedName="" />
						                <MDMObject Id="" Locator="" Include="" MappedName="" />
					                </MDMObjects>
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="CommonAtttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="CategoryAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="SystemAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="WorkflowAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="EntityType" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="RelationshipType" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="RelationshipAttributes" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
				                <MDMObjectGroup ObjectType="Locale" IncludeAll="" IncludeEmpty="" StartWith="">
					                <MDMObjects />
				                </MDMObjectGroup>
			                </MDMObjectGroups>
		                </TriggeringDataSpecification>
	                </ExecutionSpecification>
                </SyndicationExportProfileData>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read syndication export profile data

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SyndicationExportProfileData")
                        {
                            #region Read syndication export profile data Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.ConvertToInt32(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RuleSetId"))
                                {
                                    this.RuleSetId = ValueTypeHelper.ConvertToInt32(reader.ReadContentAsString());
                                }

                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProfileSettings")
                        {
                            #region Read profile settings collection

                            String profileSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(profileSettingsXml))
                            {
                                //Get collection of profilesettings and populate it in ProfileSetting collection of current syndicationExportProfileData object.
                                ProfileSettingCollection profileSettingCollection = new ProfileSettingCollection(profileSettingsXml);
                                if (profileSettingCollection != null)
                                {
                                    foreach (ProfileSetting profileSetting in profileSettingCollection)
                                    {
                                        if (!this.ProfileSettings.Contains(profileSetting))
                                        {
                                            this.ProfileSettings.Add(profileSetting);
                                        }
                                    }
                                }
                            }

                            #endregion Read profile settings collection
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Notification")
                        {
                            // Read Notification
                            #region Read Notification
                            String notificationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(notificationXml))
                            {
                                Notification notification = new Notification(notificationXml);
                                if (notification != null)
                                {
                                    this.Notification = notification;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ScopeSpecification")
                        {
                            // Read ScopeSpecification
                            #region Read ScopeSpecification
                            String scopeSpecificationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(scopeSpecificationXml))
                            {
                                ScopeSpecification scopeSpecification = new ScopeSpecification(scopeSpecificationXml);
                                if (scopeSpecification != null)
                                {
                                    this.ScopeSpecification = scopeSpecification;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "OutputSpecification")
                        {
                            // Read OutputSpecification
                            #region Read OutputSpecification
                            String outputSpecificationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(outputSpecificationXml))
                            {
                                OutputSpecification outputSpecification = new OutputSpecification(outputSpecificationXml);
                                if (outputSpecification != null)
                                {
                                    this.OutputSpecification = outputSpecification;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionSpecification")
                        {
                            // Read ExecutionSpecification
                            #region Read ExecutionSpecification
                            String executionSpecificationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(executionSpecificationXml))
                            {
                                ExecutionSpecification executionSpecification = new ExecutionSpecification(executionSpecificationXml);
                                if (executionSpecification != null)
                                {
                                    this.ExecutionSpecification = executionSpecification;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read syndication export profile data
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

        #endregion Private Methods

        #endregion
    }
}