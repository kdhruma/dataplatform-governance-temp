using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility.RSExcelValidation
{
    using Interfaces;

    /// <summary>
    /// The base attribute data validation generation contract
    /// </summary>
    public interface IAttributeValidationProviderBase
    {
        /// <summary>
        /// Indicated whether attributeModel contains validation restrictions and returns dataValidation object.
        /// </summary>
        /// <param name="attributeModel">The attribute model. </param>
        /// <param name="columnName">The column Name. </param>
        /// <param name="messages">The messages.</param>
        /// <param name="dataValidation">The resulting object </param>
        /// <returns>
        /// The boolean indicating if validation was created<see cref="DataValidation"/>.
        /// </returns>
        bool HasAttributeValidation(IAttributeModel attributeModel, String columnName, IDictionary<String, String> messages,  out DataValidation dataValidation);
    }
}
