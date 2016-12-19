using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get lookup model and record information.
    /// </summary>
    public interface ILookup : ITable
    {
        #region Fields

        /// <summary>
        /// Indicates the lookup attribute Id
        /// </summary>
        Int32 AttributeId {get; set;}

        #endregion Fields

        #region Methods

        /// <summary>
        /// Gets the lookup model
        /// </summary>
        /// <param name="returnOnlyDisplayColumns">Indicates whether to return only display columns or not</param>
        /// <returns>Lookup Model</returns>
        String GetModel(Boolean returnOnlyDisplayColumns = false);

		/// <summary>
		/// Gets the lookup record based on the identifier
		/// </summary>
		/// <param name="id">Indicates the record identifier</param>
        /// <returns>Lookup record with the specified identifier</returns>
        IRow GetRecordById(Int32 id);

    	/// <summary>
    	/// Gets the lookup record based on the display format
    	/// </summary>
    	/// <param name="displayFormat">Indicates the display format</param>
        /// <returns>Lookup record with the specified display format</returns>
    	IRow GetRecordByDisplayFormat(String displayFormat);

		/// <summary>
		/// Gets the lookup record based on the display format and application context
		/// </summary>
        /// <param name="displayFormat">Indicates the display format</param>
        /// <param name="applicationContext">Indicates the application context</param>
        /// <returns>Lookup record with the specified display format and application context</returns>
		IRow GetRecordByDisplayFormat(String displayFormat, IApplicationContext applicationContext);

		/// <summary>
        /// Gets the lookup record based on the export format
		/// </summary>
        /// <param name="exportFormat">Indicates the export format</param>
        /// <returns>Lookup record with the specified export format</returns>
		IRow GetRecordByExportFormat(String exportFormat);

		/// <summary>
        /// Gets the lookup record based on the export format and application context
		/// </summary>
        /// <param name="exportFormat">Indicates the export format</param>
        /// <param name="applicationContext">Indicates the application context</param>
        /// <returns>Lookup record with the specified export format and application context</returns>
		IRow GetRecordByExportFormat(String exportFormat, IApplicationContext applicationContext);

        /// <summary>
        /// Gets the display value of the lookup record based on the record identifier
        /// </summary>
        /// <param name="id">Indicates the lookup record identifier </param>
        /// <returns>Display value of the lookup record with the specified record identifier</returns>
        /// <example>
        /// <code language="c#" title="Sample 1: Get the display values of lookup table records" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="GetDisplayValueLookupByAttributeNameAndParentName" />
        /// <code language="c#" title="Sample 2: Get the display values of lookup table records" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get display values lookup by attribute id and locale" />
        /// <code language="c#" title="Sample 3: Get the display values for lookup table records of given search value" source="..\MDM.APISamples\LookupManager\SearchLookupdata.cs" region="Search and GetDisplayValue of lookup by Attribute id and Application Context in English Locale" />
        /// </example>
		String GetDisplayFormatById(Int32 id);

        /// <summary>
        /// Gets the export value of the lookup record based on the record identifier
        /// </summary>
        /// <param name="id">Indicates the lookup record identifier</param>
        /// <returns>Export value of the lookup record with the specified record identifier</returns>
		String GetExportFormatById(Int32 id);

        /// <summary>
        /// Add a new row.
        /// </summary>
        /// <param name="row">Indicates the lookup row</param>
        /// <returns>Indicates the new row</returns>
        void AddNewRow(IRow row);

        #endregion Methods
    }
}
