using System;
using System.Text;

namespace MDM.ProfileManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;

    public class ImportProfileDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public ImportProfile Get(Int32 profileId)
        {
            ImportProfile importProfile = new ImportProfile();

            return importProfile;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion

    }
}
