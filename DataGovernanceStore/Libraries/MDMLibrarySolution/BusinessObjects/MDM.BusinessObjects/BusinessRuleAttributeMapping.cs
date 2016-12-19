using System;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects
{

    /// <summary>
    /// Specifies a BusinessRuleAttributeMapping
    /// </summary>
    [DataContract]
    public class BusinessRuleAttributeMapping : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting BusinessRule_Rule Id
        /// </summary>
        private Int32 _businessRuleId = 0;

        /// <summary>
        /// Field denoting Attribute Id
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        /// Field denoting ShortName of Attribute
        /// </summary>
        private String _attributeName = String.Empty;
        
        /// <summary>
        /// Field denoting LongName of Attribute
        /// </summary>
        private String _attributeLongName = String.Empty;

        /// <summary>
        /// Field denoting AttributeParentName
        /// </summary>
        private String _attributeParentName = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// 

        public BusinessRuleAttributeMapping()
            :base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleArrtibuteMapping</param>
       public BusinessRuleAttributeMapping(Int32 id)
           :base(id)
       {
       }


        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for BusinessRuleAttributeMapping. </param>
        public BusinessRuleAttributeMapping(object[] objectArray)
        {
            int intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);

            this.Id = intId;

            if (objectArray[1] != null)
                Int32.TryParse(objectArray[1].ToString(), out this._attributeId);

            if (objectArray[2] != null)
                this._attributeName = objectArray[2].ToString();

            if (objectArray[3] != null)
                this._attributeLongName = objectArray[3].ToString();

            if (objectArray[4] != null)
                this._attributeParentName = objectArray[4].ToString();

        }

        #endregion

        #region Properties


        /// <summary>
        /// Indicates the BusinessRuleId of BusinessRuleAttributeMapping
        /// </summary>
        [DataMember]
        public Int32 BusinessRuleId
        {
           get { return this._businessRuleId; }
           set { this._businessRuleId = value; }
        }

        /// <summary>
        /// Indicates the AttributeId of BusinessRuleAttributeMapping
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
           get { return this._attributeId; }
           set { this._attributeId = value; }
        }

        /// <summary>
        /// Indicates the AttributeName of Mapped AttributeId
        /// </summary>
        [DataMember]
        public String AttributeName
        {
           get { return this._attributeName; }
           set { this._attributeName = value; }
        }

        /// <summary>
        /// Indicates the AttributeLongName of Mapped AttributeId
        /// </summary>
        [DataMember]
        public String AttributeLongName
        {
           get { return this._attributeLongName; }
           set { this._attributeLongName = value; }
        }

        /// <summary>
        /// Indicates the AttributeParentName of Mapped AttributeId
        /// </summary>
        [DataMember]
        public String AttributeParentName
        {
            get { return this._attributeParentName; }
            set { this._attributeParentName = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Object represents itself as XML
        /// </summary>
        /// <returns>string representation of XML of the object</returns>
        public String ToXML()
        {
            String xml = string.Empty;

            xml = "<ViewAttributeMapping Id=\"{0}\" BusinessRuleId = \"{1}\" AttributeId=\"{2}\" /> ";

            string retXML = string.Format(xml, this.Id, this.BusinessRuleId , this.AttributeId );

            return retXML;
        }

        #endregion
    }
}
