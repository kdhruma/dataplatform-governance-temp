using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("SearchWeightage")]
    [Serializable()]
    [XmlInclude(typeof(SearchWeightageAttribute))]
    public class SearchWeightage : Object
    {
        private Collection<SearchWeightageAttribute> _searchWeightageAttributes = new Collection<SearchWeightageAttribute>();

        [XmlArray("SearchWeightageAttributes")]
        [Category("SearchWeightage")]
        [Description("Property denoting SearchWeightageAttributes")]
        public Collection<SearchWeightageAttribute> SearchWeightageAttributeCollection
        {
            get
            {
                return _searchWeightageAttributes;
            }
            set
            {
                _searchWeightageAttributes = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of SearchWeightageAttributes class.
        /// </summary>
        public SearchWeightage()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of SearchWeightageAttributes class.
        /// </summary>
        public SearchWeightage(Collection<SearchWeightageAttribute> searchWeightageAttributes)
        {
            _searchWeightageAttributes = searchWeightageAttributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }
    }
}
