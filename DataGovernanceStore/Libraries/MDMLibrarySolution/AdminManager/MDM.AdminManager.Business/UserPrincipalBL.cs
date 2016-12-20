using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;
using System.Transactions;

using MDM.BusinessObjects;
using MDM.Core;
using MDM.AdminManager.Data;

namespace MDM.AdminManager.Business
{
    public class UserPrincipalBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        public UserPrincipalBL()
        {
            
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Boolean AuthenticateUser(String userLoginName, String password)
        {
            UserPrincipalDA userPrincipalDA = new UserPrincipalDA();
            Boolean isAuthenticated = userPrincipalDA.AuthenticateUser(userLoginName, password);
             
            return isAuthenticated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public ArrayList GetUserPermissions(String userLoginName)
        {
            //TODO:: Implement get user permissions
            return new ArrayList();
        }

        #endregion
    }
}
