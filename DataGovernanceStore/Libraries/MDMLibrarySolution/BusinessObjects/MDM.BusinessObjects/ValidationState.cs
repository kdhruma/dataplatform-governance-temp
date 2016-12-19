using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the Validation State
    /// </summary>
    [DataContract]
    public class ValidationState : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the id of the Validation State Attribute
        /// </summary>
        private int _attributeId = -1;

        /// <summary>
        /// Field denoting the name of the Validation State Attribute
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field denoting the value of the Validation State Attribute
        /// </summary>
        private String _value = String.Empty;

        /// <summary>
        /// Field denoting the Label of the Validation State Attribute
        /// </summary>
        private String _label = String.Empty;

        /// <summary>
        /// Field denoting the template URL for of the Validation State Attribute
        /// </summary>
        private String _templateURL = String.Empty;

        /// <summary>
        /// Field denoting the BusinessRuleID of the Validation State Attribute
        /// </summary>
        private Int32 _businessRuleId = -1;


        /// <summary>
        /// Field denoting the corresponding BusinessRuleName of the Validation State Attribute
        /// </summary>
        private String _businessRuleName = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ValidationState()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of ValidationState</param>
        public ValidationState(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Object array
        /// </summary>
        /// <param name="objectArray">Object array containing values for ValidationState </param>
        public ValidationState(Object[] objectArray)
        {
            int intId = -1, attributeID = -1, businessRuleID = -1;


            if (objectArray.Length > 0 && objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);
            this.Id = intId;

            if (objectArray.Length > 1 && objectArray[1] != null)
            {
                Int32.TryParse(objectArray[1].ToString(), out attributeID);
                this.AttributeId = attributeID;
            }

            if (objectArray.Length > 2 && objectArray[2] != null)
                this.Value = objectArray[2].ToString();

            if (objectArray.Length > 3 && objectArray[3] != null)
                this.Label = objectArray[3].ToString();

            if (objectArray.Length > 4 && objectArray[4] != null)
                this.TemplateURL = objectArray[4].ToString();

            if (objectArray.Length > 5 && objectArray[5] != null)
            {
                Int32.TryParse(objectArray[5].ToString(), out businessRuleID);
                this.BusinessRuleId = businessRuleID;
            }

        }

        #endregion

        #region Properties

        /// <summary>
        ///Property denoting Id of the Validation State Attribute
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        /// <summary>
        /// Property denoting Name of the Validation State Attribute
        /// </summary>
        [DataMember]
        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }
        /// <summary>
        /// Property denoting Value of the Validation State Attribute
        /// </summary>
        [DataMember]
        public String Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        /// <summary>
        /// Property denoting Label of the Validation Validation State Attribute for display
        /// </summary>
        [DataMember]
        public String Label
        {
            get { return this._label; }
            set { this._label = value; }
        }

        /// <summary>
        /// Property denoting TemplateURL for the Validation State Attribute
        /// </summary>
        [DataMember]
        public String TemplateURL
        {
            get { return this._templateURL; }
            set { this._templateURL = value; }
        }

        /// <summary>
        /// Property denoting BusinessRuleId of the Validation State Attribute
        /// </summary>
        [DataMember]
        public Int32 BusinessRuleId
        {
            get { return this._businessRuleId; }
            set { this._businessRuleId = value; }
        }

        /// <summary>
        /// Property denoting BusinessRule Name of the Validation State Attribute
        /// </summary>
        [DataMember]
        public String BusinessRuleName
        {
            get { return this._businessRuleName; }
            set { this._businessRuleName = value; }
        }

        #endregion

    }
}
