using MDM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICacheConfiguration
    {

        /// <summary>
        /// 
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        String KeyFormat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        String CacheType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 RetentionTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateInterval RetentionUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        String  DisplayName { get; set; }


        #region Methods

        /// <summary>
        /// Returns the properties and its value in XML form.
        /// </summary>
        /// <returns></returns>
        String ToXml();

        /// <summary>
        /// Returns the properties and its value in XML form.
        /// </summary>
        /// <returns></returns>
        String ToJson();

        /// <summary>
        /// Initializes the object and its properties from values in the XML. 
        /// </summary>
        void LoadFromXml(String xml);

        /// <summary>
        /// Returns the properties and its value in XML form.
        /// </summary>
        /// <returns></returns>
        void  LoadFromJson(String json);

        #endregion
    }
}
