using System;
using System.Collections.ObjectModel;

namespace MDM.LookupManager.Business
{
	using MDM.Interfaces;
	using MDM.BusinessObjects;

	/// <summary>
	/// Specifies arguments for events raised for Lookup data load
	/// </summary>
	public class LookupDataProcessEventArgs : EventArgs
	{
		#region Fields

		#endregion

		#region Properties

		/// <summary>
		/// Property denoting the lookup data
		/// </summary>
		public Lookup LookupData { get; private set; }

		/// <summary>
		/// Property denoting the Attribute Ids
		/// </summary>
		public Collection<Int32> AttributeIds { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates the object
		/// </summary>
		/// <param name="lookupData"></param>
		public LookupDataProcessEventArgs(Lookup lookupData)
		{
			if (lookupData == null)
			{
				throw new ArgumentNullException("lookupData");
			}

			LookupData = lookupData;
		}

		/// <summary>
		/// Instantiates the object
		/// </summary>
		/// <param name="lookupData"></param>
		public LookupDataProcessEventArgs(Lookup lookupData, Collection<Int32> attributeIds)
		{
			if (lookupData == null)
			{
				throw new ArgumentNullException("lookupData");
			}

			LookupData = lookupData;

			AttributeIds = attributeIds;
		}

		#endregion
	}
}
