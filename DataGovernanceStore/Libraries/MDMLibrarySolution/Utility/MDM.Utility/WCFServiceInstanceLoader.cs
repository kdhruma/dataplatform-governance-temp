using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace MDM.Utility
{
    using MDM.Core;
    
    /// <summary>
    /// Represents class to load WCF service instance
    /// </summary>
    public sealed class WCFServiceInstanceLoader
    {
        private MethodInfo _wcfInstanceFactoryLoaderMethod = null;

        private static Object _lockObj = new object();

        private static WCFServiceInstanceLoader _instance = null;

        private WCFServiceInstanceLoader(MethodInfo wcfInstanceFactoryLoaderMethod)
        {
            _wcfInstanceFactoryLoaderMethod = wcfInstanceFactoryLoaderMethod;
        }

        /// <summary>
        /// Initializes WCFServiceInstanceLoader based on method information
        /// </summary>
        /// <param name="wcfInstanceFactoryLoaderMethod">Indicates method info for which need to initialize WCS service instance</param>
        /// <returns>Returns WCF service instance based on given method info</returns>
        public static WCFServiceInstanceLoader Initialize(MethodInfo wcfInstanceFactoryLoaderMethod)
        {
            if (_instance == null)
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new WCFServiceInstanceLoader(wcfInstanceFactoryLoaderMethod);
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// Indicates local instance is enabled or not
        /// </summary>
        /// <returns>returns true if local instance is enabled, otherwise false</returns>
        public static Boolean IsLocalInstancesEnabled()
        {
            if (_instance == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Gets the service instance of type T
        /// </summary>
        /// <typeparam name="T">Indicates type of service instance</typeparam>
        /// <returns>Returns type of T as a result</returns>
        public static T GetServiceInstance<T>()
        {
            if (_instance == null)
                throw new NullReferenceException("WCFServiceInstanceLoader is not initialized. Please make sure to initialize this singleton instance before using");

            MethodInfo methodInfo = _instance._wcfInstanceFactoryLoaderMethod;

            Type[] argTypes = { typeof(T) };
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(argTypes);

            T result = (T)genericMethodInfo.Invoke(null, null);

            return result;
        }
    }
}
