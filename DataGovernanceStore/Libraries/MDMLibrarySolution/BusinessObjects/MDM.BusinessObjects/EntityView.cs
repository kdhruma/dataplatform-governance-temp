using System;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Entity View details
    /// </summary>
    [DataContract]
    public class EntityView : IEntityView
    {
        #region Fields

        /// <summary>
        /// Field which uniquely identifies the entity view
        /// </summary>
        private String _uniqueIdentifier = String.Empty;

        /// <summary>
        /// Field denoting the list of attribute ids configured for the view
        /// </summary>
        private Collection<Int32> _attributeIdList = new Collection<Int32>();

        /// <summary>
        /// Field denoting the completion criterion for the view
        /// </summary>
        private CompletionCriterionEnum _completionCriterion = CompletionCriterionEnum.Required;

        /// <summary>
        /// Field denoting the completion status for the view
        /// </summary>
        private Boolean _completionStatus = true;

        #endregion

        #region Properties

        /// <summary>
        /// Property which uniquely identifies the entity view
        /// </summary>
        [DataMember]
        public String UniqueIdentifier 
        {
            get
            {
                return _uniqueIdentifier;
            }
            set
            {
                _uniqueIdentifier = value;
            }
        }

        /// <summary>
        /// Property denoting the list of attribute ids configured for the view
        /// </summary>
        [DataMember]
        public Collection<Int32> AttributeIdList 
        {
            get
            {
                return _attributeIdList;
            }
            set
            {
                _attributeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting the completion criterion for the view
        /// </summary>
        [DataMember]
        public CompletionCriterionEnum CompletionCriterion
        {
            get
            {
                return _completionCriterion;
            }
            set
            {
                _completionCriterion = value;
            }
        }

        /// <summary>
        /// Property denoting the completion status for the view
        /// </summary>
        [DataMember]
        public Boolean CompletionStatus
        {
            get
            {
                return _completionStatus;
            }
            set
            {
                _completionStatus = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Entity View class
        /// </summary>
        public EntityView()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Entity View class with unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">The param which uniquely identifies the entity view</param>
        public EntityView(String uniqueIdentifier)
        {
            _uniqueIdentifier = uniqueIdentifier;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is EntityView)
                {
                    EntityView objectToBeCompared = obj as EntityView;

                    if (this.UniqueIdentifier != objectToBeCompared.UniqueIdentifier)
                        return false;

                    if (this.AttributeIdList != objectToBeCompared.AttributeIdList)
                        return false;

                    if (this.CompletionCriterion != objectToBeCompared.CompletionCriterion)
                        return false;

                    if (this.CompletionStatus != objectToBeCompared.CompletionStatus)
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
            Int32 hashCode = this.UniqueIdentifier.GetHashCode() ^ this.AttributeIdList.GetHashCode() ^ this.CompletionCriterion.GetHashCode() ^ this.CompletionStatus.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Entity View
        /// </summary>
        /// <returns>Xml representation of Entity View</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Entity View node start
            xmlWriter.WriteStartElement("EntityView");

            #region Write Entity View properties

            xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
            xmlWriter.WriteAttributeString("AttributeIdList", ValueTypeHelper.JoinCollection(this.AttributeIdList, ","));
            xmlWriter.WriteAttributeString("CompletionCriterion", this.CompletionCriterion.ToString());
            xmlWriter.WriteAttributeString("CompletionStatus", this.CompletionStatus.ToString());

            #endregion

            //Entity View node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity View based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity View</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                returnXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Entity View node start
                xmlWriter.WriteStartElement("EntityView");

                if (objectSerialization == ObjectSerialization.Compact)
                {
                    #region Write Entity View properties

                    xmlWriter.WriteAttributeString("UID", this.UniqueIdentifier);
                    xmlWriter.WriteAttributeString("AttrIdList", ValueTypeHelper.JoinCollection(this.AttributeIdList, ","));
                    xmlWriter.WriteAttributeString("ComplCrit", this.CompletionCriterion.ToString());
                    xmlWriter.WriteAttributeString("ComplStatus", this.CompletionStatus.ToString());

                    #endregion
                }
                else
                {
                    #region Write Entity View properties

                    xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
                    xmlWriter.WriteAttributeString("CompletionStatus", this.CompletionStatus.ToString());

                    #endregion
                }

                //Entity View node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return returnXml;
        }

        #endregion
    }
}
