using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace MDM.Utility
{
    using MDM.Interfaces;
    //TODO: More variations are needed to call custom method from custom assembly
    //TODO: Change all assembly invoke code to use this helper, e.g. Lookup Filter, PreRender

    /// <summary>
    /// This helper class provides methods to check and invoke methods from external assembly
    /// </summary>
    public class ExtensionHelper
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Invokes a method from external assembly and returns the value in requested type
        /// </summary>
        /// <typeparam name="ReturnType">Return type expected from the method. Output will be type casted to the type specified.</typeparam>
        /// <param name="assemblyPath">Path of the assembly from which the method is to be invoked.</param>
        /// <param name="className">Name of the class which contain the method.</param>
        /// <param name="methodName">Name of the method to be invoked.</param>
        /// <param name="parameters">Parameters to be passed to the method, in an object array.</param>
        /// <returns>
        /// Returns the return value of the method, casted to the type requested by user
        /// In case of any error, returns NULL for NULLABLE types, default value for Non NULLABLE types
        /// </returns>
        /// <exception cref="InvalidCastException">If the return value can not be casted to requested type</exception>
        /// <exception cref="FormatException">If the return value can not be casted to requested type</exception>
        public static ReturnType InvokeMethodAs<ReturnType>(String assemblyPath, String className, String methodName, object[] parameters)
        {
            ReturnType result = default(ReturnType);

            try
            {
                result = (ReturnType)InvokeMethod(assemblyPath, className, methodName, parameters);
            }
            catch (InvalidCastException ex)
            {
                MDM.ExceptionManager.ExceptionHandler exHanlder = new MDM.ExceptionManager.ExceptionHandler(ex);
                throw ex;
            }
            catch (FormatException ex)
            {
                MDM.ExceptionManager.ExceptionHandler exHanlder = new MDM.ExceptionManager.ExceptionHandler(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                MDM.ExceptionManager.ExceptionHandler exHanlder = new MDM.ExceptionManager.ExceptionHandler(ex);
                //result will be null for cases when there is an unknown error
            }

            return result;
        }

        /// <summary>
        /// Invokes a method from external assembly and returns an Object
        /// </summary>
        /// <param name="assemblyPath">Path of the assembly from which the method is to be invoked.</param>
        /// <param name="className">Name of the class to look for the method</param>
        /// <param name="methodName">Name of the method to be invoked</param>
        /// <param name="parameters">Parameters to be passed to the method, in an object array.</param>
        /// <returns>Returns an Object, which is the value returned by the method in success, false in failures </returns>
        public static Object InvokeMethod(String assemblyPath, String className, String methodName, object[] parameters)
        {
            Object result = false;

            try
            {
                if (!String.IsNullOrEmpty(assemblyPath) &&
                    !String.IsNullOrEmpty(className) &&
                    !String.IsNullOrEmpty(methodName))
                {
                    Object instanceObject = GetTypeInstance(assemblyPath, className);
                    if (instanceObject != null)
                    {
                        MethodInfo methodInfo = GetMethod(instanceObject, methodName);
                        if (methodInfo != null)
                        {
                            result = methodInfo.Invoke(instanceObject, parameters);
                        }
                        else
                        {
                            throw new Exception("Could not find method '" + methodName + "'.");
                        }
                    }
                    else
                    {
                        throw new Exception("Could not load class from assembly.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("AssemblyPath, ClassName and Method Name can not be empty.");
                }
            }
            catch (Exception ex)
            {
                MDM.ExceptionManager.ExceptionHandler exHanlder = new MDM.ExceptionManager.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Checks if the method exists in the specified class of specified external assembly
        /// </summary>
        /// <param name="assemblyPath">Path of the external assembly</param>
        /// <param name="className">Name of the class to look for</param>
        /// <param name="methodName">Name of the method to look for</param>
        /// <returns>Returns true if the method exists</returns>
        public static Boolean CheckMethod(String assemblyPath, String className, String methodName)
        {
            Boolean result = false;

            try
            {
                if (!String.IsNullOrEmpty(assemblyPath) &&
                    !String.IsNullOrEmpty(className) &&
                    !String.IsNullOrEmpty(methodName))
                {
                    Object instanceObject = GetTypeInstance(assemblyPath, className);
                    if (instanceObject != null)
                    {
                        MethodInfo methodInfo = GetMethod(instanceObject, methodName);
                        if (methodInfo != null)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MDM.ExceptionManager.ExceptionHandler exHanlder = new MDM.ExceptionManager.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Get the instance type
        /// </summary>
        /// <param name="assemblyPath">Indicates the assembly path</param>
        /// <param name="className">Indicates the class name</param>
        /// <returns>Get the instance type</returns>
        public static Object GetTypeInstance(String assemblyPath, String className)
        {
            Object instanceObject = null;

            Assembly externalAssembly = Assembly.LoadFrom(assemblyPath);
            instanceObject = externalAssembly.CreateInstance(className, true);

            return instanceObject;
        }

        /// <summary>
        /// Get the Method from requeste instance object
        /// </summary>
        /// <param name="instanceObject">Indicates the instance object</param>
        /// <param name="methodName">Indicates the method name</param>
        /// <returns>MethodInfo</returns>
        public static MethodInfo GetMethod(Object instanceObject, String methodName)
        {
            MethodInfo methodInfo = null;

            if (instanceObject != null)
            {
                Type ObjType = instanceObject.GetType();
                methodInfo = ObjType.GetMethod(methodName);
            }

            return methodInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginFullFileName"></param>
        public static void IntegratePluginAssemblies(string pluginFullFileName)
        {
            Boolean anypluginFound = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(pluginFullFileName) && File.Exists(pluginFullFileName))
                {
                    String plugins = File.ReadAllText(pluginFullFileName);

                    if (!String.IsNullOrEmpty(plugins))
                    {
                        XmlDocument pluginXmlDoc = new XmlDocument();
                        pluginXmlDoc.LoadXml(plugins);

                        if (pluginXmlDoc.HasChildNodes)
                        {
                            XmlNodeList nodes = pluginXmlDoc.SelectNodes("Plugins/Plugin");
                            if (nodes != null && nodes.Count > 0)
                            {
                                foreach (XmlNode node in nodes)
                                {
                                    String assemblyFileName = String.Empty;
                                    String typeName = String.Empty;
                                    String methodName = String.Empty;
                                    String methodType = String.Empty;

                                    anypluginFound = true;

                                    try
                                    {
                                        assemblyFileName = node.Attributes["AssemblyFileName"] != null ? node.Attributes["AssemblyFileName"].Value : String.Empty;
                                        typeName = node.Attributes["TypeName"] != null ? node.Attributes["TypeName"].Value : String.Empty;
                                        methodName = node.Attributes["MethodName"] != null ? node.Attributes["MethodName"].Value : String.Empty;
                                        methodType = node.Attributes["MethodType"] != null ? node.Attributes["MethodType"].Value : String.Empty;

                                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, String.Format("Loading external plugin[Assembly:{0}, Type:{1}, Method:{2}]...", assemblyFileName, typeName, methodName));

                                        BindingFlags methodBindingFlag = BindingFlags.Static;

                                        if (methodType.ToLower().Contains("static"))
                                            methodBindingFlag = System.Reflection.BindingFlags.Static;
                                        else
                                        {
                                            //todo: define more binding flags
                                        }

                                        if (!string.IsNullOrEmpty(assemblyFileName) && !string.IsNullOrEmpty(typeName) && !string.IsNullOrEmpty(methodName))
                                        {
                                            ExtensionHelper.InvokeMethod(assemblyFileName, typeName, methodName, methodBindingFlag, null);
                                            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, String.Format("Loaded external plugin[Assembly:{0}, Type:{1}, Method:{2}]", assemblyFileName, typeName, methodName));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, String.Format("Loading failed for external plugin[Assembly:{0}, Type:{1}, Method:{2}]. Exception:{3}", assemblyFileName, typeName, methodName, ex.Message));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:: Vishal: Handle exceptions
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, String.Format("Unhandled exception happened during external plugins integraton. Exception:{0}", ex.Message));
            }

            if (!anypluginFound)
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "No external plugin found");
            }
        }

        /// <summary>
        /// Executes a method from an external assembly with specified parameters
        /// </summary>
        /// <param name="assemblyFileName">Indicates full path of the file where the class and method is written</param>
        /// <param name="className">Indicates name of the class where method is written</param>
        /// <param name="methodName">Indicates name of the method to be executed</param>
        /// <param name="methodBindingFlag">Indicates enum for method binding flag</param>
        /// <param name="inputParameters">Indicates input parameters to be passed</param>
        /// <returns>Object returned from invoked method.</returns>
        public static object InvokeMethod(String assemblyFileName, string className, string methodName, BindingFlags methodBindingFlag, object[] inputParameters)
        {
            if (assemblyFileName == null)
                throw new ArgumentNullException("assemblyFileName is null");

            if (className == null)
                throw new ArgumentNullException("className is null");

            if (methodName == null)
                throw new ArgumentNullException("methodName is null");

            try
            {
                string _assemblyPath = null;
                if (System.Web.HttpContext.Current != null)
                {
                    _assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + @"bin\" + assemblyFileName;
                }
                else
                {
                    _assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + assemblyFileName;
                    if (!File.Exists(_assemblyPath))
                        _assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + @"bin\" + assemblyFileName;
                }

                System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFrom(_assemblyPath);

                if (_assembly != null)
                {

                    if (_assembly != null)
                    {
                        Type _type = _assembly.GetType(className);

                        if (_type != null)
                        {
                            if (methodBindingFlag == System.Reflection.BindingFlags.Static)
                            {
                                System.Reflection.MethodInfo _methodInfo = null;
                                if (inputParameters != null)
                                    _methodInfo = _type.GetMethod(methodName, methodBindingFlag | BindingFlags.Public, null, GetInputParameterTypes(inputParameters), null);
                                else
                                    _methodInfo = _type.GetMethod(methodName, methodBindingFlag | BindingFlags.Public);

                                if (_methodInfo != null)
                                {
                                    try
                                    {
                                        return _methodInfo.Invoke(null, inputParameters);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.InnerException != null)
                                        {
                                            throw ex.InnerException;
                                        }
                                        else
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                object _instance = _assembly.CreateInstance(className);
                                if (_instance != null)
                                {
                                    System.Reflection.MethodInfo _methodInfo = null;
                                    if (inputParameters != null)
                                        _methodInfo = _instance.GetType().GetMethod(methodName, methodBindingFlag | System.Reflection.BindingFlags.Public, null, GetInputParameterTypes(inputParameters), null);
                                    else
                                        _methodInfo = _instance.GetType().GetMethod(methodName, methodBindingFlag | System.Reflection.BindingFlags.Public);

                                    if (_methodInfo != null)
                                    {
                                        try
                                        {
                                            return _methodInfo.Invoke(_instance, inputParameters);
                                        }
                                        catch (Exception ex)
                                        {
                                            if (ex.InnerException != null)
                                            {
                                                throw ex.InnerException;
                                            }
                                            else
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Type[] GetInputParameterTypes(object[] inputParameters)
        {
            if (inputParameters == null)
                return null;

            List<Type> _types = new List<Type>();
            foreach (object _parameter in inputParameters)
            {
                Type _type = _parameter.GetType();
                _types.Add(_type);
            }
            return _types.ToArray();
        }

        /// <summary>
        /// Gets the name of the custom web event handler method.
        /// </summary>
        /// <param name="sPage">The page object.</param>
        /// <param name="methodNameSuffix">The method name suffix.</param>
        /// <returns>Method name</returns>
        public static String GetCustomWebEventHandlerMethodName(System.Web.UI.Page sPage, String methodNameSuffix)
        {
            if (sPage != null)
            {
                String pageName = sPage.GetType().ToString();
                pageName = pageName.Replace("_aspx", "").Replace("_ASPX", "");
                pageName = pageName.Substring(pageName.IndexOf(".") + 1).Trim();
                String pageId = sPage.UniqueID.Replace("_", "");

                return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}_{1}{2}", pageName, pageId, methodNameSuffix).Trim();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the custom web event handlers.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>List of custom web event handlers</returns>
        public static List<CustomWebEventHandler> GetCustomWebEventHandlers(String methodName)
        {
            String currentAssemblyPath = GetApplicationAssemblyPath();
            IMDMEventHandlerCollection eventHandlerCollection = MDMEventHandlerManager.GetMDMWebEventHandlers();

            List<CustomWebEventHandler> customWebEventHandlers = new List<CustomWebEventHandler>();

            if (eventHandlerCollection != null && eventHandlerCollection.Count > 0)
            {
                foreach (IMDMEventHandler eventHandler in eventHandlerCollection)
                {
                    customWebEventHandlers.Add(new CustomWebEventHandler()
                    {
                        Assembly = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}", currentAssemblyPath, eventHandler.AssemblyName),
                        Class = eventHandler.FullyQualifiedClassName,
                        Method = methodName
                    });
                }
            }
            
            customWebEventHandlers.Add(new CustomWebEventHandler()
            {
                Assembly = currentAssemblyPath + MDM.Utility.AppConfigurationHelper.GetAppConfig<String>("AssemblyFileName"),
                Class = MDM.Utility.AppConfigurationHelper.GetAppConfig<String>("AssemblyClassName"),
                Method = methodName
            });
            return customWebEventHandlers;
        }

        /// <summary>
        /// Gets the application assembly path.
        /// </summary>
        /// <returns>Application assembly path</returns>
        private static String GetApplicationAssemblyPath()
        {
            return String.Format(System.Globalization.CultureInfo.InvariantCulture,"{0}{1}", System.AppDomain.CurrentDomain.BaseDirectory, @"bin\");
        }

        #endregion Methods

    }
}
