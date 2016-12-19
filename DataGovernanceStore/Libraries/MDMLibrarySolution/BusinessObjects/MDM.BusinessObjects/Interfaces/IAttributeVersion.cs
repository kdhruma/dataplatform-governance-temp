using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;    

    /// <summary>
    /// Exposes methods or properties to set or get the attribute version.
    /// </summary>
    public interface IAttributeVersion : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting value reference id.
        /// Null in case of simple. Will be having WSID in case of complex
        /// </summary>
        Int32 InstanceRefId { get; set; }

        /// <summary>
        /// Property for attribute value source ('O' or 'I') 
        /// </summary>
        AttributeValueSource SourceFlag { get; set; }

        /// <summary>
        /// Property denoting sequence of current attribute.
        /// This is used in case of complex and complex collection attribute
        /// </summary>
        Decimal Sequence { get; set; }       

        /// <summary>
        /// Property denoting DateTime of Modification
        /// </summary>
        DateTime ModDateTime { get; set; }

        /// <summary>
        /// Property Denoting the user that modified the Attribute
        /// </summary>
        String ModUser { get; set; }

        /// <summary>
        /// Property denoting the ModProgram 
        /// </summary>
        String ModProgram { get; set; }

        /// <summary>
        /// Property denoting the UserAction 
        /// </summary>
        String UserAction { get; set; }
        #endregion

        #region Methods

        #region XML Methods

        /// <summary>
        /// Get Xml representation of AttributeVersion object
        /// </summary>
        /// <returns>Xml representation of AttributeVersion object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of AttributeVersion object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param> 
        /// <returns>Xml representation of AttributeVersion object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
        
        #endregion
    }
}
