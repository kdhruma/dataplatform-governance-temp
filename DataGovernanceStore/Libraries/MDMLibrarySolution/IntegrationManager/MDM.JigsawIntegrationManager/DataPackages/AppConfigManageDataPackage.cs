using Newtonsoft.Json.Linq;
using System;

namespace MDM.JigsawIntegrationManager.DataPackages
{
    using MDM.Core;

    /// <summary>
    /// Represents the data package used to send the messages to app config manage topic
    /// </summary>
    public class AppConfigManageDataPackage
    {
        /// <summary>
        /// Property indicating the app name.
        /// </summary>
        public JigsawIntegrationAppName AppName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the name of the attribute for the message data.
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the app manage config specific data json to be sent to Jigsaw.
        /// </summary>
        public JToken MessageData
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the action.
        /// </summary>
        public String Action
        {
            get;
            set;
        }
    }
}
