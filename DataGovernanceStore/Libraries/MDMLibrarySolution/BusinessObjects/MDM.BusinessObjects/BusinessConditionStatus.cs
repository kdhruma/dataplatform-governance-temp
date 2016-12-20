using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies business condition for the entity
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class BusinessConditionStatus : MDMObject, IBusinessConditionStatus
    {
        #region Fields

        /// <summary>
        /// Indicates status for the business condition 
        /// </summary>
        ValidityStateValue _status = ValidityStateValue.NotChecked;

        #endregion Fields
      
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BusinessConditionStatus()
        { 
        
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of entity validation score collection</param>
        public BusinessConditionStatus(String valuesAsXml)
        {
            LoadBusinessCondition(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates status for the business condition 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public ValidityStateValue Status
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
        
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get XMl representation of business condition
        /// </summary>
        /// <returns>XMl string representation of business condition</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region write BusinessCondition

                    //BusinessCondition node start
                    xmlWriter.WriteStartElement("BusinessCondition");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Status", this.Status.ToString());

                    //BusinessCondition node end
                    xmlWriter.WriteEndElement();

                    #endregion  write BusinessCondition

                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadBusinessCondition(String valuesAsXml)
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
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Status"))
                                {
                                    ValidityStateValue status = ValidityStateValue.NotChecked;
                                    ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader.ReadContentAsString(), true, out status);
                                    this.Status = status;
                                }

                                reader.Read();
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
