using System;

namespace MDM.Core
{
    /// <summary>
    /// Specifies class for Event Handler
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Invokes the provided delegate with the safe thread
        /// </summary>
        /// <typeparam name="TArgs">Delegate</typeparam>
        /// <param name="handler">Handler</param>
        /// <param name="sender">Sender</param>
        /// <param name="args">Delegate</param>
        public static void SafeInvoke<TArgs>( this EventHandler<TArgs> handler, object sender, TArgs args ) 
            where TArgs : EventArgs
        {
            var safeHandler = handler; //copy for thread safety
            if(safeHandler != null)
            {
                safeHandler(sender, args);
            }
        }
    }
}