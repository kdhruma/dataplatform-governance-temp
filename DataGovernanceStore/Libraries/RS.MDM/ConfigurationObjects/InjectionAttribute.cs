using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Core;
using System.Xml.Serialization;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("InjectionAttribute")]
    [Serializable()]
    public sealed class InjectionAttribute : Object
    {
        private Int32 _attributeID = 0;
        private AttributeModelType _attributeModelType = AttributeModelType.All;
        private String _currentValue=String.Empty;
        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        private SearchOperator _operator = SearchOperator.None;

        [XmlAttribute("AttributeID")]
        public Int32 AttributeID
        {
            set { this._attributeID = value; }
            get { return this._attributeID; }
        }

        [XmlAttribute("AttributeModelType")]
        public AttributeModelType AttributeModelType
        {
            get
            {
                return this._attributeModelType;
            }
            set
            {
                this._attributeModelType = value;
            }
        }

        [XmlAttribute("Value")]
        public String CurrentValue
        {
            get
            {
                return this._currentValue;
            }
            set
            {
                this._currentValue = value;
            }
        }

        /// <summary>
        /// Represents rule operator for search
        /// </summary>
        [XmlAttribute("Operator")]
        public SearchOperator Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }
        public InjectionAttribute()
            : base()
        {

        }

        public InjectionAttribute(Int32 attributeID, AttributeModelType attributeModelType, String currentValue,SearchOperator searchOperator)
        {
            this._attributeID = attributeID;
            this._attributeModelType = attributeModelType;
            this._currentValue = currentValue;
            this._operator =searchOperator;
        }


        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is InjectionAttribute)
                {
                    InjectionAttribute objectToBeCompared = obj as InjectionAttribute;

                    if (!this.AttributeID.Equals(objectToBeCompared.AttributeID))
                        return false;

                    if (this.AttributeModelType != objectToBeCompared.AttributeModelType)
                        return false;

                    if (this.CurrentValue != objectToBeCompared.CurrentValue)
                        return false;

                      if (this.Operator != objectToBeCompared.Operator)
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
            Int32 hashCode = base.GetHashCode() ^ this.AttributeID.GetHashCode() ^ this.AttributeModelType.GetHashCode() ^ this.CurrentValue.GetHashCode() ^ this.Operator.GetHashCode();

            return hashCode;
        }
    }
}
