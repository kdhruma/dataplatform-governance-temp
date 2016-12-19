using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DataQualityClassStatistics
    /// </summary>
    [DataContract]
    public class DataQualityClassStatistics : IDataQualityClassStatistics
    {
        #region Fields
        
        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public DataQualityClassStatistics()
            :base()
        {
        }

        /// <summary>
        /// Constructor with Data Quality Class Id as input parameter
        /// </summary>
        /// <param name="dataQualityClassId">Indicates the Identity of Data Quality Class</param>        
        public DataQualityClassStatistics(Int16 dataQualityClassId)
            : base()
        {
            this.DataQualityClassId = dataQualityClassId;            
        }

        /// <summary>
        /// Constructor with Data Quality Class Id and Entities Count as input parameteres
        /// </summary>
        /// <param name="dataQualityClassId">Indicates the Identity of Data Quality Class</param>        
        /// <param name="entitiesCount">Indicates Entities Count</param>        
        public DataQualityClassStatistics(Int16 dataQualityClassId, Int64? entitiesCount)
            : this(dataQualityClassId)
        {            
            this.EntitiesCount = entitiesCount;            
        }

        /// <summary>
        /// Constructor with Data Quality Class Id, Entities Count and Aggregations as input parameteres
        /// </summary>
        /// <param name="dataQualityClassId">Indicates the Identity of Data Quality Class</param>        
        /// <param name="entitiesCount">Indicates Entities Count</param>        
        /// <param name="aggregationsCount">Indicates Aggregations Count</param>        
        public DataQualityClassStatistics(Int16 dataQualityClassId, Int64? entitiesCount, Int64 aggregationsCount)
            : this(dataQualityClassId, entitiesCount)
        {                        
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the Data quality class Id of DataQualityClassStatistics
        /// </summary>
        [DataMember]
        public Int16 DataQualityClassId
        {
            get; set;
        }

        /// <summary>
        /// Property for the Count of entities of DataQualityClassStatistics
        /// </summary>
        [DataMember]
        public Int64? EntitiesCount
        {
            get; set;
        }       

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
                DataQualityClassStatistics objectToBeCompared = obj as DataQualityClassStatistics;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    return
                        this.DataQualityClassId == objectToBeCompared.DataQualityClassId &&
                        this.EntitiesCount == objectToBeCompared.EntitiesCount;
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
                this.DataQualityClassId.GetHashCode()
                ^ this.EntitiesCount.GetHashCode();
        }

        #endregion       

    }
}