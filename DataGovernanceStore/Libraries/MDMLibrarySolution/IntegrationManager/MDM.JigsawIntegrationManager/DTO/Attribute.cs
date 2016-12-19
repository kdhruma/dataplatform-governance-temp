
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager.DTO
{
    
    /// <summary>
    /// Represents class for Attribute.
    /// </summary>
    /// <seealso cref="MDM.JigsawIntegrationManager.JigsawHelpers.IJigsawJsonSerializable" />
    public class Attribute
    {
        private const String DeletedValue = "__deleted__";

        private String _name;
        private JToken _value;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public JToken Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Attribute()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Attribute(String name, JToken value) : base()
        {
            Name = name;
            Value = value;
        }
    }
}
