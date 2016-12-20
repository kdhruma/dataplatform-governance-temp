using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the application configuration details. 
    /// </summary>
    public interface IAppConfig : IMDMObject
    {
        /// <summary>
        /// Property for Value of AppConfig
        /// </summary>
        String Value { get; set; }
        
        /// <summary>
        /// Property for Description of AppConfig
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Property for Domain of AppConfig
        /// </summary>
        String Domain { get; set; }

        /// <summary>
        /// Property for Client of AppConfig
        /// </summary>
        String Client { get; set; }

        /// <summary>
        /// Property for Row Source Type of AppConfig
        /// </summary>
        String RowSourceType { get; set; }

        /// <summary>
        /// Property for Row Source of AppConfig
        /// </summary>
        String RowSource { get; set; }

        /// <summary>
        /// Property for User Configurable option of AppConfig
        /// </summary>
        Boolean UserConfigurable { get; set; }

        /// <summary>
        /// Property for Long Description of AppConfig
        /// </summary>
        String LongDescription { get; set; }

        /// <summary>
        /// Property for Validation Rule of AppConfig
        /// </summary>
        String ValidationRule { get; set; }

        /// <summary>
        /// Property for Validation Method of AppConfig
        /// </summary>
        String ValidationMethod { get; set; }

        /// <summary>
        /// Xml representation of AppConfig object
        /// </summary>
        /// <returns>Xml format of AppConfig</returns>
        String ToXml();
    }
}
