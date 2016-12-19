using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.Core;

namespace MDM.BusinessObjects.Interfaces.Exports
{

    using MDM.Interfaces.Exports;

    /// <summary>
    /// Export Data Formatter
    /// </summary>
    public interface IExportDataFormatter:IDataFormatter
    {

        #region Properties

        /// <summary>
        /// Property specifying data formatter export Type
        /// </summary>t
        ExportFormatterExportType ExportType { get; set; }
         
        /// <summary>
        /// Formatter  Display Name
        /// </summary>
        String DisplayName { get; set; }

        #endregion Properties
    }
}
