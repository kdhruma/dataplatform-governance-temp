using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
	using BusinessObjects;

	/// <summary>
    ///  Exposes methods or properties used for setting collection of e-mail configuration.
	/// </summary>
	public interface IMailConfigCollection : IEnumerable<MailConfig>
	{
		#region Properties
		#endregion

		#region ToXml Methods

		/// <summary>
		/// Get Xml representation of MailConfigCollection object
		/// </summary>
		/// <returns>Xml string representing the MailConfigCollection</returns>
		String ToXml();

		#endregion
	}
}
