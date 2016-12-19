using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.AdminManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using System.Collections.ObjectModel;
    using System.Data.SqlClient;
    using MDM.Utility;
    using Microsoft.SqlServer.Server;

    public class ExportTemplateDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Requests import template
        /// </summary>
        /// <param name="exportTemplateId">Requested import template Id</param>
        /// <param name="userId">User login</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        /// <returns>Requested Import Template</returns>
        public Template GetExportTemplate(Int32 exportTemplateId, String profileName, TemplateType templateType, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AdminManagerDA.Template.GetExportTemplate", false);

            SqlDataReader reader = null;
            Template template = null;
            try
            {
                var generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("usp_AdminManager_Template_Get_ParametersArray");

                parameters[0].Value = exportTemplateId;
                parameters[1].Value = profileName;
                parameters[2].Value = templateType.ToString();

                reader = ExecuteProcedureReader(AppConfigurationHelper.ConnectionString, parameters, "usp_AdminManager_Template_Get");

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        template = new Template();

                        if (reader["PK_Template"] != null)
                            template.Id = (Int32)reader["PK_Template"];
                        if (reader["ShortName"] != null)
                            template.Name = reader["ShortName"].ToString();
                        if (reader["LongName"] != null)
                            template.LongName = reader["LongName"].ToString();
                        if (reader["FileData"] != null)
                        {
                            if (reader["FileData"].ToString() != "")
                                template.FileData = (byte[])reader["FileData"];
                        }
                        if (reader["FileType"] != null)
                            template.FileType = reader["FileType"].ToString();
                        if (reader["FK_Locale"] != null)
                        {
                            LocaleEnum auditInfoLocale = LocaleEnum.UnKnown;
                            Enum.TryParse(reader["FK_Locale"].ToString(), out auditInfoLocale);
                            template.Locale = auditInfoLocale;
                        }
                        if (reader["TemplateType"] != null)
                        {
                            TemplateType exportTemplateType = TemplateType.UnKnown;
                            Enum.TryParse(reader["TemplateType"].ToString(), out exportTemplateType);
                            template.TemplateType = exportTemplateType;
                        }
                        if (reader["FK_Audit_Ref"] != null)
                            template.AuditRefId = (Int64)reader["FK_Audit_Ref"];
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AdminManagerDA.Template.GetExportTemplate");

            return template;
        }

        /// <summary>
        /// Process Import Templates items list
        /// </summary>
        /// <param name="templates">Import templates list</param>
        /// <param name="userId">User login</param>
        /// <param name="callerContext">Name of the Application and Module which calls this method</param>
        public void Process(Collection<Template> templates, Int32 userId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AdminManagerDA.Template.Process", false);

            SqlDataReader reader = null;

            try
            {
                var generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                var parameters = generator.GetParameters("AdminManager_Template_Process_ParametersArray");

                #region Populate table value parameters

                var preparedTemplates = new List<SqlDataRecord>();
                SqlMetaData[] templateMetadata = generator.GetTableValueMetadata("AdminManager_Template_Process_ParametersArray", parameters[0].ParameterName);

                foreach (var template in templates)
                {
                    var preparedTemplate = new SqlDataRecord(templateMetadata);
                    preparedTemplate.SetValue(0, template.Id);
                    preparedTemplate.SetValue(1, template.Name);
                    preparedTemplate.SetValue(2, template.LongName);
                    preparedTemplate.SetValue(3, template.TemplateType.ToString());
                    preparedTemplate.SetValue(4, template.FileData);
                    preparedTemplate.SetValue(5, template.FileType);
                    preparedTemplate.SetValue(6, (Int32)template.Locale);
                    preparedTemplate.SetValue(7, template.Action.ToString());

                    preparedTemplates.Add(preparedTemplate);
                }

                #endregion

                parameters[0].Value = preparedTemplates;
                parameters[1].Value = userId;
                parameters[2].Value = callerContext.Application.ToString();

                reader = ExecuteProcedureReader(AppConfigurationHelper.ConnectionString, parameters, "usp_AdminManager_Template_Process");

                if (reader != null)
                {
                    UpdateImportTemplateOperationResults(reader, templates);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AdminManagerDA.Template.Process");
        }

        #endregion

        #region Private Methods

        //TODO: Add ImportTemplateOperationResult support
        public static void UpdateImportTemplateOperationResults(SqlDataReader reader, Collection<Template> templates)
        {
            while (reader.Read())
            {
                Int32 sourceId = 0;
                Int32 resultId = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                //ShortName, LongName

                if (reader["SourceId"] != null)
                    Int32.TryParse(reader["SourceId"].ToString(), out sourceId);
                if (reader["ResultId"] != null)
                    Int32.TryParse(reader["ResultId"].ToString(), out resultId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorMessage"] != null)
                    errorMessage = reader["ErrorMessage"].ToString();


                //Get template
                var template = templates.SingleOrDefault(e => e.Id == sourceId);

                //Get the template operation result
                //ImportTemplate importTemplateOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == id);

                if (template != null)
                {
                    if (template.Action == ObjectAction.Create)
                    {
                        //Update the id with the new entityId
                        template.Id = resultId;
                    }

                    if (hasError)
                    {
                        throw new Exception("Error during UpdateImportTemplateOperationResults call: " + errorMessage);
                    }
                    /*
                    if (hasError)
                    {
                        //Add error
                        importTemplateOperationResults.AddEntityOperationResult(entityOperationResult.EntityId, String.Empty, errorMessage, OperationResultType.Error);
                    }
                    else
                    {
                        //No errors.. update status as Successful.
                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                    */
                }
            }
        }

        #endregion
    }
}
