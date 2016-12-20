using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get choice of operations to be performed on attribute(s) while importing. 
    /// </summary>
    public interface IAttributeProcessingOptions
    {
        /// <summary>
        /// Id of the attribute which will added/updated/deleted based on Attribute Action flag in Import Profile
        /// </summary>
        Int32 AttributeId
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the attribute which will added/updated/deleted based on Attribute Action flag in Import Profile
        /// </summary>
        String AttributeName
        {
            get;
            set;
        }

        /// <summary>
        /// Flag which will decide whether the attribute will be added
        /// </summary>
        Boolean CanAddAttribute
        {
            get;
            set;
        }

        /// <summary>
        /// Flag which will decide whether the attribute will be deleted
        /// </summary>
        Boolean CanDeleteAttribute
        {
            get;
            set;
        }

        /// <summary>
        /// Flag which will decide whether the attribute will be updated
        /// </summary>
        Boolean CanUpdateAttribute
        {
            get;
            set;
        }

        /// <summary>
        /// Locale of the Attribute
        /// </summary>
        LocaleEnum Locale
        {
            get;
            set;
        }

        /// <summary>
        /// ModelType of the Attribute For. e.g. Common
        /// </summary>
        AttributeModelType AttributeModelType
        {
            get;
            set;
        }
    }
}
