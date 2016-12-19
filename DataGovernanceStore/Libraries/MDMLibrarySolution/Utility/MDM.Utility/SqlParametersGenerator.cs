using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Collections;
using Microsoft.SqlServer.Server;

namespace MDM.Utility
{
    using MDM.CacheManager.Business;

	/// <summary>
	/// To automate SqlParameter Creation for the system
	/// </summary>
	public sealed class SqlParametersGenerator
	{
		#region Fields

        private String sqlParametersFile = String.Empty;
        private XmlDocument sqlParameterData = null;
		
        private Object _thisLock = new Object();
		
		#endregion

		#region Constructors

        /// <summary>
        /// Initializes the instance with parameter data loaded from the file pointed bye setting file key given.
        /// </summary>
        /// <param name="settingFileKey">app setting key which points to the file containing parameter detail.</param>
        public SqlParametersGenerator(String settingFileKey)
        {
            sqlParametersFile = AppConfigurationHelper.GetSettingAbsolutePath(settingFileKey);

            ICache cache = CacheFactory.GetCache();

            if (cache != null)
            {
                //get it from cache
                sqlParameterData = cache[FileName] as XmlDocument;

                //if not found create and cache it
                if (sqlParameterData == null)
                {
                    sqlParameterData = new XmlDocument();
                    sqlParameterData.Load(FileName);
                    cache.Set(FileName, sqlParameterData, DateTime.Now.AddHours(24));
                }
            }
            else
            {
                //TODO: Try another caching mechanism here
                sqlParameterData = new XmlDocument();
                sqlParameterData.Load(FileName);
            }

        }

		#endregion

		#region Properties

        /// <summary>
        /// Gets the sql parameter file name.
        /// </summary>
		private String FileName
		{
			get
			{
                return sqlParametersFile;
			}
		}

        /// <summary>
        /// Gets the Config Xml Document.
        /// </summary>
        private XmlDocument ConfigData
        {
            get
            {
                return sqlParameterData;
            }

        }

		#endregion

		#region Methods

		/// <summary>
		/// Obtains the parameter array.
		/// </summary>
        /// <param name="paramsKey">The key used to identify the parameters</param>
		/// <returns>The parameter array containing the required parameters.</returns>
		/// <remarks>The return value will be null if the parameters are not declared in config file.</remarks>
		public SqlParameter[] GetParameters(String paramsKey)
		{
			SqlParameter[] parameters = null;

            //check if the parameter collection is present in the config file
            XmlNode sqlParameterCache = GetSqlParameterItemNode(paramsKey);

            if (sqlParameterCache == null)
            {
                throw new ArgumentException("There were no parameters present for this key", paramsKey); 
            }

            parameters = GenerateParameters(sqlParameterCache);

            return parameters;
		}

