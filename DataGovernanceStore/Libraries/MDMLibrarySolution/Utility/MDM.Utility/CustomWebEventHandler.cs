using System;

namespace MDM.Utility
{
    /// <summary>
    /// Class representing the custom web event handler
    /// </summary>
    public class CustomWebEventHandler
    {
        /// <summary>
        /// Property denoting the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public String Assembly { get; set; }
        /// <summary>
        /// Property denoting the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        public String Class { get; set; }
        /// <summary>
        /// Property denoting the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public String Method { get; set; }
    }
}
