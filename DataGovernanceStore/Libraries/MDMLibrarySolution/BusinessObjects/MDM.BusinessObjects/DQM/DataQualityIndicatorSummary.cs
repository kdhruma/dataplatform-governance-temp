using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DataQualityIndicatorSummary
    /// </summary>
    [DataContract]
    public class DataQualityIndicatorSummary : IDataQualityIndicatorSummary
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public DataQualityIndicatorSummary()
            :base()
        {
            DQCStatistics = new DataQualityClassStatisticsCollection();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataQualityIndicatorSummary(Int32? containerId, Int64? categoryId, Int32? entityTypeId, Int64? totalEntitiesCount, Int64? notValidatedEntitiesCount, DateTime? measurementDate)
            : base()
        {
            DQCStatistics = new DataQualityClassStatisticsCollection();
            this.ContainerId = containerId;
            this.CategoryId = categoryId;
            this.EntityTypeId = entityTypeId;
            this.TotalEntitiesCount = totalEntitiesCount;
            this.NotValidatedEntitiesCount = notValidatedEntitiesCount;
            this.MeasurementDate = measurementDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Container Id of an entity
        /// </summary>
        [DataMember]
        public Int32? ContainerId { get; set; }

        /// <summary>
        /// Property denoting the Category Id of an entity
        /// </summary>
        [DataMember]
        public Int64? CategoryId { get; set; }

        /// <summary>
        /// Property denoting the Type Id of an entity
        /// </summary>
        [DataMember]
        public Int32? EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the Total Entities Count
        /// </summary>
        [DataMember]
        public Int64? TotalEntitiesCount { get; set; }

        /// <summary>
        /// Property denoting the Not Validated Entities Count
        /// </summary>
        [DataMember]
        public Int64? NotValidatedEntitiesCount { get; set; }

        /// <summary>
        /// Property denoting Measurement Date
        /// </summary>
        [DataMember]
        public DateTime? MeasurementDate { get; set; }

        /// <summary>
        /// Property denoting the list of Data Quality Class Statistics of Summary
        /// </summary>
        [DataMember]
        public DataQualityClassStatisticsCollection DQCStatistics { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            DataQualityIndicatorSummary objectToBeCompared = obj as DataQualityIndicatorSummary;
            if (objectToBeCompared != null)
            {
                return
                    this.ContainerId == objectToBeCompared.ContainerId &&
                    this.CategoryId == objectToBeCompared.CategoryId &&
                    this.EntityTypeId == objectToBeCompared.EntityTypeId &&
                    this.TotalEntitiesCount == objectToBeCompared.TotalEntitiesCount &&
                    this.NotValidatedEntitiesCount == objectToBeCompared.NotValidatedEntitiesCount &&
                    this.MeasurementDate == objectToBeCompared.MeasurementDate &&
                    this.DQCStatistics.Equals(objectToBeCompared.DQCStatistics);
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return
                this.ContainerId.GetHashCode()
                ^ this.CategoryId.GetHashCode()
                ^ this.EntityTypeId.GetHashCode()
                ^ this.TotalEntitiesCount.GetHashCode()
                ^ this.NotValidatedEntitiesCount.GetHashCode()
                ^ this.MeasurementDate.GetHashCode()
                ^ this.DQCStatistics.GetHashCode();
        }

        #endregion
    }
}
