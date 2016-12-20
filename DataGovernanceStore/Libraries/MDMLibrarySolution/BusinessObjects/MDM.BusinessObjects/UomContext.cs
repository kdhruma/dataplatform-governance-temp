using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;    

    /// <summary>
    /// Specifies UomContext which indicates what all information is to be loaded in Uom object
    /// </summary>
    public class UomContext : ObjectBase, IUomContext
    {
        #region Fields

        /// <summary>
        /// Field denoting Id of UOM
        /// </summary>
        Int32 _uomId = -1;

        /// <summary>
        /// Field denoting shortname of UOM
        /// </summary>
        String _uomShortName = String.Empty;

        /// <summary>
        /// Field denoting type of UOM
        /// </summary>
        String _uomType = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Specifies uom id
        /// </summary>
        [DataMember]
        public Int32 UomId
        {
            get
            {
                return _uomId;
            }
            set
            {
                _uomId = value;
            }
        }

        ///<summary>
        /// Specifies uom short name
        ///</summary>
        [DataMember]
        public String UomShortName
        {
            get
            {
                return _uomShortName;
            }
            set
            {
                _uomShortName = value;
            }
        }

        ///<summary>
        /// Specifies uom type
        ///</summary>
        [DataMember]
        public String UomType
        {
            get
            {
                return _uomType;
            }
            set
            {
                _uomType = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public UomContext()
            : base()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public UomContext(Int32 uomId)
            : base()
        {
            this.UomId = uomId;
        }

        #endregion

        #region Methods

        #region Public methods

        /// <summary>
        /// Represents ContainerContext  in Xml format
        /// </summary>
        /// <returns>String representation of current ContainerContext object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("UomContext");

            xmlWriter.WriteAttributeString("UomId", this.UomId.ToString());
            xmlWriter.WriteAttributeString("UomShortName", this.UomShortName.ToString());
            xmlWriter.WriteAttributeString("UomType", this.UomType.ToString());
            //ContainerContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}