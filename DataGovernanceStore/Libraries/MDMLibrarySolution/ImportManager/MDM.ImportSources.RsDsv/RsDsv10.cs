using System;
using System.Xml;
using System.Xml.Schema;
using System.Data;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;

namespace MDM.ImportSources.RsDsv
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using System.IO;
    using MDM.Interfaces;
    using System.Text;

    /// <summary>
    /// This class implements the source data from a RS DSV File. It reads the data in to a data set and process them as per the request from the import engine.
    /// </summary>
    public class RsDsv10 : IEntityImportSourceData
    {

        #region Private Members

        private IJobResultHandler _jobResultHandler = null;

        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        private Object readerLock = new Object();

        private ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        private string _sourceFile = string.Empty;

        private string delim = String.Empty;

        private string errorMessage = string.Empty;

        private Int32 _batchSize = 100;

        private string LocaleColumnName = "", CatalogColumnName = "", NodetypeColumnName = "", CategoryPathColumnName = "", ShortNameColumnName = "", LongNameColumnName = "", ParentColumnName = "";

        private DataSet dsEntity = new DataSet();

        private AttributeMapCollection attributeCollection = new AttributeMapCollection();

        #endregion

        #region Constructors
        public RsDsv10()
        {
        }

        public RsDsv10(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                throw new ArgumentNullException("RSDsv File not Specified or Available");
            }

            _sourceFile = filepath;
        }
        #endregion

        #region IEntityImportSourceData Properties
        public IJobResultHandler JobResultHandler
        {
            get
            {
                return _jobResultHandler;
            }
            set
            {
                _jobResultHandler = value;
            }
        }
        #endregion

        #region IImportSourceData methods

        public bool Initialize(Job job, ImportProfile importProfile)
        {
            if (importProfile.InputSpecifications.ReaderSettings["ConfigDelim"] != null && importProfile.InputSpecifications.ReaderSettings["ConfigDelim"].Value != "")
            {
                delim = importProfile.InputSpecifications.ReaderSettings["ConfigDelim"].Value;
            }
            else
            {
                throw new ArgumentNullException("No delimiter Value Provided");
            }

            if (delim.Contains("\\"))
            {
                if (delim == "\\t")
                {
                    delim = "\t";
                }

                else if (delim == "\\\\")
                {
                    delim = "\\";
                }

                else if (delim == "\\v")
                {
                    delim = "\v";
                }

                else if (delim == "\\'")
                {
                    delim = "\'";
                }

                else throw new ArgumentException("Please choose a valid delimiter.");
            }

            if (importProfile.MappingSpecifications.EntityMetadataMap.ContainerMap.Mode == MappingMode.InputField)
            {
                CatalogColumnName = importProfile.MappingSpecifications.EntityMetadataMap.ContainerMap.InputFieldName;
            }

            if (importProfile.MappingSpecifications.EntityMetadataMap.EntityTypeMap.Mode == MappingMode.InputField)
            {
                NodetypeColumnName = importProfile.MappingSpecifications.EntityMetadataMap.EntityTypeMap.InputFieldName;
            }

            if (importProfile.MappingSpecifications.EntityMetadataMap.ShortNameMap.Mode == MappingMode.InputField)
            {
                ShortNameColumnName = importProfile.MappingSpecifications.EntityMetadataMap.ShortNameMap.InputFieldName;
            }

            if (importProfile.MappingSpecifications.EntityMetadataMap.LongNameMap.Mode == MappingMode.InputField)
            {
                LongNameColumnName = importProfile.MappingSpecifications.EntityMetadataMap.LongNameMap.InputFieldName;
            }

            if (importProfile.MappingSpecifications.EntityMetadataMap.SourceCategoryMap.Mode == MappingMode.InputField)
            {
                CategoryPathColumnName = importProfile.MappingSpecifications.EntityMetadataMap.SourceCategoryMap.InputFieldName;
            }

            if (importProfile.MappingSpecifications.EntityMetadataMap.HierarchyParentEntityMap.Mode == MappingMode.InputField)
            {
                ParentColumnName = importProfile.MappingSpecifications.EntityMetadataMap.HierarchyParentEntityMap.InputFieldName;
            }

            if (importProfile.MappingSpecifications.LocaleMap.Mode == MappingMode.InputField)
            {
                LocaleColumnName = importProfile.MappingSpecifications.LocaleMap.InputFieldName;
            }

            attributeCollection = importProfile.MappingSpecifications.AttributeMaps;
            
            FillData();

            return true;
        }

        #endregion

        #region Methods

        public long GetEntityDataCount()
        {
            return 0;
        }

        public long GetEntityDataSeed()
        {
            return 0;
        }

        public long GetEntityEndPoint()
        {
            return 0;
        }

        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }

        public ImportProviderBatchingType GetBatchingType()
        {
            return BatchingType;
        }

        public Int32 GetEntityDataBatchSize()
        {
            return _batchSize;
        }

        public EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return null;
        }

        public EntityCollection GetEntityDataBatch(long startPK, long endPK, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return null;
        }

        public EntityCollection GetEntityDataNextBatch(int batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            EntityCollection entityCollection = new EntityCollection();
            String containerName = String.Empty;

            if (entityProviderContext.EntityProviderContextType == EntityProviderContextType.Container)
                containerName = entityProviderContext.ContainerName;

            lock (readerLock)
            {
                DataRow[] rowcollection = null;

                if (String.IsNullOrEmpty(containerName))
                    rowcollection = dsEntity.Tables[0].Select("$Processed$ = 0");
                else
                    rowcollection = dsEntity.Tables[0].Select(String.Format("$Processed$ = 0 AND Container = {0}", containerName));

                List<DataRow> dataRowCollectionTobeProcessed = new List<DataRow>();

                Int32 BatchCounter = 1;

                foreach (DataRow dr in rowcollection)
                {
                    dataRowCollectionTobeProcessed.Add(dr);

                    dr["$Processed$"] = 1;

                    if (BatchCounter == batchSize)
                    {
                        break;
                    }

                    BatchCounter++;
                }

                entityCollection = FillEntityObjects(dataRowCollectionTobeProcessed.ToArray());
            }

            return entityCollection;
        }

        public AttributeCollection GetAttributeDataforEntities(AttributeModelType attributeType, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        public AttributeCollection GetAttributeDataforEntityList(AttributeModelType attributeType, string entityList, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        public bool UpdateErrorEntities(EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateErrorRelationships(RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateErrorAttributes(AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateErrorAttributes(AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateSuccessEntities(EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        public bool UpdateSuccessAttributes(AttributeModelType attributeType, EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Get input specification.
        /// This is mostly used on UI for profile management. It will get list of entity metadata and attributes, which can be used later for changing attribute mapping.
        /// </summary>
        /// <param name="entityCountToRead">No. of entities to read from sample.</param>
        /// <param name="callerContext">Indicates who called this method</param>
        /// <returns>DataSet representing schema available in excel.</returns>
        public DataSet GetInputSpecification(Int32 entityCountToRead, CallerContext callerContext)
        {
            DataSet iss = new DataSet();
            DataTable ist = new DataTable();
            DataSet dsDat = new DataSet();

            dsDat = ReadTextFile(_sourceFile, delim);

            if (dsDat != null && dsDat.Tables != null)
            {
                foreach (DataColumn column in dsDat.Tables[0].Columns)
                {
                    ist.Columns.Add(String.Concat("[", column.ColumnName, "]"));
                }
            }

            iss.Tables.Add(ist);

            return iss;
        }

        public long GetRelationshipDataCount()
        {
            return 0;
        }

        public long GetRelationshipDataSeed()
        {
            return 0;
        }

        public long GetRelationshipEndPoint()
        {
            return 0;
        }

        public Int32 GetRelationshipDataBatchSize()
        {
            return _batchSize;
        }

        public BusinessObjects.EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetRelationshipDataBatch(long startPK, long endPK, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public BusinessObjects.EntityCollection GetRelationshipDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public void FillData()
        {
            String filename = _sourceFile;

            dsEntity = ReadTextFile(filename, delim);

            if (dsEntity != null && dsEntity.Tables != null && dsEntity.Tables[0] != null)
            {
                DataColumn dc = new DataColumn("$Processed$");
                dc.DataType = System.Type.GetType("System.Int32");
                dsEntity.Tables[0].Columns.Add(dc);

                foreach (DataRow dr in dsEntity.Tables[0].Rows)
                {
                    dr["$Processed$"] = 0;
                }
            }
        }


        #endregion

        #region IImportSourceData methods for Extension Relationships
        /// <summary>
        /// Gets the parent extension relationship for a given entity
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public ExtensionRelationship GetParentExtensionRelationShip(Entity entity, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }
        #endregion

        #region Mapping Related Interface Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFieldName"></param>
        /// <returns></returns>
        public Attribute GetAttributeInfoFromInputFieldName(String inputFieldName)
        {
            return null;
        }

        #endregion

        #region TexttoDataSet Reader

        public static DataSet ReadTextFile(string fileName, string delimiter)
        {           
            DataSet ds = new DataSet();
            //Updated stremreader to use Default Encoding to resolve German umlaut rendering issue
            StreamReader r = new StreamReader(fileName, Encoding.Default, true);

            string[] column = r.ReadLine().Split(delimiter.ToCharArray());

            ds.Tables.Add();

            // The first row in the DSV should be column names. Here we cycle through the first row, and extract column names.
            foreach (string c in column)
            {
                bool added = false;
                while (added == false)
                {
                    string columnname = c;

                    if (!ds.Tables[0].Columns.Contains(columnname))
                    {
                        ds.Tables[0].Columns.Add(columnname);
                        added = true;
                    }
                }
            }

            string data = r.ReadToEnd();

            // Defines where the dataset row should end in the DSV (traditionally when the current line ends).
            string[] row = data.Split("\n\r".ToCharArray());

            foreach (string ro in row)
            {
                if (ro.Length != 0)
                {
                    string[] item = ro.Split(delimiter.ToCharArray());

                    ds.Tables[0].Rows.Add(item);
                }
            }

            return ds;
        }

        #endregion

        #region Private Methods

        private EntityCollection FillEntityObjects(DataRow[] rows)
        {
            EntityCollection entitycollection = new EntityCollection();

            foreach (DataRow dr in rows)
            {
                Entity entity = new Entity();

                #region Fill Entity object level properties

                if (dsEntity.Tables[0].Columns.Contains(CatalogColumnName) && dr[CatalogColumnName].ToString().Trim() != "")
                    entity.ContainerName = dr[CatalogColumnName].ToString();

                if (dsEntity.Tables[0].Columns.Contains(NodetypeColumnName) && dr[NodetypeColumnName].ToString().Trim() != "")
                    entity.EntityTypeName = dr[NodetypeColumnName].ToString();

                if (dsEntity.Tables[0].Columns.Contains(CategoryPathColumnName) && dr[CategoryPathColumnName].ToString().Trim() != "")
                {
                    entity.CategoryPath = dr[CategoryPathColumnName].ToString();
                    //entity.ParentExternalId = entity.CategoryPath; 
                }

                if (dsEntity.Tables[0].Columns.Contains(ParentColumnName) && dr[ParentColumnName].ToString().Trim() != "")
                {
                    entity.ParentExternalId = dr[ParentColumnName].ToString();//entity.CategoryPath;
                }

                if (dsEntity.Tables[0].Columns.Contains(ShortNameColumnName) && dr[ShortNameColumnName].ToString().Trim() != "")
                {
                    entity.Name = dr[ShortNameColumnName].ToString();
                    entity.ExternalId = dr[ShortNameColumnName].ToString();
                    entity.SKU = dr[ShortNameColumnName].ToString();

                }

                if (dsEntity.Tables[0].Columns.Contains(LongNameColumnName) && dr[LongNameColumnName].ToString().Trim() != "")
                    entity.LongName = dr[LongNameColumnName].ToString();
                else
                    entity.LongName = entity.Name;

                #endregion

                #region Fill Entity Attribute Objects

                foreach (AttributeMap attrMap in attributeCollection)
                {
                    bool hasValue = (dsEntity.Tables[0].Columns.Contains(attrMap.AttributeSource.Name) && dr[attrMap.AttributeSource.Name].ToString().Trim() != "");

                    if (attrMap.AttributeSource.IsMandatory == true && !hasValue)
                        throw new ArgumentNullException("Mandatory Attribute not provided.");

                    if (hasValue)
                    {
                        MDM.BusinessObjects.Attribute attr = new Attribute();

                        attr.Name = attrMap.AttributeTarget.Name;

                        attr.LongName = attrMap.AttributeTarget.Name;

                        //attr.Locale = DefaultLocale;

                        attr.AttributeParentName = attrMap.AttributeTarget.ParentName;

                        attr.AttributeModelType = attrMap.AttributeTarget.ModelType;

                        attr.Locale = attrMap.AttributeTarget.Locale;

                        attr.Action = ObjectAction.Update;

                        Value val = new Value();

                        val.AttrVal = dr[attrMap.AttributeSource.Name].ToString();

                        val.Action = ObjectAction.Update;
                        //Updating value's locale same as attribute's one; it was taking en_WW by default
                        val.Locale = attr.Locale;

                        attr.SetValue((IValue)val);

                        entity.Attributes.Add(attr);
                    }
                }                
                #endregion

                entitycollection.Add(entity);

            }

            return entitycollection;
        }
        #endregion
    }
}
