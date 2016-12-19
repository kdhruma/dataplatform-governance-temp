using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the categorygroup object
    /// </summary>
    [DataContract] 
    [KnownType(typeof(CategoryCollection))]
    public class CategoryGroup : ICategoryGroup
    {
        #region Fields
        
        /// <summary>
        /// Field specifying collection of categories
        /// </summary>
        private CategoryCollection _categories = new CategoryCollection();

        #endregion Fields

        #region Properties
        /// <summary>
        /// Property specifies categoryCollection object
        /// </summary>
        [DataMember]
        public CategoryCollection Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes Categorygroup object with default parameters
        /// </summary>
        public CategoryGroup() : base() { }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public CategoryGroup(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadCategoryGroup(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods
        
        /// <summary>
        /// Represents categorygroup in Xml format
        /// </summary>
        /// <returns>String representation of current categorygroup object</returns>
        public String ToXml()
        {
            String categoryGroupXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //categoryGroup node start
            xmlWriter.WriteStartElement("CategoryGroup");

            #region write CategoryGroup properties for full CategoryGroup xml
            #endregion  write CategoryGroup properties for full CategoryGroup xml

            #region write Categoriess for Full Category Xml

            if (this.Categories != null)
                xmlWriter.WriteRaw(this.Categories.ToXml());

            #endregion write categoriesfor Full category Xml

            //CategoryGroup node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            categoryGroupXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return categoryGroupXml;
        }

        /// <summary>
        /// Represents categorygroup in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current categorygroup object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String categoryGroupXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //categoryGroup node start
                xmlWriter.WriteStartElement("CategoryGroup");

                #region write CategoryGroup properties for full CategoryGroup xml
                
                #endregion  write CategoryGroup properties for full CategoryGroup xml

                #region write Categories for Full category Xml

                if (this.Categories != null)
                    xmlWriter.WriteRaw(this.Categories.ToXml(objectSerialization));

                #endregion write categories for Full category Xml

                //categoryGroup node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                categoryGroupXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            else
            {
                categoryGroupXml = this.ToXml();
            }

            return categoryGroupXml;
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
                if (obj is CategoryGroup)
                {
                    CategoryGroup objectToBeCompared = obj as CategoryGroup;

                    if (!this.Categories.Equals(objectToBeCompared.Categories))
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
            hashCode = base.GetHashCode() ^ this.Categories.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the categorygroup with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadCategoryGroup(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read CategoryGroup

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Categories")
                        {
                            #region Read category collection

                            String categoryXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(categoryXml))
                            {
                                CategoryCollection categoryCollection = new CategoryCollection(categoryXml);
                                if (categoryCollection != null)
                                {
                                    foreach (Category category in categoryCollection)
                                    {
                                        if (!this.Categories.Contains(category))
                                        {
                                            this.Categories.Add(category);
                                        }
                                    }
                                }
                            }

                            #endregion Read category collection
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read CategoryGroup
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
    }
}
