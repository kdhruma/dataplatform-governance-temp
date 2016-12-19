using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
namespace MDM.InstrumentationManager.Utility
{
    using MDM.Core;
    using MDM.OperationContextManager.Business;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// LogicalCallStackManager
    /// </summary>
    public class LogicalCallStackManager
    {
        #region Fields

        private static Object _syncObject = new object();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Static Methods

        /// <summary>
        /// Returns activity from the top of the stack and returns null if stack is empty
        /// </summary>
        /// <returns></returns>
        public static IActivityBase Peek()
        {
            IActivityBase activity = null;

            var activityStack = MDMOperationContext.Current.RequestContextData.LogicalActivityCallStack;
            var threadActivityStack = MDMOperationContext.Current.RequestContextData.ThreadActivityCallStack;
            var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            Tuple<Boolean, Stack<IActivityBase>> stackByThreadId = GetStackByThreadId(currentThreadId, activityStack, threadActivityStack);

            Stack<IActivityBase> currentActivityStack = stackByThreadId.Item2;

            if (currentActivityStack != null)
            {
                activity = currentActivityStack.Peek();
            }

            return activity;
        }

        /// <summary>
        /// Returns Guid of the top of the stack and returns default Guid if stack is empty
        /// Returns Guid of the top of the stack for current thread or 
        /// returns default Guid if stack is empty
        /// </summary>
        /// <returns></returns>
        public static Guid GetTopActivityId()
        {
            Guid activityGuid = new Guid();

            IActivityBase activity = Peek();

            if (activity != null)
                activityGuid = activity.ActivityId;

            return activityGuid;
        }

        /// <summary>
        /// Adds activity to the Activity Stack Dictionary based on current thread
        /// </summary>
        /// <param name="activity"></param>
        public static void PushActivity(DiagnosticActivity activity)
        {
            lock (_syncObject)
            {
                var activityStack = MDMOperationContext.Current.RequestContextData.LogicalActivityCallStack;
                var threadActivityStack = MDMOperationContext.Current.RequestContextData.ThreadActivityCallStack;
                var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                DiagnosticActivity parentActivity = null;
          
                //TODO:: WE dont need child activity chain once we start sending activity for persistnace

                Tuple<Boolean, Stack<IActivityBase>> stackByThreadId = GetStackByThreadId(currentThreadId, activityStack, threadActivityStack);

                Boolean isActivityOnMainThread = stackByThreadId.Item1;

                Stack<IActivityBase> currentActivityStack = stackByThreadId.Item2;

                // Found  valid stack on main or child thread
                if (currentActivityStack != null)
                {
                    if (currentActivityStack.Count > 0)
                    {
                        parentActivity = (DiagnosticActivity)currentActivityStack.Peek();
                        parentActivity.AddDiagnosticActivity(activity);
                    }

                    currentActivityStack.Push(activity);
                    return;
                }

                // Current thread is not main thread and there is no current Stack for it
                // Create Stack for this thread and add to Dictionary
                if (!isActivityOnMainThread)
                {
                    var threadStack = new Stack<IActivityBase>();
                    threadActivityStack.Add(currentThreadId, threadStack);
                    threadStack.Push(activity);
                    
                    return;
                }

                // This is the first activity on main thread. Push it on to main stack.
                activityStack.Push(activity);
            }
        }

        /// <summary>
        /// PopActivity
        /// </summary>
        /// <returns></returns>
        public static DiagnosticActivity PopActivity()
        {
            DiagnosticActivity currentActivity = null;

            var activityStack = MDMOperationContext.Current.RequestContextData.LogicalActivityCallStack;
            var threadActivityStack = MDMOperationContext.Current.RequestContextData.ThreadActivityCallStack;
            var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            Tuple<Boolean, Stack<IActivityBase>> stackByThreadId = GetStackByThreadId(currentThreadId, activityStack, threadActivityStack);

            Stack<IActivityBase> currentActivityStack = stackByThreadId.Item2;

            if (currentActivityStack != null && currentActivityStack.Count > 0)
            {
                currentActivity = (DiagnosticActivity)currentActivityStack.Pop();
            }

            return currentActivity;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets Stack for given thread. Returns null if current thread doesn't have its own stack yet.
        /// </summary>
        /// <param name="currentThreadId"></param>
        /// <param name="activityStack"></param>
        /// <param name="threadActivityStack"></param>
        /// <returns>Returns Stack that this current thread belongs to, along with flag indicating if current thread is main thread or child thread.</returns>
        private static Tuple<Boolean, Stack<IActivityBase>> GetStackByThreadId(Int32 currentThreadId, Stack<IActivityBase> activityStack, Dictionary<Int32, Stack<IActivityBase>> threadActivityStack)
        {
            DiagnosticActivity parentActivity = null;

            if (activityStack.Count > 0)
            {
                parentActivity = (DiagnosticActivity)activityStack.Peek();
            }

            if (parentActivity != null)
            {
                if (parentActivity.ThreadId == currentThreadId)
                {
                    return new Tuple<Boolean, Stack<IActivityBase>> (true,  activityStack);
                }
                else
                {
                    Stack<IActivityBase> currentThreadStack;

                    // See if there is existing stack for current thread
                    if (threadActivityStack.TryGetValue(currentThreadId, out currentThreadStack))
                    {
                        if (currentThreadStack.Count > 0)
                        {
                            parentActivity = (DiagnosticActivity)currentThreadStack.Peek();
                            return new Tuple<Boolean, Stack<IActivityBase>>(false, currentThreadStack);
                        }
                    }
                    else
                    {
                        return new Tuple<bool, Stack<IActivityBase>>(false, null);
                    }
                }
            }

            // no main stack found, return that its 'main thread' and 'no stack'
            return new Tuple<bool, Stack<IActivityBase>>(true, null);
        }

        #endregion

        #endregion
    }
}