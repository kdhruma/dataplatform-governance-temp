using System;
using System.Collections.Generic;
using System.Text;

namespace RS.MDM.Timers
{
    /// <summary>
    /// Provides a base class to process an activity periodically.  
    /// </summary>
    public abstract class Timer : 
        RS.MDM.Object
    {

        #region Fields

        /// <summary>
        /// field for the processing thread
        /// </summary>
        private System.Threading.Thread _processThread;

        /// <summary>
        /// field to indicate if the timer has started
        /// </summary>
        private bool _isStarted;

        /// <summary>
        /// field for process interval
        /// </summary>
        private int _processInterval;

        /// <summary>
        /// 
        /// </summary>
        private string _configuration = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Timer()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the process Interval in milliseconds
        /// </summary>
        public int ProcessInterval
        {
            get
            {
                return this._processInterval;
            }
            set
            {
                this._processInterval = value;
            }
        }

        /// <summary>
        /// Indicates if the timer has started
        /// </summary>
        public bool IsStarted
        {
            get
            {
                return this._isStarted;
            }
        }

        /// <summary>
        /// Indicates the configuration parameters for the Timer Object
        /// </summary>
        public string Configuration
        {
            get
            {
                return this._configuration;
            }
            set
            {
                this._configuration = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Private methods that will processes an activity periodically 
        /// </summary>
        private void Run()
        {
            while (this._isStarted)
            {
                try
                {
                    this.Process();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.EventLog.WriteEntry("Riversand Service Manager", exception.ToString(), System.Diagnostics.EventLogEntryType.Error);
                }
                System.Threading.Thread.Sleep(this._processInterval);
            }
        }

        /// <summary>
        /// Starts the thread that will process an activity periodically
        /// </summary>
        public void Start()
        {
            if (!this._isStarted)
            {
                try
                {
                    this._processThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.Run));
                    this._isStarted = true;
                    this._processThread.Start();
                }
                catch
                {
                    this._isStarted = false;
                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot start the Listener while running.");
            }
        }

        /// <summary>
        /// Stops the thread that is processing an activity periodically
        /// </summary>
        public void Stop()
        {
            this._isStarted = false;
            try
            {
                this._processThread.Abort();
                if (this._processThread != null)
                {
                    this._processThread.Join(500);
                    this._processThread = null;
                }
            }
            catch //(Exception exception)
            {
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Method that needs to be processsed periodically
        /// </summary>
        protected abstract void Process();

        #endregion

        #region IDisposable Interface

        /// <summary>
        /// 
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposedValue && disposing)
            {
                this._isStarted = false;
                try
                {
                    this.Stop();
                }
                catch
                {
                }
            }
            this.disposedValue = true;
        }

        #endregion

    }
}
