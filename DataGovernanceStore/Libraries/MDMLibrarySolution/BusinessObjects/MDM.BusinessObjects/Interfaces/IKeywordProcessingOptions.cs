using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    
    /// <summary>
    /// Exposes methods or properties to set or get entity processing options, which specifies various flags and indications to handle keywords in entity processing.
    /// </summary>
    public interface IKeywordProcessingOptions
    {
        #region Properties

        /// <summary>
        /// Property indicates if process Ignore keyword
        /// </summary>
        Boolean EnableIgnoreKeyword
        {
            get; 
            set;
        }

        /// <summary>
        /// Property indicates the keyword which should be interpreted as Ignore
        /// </summary>
        String IgnoreKeyword
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates if process Blank keyword
        /// </summary>
        Boolean EnableBlankKeyword
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates the keyword which should be interpreted as Blank
        /// </summary>
        String BlankKeyword
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates if process Delete keyword
        /// </summary>
        Boolean EnableDeleteKeyword
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates the keyword which should be interpreted as Delete
        /// </summary>
        String DeleteKeyword
        {
            get;
            set;
        }
        
        #endregion Properties

        #region Methods
         
        /// <summary>
        /// Represents KeywordProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current KeywordProcessingOptions object instance</returns>
        String ToXml();

        #endregion Methods

    }
}