        /// <summary>
        /// Obtains table value parameter metadata
        /// </summary>
        /// <param name="paramsKey">The key used to identify the parameters</param>
        /// <param name="parameterName">Name of the parameter for which metadata is required</param>
        /// <returns>The parameter metadata</returns>
        public SqlMetaData[] GetTableValueMetadata(String paramsKey, String parameterName)
        {
            SqlMetaData[] tableValueMetadata = null;

            String cacheKeyName = String.Format("RS_TableValueMetaData_{0}_{1}", paramsKey, parameterName);

            ICache cacheManager = CacheFactory.GetCache();

            tableValueMetadata = cacheManager.Get<SqlMetaData[]>(cacheKeyName);

            if (tableValueMetadata == null)
            {
                lock (_thisLock)
                {
                    if (tableValueMetadata == null)
                    {
                        //Get XML metadata
                        String xpathString = String.Empty;
                        XmlNode sqlMetadataRoot = null;

                        xpathString = String.Format("/SqlParameterItems/SqlParameterItem[@id='{0}']/SqlParameters/SqlParameter[@name='{1}']/SqlMetadata", paramsKey, parameterName);

                        //Check if configuration data is present
                        if (ConfigData == null)
                            throw new ApplicationException(String.Format("Sql Parameters Configuration Cache Data for {0} was not found", paramsKey));

                        //select single parameter cache node.
                        sqlMetadataRoot = ConfigData.SelectSingleNode(xpathString);

                        if (sqlMetadataRoot == null || !sqlMetadataRoot.HasChildNodes)
                            throw new ApplicationException(String.Format("Sql Metadata for parameter {0} was not found", parameterName));

                        //Get the column and metadata object
                        Int32 totalColumns = sqlMetadataRoot.ChildNodes.Count;

                        tableValueMetadata = new SqlMetaData[totalColumns];

                        for (Int32 counter = 0; counter < totalColumns; counter++)
                        {
                            XmlAttributeCollection columnElement = sqlMetadataRoot.ChildNodes[counter].Attributes;

                            //stop processing if required attributes are not present
                            if (columnElement["type"] == null)
                                throw new ArgumentException("Metadata config type is not specified");
                            else if (columnElement["name"] == null)
                                throw new ArgumentException("Metadata config name is not specified");

                            String columnType = columnElement["type"].Value;
                            String columnName = columnElement["name"].Value;
                            Byte decimalPrecision = 0;
                            Byte decimalScale = 0;
                            String columnSize = String.Empty;

                            SqlMetaData columnMetadata = null;

                            switch (columnType)
                            {
                                case "Bit":
                                case "Int":
                                case "BigInt":
                                case "SmallInt":
                                case "TinyInt":
                                case "Text":
                                case "DateTime":
                                case "SmallDateTime":
                                case "UniqueIdentifier":
                                case "Structured":
                                case "Xml":
                                case "Float":
                                    columnMetadata = new SqlMetaData(columnName, GetSqlDBType(columnType));
                                    break;
                                case "Binary":
                                case "Image":
                                case "Char":
                                case "NChar":
                                case "VarChar":
                                case "NVarChar":
                                    if (columnElement["size"] == null)
                                        throw new ArgumentException("Sql metadata column size is not specified");

                                    columnSize = columnElement["size"].Value;
                                    columnMetadata = new SqlMetaData(columnName, GetSqlDBType(columnType), Int32.Parse(columnSize));
                                    break;
                                case "VarBinary":
                                    if (columnElement["size"] == null)
                                        throw new ArgumentException("Sql metadata column size is not specified");
                                    Int32 columnSizeInt = Int32.Parse(columnElement["size"].Value);
                                    if (columnSizeInt > 8000 && columnSizeInt != Int32.MaxValue)
                                    {
                                        throw new ArgumentException("Sql metadata column size for VarBinary must be below or equal 8000 or equal to 2147483647");
                                    }
                                    columnMetadata = new SqlMetaData(columnName, SqlDbType.VarBinary, (columnSizeInt == Int32.MaxValue) ? SqlMetaData.Max : columnSizeInt);
                                    break;
                                case "Decimal":
                                    if (columnElement["precision"] == null)
                                        throw new ArgumentException("Sql metadata column Precision is not specified");
                                    if (columnElement["scale"] == null)
                                        throw new ArgumentException("Sql metadata column Scale is not specified");

                                    if (!Byte.TryParse(columnElement["precision"].Value, out decimalPrecision))
                                    {
                                        throw new ArgumentException("Precision value is not in proper format");
                                    }

                                    if (!Byte.TryParse(columnElement["scale"].Value, out decimalScale))
                                    {
                                        throw new ArgumentException("Scale value is not in proper format");
                                    }

                                    /*
                                     * Precision is the number of digits in a number. Scale is the number of digits to the right of the decimal point in a number. 
                                     * For example, the number 123.45 has a precision of 5 and a scale of 2.
                                     */

                                    columnMetadata = new SqlMetaData(columnName, SqlDbType.Decimal, decimalPrecision, decimalScale);

                                    break;

                                case "DateTime2":
                                    {
                                        decimalScale = 7; // Default value
                                        if (columnElement["scale"] != null)
                                        {
                                            String scaleStr = columnElement["scale"].Value;
                                            if (!String.IsNullOrWhiteSpace(scaleStr))
                                            {
                                                if (!Byte.TryParse(scaleStr, out decimalScale))
                                                {
                                                    throw new ArgumentException("Scale value is not in proper format");
                                                }
                                                if (decimalScale > 7)
                                                {
                                                    throw new ArgumentException("Scale value is outside the bounds. Should be 0 to 7");
                                                }
                                            }
                                        }

                                        /*
                         * Scale is the number of digits in seconds fractional part (YYYY-MM-DD hh:mm:ss.fractional_seconds).
                         * For example, the date '1979-12-23 00:00:00.1234' has a scale of 4.
                         * Storage size details: 6 bytes for precisions less than 3; 7 bytes for precisions 3 and 4. All other precisions require 8 bytes. 
                         */

                                        columnMetadata = new SqlMetaData(columnName, SqlDbType.DateTime2, 0, decimalScale);

                                        break;
                                    }

                                default:
                                    throw new ArgumentException("The data type specified in the table value metadata config is not supported", columnType);
                            }

                            tableValueMetadata[counter] = columnMetadata;
                        }

                        //Set Cache
                        cacheManager.Set<SqlMetaData[]>(cacheKeyName, tableValueMetadata, DateTime.Now.AddHours(48.0));
                    }
                }
            }

            return tableValueMetadata;
        }

