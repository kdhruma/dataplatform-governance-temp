using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using MDM.Core;

namespace MDM.OperationContextManager.Business
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RequestContextData : IRequestContextData
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Guid _operationId = Guid.Empty;

        /// <summary>
        /// 
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings(false, TracingMode.None, TracingLevel.None);
        
        /// <summary>
        /// 
        /// </summary>
        private Stack<IActivityBase> _locialActivityCallstack = new Stack<IActivityBase>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Int32, Stack<IActivityBase>> _threadActivityCallstack = new Dictionary<int, Stack<IActivityBase>>();
        
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<String, String> _stateDataDictionary = new Dictionary<String, String>();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Guid OperationId
        {
            get { return _operationId; }
            set { _operationId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TraceSettings TraceSettings
        {
            get { return _traceSettings; }
            set { _traceSettings = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Stack<IActivityBase> LogicalActivityCallStack
        {
            get { return _locialActivityCallstack; }
            set { _locialActivityCallstack = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<Int32, Stack<IActivityBase>> ThreadActivityCallStack
        {
            get { return _threadActivityCallstack; }
            set { _threadActivityCallstack = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, String> StateDataDictionary
        {
            get { return _stateDataDictionary; }
            set { _stateDataDictionary = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public String Get(string key, String defaultValue)
        {
            if (StateDataDictionary.ContainsKey(key))
            {
                return StateDataDictionary[key];
            }
            else
            {
                StateDataDictionary.Add(key, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Set(string key, String data)
        {
            StateDataDictionary[key] = data;
        }

        #endregion

    }
}