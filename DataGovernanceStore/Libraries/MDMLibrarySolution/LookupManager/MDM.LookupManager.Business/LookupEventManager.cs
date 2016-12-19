using System;

namespace MDM.LookupManager.Business
{
    using MDM.Core;

    /// <summary>
    /// Manages events
    /// </summary>
    public sealed class LookupEventManager
    {
        #region Properties

        /// <summary>
        /// Instance of the event manager
        /// </summary>
        public static readonly LookupEventManager Instance = new LookupEventManager();

        /// <summary>
        /// Event handler for LookupDataLoad
        /// </summary>
        public EventHandler<LookupDataLoadEventArgs> LookupDataLoad;

		/// <summary>
		/// Event handler for LookupDataProcessing
		/// </summary>
		public EventHandler<LookupDataProcessEventArgs> LookupDataProcessing;

		/// <summary>
		/// Event handler for LookupDataProcessed
		/// </summary>
		public EventHandler<LookupDataProcessEventArgs> LookupDataProcessed;

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor
        /// </summary>
        private LookupEventManager()
        {

        }

        #endregion

        #region Events

		/// <summary>
		/// Invoke the LookupDataLoad event
		/// </summary>
		/// <param name="e"></param>
        public void OnLookupDataLoad(LookupDataLoadEventArgs e)
        {
            LookupDataLoad.SafeInvoke(this, e);
        }

		/// <summary>
		/// Invoke the LookupDataProcessing event
		/// </summary>
		/// <param name="e"></param>
		public void OnLookupDataProcessing(LookupDataProcessEventArgs e)
		{
			LookupDataProcessing.SafeInvoke(this, e);
		}

		/// <summary>
		/// Invoke the LookupDataProcessed event
		/// </summary>
		/// <param name="e"></param>
		public void OnLookupDataProcessed(LookupDataProcessEventArgs e)
		{
			LookupDataProcessed.SafeInvoke(this, e);
		}

        #endregion
    }
}
