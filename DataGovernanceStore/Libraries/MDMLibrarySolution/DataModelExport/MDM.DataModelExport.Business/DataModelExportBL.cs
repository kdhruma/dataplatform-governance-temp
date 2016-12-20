using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.BusinessObjects.Exports;
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.Core.Exceptions;
    using MDM.Utility;
    using Bo = MDM.BusinessObjects;

    /// <summary>
    /// Specifies operations for data model export.
    /// </summary>
    public class DataModelExportBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        private Boolean _isJobServiceRequest = false;

        /// <summary>
        /// 
        /// </summary>
        private const String DEFAULT_TEMPLATE_NAME = "Default DataModel Template";

        /// <summary>
        ///
        /// </summary>
        private Bo.SecurityPrincipal _securityPrincipal;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor.
        /// </summary>
        public DataModelExportBL()
        {
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Export the Data Model as an excel file
        /// </summary>
        /// <param name="dataModelExportContext">Indicates export context based on which needs to export data model</param>
        /// <param name="callerContext">Callercontext to denote application and module information</param>
        /// <returns>Returns Excel File</returns>
        public Bo.File ExportDataModelAsExcel(DataModelExportContext dataModelExportContext, Bo.CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("DataModelExportBL.ExportDataModelAsExcel", MDMTraceSource.DataModelExport, false);
            }

            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            Bo.File dataModelFile = null;

            try
            {
                ValidateCallerContext(callerContext);

                // Prepare temporary file based on template 
                String fileName = PrepareTemporaryFileBasedOnTemplate(callerContext);

                // Generates data model as an Excel file
                GenerateDataModelAsExcelFile(fileName, dataModelExportContext, callerContext);

                // Converts the generated file as an object
                dataModelFile = ConvertAsFileObject(fileName);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time for data model export",
                        overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.DataModelExport);
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("DataModelExportBL.ExportDataModelAsExcel", MDMTraceSource.DataModelExport);
                }
            }
            return dataModelFile;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private void ValidateCallerContext(Bo.CallerContext callerContext)
        {
            if (callerContext == null)
            {
                String errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "DataModelExport.DataModelExportBL", String.Empty, "Get");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private String PrepareTemporaryFileBasedOnTemplate(Bo.CallerContext callerContext)
        {
            var durationHelper = new DurationHelper(DateTime.Now);
            String tempFileLocation = DataModelHelper.GetTempFileLocation(_isJobServiceRequest);

            String fileName = String.Format("{0}\\{1}", tempFileLocation, Guid.NewGuid());

            if (String.IsNullOrWhiteSpace(fileName))
            {
                String errorMessage = "File name cannot be null.";
                throw new MDMOperationException(String.Empty, errorMessage, "DataModelExport.DataModelExportBL", String.Empty, "Get");
            }

            String templateFileName = DataModelHelper.GetTemplateFileName(DEFAULT_TEMPLATE_NAME, tempFileLocation, callerContext);

            if (String.IsNullOrWhiteSpace(templateFileName))
            {
                String errorMessage = "TemplateFileName cannot be null.";
                throw new MDMOperationException("", errorMessage, "DataModelExport.DataModelExportBL", String.Empty, "Get");
            }

            File.Copy(templateFileName, fileName, true);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Prepared temporary file: {1} based on template",
                    durationHelper.GetDurationInMilliseconds(DateTime.Now), fileName), MDMTraceSource.DataModelExport);
            }

            return fileName;
        }

        /// <summary>
        /// Generate the excel file with the data model data
        /// </summary>
        /// <param name="tempFileName">Indicates the template file name</param>
        /// <param name="dataModelExportContext">Indicates the data model export context specifying details about the kind of data to be exported.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and module which called this operation.</param>
        private void GenerateDataModelAsExcelFile(String tempFileName, DataModelExportContext dataModelExportContext, Bo.CallerContext callerContext)
        {
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(tempFileName, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                Collection<DataModelFileBuilderBase> dataModelBuilderCollection = new Collection<DataModelFileBuilderBase>();

                if (workbookPart != null)
                {
                    dataModelBuilderCollection = GetDataModelBuilders(dataModelExportContext);
                    Collection<String> sheetNames = new Collection<String>();
                    String sheetName = String.Empty;

                    foreach (DataModelFileBuilderBase dataModelBuilder in dataModelBuilderCollection)
                    {
                        sheetName = dataModelBuilder.PopulateDataModelInFile(dataModelExportContext, workbookPart, callerContext);

                        if (!String.IsNullOrEmpty(sheetName))
                        {
                            sheetNames.Add(sheetName);
                        }
                    }

                    #region Hide the sheets based on selection of sheet name

                    if (sheetNames != null && sheetNames.Count > 0)
                    {
                        //Added metadata and configuration column to the sheetName to be exported as they should be available in the exported file.
                        sheetNames.Add(DataModelDictionary.ObjectsDictionary[ObjectType.DataModelMetadata].ToString());
                        sheetNames.Add(DataModelDictionary.ObjectsDictionary[ObjectType.Configuration].ToString());

                        foreach (Sheet sheet in workbookPart.Workbook.Descendants<Sheet>())
                        {
                            if (sheet != null)
                            {
                                Worksheet worksheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;

                                if (!sheetNames.Contains(sheet.Name))
                                {
                                    sheet.State = SheetStateValues.VeryHidden;
                                }
                                else
                                {
                                    sheet.State = SheetStateValues.Visible;
                                }
                            }
                        }

                        workbookPart.Workbook.Save();
                    }

                    #endregion
                }

                document.Close();
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Generated file {1} with all data models",
                    durationHelper.GetDurationInMilliseconds(DateTime.Now), tempFileName), MDMTraceSource.DataModelExport);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Bo.File ConvertAsFileObject(String fileName)
        {
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            Bo.File file = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                DataModelHelper.WriteFileToTargetStream(fileName, outStream);

                //Deleting temporary file
                File.Delete(fileName);
                file = DataModelHelper.GetFileFromStream(outStream, "Excel");
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Converted the data model file {1} as object",
                    durationHelper.GetDurationInMilliseconds(DateTime.Now), fileName), MDMTraceSource.DataModelExport);
            }

            return file;
        }

        /// <summary>
        /// Gets data model builder based on data model export context
        /// </summary>
        /// <returns>Returns collection of data model file builder</returns>
        private Collection<DataModelFileBuilderBase> GetDataModelBuilders(DataModelExportContext dataModelExportContext)
        {
            Collection<DataModelFileBuilderBase> dataModelBuilderCollection = new Collection<DataModelFileBuilderBase>();

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.Organization) || dataModelExportContext.SheetNames.Contains(DataModelSheet.Container))
            {
                dataModelBuilderCollection.Add(new OrganizationDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.Hierarchy))
            {
                dataModelBuilderCollection.Add(new HierarchyDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.Container))
            {
                dataModelBuilderCollection.Add(new ContainerDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.ContainerLocale) && dataModelExportContext.Locales.Count > 0)
            {
                dataModelBuilderCollection.Add(new ContainerLocaleMapFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.EntityType))
            {
                dataModelBuilderCollection.Add(new EntityTypeDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.RelationshipType))
            {
                dataModelBuilderCollection.Add(new RelationshipTypeDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.Attribute))
            {
                dataModelBuilderCollection.Add(new AttributeModelDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.AttributeLocale) && dataModelExportContext.Locales.Count > 0)
            {
                dataModelBuilderCollection.Add(new AttributeModelLocaleMapFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.Category))
            {
                dataModelBuilderCollection.Add(new CategoryDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.CategoryLocale) && dataModelExportContext.Locales.Count > 0)
            {
                dataModelBuilderCollection.Add(new CategoryLocaleMapFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.ContainerEntityTypeMapping))
            {
                dataModelBuilderCollection.Add(new ContainerEntityTypeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.EntityTypeAttributeMapping))
            {
                dataModelBuilderCollection.Add(new EntityTypeAttributeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.ContainerEntityTypeAttributeMapping))
            {
                dataModelBuilderCollection.Add(new ContainerEntityTypeAttributeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.CategoryAttributeMapping))
            {
                dataModelBuilderCollection.Add(new CategoryAttributeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.RelationshipTypeEntityTypeMapping))
            {
                dataModelBuilderCollection.Add(new RelationshipTypeEntityTypeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.RelationshipTypeEntityTypeCardinality))
            {
                dataModelBuilderCollection.Add(new RelationshipTypeEntityTypeCardinalityFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.ContainerRelationshipTypeEntityTypeMapping))
            {
                dataModelBuilderCollection.Add(new ContainerRelationshipTypeEntityTypeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.ContainerRelationshipTypeEntityTypeCardinality))
            {
                dataModelBuilderCollection.Add(new ContainerRelationshipTypeEntityTypeCardinalityFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.RelationshipTypeAttributeMapping))
            {
                dataModelBuilderCollection.Add(new RelationshipTypeAttributeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.ContainerRelationshipTypeAttributeMapping))
            {
                dataModelBuilderCollection.Add(new ContainerRelationshipTypeAttributeMappingFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.SecurityRole))
            {
                dataModelBuilderCollection.Add(new SecurityRoleDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.SecurityUser))
            {
                dataModelBuilderCollection.Add(new SecurityUserDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.LookupModel))
            {
                dataModelBuilderCollection.Add(new LookupModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.EntityVariantDefinition))
            {
                dataModelBuilderCollection.Add(new EntityVariantDefinitionDataModelFileBuilder());
            }

            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.EntityVariantDefinitionMapping))
            {
                dataModelBuilderCollection.Add(new EntityVariantDefinitionMappingFileBuilder());
            }

            return dataModelBuilderCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        private Bo.SecurityPrincipal SecurityPrincipal
        {
            get
            {
                if (_securityPrincipal == null)
                    _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                return _securityPrincipal;
            }
        }

        #endregion

        #endregion
    }
}