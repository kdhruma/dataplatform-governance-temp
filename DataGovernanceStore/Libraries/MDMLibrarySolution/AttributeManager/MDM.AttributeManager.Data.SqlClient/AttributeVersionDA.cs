using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MDM.AttributeManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using Microsoft.SqlServer.Server;

    public class AttributeVersionDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Version History of Attribute
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="attributeId">Specifies Id of Attribute</param>
        /// <param name="localeId">Specifies Id of Locale</param>
        /// <param name="catalogId">Specifies Id of Catalog</param>
        /// <param name="entityParentId">Specifies Id of Parent EntityId</param>
        /// <param name="command">Specifies Connection Properties</param>
        /// <returns>Versions of Attribute</returns>
        public AttributeVersionCollection Get(Int64 entityId, Int32 attributeId, LocaleEnum systemDataLocale, Int32 localeId, Int32 catalogId, Int64 entityParentId,DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            AttributeVersionCollection attributeVersions = new AttributeVersionCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeManager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_AttributeVersion_Get_ParametersArray");

                #region Populate table value parameters for Locale

                List<SqlDataRecord> localeList = new List<SqlDataRecord>();
                SqlMetaData[] localeData = generator.GetTableValueMetadata("AttributeManager_AttributeVersion_Get_ParametersArray", parameters[5].ParameterName);

                SqlDataRecord localeRecord = new SqlDataRecord(localeData); ;
                localeRecord.SetValue(0, localeId);
                localeList.Add(localeRecord);
                
                #endregion Populate table value parameters for Locale

                parameters[0].Value = entityId;
                parameters[1].Value = entityParentId;
                parameters[2].Value = attributeId;
                parameters[3].Value = catalogId;
                parameters[4].Value = systemDataLocale;
                parameters[5].Value = localeList;
                

                storedProcedureName = "usp_AttributeManager_AttributeVersion_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    AttributeVersion attributeVersion = new AttributeVersion();

                    if (reader["Id"] != null)
                        attributeVersion.Id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), attributeVersion.Id);

                    if (reader["Parent"] != null)
                        attributeVersion.ParentId = ValueTypeHelper.Int64TryParse(reader["Parent"].ToString(), attributeVersion.ParentId);

                    if (reader["ValueKey"] != null)
                        attributeVersion.InstanceRefId = ValueTypeHelper.Int32TryParse(reader["ValueKey"].ToString(),-1);

                    if (reader["SRCflag"] != null)
                        attributeVersion.SourceFlag = Utility.GetSourceFlagEnum(reader["SRCflag"].ToString());

                    if (reader["seq"] != null)
                        attributeVersion.Sequence = ValueTypeHelper.ConvertToDecimal(reader["seq"].ToString());

                    if (reader["LocaleName"] != null)
                    {
                        String strLocale = reader["LocaleName"].ToString();
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        Enum.TryParse<LocaleEnum>(strLocale, out locale);
                        attributeVersion.Locale = locale;
                    }

                    if (reader["Value"] != null)
                    {
                        Value value = new Value();
                        value.AttrVal = reader["Value"].ToString();
                        value.Sequence = ValueTypeHelper.ConvertToDecimal(reader["seq"].ToString());
                        value.ValueRefId = ValueTypeHelper.Int32TryParse(reader["ValueKey"].ToString(), -1);
                        value.Uom = reader["UOM"].ToString();
                        attributeVersion.Value = value;
                    }  

                    if (reader["ModDateTime"] != null)
                    {
                        attributeVersion.ModDateTime =ValueTypeHelper.ConvertToDateTime(reader["ModDateTime"].ToString());                    
                    }

                    if (reader["ModUser"] != null)
                    {
                        attributeVersion.ModUser = reader["ModUser"].ToString();
                    }
                    if (reader["ModProgram"] != null)
                    {
                        attributeVersion.ModProgram = reader["ModProgram"].ToString();
                    }
                    if (reader["UserAction"] != null)
                    {
                        attributeVersion.UserAction = reader["UserAction"].ToString();
                    }
                    if (reader["GroupId"] != null)
                    {
                        attributeVersion.GroupId = ValueTypeHelper.Int32TryParse(reader["GroupId"].ToString(), -1);
                    }
                    
                    if (reader["IsInvalidData"] != null)
                    {
                        attributeVersion.HasInvalidValue = ValueTypeHelper.BooleanTryParse(reader["IsInvalidData"].ToString(), false);
                    }

                    attributeVersions.Add(attributeVersion);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return attributeVersions;
        }

        /// <summary>
        /// Get Version History of Attribute
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="attributeId">Specifies Id of Attribute</param>
        /// <param name="entityParentId">Specifies ParentId of entity</param>
        /// <param name="localeId">Specifies Id of Locale</param>
        /// <param name="catalogId">Specifies Id of Catalog</param>
        /// <param name="locales">Specifies the data Locales</param>
        /// <param name="sequence">Specifies a sequence</param>
        /// <param name="command">Specifies Connection Properties</param>
        /// <returns>AttributeVersionCollection</returns>
        public AttributeVersionCollection GetComplexAttributeVersions(Int64 entityId, Int64 entityParentId, Int32 attributeId, Int32 catalogId, Collection<LocaleEnum> locales, Int32 sequence, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            AttributeVersionCollection attributeVersions = new AttributeVersionCollection();
            List<SqlDataRecord> localeList = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeManager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_AttributeVersion_GetComplexAttributeVersions_ParametersArray");
               
                #region Populate table value parameters and also populate return result collection

                SqlMetaData[] sqllocales = generator.GetTableValueMetadata("AttributeManager_AttributeVersion_GetComplexAttributeVersions_ParametersArray", parameters[5].ParameterName);

                LocaleEnum systemLocale =  MDM.Utility.GlobalizationHelper.GetSystemDataLocale();

                if (systemLocale != LocaleEnum.UnKnown)               
                {
                    if(locales == null)
                    {
                        locales = new Collection<LocaleEnum>();
                    }

                    locales.Add(systemLocale);
                }

                if(locales!=null && locales.Count>0)
                {
                    localeList = new List<SqlDataRecord>();

                    foreach (LocaleEnum locale in locales)
                    {
                        SqlDataRecord localeRecord = new SqlDataRecord(sqllocales);
                        localeRecord.SetValue(0, (Int32)locale);
                        localeRecord.SetValue(1, locale.ToString());

                        if ((Int32)systemLocale == (Int32)locale)
                        {
                            localeRecord.SetValue(2, true);
                        }
                        else
                        {
                            localeRecord.SetValue(2, false);
                        }

                        localeList.Add(localeRecord);
                    }
                }

                #endregion

                parameters[0].Value = entityId;
                parameters[1].Value = entityParentId;
                parameters[2].Value = attributeId;
                parameters[3].Value = catalogId;
                parameters[4].Value = systemLocale;
                parameters[5].Value = localeList;
                parameters[6].Value = sequence;


                storedProcedureName = "usp_AttributeManager_AttributeVersion_Complex_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    AttributeVersion attributeVersion = new AttributeVersion();

                    if (reader["Id"] != null)
                    {
                        attributeVersion.Id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), attributeVersion.Id);
                    }

                    if (reader["Parent"] != null)
                    {
                        attributeVersion.ParentId = ValueTypeHelper.Int64TryParse(reader["Parent"].ToString(), attributeVersion.ParentId);
                    }

                    if (reader["seq"] != null)
                    {
                        attributeVersion.Sequence = ValueTypeHelper.ConvertToDecimal(reader["seq"].ToString());
                    }

                    if (reader["LocaleName"] != null)
                    {
                        String strLocale = reader["LocaleName"].ToString();
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        Enum.TryParse<LocaleEnum>(strLocale, out locale);
                        attributeVersion.Locale = locale;
                    }

                    if (reader["Value"] != null)
                    {
                        Value value = new Value();
                        value.AttrVal = reader["Value"].ToString();

                        if (reader["seq"] != null)
                        {
                            value.Sequence = ValueTypeHelper.ConvertToDecimal(reader["seq"].ToString());
                        }

                        attributeVersion.Value = value;
                    }

                    if (reader["ModDateTime"] != null)
                    {
                        attributeVersion.ModDateTime = ValueTypeHelper.ConvertToDateTime(reader["ModDateTime"].ToString());
                    }

                    if (reader["ModUser"] != null)
                    {
                        attributeVersion.ModUser = reader["ModUser"].ToString();
                    }

                    if (reader["ModProgram"] != null)
                    {
                        attributeVersion.ModProgram = reader["ModProgram"].ToString();
                    }

                    if (reader["DisplayValue"] != null)
                    {
                        attributeVersion.DisplayValue = reader["DisplayValue"].ToString();
                    }

                    if (reader["UserAction"] != null)
                    {
                        attributeVersion.UserAction = reader["UserAction"].ToString();
                    }

                    if (reader["IsInvalidData"] != null)
                    {
                        attributeVersion.HasInvalidValue =
                            ValueTypeHelper.BooleanTryParse(reader["IsInvalidData"].ToString(), false);
                    }

                    attributeVersions.Add(attributeVersion);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return attributeVersions;
        }


        #endregion
    }
}
