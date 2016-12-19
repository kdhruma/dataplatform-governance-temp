using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ServiceUserContext
    {
        private SecurityPrincipal _securityPrincipal = null;

        private static ServiceUserContext _serviceUserContext = null;

        private static Object _lockObj = new Object();

        /// <summary>
        /// 
        /// </summary>
        public static ServiceUserContext Current
        {
            get
            {
                return GetCurrent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SecurityPrincipal SecurityPrincipal
        {
            get
            {
                return _securityPrincipal;
            }
            set
            {
                _securityPrincipal = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ServiceUserContext Initialize(SecurityPrincipal securityPrinicipal)
        {
            if (_serviceUserContext == null)
            {
                lock (_lockObj)
                {
                    if (_serviceUserContext == null)
                    {
                        _serviceUserContext = new ServiceUserContext();
                        _serviceUserContext._securityPrincipal = securityPrinicipal;
                    }
                }
            }

            return _serviceUserContext;
        }

        private static ServiceUserContext GetCurrent()
        {
            return _serviceUserContext;
        }
    }
}
