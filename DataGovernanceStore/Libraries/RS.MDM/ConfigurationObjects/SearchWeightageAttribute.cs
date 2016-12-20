using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("SearchWeightageAttribute")]
    [Serializable()]
    [XmlInclude(typeof(WeightageValue))]
    public class SearchWeightageAttribute : Object
    {
        private Int32 _id=0;
        private Int32 _localeId=0;
        private String _name=String.Empty;
        private Collection<WeightageValue> _weightageValues = new Collection<WeightageValue>();

        [XmlAttribute("Id")]
        [Category("SearchWeightageAttribute")]
        [Description("Property denoting AttributeId")]
        public new Int32 Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        [XmlAttribute("LocaleId")]
        [Category("SearchWeightageAttribute")]
        [Description("Property denoting Locale Id")]
        public Int32 LocaleId
        {
            get
            {
                return _localeId;
            }
            set
            {
                _localeId = value;
            }
        }

        [XmlAttribute("Name")]
        [Category("SearchWeightageAttribute")]
        [Description("Property denoting Attribute Name")]
        public new String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        [XmlArray("WeightageValues")]
        [Category("SearchWeightageAttribute")]
        [Description("Property denoting WeightageValues")]
        public Collection<WeightageValue> WeightageValues
        {
            get
            {
                return _weightageValues;
            }
            set
            {
                _weightageValues = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of SearchWeightageAttribute class.
        /// </summary>
        public SearchWeightageAttribute()
            : base()
        {

        }
        /// <summary>
        /// Initializes a new instance of SearchWeightageAttribute class.
        /// </summary>
        public SearchWeightageAttribute(Int32 id, Int32 localeId, String name, Collection<WeightageValue> weightageValues)
            
        {
            _id = id;
            _localeId = localeId;
            _name = name;
            _weightageValues = weightageValues;
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
                if (obj is SearchWeightageAttribute)
                {
                    SearchWeightageAttribute objectToBeCompared = obj as SearchWeightageAttribute;

                    if (!this.Name.Equals(objectToBeCompared.Name))
                        return false;

                    if (this.Id != objectToBeCompared.Id)
                        return false;

                    if (this.LocaleId != objectToBeCompared.LocaleId)
                        return false;

                    if (this.WeightageValues != objectToBeCompared.WeightageValues)
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
            Int32 hashCode = base.GetHashCode() ^ this.Id.GetHashCode() ^ this.LocaleId.GetHashCode() ^ this.Name.GetHashCode() ^ this.WeightageValues.GetHashCode();

            return hashCode;
        }
    }
}
