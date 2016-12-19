using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MDM.DataModelManager.Data
{
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.Core;
    using MDM.BusinessObjects.DataModel;
    using MDM.MessageManager.Business;

    /// <summary>
    /// Specifies the helper class for data model.
    /// </summary>
    internal class DataModelHelper
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Populate OpeationResult by using data model map.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="operationResults"></param>
        /// <param name="dataModelMap"></param>
        public static void PopulateOperationResult(SqlDataReader reader, DataModelOperationResultCollection operationResults, Dictionary<Int32, IDataModelObject> dataModelMap)
        {
            while (reader.Read())
            {
                Boolean isError = false;
                String errorCode = String.Empty;
                Int32 mappingId = 0;
                Int32 mappingReferenceId = 0;
                IDataModelObject dataModelObject = null;

                ReadReaderData(reader, ref isError, ref errorCode, ref mappingId, ref mappingReferenceId);

                if (dataModelMap.ContainsKey(mappingReferenceId))
                    dataModelObject = dataModelMap[mappingReferenceId];


                if (dataModelObject != null)
                {
                    //Get data model operation result
                    DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(dataModelObject.ReferenceId);

                    //Updates operation result
                    UpdateOperationResult(operationResult, mappingId, isError, errorCode);
                }
            }

            operationResults.RefreshOperationResultStatus();
        }

        /// <summary>
        /// Populates operation result for data model object
        /// </summary>
        /// <param name="reader">Indicates sql data reader from which needs to read data</param>
        /// <param name="operationResults">Indicates collection of data model operation result</param>
        /// <param name="dataModelObjects">Indicates collection of data model objects</param>
        public static void PopulateOperationResult(SqlDataReader reader, DataModelOperationResultCollection operationResults, IDataModelObjectCollection dataModelObjects)
        {
            while (reader.Read())
            {
                Boolean isError = false;
                String errorCode = String.Empty;
                Int32 mappingId = 0;
                Int32 mappingReferenceId = 0;
                IDataModelObject dataModelObject = null;

                ReadReaderData(reader, ref isError, ref errorCode, ref mappingId, ref mappingReferenceId);

                foreach (IDataModelObject iDataModelObject in dataModelObjects)
                {
                    if (iDataModelObject.Id == mappingReferenceId)
                    {
                        dataModelObject = iDataModelObject;
                        break;
                    }
                }

                if (dataModelObject != null)
                {
                    //Get data model operation result
                    DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(dataModelObject.ReferenceId);

                    //Updates operation result
                    UpdateOperationResult(operationResult, mappingId, isError, errorCode);
                }
            }

            operationResults.RefreshOperationResultStatus();
        }

        /// <summary>
        /// Populates operation result based on reader
        /// </summary>
        /// <param name="reader">Indicates sql data reader from which needs to read data</param>
        /// <param name="operationResult">Indicates operation result</param>
        /// <returns></returns>
        public static void PopulateOperationResult(SqlDataReader reader, OperationResult operationResult)
        {
            while (reader.Read())
            {
                Int32 mappingId = 0;
                Boolean isError = false;
                String errorCode = String.Empty;
                Int32 mappingReferenceId = 0;

                ReadReaderData(reader, ref isError, ref errorCode, ref mappingId, ref mappingReferenceId);

                //Updates operation result
                UpdateOperationResult(operationResult, mappingId, isError, errorCode);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Reads data from reader
        /// </summary>
        /// <param name="reader">Indicates sql data reader from which needs to read data</param>
        /// <param name="isError">Indicates true if error, otherwise false</param>
        /// <param name="errorCode">Indicates error code</param>
        /// <param name="mappingId">Indicates identifier of mapping</param>
        /// <param name="mappingReferenceId">Indicates reference identifier of mapping</param>
        private static void ReadReaderData(SqlDataReader reader, ref Boolean isError, ref String errorCode, ref Int32 mappingId, ref Int32 mappingReferenceId)
        {
            if (reader["Id"] != null)
            {
                mappingId = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
            }

            if (reader["IsError"] != null)
            {
                isError = ValueTypeHelper.BooleanTryParse(reader["IsError"].ToString(), false);
            }

            if (reader["ErrorCode"] != null)
            {
                errorCode = reader["ErrorCode"].ToString();
            }

            if (reader["ReferenceId"] != null)
            {
                mappingReferenceId = ValueTypeHelper.ConvertToInt32(reader["ReferenceId"].ToString());
            }
        }

        /// <summary>
        /// Updates operation result
        /// </summary>
        /// <param name="operationResult">Indicates operation result</param>
        /// <param name="mappingId">Indicates identifier of mapping</param>
        /// <param name="isError">Indicates true if error, otherwise false</param>
        /// <param name="errorCode">Indicates error code</param>
        private static void UpdateOperationResult(OperationResult operationResult, Int32 mappingId, Boolean isError, String errorCode)
        {
            if (operationResult != null)
            {
                if (isError & !String.IsNullOrEmpty(errorCode))
                {
                    operationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    operationResult.Id = mappingId;
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    operationResult.ReturnValues.Add(mappingId);
                }
            }
        }

        #endregion

        #endregion
    }
}
