using System;
using System.Collections.ObjectModel;
using System.Collections;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get data model operation result summary.
    /// </summary>
    public interface IDataModelOperationResultSummary
    {
        #region Properties

        /// <summary>
        /// Property denoting Data Model ObjectType; 
        /// </summary>
        String ObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Sheet Name
        /// </summary>
        String SummaryObjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting total object count
        /// </summary>
        Int32 ToatalCount
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Pending object count
        /// </summary>
        Int32 PendingCount
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Success object count
        /// </summary>
        Int32 SuccessCount
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Failed object count
        /// </summary>
        Int32 FailedCount
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting CompletedWithErrors object count
        /// </summary>
        Int32 CompletedWithErrorsCount
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting CompletedWithWarnings object count
        /// </summary>
        Int32 CompletedWithWarningsCount
        {
            get;
            set;
        }

        /// <summary>
        /// Determines overall status of Operation.
        /// </summary>
        OperationResultStatusEnum SummaryStatus 
        { 
            get; 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of DataModel operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of DataModel operation result</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
