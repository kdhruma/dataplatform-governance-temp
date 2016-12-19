using System;
using System.Collections;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.OperationContextManager.Business
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MDMOperationContext : MarshalByRefObject, ILogicalThreadAffinative
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private const string MDMOperationContextDataSlotName = "MDM.RequestContextManager.MDMOperationContext.CurrentContext.DataSlot";

        /// <summary>
        /// 
        /// </summary>
        private RequestContextData _requestContextData = null;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public RequestContextData RequestContextData
        {
            get { return _requestContextData ?? (_requestContextData = new RequestContextData()); }
            set { _requestContextData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MDMOperationContext Current
        {
            get
            {
                Object data = CallContext.LogicalGetData(MDMOperationContextDataSlotName);

                MDMOperationContext instance = null;

                if (data is MDMOperationContext)
                {
                    instance = (MDMOperationContext)data;
                }
                else
                {
                    instance = new MDMOperationContext { RequestContextData = new RequestContextData() };
                    CallContext.LogicalSetData(MDMOperationContextDataSlotName, instance);
                }

                return instance;
            }
            set
            {
                CallContext.LogicalSetData(MDMOperationContextDataSlotName, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Reset()
        {
            var instance = new MDMOperationContext { RequestContextData = new RequestContextData() };
            Current = instance;
        }

        #endregion
    }
}

