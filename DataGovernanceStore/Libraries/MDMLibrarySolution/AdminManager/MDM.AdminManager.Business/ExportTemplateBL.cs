using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;
using System.Text;

namespace MDM.AdminManager.Business
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.AdminManager.Data;
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using System.Diagnostics;

    public class ExportTemplateBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Process Export Templates items list
        /// </summary>
        /// <param name="templates">Export templates list</param>
        /// <param name="userId">User login</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        public void Process(Collection<Template> templates, Int32 userId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AdminManager.Template.Process", false);

            if (templates == null || templates.Count < 1)
            {
                throw new MDMOperationException("111941", "Please provide template to process. No Template found to process.", "AdminManager", String.Empty, "Process");
            }

            foreach (Template template in templates)
            {
                if (String.IsNullOrWhiteSpace(template.Name) || String.IsNullOrWhiteSpace(template.LongName))
                {
                    throw new MDMOperationException("111942", "File name or Template name is not available.", "AdminManager", String.Empty, "Process");
                }

                if (template.TemplateType == TemplateType.UnKnown)
                    template.TemplateType = TemplateType.Export;
            }

            ExportTemplateDA exportTemplateDA = new ExportTemplateDA();

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                exportTemplateDA.Process(templates, userId, callerContext);
                transactionScope.Complete();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AdminManager.Template.Process");
        }

        /// <summary>
        /// Requests Export template
        /// </summary>
        /// <param name="exportTemplateId">Requested Export template Id</param>
        /// <param name="userId">User login</param>
        /// <param name="templateType">Type of Template</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        /// <returns>Requested Export Template</returns>
        public Template GetExportTemplate(Int32 exportTemplateId, CallerContext callerContext)
        {
            if (exportTemplateId < 0)
            {
                throw new MDMOperationException("111943", "Requested ProfileId cannot be less than 0.", "AdminManager", String.Empty, "GetImportTemplate");
            }

            return GetExportTemplate(exportTemplateId, String.Empty, callerContext);
        }

        /// <summary>
        /// Requests Export template
        /// </summary>
        /// <param name="exportProfileName">Requested Export Export Name</param>
        /// /// <param name="userId">User login</param>
        /// <param name="templateType">Type of Template</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        /// <returns>Requested Export Template</returns>
        public Template GetExportTemplateByName(String exportProfileName, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(exportProfileName))
            {
                throw new MDMOperationException("111944", "No Profile Name found. Please provide profile Name.", "AdminManager", String.Empty, "GetImportTemplateByName");
            }

            return GetExportTemplate(0, exportProfileName, callerContext);
        }

        #endregion

        #region Private Methods

        private Template GetExportTemplate(Int32 exportTemplateId, String exportProfileName, CallerContext callerContext)
        {

            ExportTemplateDA exportTemplateDA = new ExportTemplateDA();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AdminManager.Template.GetExportTemplate from DataBase", false);

            Template exportTemplate = exportTemplateDA.GetExportTemplate(exportTemplateId, exportProfileName, TemplateType.Export, callerContext);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AdminManager.Template.GetExportTemplate from DataBase");

            return exportTemplate;
        }

        #endregion
    }
}
