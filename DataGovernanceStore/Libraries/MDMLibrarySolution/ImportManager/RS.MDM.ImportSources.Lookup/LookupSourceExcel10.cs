using System;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace MDM.ImportSources.Lookup
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Utility;
    using System.Data;
    using System.Collections;
    using MDM.ExceptionManager;
    using System.Diagnostics;

    /// <summary>
    /// This class implements the source data from a Lookup Data Xml source. It has one forward only reader.
    /// </summary>
    public class LookupSourceExcel10 : BaseLookupSource, ILookupImportSourceData
    {
        #region Fields

        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        private Object readerLock = new Object();

        LookupCollection lookupTables = null;

        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        string _sourceFile = string.Empty;

        string _schemaFilePath = String.Empty;

        string errorMessage = string.Empty;

        Int32 currentLookupIndex = 0;

        List<Lookup> lookupList = new List<Lookup>();

        String UniqueKey = String.Empty;

        String LookupTableName = String.Empty;

        String MetadataSheetName = "MetaData";

        String MainSheetName = "LookupData";

        DataSet LookupData = new DataSet();

        #endregion Fields

        #region Constructors

        public LookupSourceExcel10(string filePath)
        {
            _sourceFile = filePath;
        }

        #endregion Constructors

        #region Public Properties
        public new ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }
        #endregion

        #region IImportSourceData Methods For Entity

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="lookupImportProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, LookupImportProfile lookupImportProfile)
        {
            if (string.IsNullOrEmpty(_sourceFile))
            {
                throw new ArgumentNullException("Lookup Excel file is not available");
            }

            // If validation is required...
            ExecutionStep coreStep = null;
            foreach (ExecutionStep executionStep in lookupImportProfile.ExecutionSteps)
            {
                if (executionStep.StepType == ExecutionStepType.Core)
                {
                    coreStep = executionStep;
                }
            }

            if (System.IO.File.Exists(_sourceFile))
            {
                readMetadata();
                readFile();
                validateUniqueKey();
            }
            else
            {
                throw new ArgumentException(String.Format("Lookup XML file is not available in the specified location {0}", _sourceFile));
            }
            return true;
        }

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        /// <returns></returns>
        public new ImportProviderBatchingType GetBatchingType()
        {
            return BatchingType;
        }

        public new Int64 GetLookupTableCount(MDMCenterApplication application, MDMCenterModules module)
        {
            Int32 count = 0;

            this.GetAllLookupTables(application, module);

            if (lookupTables != null)
            {
                count = lookupTables.Count;
            }
            return count;
        }
        /// <summary>
        /// Gets all lookup table objects
        /// </summary>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public new BusinessObjects.LookupCollection GetAllLookupTables(MDMCenterApplication application, MDMCenterModules module)
        {
            if (lookupTables == null)
            {
                lock (readerLock)
                {
                    if (lookupTables == null)
                    {
                        lookupTables = createLookupCollection();
                        lookupList = lookupTables.ToList();
                    }
                }
            }
            return lookupTables;
        }

        public new BusinessObjects.Lookup GetSingleLookupTable(String lookupTableName, MDMCenterApplication application, MDMCenterModules module)
        {
            Lookup lookup = null;

            this.GetAllLookupTables(application, module);

            if (lookupTables != null)
            {
                lookup = lookupTables.FirstOrDefault(lk => lk.Name.Equals(lookupTableName));
            }

            return lookup;
        }

        public new BusinessObjects.Lookup GetNextLookupTable(MDMCenterApplication application, MDMCenterModules module)
        {
            Lookup lookup = null;

            this.GetAllLookupTables(application, module);

            if (lookupList != null)
            {
                if (lookupList.Count > currentLookupIndex)
                {
                    lookup = lookupList[currentLookupIndex];
                    currentLookupIndex++;
                }
            }

            return lookup;
        }

        #endregion

        #region Lookup Object Method
        private BusinessObjects.LookupCollection createLookupCollection()
        {
            LookupCollection lookups = new LookupCollection();
            Lookup lookup = new Lookup();
            lookup.ExtendedProperties.Add("UniqueKey",UniqueKey);
            lookup.Name = LookupTableName;
            lookup.LongName = LookupTableName;

            foreach (DataRow dr in LookupData.Tables[0].Rows)
            {
                Row row = new Row();
                row.Action = ObjectAction.Update;
                foreach (DataColumn column in dr.Table.Columns)
                {
                    if (column.ColumnName != "$Processed$" && column.ColumnName != "RowNum")
                    {
                        Cell cell = new Cell();
                        cell.ColumnName = column.ColumnName;
                        cell.Value = dr[column.ColumnName].ToString();
                        row.Cells.Add(cell);
                    }
                }
                lookup.Rows.Add(row);
            }

            lookups.Add(lookup);
            return lookups;
        }
        #endregion

        #region Excel Method

        private void readMetadata()
        {
            try
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Reading file starting....");

                string validSheetName = ExternalFileReader.ValidateExcelWorkSheet(_sourceFile, MetadataSheetName);

                if (MetadataSheetName.ToLower() == validSheetName.Replace("$", "").ToLower())
                {
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Reading rows from excel file");

                    DataSet MetadataLookupData = ExternalFileReader.ReadExternalFile("excel", _sourceFile, MetadataSheetName, "", false, null, null, null, 0, "ANSI");

                    if (MetadataLookupData != null && MetadataLookupData.Tables != null && MetadataLookupData.Tables.Count > 0)
                    {
                        foreach (DataRow dr in MetadataLookupData.Tables[0].Rows)
                        {
                            UniqueKey = dr["UniqueKey"].ToString();
                            LookupTableName = dr["LookupTableName"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Error: " + ex.Message);
            }
        }

        private void readFile()
        {
            try
            {
                MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Reading file starting....");

                string validSheetName = ExternalFileReader.ValidateExcelWorkSheet(_sourceFile, MainSheetName);

                if (MainSheetName.ToLower() == validSheetName.Replace("$", "").ToLower())
                {
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Reading rows from excel file");

                    LookupData = ExternalFileReader.ReadExternalFile("excel", _sourceFile, MainSheetName, "", false, null, null, null, 0, "ANSI");

                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Reading external file completed");

                    //remove blank rows
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Remove Blank row from data set started");
                    DataColumn idColumn = new DataColumn("RowNum", typeof(System.Int32));
                    LookupData.Tables[0].Columns.Add(idColumn);
                    ArrayList deleteList = new ArrayList();
                    int RowCount = 2; //Ignore the Header.
                    foreach (DataRow dr in LookupData.Tables[0].Rows)
                    {
                        //remove the blank lines that may be entered
                        bool valuesFound = false;
                        for (int i = 0; i < LookupData.Tables[0].Columns.Count; i++)
                        {
                            if ((dr[i] != null) && (dr[i].ToString().Length > 0))
                            {
                                valuesFound = true;
                                break;
                            }
                        }
                        if (valuesFound == true)
                            dr["RowNum"] = RowCount++;
                        else
                            deleteList.Add(dr);
                    }
                    foreach (DataRow item in deleteList)
                        LookupData.Tables[0].Rows.Remove(item);
                    LookupData.AcceptChanges();

                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Cleaning up blank row completed");


                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Addind Process flag started");
                    if (LookupData != null && LookupData.Tables != null && LookupData.Tables[0] != null)
                    {
                        DataColumn dc = new DataColumn("$Processed$");
                        dc.DataType = System.Type.GetType("System.Int32");
                        LookupData.Tables[0].Columns.Add(dc);

                        foreach (DataRow dr in LookupData.Tables[0].Rows)
                        {
                            dr["$Processed$"] = 0;
                        }
                    }
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Adding Process flag completed");

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler exHandler = new ExceptionHandler(ex);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Error: " + ex.Message);
            }
        }

        private void validateUniqueKey()
        {
            String[] uniqueKeyColumnArray = UniqueKey.Split(',');
            Int32 UniqueKeyCount = 0;

            if (LookupData.Tables != null && LookupData.Tables.Count > 0)
            {
                DataTable dtlookup = LookupData.Tables[0];
                foreach (DataColumn column in dtlookup.Columns)
                {
                    if (uniqueKeyColumnArray.Contains(column.ColumnName))
                    {
                        UniqueKeyCount++;
                    }
                }
            }

            if (uniqueKeyColumnArray.Length > UniqueKeyCount)
            {
                throw new Exception("Lookup excel file does not have column specified in uniqueKey");
            }
        }

        #endregion
    }
}