        private XmlNode GetSqlParameterItemNode(String paramsKey)
        {
            String xpathString = String.Empty;
            XmlNode sqlParameterItemRoot = null;

            xpathString = "/SqlParameterItems/SqlParameterItem[@id='" + paramsKey + "']";
            //Check if configuration data is present
            if (ConfigData == null)
            {
                throw new ApplicationException(String.Format("Sql Parameters Configuration Cache Data for {0} was not found", paramsKey));
            }

            //select single parameter cache node.
            sqlParameterItemRoot = ConfigData.SelectSingleNode(xpathString);

            return sqlParameterItemRoot;
        }

		private SqlParameter[] GenerateParameters(XmlNode sqlParameterItemRoot)
		{
			SqlParameter[] parameters = null;

			//cause there is only one node for SqlParameters
			XmlNode sqlParametersRoot = sqlParameterItemRoot.ChildNodes[0]; 

			//add the parameter from the child nodes.
			if (sqlParametersRoot.HasChildNodes)
			{
				Int32 totalParameters = sqlParametersRoot.ChildNodes.Count;

				parameters = new SqlParameter[totalParameters];

				for (Int32 counter = 0; counter < totalParameters; counter++)
				{
                    XmlAttributeCollection parameterElement = sqlParametersRoot.ChildNodes[counter].Attributes;

                    //stop processing if required attributes are not present
                    if (parameterElement["type"] == null)
                    {
                        throw new ArgumentException("Parameter config type is not specified");
                    }
                    else if (parameterElement["name"] == null)
                    {
                        throw new ArgumentException("Parameter config name is not specified");
                    }
                    else if (parameterElement["direction"] == null)
                    {
                        throw new ArgumentException("Parameter config direction is not specified");
                    }

					String parameterType = sqlParametersRoot.ChildNodes[counter].Attributes["type"].Value;
					String parameterName = sqlParametersRoot.ChildNodes[counter].Attributes["name"].Value;
                    String parameterDirection = sqlParametersRoot.ChildNodes[counter].Attributes["direction"].Value;
					String parameterSize = String.Empty;
                    Byte decimalPrecision = 0;
                    Byte decimalScale = 0;

                    SqlParameter parameter = null;
         		
					switch(parameterType)
					{
						case "Bit":
						case "Int":
                        case "TinyInt":
                        case "BigInt":
						case "SmallInt":
						case "Text":
						case "DateTime":
                        case "SmallDateTime":
                        case "UniqueIdentifier":
                        case "Structured":
                        case "Xml":
                            parameter = new SqlParameter(parameterName, GetSqlDBType(parameterType));
							break;					
						case "Binary":
                        case "Image":
						case "Char":
                        case "NChar":
						case "VarChar":
                        case "NVarChar":
                            if (parameterElement["size"] == null)
                            {
                                throw new ArgumentException("Parameter config size is not specified");
                            }
							parameterSize = sqlParametersRoot.ChildNodes[counter].Attributes["size"].Value;
                            parameter =  new SqlParameter(parameterName, GetSqlDBType(parameterType), Int32.Parse(parameterSize));
							break;
                        case "VarBinary":
                            if (parameterElement["size"] == null)
                                throw new ArgumentException("Sql metadata column size is not specified");
                            Int32 columnSizeInt = Int32.Parse(sqlParametersRoot.ChildNodes[counter].Attributes["size"].Value);
                            if (columnSizeInt > 8000 && columnSizeInt != Int32.MaxValue)
                            {
                                throw new ArgumentException("Sql metadata column size for VarBinary must be below or equal 8000 or equal to 2147483647");
                            }
                            parameter = new SqlParameter(parameterName, SqlDbType.VarBinary, (columnSizeInt == Int32.MaxValue) ? -1 : columnSizeInt);
                            break;
                        case "Decimal":
                            if(parameterElement["precision"] == null)
                                throw new ArgumentException("Parameter precision size is not specified");
                            if(parameterElement["scale"] == null)
                                throw new ArgumentException("Parameter scale size is not specified");

                            if(!Byte.TryParse(parameterElement["precision"].Value, out decimalPrecision))
                            {
                                throw new ArgumentException("Precision value is not in proper format");
                            }

                            if(!Byte.TryParse(parameterElement["scale"].Value, out decimalScale))
                            {
                                throw new ArgumentException("Scale value is not in proper format");
                            }

                            /*
                             * Precision is the number of digits in a number. Scale is the number of digits to the right of the decimal point in a number. 
                             * For example, the number 123.45 has a precision of 5 and a scale of 2.
                             */

                            parameter = new SqlParameter(parameterName, GetSqlDBType(parameterType));
                            parameter.Precision = decimalPrecision;
                            parameter.Scale = decimalScale;
                            break;

                        case "DateTime2":
                        {
                            Byte scale = 7; // Default value
                            if (parameterElement["scale"] != null)
                            {
                                String scaleStr = parameterElement["scale"].Value;
                                if (!String.IsNullOrWhiteSpace(scaleStr))
                                {
                                    if (!Byte.TryParse(scaleStr, out scale))
                                    {
                                        throw new ArgumentException("Scale value is not in proper format");
                                    }
                                    if (scale > 7)
                                    {
                                        throw new ArgumentException("Scale value is outside the bounds. Should be 0 to 7");
                                    }
                                }
                            }

                            /*
                             * Scale is the number of digits in seconds fractional part (YYYY-MM-DD hh:mm:ss.fractional_seconds).
                             * For example, the date '1979-12-23 00:00:00.1234' has a scale of 4.
                             * Storage size details: 6 bytes for precisions less than 3; 7 bytes for precisions 3 and 4. All other precisions require 8 bytes. 
                             */

                            parameter = new SqlParameter(parameterName, SqlDbType.DateTime2);
                            parameter.Scale = scale;

                            break;
                        }

                        default:
                            throw new ArgumentException("The data type specified in the parameter config is not supported", parameterType);
					}

                    parameter.Direction = GetParameterDirection(parameterDirection);
                    parameters[counter] = parameter;
				}				
			}

			return parameters;     
		}

