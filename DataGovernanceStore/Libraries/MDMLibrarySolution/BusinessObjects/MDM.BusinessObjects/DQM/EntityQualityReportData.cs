using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for Entity data quality report
    /// </summary>
    [DataContract]
    public class EntityQualityReportData : Entity, IEntityQualityReportData
    {
        #region Properties

        /// <summary>
        /// Property denoting the list of DataQualityIndicator Values of the entity
        /// </summary>
        [DataMember]
        public DataQualityIndicatorValueCollection DataQualityIndicatorValues { get; set; }

        /// <summary>
        /// Property denoting DataQualityIndicatorValues overallscore
        /// </summary>
        [DataMember]
        public Byte? OverallScore { get; set; }

        /// <summary>
        /// Property denoting Measurement Date
        /// </summary>
        [DataMember]
        public DateTime? MeasurementDate { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj != null)
            {
                if (base.Equals(obj))
                {
                    EntityQualityReportData objectToBeCompared = obj as EntityQualityReportData;
                    if (objectToBeCompared != null)
                    {
                        return
                            this.OverallScore == objectToBeCompared.OverallScore &&
                            this.MeasurementDate == objectToBeCompared.MeasurementDate &&
                            this.DataQualityIndicatorValues.Equals(objectToBeCompared.DataQualityIndicatorValues);
                    }
                }
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
                base.GetHashCode()
                ^ this.OverallScore.GetHashCode()
                ^ this.MeasurementDate.GetHashCode()
                ^ this.DataQualityIndicatorValues.GetHashCode();
        }

        #endregion
    }
}