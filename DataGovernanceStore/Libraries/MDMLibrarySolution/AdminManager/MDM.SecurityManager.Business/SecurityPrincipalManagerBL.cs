using MDM.BufferManager;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Diagnostics;
using MDM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.SecurityManager.Business
{
    public class SecurityPrincipalManagerBL : BusinessLogicBase
    {
        #region private fields

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private SecurityPrincipal GetSecurityPrincipal(String username)
        {
            var diagActivity = new DiagnosticActivity();

            try
            {
                var sb = new SecurityPrincipalBufferManager();

                var sp = sb.GetSecurityPrincipal(username);

                if (sp == null)
                {
                    //Get it from DB first and update it in cache
                    //TODO: get it from db
                    sb.UpdateSecurityPrincipal(sp, 0, true);
                }

                return sp;
            }
            catch (Exception ex)
            {
                diagActivity.LogError(ex.Message);
            }

            return null ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private OperationResult RemoveSecurityPrincipal(String username)
        {
            var diagActivity = new DiagnosticActivity();

            var or = new OperationResult();
            or.OperationResultStatus = OperationResultStatusEnum.Failed;

            try
            {
                var sb = new SecurityPrincipalBufferManager();

                if (!sb.RemoveSecurityPrincipal(username))
                    diagActivity.LogWarning("Security Principal was not removed from cache. Username : " + username);

                //Remove in the DB

                or.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            catch (Exception ex)
            {
                diagActivity.LogError(ex.Message);
                or.Errors.Add(new Error("1001", ex.Message));
            }

            return or;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        private OperationResult UpdateSecurityPrincipal(SecurityPrincipal sec)
        {
            var diagActivity = new DiagnosticActivity();

            var or = new OperationResult();
            or.OperationResultStatus = OperationResultStatusEnum.Failed;

            try
            {
                var sb = new SecurityPrincipalBufferManager();

                sb.UpdateSecurityPrincipal(sec, 0, true);

                //if (!sb.UpdateSecurityPrincipal(sec, 0, true))
                 //   diagActivity.LogWarning("Security Principal was not removed from cache. Username : " + sec.CurrentUserName);

                //Update in the DB

                or.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            catch (Exception ex)
            {
                diagActivity.LogError(ex.Message);
                or.Errors.Add(new Error("1001", ex.Message));
            }

            return or;
        }

        #endregion
    }
}
