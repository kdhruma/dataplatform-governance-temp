using System;
using System.Collections.Generic;
using System.Text;

namespace RS.MDM.Services
{
    /// <summary>
    /// Provides a base class with all the basic functionality for a Service Manager
    /// </summary>
    public abstract class ServiceManagerBase : IDisposable
    {
        #region Fields

        /// <summary>
        /// field for the Service Manager Host
        /// </summary>
        private IServiceManagerHost _serviceManagerHost = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Private Parameterless Constructor
        /// </summary>
        private ServiceManagerBase()
        {
        }

        /// <summary>
        /// Constructor with Service Manager Host as input parameter
        /// </summary>
        /// <param name="serviceManagerHost">Indicate the Service Manager Host</param>
        public ServiceManagerBase(IServiceManagerHost serviceManagerHost)
        {
            this._serviceManagerHost = serviceManagerHost;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Service Manager Host that is hosting the Service Manager
        /// </summary>
        public IServiceManagerHost ServiceManagerHost
        {
            get 
            { 
                return this._serviceManagerHost; 
            }
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Starts the Service Manager
        /// </summary>
        public abstract void Start();


        /// <summary>
        /// Stops the Service Manager
        /// </summary>
        public abstract void Stop();


        #endregion

        #region IDisposable Interface

        /// <summary>
        /// Tracks whether Dispose has been called.
        /// </summary>
        bool disposed;

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        /// <summary>
        /// Cleans up the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        /// <summary>
        /// Cleans up Managed and Unmanaged resources based on the parameter 'disposing'
        /// </summary>
        /// <param name="disposing">Indicates if the user code is calling it.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        /// <summary>
        /// Allows an Object to attempt to free resources and perform other cleanup operations before the Object is reclaimed by garbage collection.
        /// </summary>
        ~ServiceManagerBase()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion
    }
}
