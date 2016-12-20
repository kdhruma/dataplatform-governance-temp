using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity get scope options
    /// </summary>
    public interface IEntityScope
    {
        #region Properties

        /// <summary>
        /// Property denoting Ids of entities for which information needs to be fetched
        /// </summary>
        Collection<Int64> EntityIdList { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents EntityContext  in Xml format
        /// </summary>
        /// <returns>
        /// EntityContext  in Xml format
        /// </returns>
        String ToXml();

        /// <summary>
        /// Adds entitycontext associated with this entityscope
        /// </summary>
        /// <param name="iEntityContext">Indicates an instance of entitycontext</param>
        void AddContext(IEntityContext iEntityContext);

        #endregion Methods
    }
}
