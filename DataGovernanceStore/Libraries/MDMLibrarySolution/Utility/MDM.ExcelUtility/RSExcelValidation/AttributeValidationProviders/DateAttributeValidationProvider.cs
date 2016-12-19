using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility.RSExcelValidation.AttributeValidationProviders
{
    using Interfaces;

    /// <summary>
    /// Validator provider for Date DataTypeAttribute
    /// </summary>
    internal class DateAttributeValidationProvider : IAttributeValidationProviderBase
    {
        /// <summary>
        /// Indicated whether attributeModel contains validation restrictions and returns dataValidation object considering IAttributeModel.AttributeDataTypeName == "Date".
        /// </summary>
        /// <param name="attributeModel">The attribute model. </param>
        /// <param name="columnName">The column Name. </param>
        /// <param name="messages"> The messages. </param>
        /// <param name="dataValidation">The data Validation. </param>
        /// <returns>
        /// The boolean indicating if validation was created<see cref="DataValidation"/>.
        /// </returns>
        public bool HasAttributeValidation(IAttributeModel attributeModel, String columnName, IDictionary<String, String> messages, out DataValidation dataValidation)
        {
            Boolean result = false;
            dataValidation = new DataValidation();

            if (String.IsNullOrWhiteSpace(attributeModel.AllowableValues))
            {
                dataValidation.AddDateTimeTypeValidation(columnName, attributeModel, messages);
                result = true;
            }

            return result;
        }
    }
}
