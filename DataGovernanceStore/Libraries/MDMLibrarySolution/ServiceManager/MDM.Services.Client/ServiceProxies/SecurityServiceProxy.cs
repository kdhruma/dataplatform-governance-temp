using System;
using System.Collections.ObjectModel;

namespace MDM.Services.ServiceProxies
{
    using MDM.BusinessObjects;
    using MDM.Services.SecurityServiceClient;

    /// <summary>
    /// Represents class for security service proxy
    /// </summary>
    public class SecurityServiceProxy : SecurityServiceClient, MDM.WCFServiceInterfaces.ISecurityService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public SecurityServiceProxy()
        { 
        
        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public SecurityServiceProxy(String endpointConfigurationName) 
            : base(endpointConfigurationName) 
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public SecurityServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) 
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        /* Note: Below are methods which has different name in the WCF contract so not coming up as part of Service Reference class
         * We need to explicitly divert call for all the mismatched method names.
         */

        #region ISecurityService Members

        // In SecurityService WCF Contract there are no methods which has different name in the WCF contract. 
        //Hence we did not add any method here.

        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <param name="userType">Indicates type of the user</param>
        /// <param name="countFrom">Indicates from which user list should be started</param>
        /// <param name="countTo">Indicates to which user list should be</param>
        /// <param name="sortColumn">Indicates column name on which user should be sorted</param>
        /// <param name="searchColumn">Indicates column name on which user should be searched</param>
        /// <param name="searchParameter">Indicates search parameter name</param>
        /// <param name="userLogin">Indicates current logged in user</param>
        /// <returns>Returns collection of all requested user</returns>
        public Collection<SecurityUser> GetAllUsers(int userType, int countFrom, int countTo, string sortColumn, string searchColumn, string searchParameter, string userLogin)
        {
            return this.GetAllUsersOld(userType, countFrom, countTo, sortColumn, searchColumn, searchParameter, userLogin);
        }

        #endregion ISecurityService
    }
}
