using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MDM.Core;
using System.IO;
using MDM.BusinessObjects;
using System.Xml.Serialization;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("InjectionSearchAttributeRule")]
    [Serializable()]
    [XmlInclude(typeof(InjectionAttribute))]
    public sealed class InjectionSearchAttributeRule:Object
    {
        #region Fields

        /// <summary>
        /// Specifies the attribute for which rule is defined
        /// </summary>
        private InjectionAttribute _injectionAttribute = new InjectionAttribute();

        ///// <summary>
        ///// Represents version of workflow
        ///// </summary>
        //private Int32 _workflowVersion = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the attribute for which rule is defined
        /// </summary>
        public InjectionAttribute InjectionAttribute
        {
            get
            {
                return _injectionAttribute;
            }
            set
            {
                _injectionAttribute = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search InjectionAttribute Rule class.
        /// </summary>
        public InjectionSearchAttributeRule()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Search InjectionAttribute Rule class.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="searchOperator"></param>
        public InjectionSearchAttributeRule(InjectionAttribute attribute)
        {
            this._injectionAttribute = attribute;
        }

        /// <summary>
        /// Initializes a new instance of the Search InjectionAttribute Rule class.
        /// </summary>
        /// <param name="attributeId">Id of the search attribute</param>
        /// <param name="attributeModelType">InjectionAttribute model type</param>
        /// <param name="searchValue">Search value</param>
        /// <param name="searchOperator">Search operator</param>
        public InjectionSearchAttributeRule(Int32 attributeId, AttributeModelType attributeModelType, String searchValue, SearchOperator searchOperator)
        {
            this._injectionAttribute = new InjectionAttribute(attributeId, attributeModelType, searchValue,searchOperator);
        }


        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is InjectionSearchAttributeRule)
                {
                    InjectionSearchAttributeRule objectToBeCompared = obj as InjectionSearchAttributeRule;

                    if (!this.InjectionAttribute.Equals(objectToBeCompared.InjectionAttribute))
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
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.InjectionAttribute.GetHashCode();

            return hashCode;
        }


        #endregion Public Methods
       
        #endregion Methods
    }
}