		private static SqlDbType GetSqlDBType(String columnType)
		{
			switch(columnType)
			{
				case "Text":
					return SqlDbType.Text;
				case "Int":
					return SqlDbType.Int;
                case "BigInt":
                    return SqlDbType.BigInt;
				case "SmallInt":
					return SqlDbType.SmallInt;
				case "Bit":
					return SqlDbType.Bit;
				case "Char":
					return SqlDbType.Char;
				case "DateTime":
					return SqlDbType.DateTime;
				case "DateTime2":
					return SqlDbType.DateTime2;
                case "SmallDateTime":
                    return SqlDbType.SmallDateTime;
				case "Binary":
					return SqlDbType.Binary;
                case "Image":
                    return SqlDbType.Image;
                case "VarBinary":
                    return SqlDbType.VarBinary;
				case "Decimal":
					return SqlDbType.Decimal;
                case "Float":
                    return SqlDbType.Float;
                case "UniqueIdentifier":
					return SqlDbType.UniqueIdentifier;
                case "NChar":
                    return SqlDbType.NChar;
                case "NVarChar":
                    return SqlDbType.NVarChar;
                case "Xml":
                    return SqlDbType.Xml;
                case "TinyInt":
                    return SqlDbType.TinyInt;
                case "Structured":
                    return SqlDbType.Structured;
				case "VarChar": 
				default:
					return SqlDbType.VarChar;
			}
		}

        private static ParameterDirection GetParameterDirection(String value)
        {
            switch (value)
            {
                case "out":
                    return ParameterDirection.Output;
                case "in":
                    return ParameterDirection.Input;
                default:
                    throw new ArgumentException("The direction specified in parameter config is invalid", value);
            }
        }

		/// <summary>
		/// Gets the parameters and then Loop through the parameters and Assign the appropriate value.
		/// </summary>
		/// <remarks>The Hashtable key name should match with the parameter name exluding @ symbol</remarks>
        /// <param name="paramsKey">The param item key</param>
		/// <param name="inputData">The Hashtable containing the values</param>
		/// <returns>The sqlparameter array collection filled with hashtable values</returns>
		public SqlParameter[] AssignParameters(String paramsKey, Hashtable inputData)
		{
			SqlParameter[] parameters = null;
			parameters = GetParameters(paramsKey);				
							
			foreach(SqlParameter parameter in parameters)
			{
                //The substring is to remove the @ present in the parameter name
				if(inputData.Contains(parameter.ParameterName.Substring(1)))
				{
					parameter.Value =  inputData[parameter.ParameterName.Substring(1)];
				}
				else
				{
					parameter.Value = System.DBNull.Value;
				}
			}                    						
	
			return parameters;
		}

