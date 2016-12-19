using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for specifying entity history details template.
    /// </summary>
    public interface IEntityHistoryDetailsTemplate
    {
        #region Properties

       /// <summary>
        /// Property denoting type of change happened for entity
        /// </summary>
        EntityChangeType ChangeType
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting template code 
        /// </summary>
        String TemplateCode
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting template text
        /// </summary>
        String TemplateText
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting description about template
        /// </summary>
        String Description
        {
            get;
            set;
        }
       #endregion

        #region Methods

        /// <summary>
        /// Represents IEntityHistoryDetailsTemplate  in Xml format
        /// </summary>
        /// <returns>
        /// IEntityHistoryDetailsTemplate  in Xml format
        /// </returns>
        String ToXml();

        #endregion
    }
}
