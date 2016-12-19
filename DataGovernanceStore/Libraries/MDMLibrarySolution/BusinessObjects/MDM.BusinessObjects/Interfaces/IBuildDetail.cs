using System;

namespace MDM.Interfaces
{
	using MDM.Core;

	/// <summary>
    /// Exposes methods or properties used for providing build related information.
	/// </summary>
	public interface IBuildDetail : IMDMObject
	{
		#region Properties

		/// <summary>
		/// Field for the id of a Build
		/// </summary>
		 new Int32 Id { get; set; }

		/// <summary>
		/// Field for the BuildVersion of the Build
		/// </summary>
		String BuildVersion { get; set; }

        /// <summary>
        /// Field for the BuildType  of the Build
        /// </summary>
        String BuildType { get; set; }

        /// <summary>
        /// Field for the BuildServer of the Build
        /// </summary>
        String BuildServer { get; set; }

        /// <summary>
        /// Field for the BuildUser of the Build
        /// </summary>
        String BuildUser { get; set; }

		#endregion

		#region Method

		#endregion
	}
}