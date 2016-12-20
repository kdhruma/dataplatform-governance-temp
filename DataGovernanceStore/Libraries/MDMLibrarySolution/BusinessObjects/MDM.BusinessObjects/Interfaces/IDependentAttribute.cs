using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get dependent attributes.
    /// </summary>
    public interface IDependentAttribute : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Attribute Name
        /// </summary>
        String AttributeName { get; set; }

        /// <summary>
        /// Property denoting Attribute Id
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property denoting Lookup Table Name
        /// </summary>
        String LookupTableName { get; set; }

        /// <summary>
        /// Property denoting Link Table Name
        /// </summary>
        String LinkTableName { get; set; }

        /// <summary>
        /// Property denoting Link Table Column
        /// </summary>
        String LinkTableColumn { get; set; }

        /// <summary>
        /// Property denoting Attribute Value.
        /// If the attribute is collection then the attribute value as the format of 'Value#$#Value'.
        /// The value separated by '$@$'
        /// </summary>
        String AttributeValue { get; }

        /// <summary>
        /// Property denoting the current dependent attribute type whether is child or parent.
        /// </summary>
        DependencyType DependencyType { get; set; }

        /// <summary>
        /// Property to decide if attribute is collection 
        /// </summary>
        Boolean IsCollection { get; }

        /// <summary>
        /// Property denoting Parent Attribute Name
        /// </summary>
        String ParentAttributeName { get; set; }

        /// <summary>
        /// Property denoting Parent Attribute Id
        /// </summary>
        Int32 ParentAttributeId { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Dependent Attribute
        /// </summary>
        /// <returns>Xml representation of Dependent Attribute</returns>
        String ToXml();

        /// <summary>
        /// Get Application Contexts
        /// </summary>
        /// <returns>ApplicationContextCollection</returns>
        IApplicationContextCollection GetApplicationContexts();

        /// <summary>
        /// Add ApplicationContext for current dependent attribute
        /// </summary>
        /// <param name="applicationContext">Application Context Object</param>
        void AddApplicationContext(IApplicationContext applicationContext);

        /// <summary>
        /// Remove accurance of applicationContext from current dependent Attribute
        /// </summary>
        /// <param name="applicationContext">allpication context</param>
        void RemoveApplicationContext(IApplicationContext applicationContext);

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="iValueCollection">Indicates the attribute value collection interface</param>
        void SetAttributeValue(IValueCollection iValueCollection);

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="iAttribute">Indicates the Attribute Interface</param>
        void SetAttributeValue(IAttribute iAttribute);

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="attributeValue">Indicates the attribute value</param>
        void SetAttributeValue(String attributeValue);

        /// <summary>
        /// Get the Dependency Attribute Value.
        /// If the attribute is collection then the value as the format of 'Value#$#Value'.
        /// Values are separated by '#$#'
        /// </summary>
        /// <returns>Attribute value as String</returns>
        String GetAttributeValue();

        #endregion

        #endregion
    }
}
