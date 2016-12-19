using MDM.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.DQM
{
    /// <summary>
    /// Exposes methods and properties to set or get the match attribute used for match
    /// </summary>
    interface IMatchAttribute
    {
        #region Properties

        /// <summary>
        /// Indicates name of match attribute
        /// </summary>
        String AttributeName { get; set; }

        /// <summary>
        /// Indicates locale of attribute
        /// </summary>
        LocaleEnum Locale { get; set; }

        /// <summary>
        /// Indicates collection of values which has to be excluded
        /// </summary>
        Collection<String> ExcludedValues { get; set; }

        #endregion Properties

        #region Method

        #endregion Method
    }
}
