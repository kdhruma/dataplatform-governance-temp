using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.MergeCopy;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies MergingProfile
    /// </summary>
    [DataContract]
    public class MergingProfile : BaseProfile, IMergingProfile
    {
        #region Constants

        /// <summary>
        /// Field describing Outer Node name in ProfileData column
        /// </summary>
        private const String ProfileNodeName = "MergingProfile";

        #endregion

        #region Fields

        /// <summary>
        /// Field for Merge Mode
        /// </summary>
        private MergeMode _mode = MergeMode.Unknown;

        /// <summary>
        /// Field for Selected Context Flags
        /// </summary>
        private HashSet<ExposableMergeContextFlag> _selectedContextFlags = null;

        /// <summary>
        /// Field denoting locales which should be used by merge engine
        /// </summary>
        private Collection<LocaleEnum> _locales = new Collection<LocaleEnum>();

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs MergingProfile
        /// </summary>
        public MergingProfile() { }

        /// <summary>
        /// Copy constructor. Constructs MergingProfile using specified instance data
        /// </summary>
        public MergingProfile(MergingProfile source)
            : base(source.Id, source.Name, source.LongName, source.Locale, source.AuditRefId, source.ProgramName)
        {
            this.Action = source.Action;
            this.Mode = source.Mode;
            this.SelectedContextFlags = source.SelectedContextFlags;
            this.Locales = source.Locales;
        }

        /// <summary>
        /// Deserialize string and constructs Merging profile
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public MergingProfile(String valuesAsXml)
        {
            LoadFromXmlWithOuterNode(valuesAsXml, false);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Property denoting Merge Mode
        /// </summary>
        [DataMember]
        public MergeMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        /// Property denoting Selected Context Flags
        /// </summary>
        [DataMember]
        public HashSet<ExposableMergeContextFlag> SelectedContextFlags
        {
            get { return _selectedContextFlags; }
            set { _selectedContextFlags = value; }
        }

        /// <summary>
        /// Property denoting _locales which should be used by merge engine
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> Locales
        {
            get { return _locales; }
            set { _locales = value; }
        }

        
        #endregion

        #region Public Methods

        /// <summary>
        /// Loads MergingProfile from XML node
        /// </summary>
        /// <param name="node">MergingProfile xml node</param>
        /// <param name="propertiesOnly">If true, only MergingProfile's properties will be loaded 
        /// (excluding MDMObject's properties)</param>
        public void LoadFromXml(XmlNode node, Boolean propertiesOnly)
        {
            #region Reset Properties to default values

            Mode = MergeMode.Unknown;
            SelectedContextFlags = new HashSet<ExposableMergeContextFlag>();

            #endregion

            if (node == null)
            {
                return;
            }

            #region Read Organization properties

            if (!propertiesOnly && node.Attributes != null && node.Attributes.Count > 0)
            {
                if (node.Attributes["Id"] != null)
                {
                    Id = ValueTypeHelper.Int32TryParse(node.Attributes["Id"].InnerText, 0);
                }

                if (node.Attributes["Name"] != null)
                {
                    Name = node.Attributes["Name"].InnerText;
                }

                if (node.Attributes["LongName"] != null)
                {
                    LongName = node.Attributes["LongName"].InnerText;
                }

                if (node.Attributes["AuditRefId"] != null)
                {
                    AuditRefId = ValueTypeHelper.Int32TryParse(node.Attributes["AuditRefId"].InnerText, 0);
                }

                if (node.Attributes["Action"] != null)
                {
                    ObjectAction action;
                    Enum.TryParse(node.Attributes["Action"].InnerText, out action);
                    Action = action;
                }
            }

            #endregion Read Organization properties

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Mode")
                {
                    MergeMode mode;
                    if (Enum.TryParse(child.InnerText, out mode))
                    {
                        Mode = mode;
                    }
                }
                else if (child.Name == "SelectedContextFlags")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        SelectedContextFlags = new HashSet<ExposableMergeContextFlag>(
                            ValueTypeHelper.SplitStringToEnumCollection<ExposableMergeContextFlag>(child.InnerText, ',')
                        );
                    }
                }
                else if (child.Name == "SelectedLocales")
                {
                    if (!String.IsNullOrEmpty(child.InnerText))
                    {
                        Locales = new Collection<LocaleEnum>(
                            ValueTypeHelper.SplitLocaleIdStringToLocaleEnumCollection(child.InnerText, ',')
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Loads MergingProfile from XML
        /// </summary>
        /// <param name="xml">MergingProfile xml string</param>
        /// <param name="propertiesOnly">If true, only MergingProfile's properties will be loaded 
        /// (excluding MDMObject's properties)</param>
        public void LoadFromXmlWithOuterNode(String xml, Boolean propertiesOnly)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode node = doc.SelectSingleNode(ProfileNodeName);
                if (node != null)
                {
                    LoadFromXml(node, propertiesOnly);
                }
            }
        }

        /// <summary>
        /// Get Xml representation (excluding MDMObject's properties) of MergingProfile
        /// </summary>
        public String PropertiesOnlyToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MergingProfile node start
            xmlWriter.WriteStartElement(ProfileNodeName);

            xmlWriter.WriteRaw(PropertiesToXml());

            //MergingProfile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Get Xml representation (including MDMObject's properties) of MergingProfile
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MergingProfile node starts
            xmlWriter.WriteStartElement(ProfileNodeName);

            xmlWriter.WriteAttributeString("Id", Id.ToString());
            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("LongName", LongName);
            xmlWriter.WriteAttributeString("Action", Action.ToString());
            xmlWriter.WriteAttributeString("AuditRefId", AuditRefId.ToString());
            xmlWriter.WriteAttributeString("ReferenceId", ReferenceId);

            //Write Merging Profile's properties
            xmlWriter.WriteRaw(PropertiesToXml());

            //MergingProfile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            String mergingProfileAsXmlString = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mergingProfileAsXmlString;
        }

        /// <summary>
        /// Updates passed MergeCopyContext.Context with the data stored in current MergingProfile
        /// </summary>
        /// <param name="context">MergeCopyContext.Context that need to be updated</param>
        public void UpdateContext(MergeCopyContext.Context context)
        {
            if (SelectedContextFlags != null)
            {
                context.copyRelationshipAttributes = SelectedContextFlags.Contains(ExposableMergeContextFlag.CopyRelationshipAttributes);
                context.copyRelationships = SelectedContextFlags.Contains(ExposableMergeContextFlag.CopyRelationships);
                context.copySystemAttributes = SelectedContextFlags.Contains(ExposableMergeContextFlag.CopySystemAttributes);
                context.copyTechnicalAttributes = SelectedContextFlags.Contains(ExposableMergeContextFlag.CopyTechnicalAttributes);
                context.copyCommonAttributes = SelectedContextFlags.Contains(ExposableMergeContextFlag.CopyCommonAttributes);
                context.copyComplexAttributes = SelectedContextFlags.Contains(ExposableMergeContextFlag.CopyComplexAttributes);

                context.GetCallerContext().SetProcessOnlyRootLevelFlag(SelectedContextFlags.Contains(ExposableMergeContextFlag.ProcessOnlyFirstLevelFlag));
                context.GetCallerContext().SetProcessRelationshipsFlag(SelectedContextFlags.Contains(ExposableMergeContextFlag.ProcessRelationshipsFlag));
                context.GetCallerContext().SetProcessRelationshipsOnlyAtFirstLevelFlag(SelectedContextFlags.Contains(ExposableMergeContextFlag.ProcessRelationshipsOnlyAtFirstLevelFlag));
            }

            if (!Locales.IsNullOrEmpty())
            {
                context.SetLocales(Locales);
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MergingProfile
        /// </summary>
        /// <returns>Cloned MergingProfile object</returns>
        public object Clone()
        {
            MergingProfile cloned = new MergingProfile(this);
            return cloned;
        }

        #endregion

        #region Private methods
        
        private String PropertiesToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Mode");
            xmlWriter.WriteRaw(Mode.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SelectedContextFlags");
            if (SelectedContextFlags != null && SelectedContextFlags.Any())
            {
                xmlWriter.WriteRaw(String.Join(",", SelectedContextFlags));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SelectedLocales");
            if (Locales != null && Locales.Any())
            {
                xmlWriter.WriteRaw(String.Join(",", Locales.Select(locale=>(Int32)locale)));
            }
            xmlWriter.WriteEndElement();

            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        #endregion


    }
}
