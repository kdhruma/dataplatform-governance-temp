using System;

namespace MDM.Interfaces
{
	using MDM.Core;
    using System.Collections.Generic;

	/// <summary>
    /// Exposes methods or properties used for providing build context related information.
	/// </summary>
	public interface IBuildDetailContext : IMDMObject
	{
		#region Properties

		/// <summary>
		/// Field for the id 
		/// </summary>
		 Int32 BuildFeatureId { get; set; }

		/// <summary>
         /// Field for the FilePath 
		/// </summary>
		String FilePath { get; set; }

        /// <summary>
        /// Collection for storing file path and checksum
        /// </summary>
        Dictionary<String, String> FileHashDetails { get; set; }

        /// <summary>
        /// Field for the BuildServer of the Build
        /// </summary>
        String BuildServer { get; set; }

        /// <summary>
        /// Field for the FeatureDescription of the Build
        /// </summary>
        String FeatureDescription { get; set; }

        /// <summary>
        /// Field for the Feature of the Build
        /// </summary>
        String Feature { get; set; }

        /// <summary>
        /// Field for the Version of the Build
        /// </summary>
        String Version { get; set; }

        /// <summary>
        /// Field for the CoreError of the Build
        /// </summary>
        Int32 CoreError { get; set; }

        /// <summary>
        /// Field for the MdmCenterLog of the Build
        /// </summary>
        String MdmCenterLog { get; set; }

        /// <summary>
        /// Field for the WorkFlowError of the Build
        /// </summary>
        Int32 WorkFlowError { get; set; }
        /// <summary>
        /// Field for the WorkFlowErrorLog of the Build
        /// </summary>
        String WorkFlowErrorLog { get; set; }
        /// <summary>
        /// Field for the VpError of the Build
        /// </summary>
        Int32 VpError { get; set; }
        /// <summary>
        /// Field for the VpErrorLog of the Build
        /// </summary>
        String VpErrorLog { get; set; }
        /// <summary>
        /// Field for the BuildType of the Build
        /// </summary>
        String BuildType { get; set; }
        /// <summary>
        /// Field for the BuildServer of the Build
        /// </summary>
        String BuildUser { get; set; }
		#endregion

		#region Method

		#endregion
	}
}