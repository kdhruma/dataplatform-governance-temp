using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies business condition collection
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class BusinessConditionStatusCollection : InterfaceContractCollection<IBusinessConditionStatus, BusinessConditionStatus>, IBusinessConditionStatusCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BusinessConditionStatusCollection()
        { 
        
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of business condition collection</param>
        public BusinessConditionStatusCollection(String valuesAsXml)
        {
            LoadBusinessConditionCollection(valuesAsXml);
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets Xml representation of the BusinessConditionCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the BusinessConditionCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region Write BusinessConditionCollection

                    //BusinessConditionCollection node start
                    xmlWriter.WriteStartElement("BusinessConditions");

                    if (this._items != null)
                    {
                        foreach (BusinessConditionStatus businessCondition in this._items)
                        {
                            xmlWriter.WriteRaw(businessCondition.ToXml());
                        }
                    }

                    //BusinessConditionCollection node end
                    xmlWriter.WriteEndElement();

                    #endregion Write BusinessConditionCollection

                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gets the business condition status by business condition id
        /// </summary>
        /// <param name="businessConditionId"></param>
        /// <returns>Returns the business condition status based on the business condition id</returns>
        public BusinessConditionStatus GetById(Int32 businessConditionId)
        {
            if (businessConditionId < 1 || this._items == null)
            {
                return null;
            }

            foreach (BusinessConditionStatus businessCondition in this._items)
            {
                if (businessCondition.Id == businessConditionId)
                {
                    return businessCondition;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the business condition status collection by business condition id
        /// </summary>
        /// <param name="businessConditionId">indicates business condition id</param>
        /// <returns>Returns the business condition status collection based on the business condition id</returns>
        public BusinessConditionStatusCollection Get(Int32 businessConditionId)
        {
            BusinessConditionStatusCollection businessConditionStatusCollection = null;

            if (this._items != null && this.Count > 0)
            {
                businessConditionStatusCollection = new BusinessConditionStatusCollection();

                foreach (BusinessConditionStatus businessConditionStatus in this._items)
                {
                    if (businessConditionStatus.Id == businessConditionId)
                    {
                        businessConditionStatusCollection.Add(businessConditionStatus);
                    }
                }
            }

            return businessConditionStatusCollection;
        }

        /// <summary>
        /// Gets business conditions id list
        /// </summary>
        /// <returns>Returns business conditions id list</returns>
        public Collection<Int32> GetBusinessConditionIdList()
        {
            Collection<Int32> businessConditionIdList = null;

            if (this._items != null && this._items.Count > 0)
            {
                businessConditionIdList = new Collection<Int32>();

                foreach (BusinessConditionStatus businessConditionStatus in this._items)
                {
                    if (!businessConditionIdList.Contains(businessConditionStatus.Id))
                    {
                        businessConditionIdList.Add(businessConditionStatus.Id);
                    }
                }
            }

            return businessConditionIdList;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadBusinessConditionCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "BusinessCondition")
                        {
                            String businessConditionXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(businessConditionXml))
                            {
                                BusinessConditionStatus entityBusinessCondition = new BusinessConditionStatus(businessConditionXml);
                                this.Add(entityBusinessCondition);
                            }
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

        #endregion Private Methods

        #endregion Methods
    }
}
