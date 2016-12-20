using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects.RuleFramework
{
    /// <summary>
    /// Specifies and behaves as Generic property Type for the Rule Framework
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.riversand.com/schemas")]
    public class RuleGenericPropertyType
    {
        #region Fields

        /// <summary>
        /// Field for property name
        /// </summary>
        private string _propName;

        /// <summary>
        /// Field for property value
        /// </summary>
        private string _propValue;

        #endregion

        #region Properties

        /// <summary>
        /// Property name
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string propName
        {
            get
            {
                return this._propName;
            }
            set
            {
                this._propName = value;
            }
        }

        /// <summary>
        /// Property value
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string propValue
        {
            get
            {
                return this._propValue;
            }
            set
            {
                this._propValue = value;
            }
        }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion
    }
}
