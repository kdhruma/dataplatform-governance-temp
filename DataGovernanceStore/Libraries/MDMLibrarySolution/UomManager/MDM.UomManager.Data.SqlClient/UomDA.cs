using System;
using System.Data.SqlClient;

namespace MDM.UomManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using System.Text;

    /// <summary>
    /// Represents class for UOM data access
    /// </summary>
    public class UomDA : SqlClientDataAccessBase
    {

        #region Public Methods

      /// <summary>
      /// GetAll Uoms
      /// </summary>
      /// <param name="uomContext"></param>
      /// <param name="command"></param>
      /// <returns></returns>
        public UOMCollection GetAll(UomContext uomContext, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            UOMCollection uomCollection = new UOMCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("UomManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("UomManager_Get_ParametersArray");

                parameters[0].Value = -1;
                parameters[1].Value = -1;

                const String storedProcedureName = "usp_UOMManager_UOM_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        UOM uom = new UOM();
                        if (reader["Id"] != null)
                        {
                            uom.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                        }

                        if (reader["ShortName"] != null)
                        {
                            uom.Name = reader["ShortName"].ToString();
                        }

                        if (reader["LongName"] != null)
                        {
                            uom.LongName = reader["LongName"].ToString();
                        }

                        if (reader["UOMTypeShortName"] != null)
                        {
                            uom.UnitTypeShortName = reader["UOMTypeShortName"].ToString();
                        }

                        if (reader["UOMTypeLongName"] != null)
                        {
                            uom.UnitTypeLongName = reader["UOMTypeLongName"].ToString();
                        }

                        if (reader["UOMKey"] != null)
                        {
                            uom.Key = reader["UOMKey"].ToString();
                        }
                        uomCollection.Add(uom);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return uomCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public String GetUomConversionsAsXml(DBCommandProperties command)
        {
            StringBuilder returnXml = new StringBuilder();
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            SqlParameter[] parameters = null;
            
            try
            {
                storedProcedureName = "usp_N_getUOMConversions_XML";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        returnXml.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return returnXml.ToString();
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods
    }
}

