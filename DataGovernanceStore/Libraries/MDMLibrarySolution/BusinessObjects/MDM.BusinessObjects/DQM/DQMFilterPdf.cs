using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// Specifies DQM Filter for PDF export
    /// </summary>
    public class DqmFilterPdf
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DqmFilterPdf()
        {
            CatalogData = String.Empty;
            EntityTypeData = new List<String>();
            QualityData = new List<String>();
            CategoryData = new List<String>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property with catalog data
        /// </summary>
        public String CatalogData { get; set; }

        /// <summary>
        /// Property with entity types data
        /// </summary>
        public List<String> EntityTypeData { get; set; }

        /// <summary>
        /// Property with entity types data
        /// </summary>
        public List<String> QualityData { get; set; }

        /// <summary>
        /// Property with categories data
        /// </summary>
        public List<String> CategoryData { get; set; }

        /// <summary>
        /// Property denoting quality data Date time
        /// </summary>
        public DateTime? QualityDate { get; set; }

        /// <summary>
        /// Property denoting start trends date Date time
        /// </summary>
        public DateTime? StartTrendsDate { get; set; }

        /// <summary>
        /// Property denoting end trends date Date time
        /// </summary>
        public DateTime? EndTrendsDate { get; set; }

        #endregion
    }
}
