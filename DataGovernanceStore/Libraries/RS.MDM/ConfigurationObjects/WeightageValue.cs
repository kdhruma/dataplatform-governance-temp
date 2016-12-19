using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("WeightageValue")]
    [Serializable()]
    public class WeightageValue:Object
    {
        #region Fields

        private String _value = String.Empty;
        private String _weightage = String.Empty;

        #endregion Fields

        #region Properties

        [XmlAttribute("Value")]
        [Category("WeightageValue")]
        [Description("Property denoting Weightage Value")]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        [XmlAttribute("Weightage")]
        [Category("WeightageValue")]
        [Description("Property denoting Weightage")]
        public string Weightage
        {
            get
            {
                return _weightage;
            }
            set
            {
                _weightage = value;
            }
        }

        #endregion Properties

         /// <summary>
        /// Initializes a new instance of WeightageValue class.
        /// </summary>
        public WeightageValue()
            : base()
        {

        }
        /// <summary>
        /// Initializes a new instance of WeightageValue class.
        /// </summary>
        public WeightageValue(String value, String weightage)
            
        {
            _value = value;
            _weightage = weightage;
        }

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion  

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is WeightageValue)
                {
                    WeightageValue objectToBeCompared = obj as WeightageValue;

                    if (!this.Value.Equals(Value))
                        return false;

                    if (!this.Weightage.Equals(Weightage))
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
            Int32 hashCode = base.GetHashCode() ^ this.Value.GetHashCode() ^ this.Weightage.GetHashCode();

            return hashCode;
        }
    }
}
