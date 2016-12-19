using System;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Diagnostics;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.Core.Exceptions;

    public class ImportTemplateBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Process Import Templates items list
        /// </summary>
        /// <param name="templates">Import templates list</param>
        /// <param name="userId">User login</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        public void Process(Collection<Template> templates, Int32 userId, CallerContext callerContext)
        {
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
                    template.TemplateType = TemplateType.Import;
            }

            ImportTemplateDA importTemplateDA = new ImportTemplateDA();

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                Boolean success = false;
                importTemplateDA.Process(templates, userId, callerContext);

                foreach (Template template in templates)
                {
                    if (template.Id > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                        break;
                    }
                }

                if (success == true)
                    transactionScope.Complete();
            }
        }

        /// <summary>
        /// Requests import template
        /// </summary>
        /// <param name="importTemplateId">Requested import template Id</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        /// <returns>Requested Import Template</returns>
        public Template GetImportTemplate(Int32 importTemplateId, CallerContext callerContext)
        {
            if (importTemplateId < 0)
            {
                throw new MDMOperationException("111943", "Requested ProfileId cannot be less than 0.", "AdminManager", String.Empty, "GetImportTemplate");
            }

            return this.GetImportTemplate(importTemplateId, String.Empty, callerContext);
        }

        /// <summary>
        /// Requests import template
        /// </summary>
        /// <param name="importProfileName">Requested import template Name</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        /// <returns>Requested Import Template</returns>
        public Template GetImportTemplateByName(String importProfileName, CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(importProfileName))
            {
                throw new MDMOperationException("111944", "No Profile Name found. Please provide profile Name.", "AdminManager", String.Empty, "GetImportTemplateByName");
            }

            return this.GetImportTemplate(0, importProfileName, callerContext);
        }

        #endregion

        #region Private Methods

        private Template GetImportTemplate(Int32 importTemplateId, String importProfileName, CallerContext callerContext)
        {
            ImportTemplateDA importTemplateDA = new ImportTemplateDA();
            return importTemplateDA.GetImportTemplate(importTemplateId, importProfileName, TemplateType.Import, callerContext);
        }

        #endregion
    }
}