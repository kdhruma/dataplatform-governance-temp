using System;
using System.Web;
using System.ServiceModel;
using System.Security.Principal;
using System.Threading;
using System.Diagnostics;

namespace MDM.Utility
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;

    /// <summary>
    /// Specifies SecurityPrincipalHelper having helper methods related to security
    /// </summary>
    public class SecurityPrincipalHelper
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Gets the User Preference from Current context.
        /// </summary>
        /// <returns>UserPreference object.</returns>
        public static UserPreferences GetCurrentUserPreferences()
        {
            if (HttpContext.Current != null)
            {
                return GetUserPreferences(HttpContext.Current);
            }
            else
            {
                return GetUserPreferences();
            }
        }

        /// <summary>
        /// Gets the User preference from the given context.
        /// </summary>
        /// <param name="context">Context for which to get the user preferences.</param>
        /// <returns>User Preference object.</returns>
        public static UserPreferences GetUserPreferences(HttpContext context)
        {
            UserPreferences userPreferences = null;
            SecurityPrincipal securityPrincipal = SecurityPrincipalHelper.GetSecurityPrincipal(context);

            //User principal can't be null
            if (securityPrincipal.UserPreferences == null)
            {
                throw new ApplicationException("User preferences is not available. Contact system administrator.");
            }

            if (securityPrincipal != null)
                userPreferences = securityPrincipal.UserPreferences;

            return userPreferences;
        }

        /// <summary>
        /// Gets the User preference if there is no context mainly if called from Console Application (API testing)
        /// </summary>
        /// <returns>User Preference object.</returns>
        public static UserPreferences GetUserPreferences()
        {
            UserPreferences userPreferences = null;
            SecurityPrincipal securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();

            //User principal can't be null
            if (securityPrincipal.UserPreferences == null)
            {
                throw new ApplicationException(String.Format("User preference is not available for the user '{0}'.", securityPrincipal.CurrentUserName));
            }

            if (securityPrincipal != null)
                userPreferences = securityPrincipal.UserPreferences;

            return userPreferences;
        }

        /// <summary>
        /// Gets Security Principals for current context.
        /// </summary>
        /// <returns>SecurityPrincipal from current context.</returns>
        public static SecurityPrincipal GetCurrentSecurityPrincipal()
        {
            
            SecurityPrincipal securityPrincipal = null;

            try
            {
                //NOTE:: This is being called at very bottom stack. 
                //Commenting logging of security principal get calls. Eventually, when we introduce verbose log, this can be enabled back with proper condition

                //if (Constants.TRACING_ENABLED)
                //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Starting GetCurrentSecurityPrincipal...");

                try
                {
                    if (HttpContext.Current != null)
                    {
                        //if (Constants.TRACING_ENABLED)
                        //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "HttpContext.Current is provided, system will try to get user identity using HttpContext.Current");

                        securityPrincipal = GetSecurityPrincipal(HttpContext.Current);
                    }
                    else if (OperationContext.Current != null
                            && OperationContext.Current.ServiceSecurityContext != null)
                    {
                        //if (Constants.TRACING_ENABLED)
                        //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "OperationContext.Current is provided, system will try to get user identity using OperationContext.Current");
                        
                        securityPrincipal = GetSecurityPrincipal(System.ServiceModel.OperationContext.Current);
                    }
                }
                catch (ObjectDisposedException)
                {
                    //System can get stale operation context object only in below two cases and can throw object disposed exception..
                    //1. While running parallel tasks(through parallel engine)
                    //2. Request has been returned back to caller but some async thread is held up using the operation context
                    
                    //if (Constants.TRACING_ENABLED)
                    //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "OperationContext.Current.ServiceSecurityContext is disposed.");
                    
                    OperationContext.Current = null; // Operation context is stale so just set it to null..This would mkae sure the repeative requests are not getting performance issues due to stale OC.
                }

                if (securityPrincipal == null
                    && ServiceUserContext.Current != null
                    && ServiceUserContext.Current.SecurityPrincipal != null)
                {
                    //if (Constants.TRACING_ENABLED)
                    //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Not able to load security principal from HttpContext and OperationContext, system will try to get security principal from service user context");
                    
                    securityPrincipal = ServiceUserContext.Current.SecurityPrincipal;
                }
                else if (securityPrincipal == null)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Not able to load security principal from HttpContext, OperationContext or ServiceUserContext, system will try to get security principal for 'system user or cfadmin'");

                    ICache cacheMananger = CacheFactory.GetCache();
                    String secPrincipalCacheKeyForSystemUser = CacheKeyGenerator.GetSecurityPrincipalCacheKey("system");
                    securityPrincipal = (SecurityPrincipal)cacheMananger[secPrincipalCacheKeyForSystemUser];

                    if (securityPrincipal == null)
                    {
                        secPrincipalCacheKeyForSystemUser = "SecurityPrincipal_cfadmin_WithoutTimeStamp";
                        securityPrincipal = (SecurityPrincipal)cacheMananger[secPrincipalCacheKeyForSystemUser];
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Unhandled exception while loading current security principal. Exception: " + ex.ToString());
            }
            finally
            {
                if (securityPrincipal == null)
                {
                    StackTrace stackTrace = new StackTrace(false);
                    String callStack = stackTrace.ToString();
                    String traceMessage = "Security loading failed for the requested operation. Operation call stack is: " + callStack;
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, traceMessage);
                    String message = "Security loading failed. Contact system administrator.";
                    UnauthorizedAccessException unAuthException = new UnauthorizedAccessException(message);
                    throw unAuthException;
                }
            }

            ValidateSecurityPrincipal(securityPrincipal);

            return securityPrincipal;
        }

        /// <summary>
        /// Gets Security Principals for the given context.
        /// </summary>
        /// <param name="context">Http context for which to get the security principals.</param>
        /// <returns>Security Principals from given context.</returns>
        public static SecurityPrincipal GetSecurityPrincipal(HttpContext context)
        {
            SecurityPrincipal securityPrincipal = null;

            if (context != null)
            {
                if (context.User is SecurityPrincipal)
                {
                    securityPrincipal = (SecurityPrincipal)context.User;
                }
            }

            return securityPrincipal;
        }

        /// <summary>
        /// Gets Security Principals for the given service operation context.
        /// </summary>
        /// <param name="context">Http context for which to get the security principals.</param>
        /// <returns>Security Principals from given context.</returns>
        public static SecurityPrincipal GetSecurityPrincipal(OperationContext context)
        {
            SecurityPrincipal securityPrincipal = null;

            String userName = String.Empty;

            if (OperationContext.Current != null
                && OperationContext.Current.ServiceSecurityContext != null)
            {
                if (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name != null
                    && !String.IsNullOrWhiteSpace(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name))
                {
                    //if (Constants.TRACING_ENABLED)
                    //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name is:" + OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);

                    userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                }
            }


            ICache cache = CacheFactory.GetCache();
            String securityPrincipalCacheKey = AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled ?
                CacheKeyGenerator.GetSecurityPrincipalCacheKey(userName) : String.Concat("SecurityPrincipal_", userName.ToLower() + "_NoTimeStamp");

            if (cache != null)
            {
                //get login time from cookie, to prepare Security Principal cache key
                securityPrincipal = (SecurityPrincipal)cache[securityPrincipalCacheKey];
            }

            return securityPrincipal;
        }

        /// <summary>
        /// Gets Security Principals for the given service Service Security context.
        /// </summary>
        /// <param name="context">Context for which to get the security principals.</param>
        /// <returns>Security Principals from given context.</returns>
        public static SecurityPrincipal GetSecurityPrincipal(ServiceSecurityContext context)
        {
            SecurityPrincipal securityPrincipal = null;

            String userName = String.Empty;

            if (context.PrimaryIdentity.Name != null)
            {
                userName = context.PrimaryIdentity.Name;
            }

            ICache cache = CacheFactory.GetCache();

            String securityPrincipalCacheKey = AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled ?
                CacheKeyGenerator.GetSecurityPrincipalCacheKey(userName) : String.Concat("SecurityPrincipal_", userName.ToLower() + "_NoTimeStamp");

            if (cache != null)
            {
                //get login time from cookie, to prepare Security Principal cache key
                securityPrincipal = (SecurityPrincipal)cache[securityPrincipalCacheKey];
            }

            return securityPrincipal;
        }

        /// <summary>
        /// Method to validate if we have the security principal is present
        /// <param name="securityPrincipal">The current security principal</param>
        /// </summary>
        public static void ValidateSecurityPrincipal(SecurityPrincipal securityPrincipal)
        {
            if (securityPrincipal.CurrentAuthenticationType == Core.AuthenticationType.Windows)
            {
                if (securityPrincipal.WindowsIdentity == null)
                {
                    throw new ApplicationException("No windows identity found for authentication type 'Windows'. Contact system administrator.");
                }
            }
            else if (securityPrincipal.CurrentAuthenticationType == Core.AuthenticationType.Claims)
            {
                if (securityPrincipal.ClaimsIdentity == null)
                {
                    throw new ApplicationException("No claims identity found for authentication type 'Claims'. Contact system administrator.");
                }
            }
            else if (securityPrincipal.CurrentAuthenticationType == Core.AuthenticationType.Forms)
            {
                if (securityPrincipal.UserIdentity == null)
                {
                    throw new ApplicationException("No user identity found for authentication type 'Forms'. Contact system administrator.");
                }

                if (String.IsNullOrWhiteSpace(securityPrincipal.UserIdentity.Name))
                {
                    throw new ApplicationException("UserName is not provided or invalid for authentication type 'Forms'. Contact system administrator.");
                }
            }
            else
            {
                throw new ApplicationException(String.Format("Provided authentication type '{0}' is not supported. Contact system administrator.", securityPrincipal.CurrentAuthenticationType.ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static WindowsIdentity GetCurrentWindowsIdentity()
        {
            //Try to get windows identity...
            WindowsIdentity _windowsIdentity = null;

            if (Thread.CurrentPrincipal.Identity != null && Thread.CurrentPrincipal.Identity is WindowsIdentity)
            {
                //Try to get windows identity and create security prinicipal based on it..
                _windowsIdentity = (WindowsIdentity)Thread.CurrentPrincipal.Identity;
            }

            // If windows identity not found using Thread.CurrentPrincipal then try using ServiceSecurityContext.
            if (_windowsIdentity == null && ServiceSecurityContext.Current != null)
            {
                _windowsIdentity = ServiceSecurityContext.Current.WindowsIdentity;
            }

            if (_windowsIdentity == null)
            {
                _windowsIdentity = WindowsIdentity.GetCurrent();
            }

            return _windowsIdentity;
        }

        /// <summary>
        /// Specifies if the security principal is available for the current user, i.e. if a user has logged in.
        /// </summary>
        /// <returns></returns>
        public static Boolean IsSecurityPrincipalAvailableForCurrentUser()
        {
            Boolean isSecurityPrincipalAvailable = false;

            HttpContext currentContext = HttpContext.Current;
            if (currentContext != null)
            {
                isSecurityPrincipalAvailable = ((currentContext.User != null) && (currentContext.User is SecurityPrincipal));
            }
            return isSecurityPrincipalAvailable;
        }

        #endregion
    }
}