		/// <summary>
		/// Gets the parameters and then Loop through the parameters and Assign the appropriate value
		/// </summary>
		/// <remarks>In DataRow the colum name should match with the parameter name exluding @ symbol</remarks>
        /// <param name="paramsKey">The params item key</param>
		/// <param name="drData">The datatable row</param>
		/// <returns>The sqlparameter array collection filled with data row values</returns>
        public SqlParameter[] AssignParameters(String paramsKey, DataRow drData)
		{
			SqlParameter[] parameters = null;
			parameters = GetParameters(paramsKey);								

			foreach(SqlParameter parameter in parameters)
			{
                //The substring is to remove the @ present in the parameter name
				parameter.Value =  drData[parameter.ParameterName.Substring(1)];
			}

			return parameters;
		}

        ///<summary>
        ///Obtains the parameter array with values populated from the provided data.
        ///Since we are using the params keyword we could pass variable number of arguments to this function 
        ///but the total agruments must match the SqlParameters Collection
        ///</summary>
        ///<param name="paramsKey">The key used to identify the parameters</param>
        ///<param name="objectData">The values used to populate the parameters</param>
        ///<returns>The parameter array containing the required </returns>
        ///<remarks>The return value will be null if the parameters are not declared in config file.</remarks>
        public SqlParameter[] AssignParameters(String paramsKey, params Object[] objectData)
        {

            SqlParameter[] parameters = null;
            Int32 parametersLength = 0;
            Int32 objectArrayLength = 0;

            if (objectData == null)
            {
                throw new ArgumentNullException(
                    "objectData", "Can't assign an object array which is null");
            }

            parameters = GetParameters(paramsKey);

            parametersLength = parameters.Length;
            objectArrayLength = objectData.Length;

            if (parametersLength != objectArrayLength)
            {
                throw new InvalidOperationException("The object array length doesn't match the parameters collection length");
            }

            for (Int32 counter = 0; counter < objectArrayLength; counter++)
            {
                parameters[counter].Value = objectData[counter];
            }

            return parameters;
        }

        /// <summary>
        /// Prepares collection typed parameter value
        /// </summary>
        /// <param name="itemsList">Indicates list of parameter items</param>
        /// <param name="paramsKey">The key used to identify the parameters</param>
        /// <param name="parameterName">Name of the parameter for which data generation is requested</param>
        private List<SqlDataRecord> GenerateCollectionTypedParameterValue<T>(Collection<T> itemsList, String paramsKey, String parameterName)
        {
            List<SqlDataRecord> result = null;
            if (itemsList != null && itemsList.Count > 0)
            {
                SqlMetaData[] sqlContainerIdMetadata = GetTableValueMetadata(paramsKey, parameterName);

                result = new List<SqlDataRecord>();
                foreach (T item in itemsList)
                {
                    SqlDataRecord containerIdRecord = new SqlDataRecord(sqlContainerIdMetadata);
                    containerIdRecord.SetValue(0, item);
                    result.Add(containerIdRecord);
                }
            }
            return result;
        }

        /// <summary>
        /// Prepares collection typed parameter value
        /// </summary>
        /// <param name="itemsList">Indicates list of parameter items</param>
        /// <param name="paramsKey">The key used to identify the parameters</param>
        /// <param name="parameterName">Name of the parameter for which data generation is requested</param>
        public List<SqlDataRecord> PrepareCollectionTypedParameterValue(Collection<Int32> itemsList, String paramsKey, String parameterName)
        {
            return GenerateCollectionTypedParameterValue(itemsList, paramsKey, parameterName);
        }

        /// <summary>
        /// Prepares collection typed parameter value
        /// </summary>
        /// <param name="itemsList">Indicates list of parameter items</param>
        /// <param name="paramsKey">The key used to identify the parameters</param>
        /// <param name="parameterName">Name of the parameter for which data generation is requested</param>
        public List<SqlDataRecord> PrepareCollectionTypedParameterValue(Collection<Int64> itemsList, String paramsKey, String parameterName)
        {
            return GenerateCollectionTypedParameterValue(itemsList, paramsKey, parameterName);
        }

        /// <summary>
        /// Prepares collection typed parameter value
        /// </summary>
        /// <param name="itemsList">Indicates list of parameter items</param>
        /// <param name="paramsKey">The key used to identify the parameters</param>
        /// <param name="parameterName">Name of the parameter for which data generation is requested</param>
        public List<SqlDataRecord> PrepareCollectionTypedParameterValue(Collection<String> itemsList, String paramsKey, String parameterName)
        {
            return GenerateCollectionTypedParameterValue(itemsList, paramsKey, parameterName);
        }

		#endregion
	}
}