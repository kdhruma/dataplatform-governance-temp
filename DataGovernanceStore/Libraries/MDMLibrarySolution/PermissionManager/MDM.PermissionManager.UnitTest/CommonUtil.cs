using System;
using System.Collections;
using System.Configuration;

namespace MDM.PermissionManager.UnitTest
{
    public class CommonUtil
    {
        /// <summary>
        /// Read parameters from App.Config file and populate Hashtable.
        /// </summary>
        /// <param name="configParameters">Hashtable containing values from App.Config</param>
        public static void ReadParameters(Hashtable configParameters)
        {
           String importFilePath = ConfigurationManager.AppSettings["MDM.PermissionManager.UnitTest.InputFolderPath"];

            configParameters["InputFileFolderPath"] = (importFilePath != null) ? importFilePath : String.Empty;
        }
    }
}
