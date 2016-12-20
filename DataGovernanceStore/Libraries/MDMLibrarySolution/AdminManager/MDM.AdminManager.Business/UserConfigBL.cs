using System;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;

using MDM.BusinessObjects;
using MDM.AdminManager.Data;
using MDM.Utility ;
using MDM.Core;

namespace MDM.AdminManager.Business
{
    public class UserConfigBL : BusinessLogicBase
    {
        #region Fields

        private String _userName = SecurityPrincipalHelper.GetCurrentUserPreferences().LoginName;
        private String _programName = "MDM.AdminManager.Business.UserConfigBL";
        private UserConfigDA _userConfigDA = new UserConfigDA();

        #endregion

        #region Constructors
        
        #endregion

        #region Properties
        
        #endregion

        #region Methods

        public void Process(UserConfig userConfig)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                _userConfigDA.Process(userConfig, _userName, _programName);
                transactionScope.Complete();
            }
        }

        #endregion
    }
}
