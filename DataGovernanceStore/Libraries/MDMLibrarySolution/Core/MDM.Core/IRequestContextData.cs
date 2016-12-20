using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestContextData
    {
        /// <summary>
        /// 
        /// </summary>
        Guid OperationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        TraceSettings TraceSettings { get; set; }

        /// <summary>
        /// Stack managing the main thread activities
        /// </summary>
        Stack<IActivityBase> LogicalActivityCallStack { get; set; }

        /// <summary>
        /// Stack managing each child thread's activities
        /// </summary>
        Dictionary<Int32, Stack<IActivityBase>> ThreadActivityCallStack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        String Get(string key, String defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void Set(string key, String data);
    }
}
