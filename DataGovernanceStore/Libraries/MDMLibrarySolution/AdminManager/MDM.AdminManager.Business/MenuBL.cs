using System;
using System.Diagnostics;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.AdminManager.Data;
    using MDM.Utility;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Business Logic Class for Menus
    /// </summary>
    public class MenuBL : BusinessLogicBase
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get All Menu for given User
        /// </summary>
        /// <param name="userLogin">Indicates Login User Name</param>
        /// <returns>Collection of Menu</returns>
        /// <exception cref="MDMOperationException">Thrown when UserLogin Paramenter is Null or Empty</exception>
        public MenuCollection GetUserMenus(String userLogin)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AdminManager.GetUserMenus", false);

            MenuCollection menus = null;

            #region Parameter Validation

            if (String.IsNullOrWhiteSpace(userLogin))
            {
                throw new MDMOperationException("111648", "Failed to get Menus. UserName is not available.", "AdminManager", String.Empty, "Get");
            }

            #endregion

            MenuDA menuDA = new MenuDA();

            menus = menuDA.GetUserMenus(userLogin);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AdminManager.GetUserMenus");

            return menus;
        }

        #endregion
    }
}
