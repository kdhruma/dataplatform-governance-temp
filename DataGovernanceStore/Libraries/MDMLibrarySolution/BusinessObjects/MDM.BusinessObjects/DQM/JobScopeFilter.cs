using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.BusinessObjects;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Job Scope Filter
    /// </summary>
    [DataContract]
    public class JobScopeFilter : IJobScopeFilter, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field for container ids
        /// </summary>
        private Collection<Int32> _containerIds = new Collection<Int32>();

        /// <summary>
        /// Field for category ids
        /// </summary>
        private Collection<Int64> _categoryIds = new Collection<Int64>();

        /// <summary>
        /// Field for entity type ids
        /// </summary>
        private Collection<Int32> _entityTypeIds = new Collection<Int32>();

        /// <summary>
        /// Field for attribute rules
        /// </summary>
        private SearchAttributeRuleCollection _searchAttributeRules = new SearchAttributeRuleCollection();

        #endregion

        #region Properties

        /// <summary>
        /// Property for container ids
        /// </summary>
        [DataMember]
        public Collection<Int32> ContainerIds
        {
            get { return _containerIds; }
            set { _containerIds = value; }
        }

        /// <summary>
        /// Property for category ids
        /// </summary>
        [DataMember]
        public Collection<Int64> CategoryIds
        {
            get { return _categoryIds; }
            set { _categoryIds = value; }
        }

        /// <summary>
        /// Property for entity type ids
        /// </summary>
        [DataMember]
        public Collection<Int32> EntityTypeIds
        {
            get { return _entityTypeIds; }
            set { _entityTypeIds = value; }
        }
        
        /// <summary>
        /// Property for attribute rules
        /// </summary>
        [DataMember]
        public SearchAttributeRuleCollection SearchAttributeRules
        {
            get { return _searchAttributeRules; }
            set { _searchAttributeRules = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs JobScopeFilter using specified instance data
        /// </summary>
        public JobScopeFilter(JobScopeFilter source)
        {
            this.ContainerIds = source.ContainerIds == null ? null : new Collection<Int32>(source.ContainerIds);
            this.CategoryIds = source.CategoryIds == null ? null : new Collection<Int64>(source.CategoryIds);
            this.EntityTypeIds = source.EntityTypeIds == null ? null : new Collection<Int32>(source.EntityTypeIds);

            // Clone a collectoin of SearchAttributeRules
            if (source.SearchAttributeRules == null)
            {
                this.SearchAttributeRules = null;
            }
            else
            {
                this.SearchAttributeRules = new SearchAttributeRuleCollection(source.SearchAttributeRules);
            }
        }

        /// <summary>
        /// Constructs JobScopeFilter
        /// </summary>
        public JobScopeFilter()
        {
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clones Job Scope Filter
        /// </summary>
        public virtual object Clone()
        {
            JobScopeFilter result = new JobScopeFilter(this);
            return result;
        }

        /// <summary>
        /// Get Xml representation of JobScopeFilter
        /// </summary>
        public virtual String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region Write Containers Ids

            xmlWriter.WriteStartElement("Containers");

            if (!ContainerIds.IsNullOrEmpty())
            {
                List<Int32> containers = ContainerIds.ToList();
                containers.Sort();
                foreach (Int32 containerId in containers)
                {
                    xmlWriter.WriteStartElement("Container");

                    xmlWriter.WriteAttributeString("Id", containerId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion

            #region Write Categories Ids

            xmlWriter.WriteStartElement("Categories");

            if (!CategoryIds.IsNullOrEmpty())
            {
                List<Int64> categories = CategoryIds.ToList();
                categories.Sort();
                foreach (Int64 categoryId in categories)
                {
                    xmlWriter.WriteStartElement("Category");

                    xmlWriter.WriteAttributeString("Id", categoryId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion

            #region Write Entity Types Ids

            xmlWriter.WriteStartElement("EntityTypes");

            if (!EntityTypeIds.IsNullOrEmpty())
            {
                List<Int32> entityTypeIds = EntityTypeIds.ToList();
                entityTypeIds.Sort();
                foreach (Int32 entityTypeId in entityTypeIds)
                {
                    xmlWriter.WriteStartElement("EntityType");

                    xmlWriter.WriteAttributeString("Id", entityTypeId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion

            #region Write SearchAttributeRules

            xmlWriter.WriteStartElement("SearchAttributeRules");

            xmlWriter.WriteAttributeString("DataSource", "DNTables");

            if (!SearchAttributeRules.IsNullOrEmpty())
            {
                foreach (SearchAttributeRule searchAttributeRule in SearchAttributeRules)
                {
                    xmlWriter.WriteRaw(searchAttributeRule.ToXml());
                }
            }

            xmlWriter.WriteEndElement();

            #endregion

            xmlWriter.Flush();

            //Get the actual XML
            String resultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return resultXml;
        }

        /// <summary>
        /// Loads JobScopeFilter from XML node
        /// </summary>
        public virtual void LoadFromXml(XmlNode xmlNode)
        {
            #region Read Containers Ids

            XmlNodeList nodes = xmlNode.SelectNodes(@"Containers/Container");
            ContainerIds.Clear();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (ContainerIds == null)
                        {
                            ContainerIds = new Collection<Int32>();
                        }
                        if (!ContainerIds.Contains(value))
                        {
                            ContainerIds.Add(value);
                        }
                    }
                }
            }
            #endregion Read Containers Ids

            #region Read Categories Ids

            nodes = xmlNode.SelectNodes(@"Categories/Category");
            CategoryIds.Clear();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int64 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int64.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (CategoryIds == null)
                        {
                            CategoryIds = new Collection<Int64>();
                        }
                        if (!CategoryIds.Contains(value))
                        {
                            CategoryIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Categories Ids

            #region Read Entity Types Ids

            nodes = xmlNode.SelectNodes(@"EntityTypes/EntityType");
            EntityTypeIds.Clear();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (EntityTypeIds == null)
                        {
                            EntityTypeIds = new Collection<Int32>();
                        }
                        if (!EntityTypeIds.Contains(value))
                        {
                            EntityTypeIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Entity Typed Ids

            #region Read SearchAttributeRules

            nodes = xmlNode.SelectNodes(@"SearchAttributeRules/Attribute");
            SearchAttributeRules = new SearchAttributeRuleCollection();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    SearchAttributeRules.Add(new SearchAttributeRule(node.OuterXml));
                }
            }

            #endregion Read SearchAttributeRules
        }
        
        #endregion

        #endregion
    }
}