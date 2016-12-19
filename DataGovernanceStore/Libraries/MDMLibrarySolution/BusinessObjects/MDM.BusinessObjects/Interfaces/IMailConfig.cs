using System;

namespace MDM.Interfaces
{
	using Core;

	/// <summary>
    /// Exposes methods or properties to set or get e-mail configuration.
	/// </summary>
	public interface IMailConfig : IMDMObject
	{
		#region Properties

		/// <summary>
		/// Name of application which is performing action
		/// </summary>
		MDMCenterApplication Application { get; set; }

		/// <summary>
		/// Name of module which is performing action
		/// </summary>
		MDMCenterModules Module { get; set; }

		/// <summary>
		/// Property denoting the From of MailConfig
		/// </summary>
		String From
		{
			get;
			set;
		}

		/// <summary>
		/// Property denoting the Host of SMTP Server
		/// </summary>
		String Host
		{
			get;
			set;
		}

		/// <summary>
		/// Property denoting the Port SMTP Server
		/// </summary>
		Int32 Port
		{
			get;
			set;
		}

		/// <summary>
		/// Property denoting the SMTP User's password
		/// </summary>
		String Password
		{
			get;
			set;
		}

		/// <summary>
		/// Property denoting the SMTP SSL Setting
		/// </summary>
		Boolean EnableSSL
		{
			get;
			set;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get Xml representation of MailConfig object
		/// </summary>
		/// <returns>Xml representation of object</returns>
		String ToXml();

		#endregion Methods
	}
}
