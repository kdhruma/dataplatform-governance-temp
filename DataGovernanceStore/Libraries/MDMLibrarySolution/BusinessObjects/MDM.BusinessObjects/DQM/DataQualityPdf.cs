
namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// Specifies quality data for PDF export
    /// </summary>
    public class DataQualityPdf
    {
        #region Constructors
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataQualityPdf()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting DQM filter for PDF export
        /// </summary>
        public DqmFilterPdf DqmFilterPdf { get; set; }

        /// <summary>
        /// Property denoting list of Data Quality Class Statistics
        /// </summary>
        public DataQualityClassStatisticsCollection DataQualityClassStatisticsCollection { get; set; }

        /// <summary>
        /// Property denoting Trends Data
        /// </summary>
        public DataQualityIndicatorSummaryCollection TrendsData { get; set; }

        /// <summary>
        /// Property denoting list of Data Quality Classes
        /// </summary>
        public DataQualityClassCollection DataQualityClassCollection { get; set; }

        #endregion
    }
}
