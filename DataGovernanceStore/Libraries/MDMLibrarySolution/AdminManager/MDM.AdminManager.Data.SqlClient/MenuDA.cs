using System;
using System.Data.SqlClient;

namespace MDM.AdminManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;

    /// <summary>
    /// Data Access Class for Menus
    /// </summary>
    public class MenuDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get All Menu for given User
        /// </summary>
        /// <param name="userLogin">Indicates Login User Name</param>
        /// <returns>Collection of Menu</returns>
        public MenuCollection GetUserMenus(String userLogin)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            
            MenuCollection menus = new MenuCollection();

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_Menu_Get_ParametersArray");

                parameters[0].Value = userLogin;

                storedProcedureName = "usp_AdminManager_Menu_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Menu menu = new Menu();

                        if (reader["Id"] != null)
                            menu.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(),0);

                        if (reader["MenuParent"] != null)
                            menu.MenuParentId = ValueTypeHelper.Int32TryParse(reader["MenuParent"].ToString(),0);
                        
                        if (reader["sequence"] != null)
                            menu.Sequence = ValueTypeHelper.Int32TryParse(reader["sequence"].ToString(),0);
                        
                        if (reader["Name"] != null)
                            menu.Name = reader["Name"].ToString();
                        
                        if (reader["LongName"] != null)
                            menu.LongName = reader["LongName"].ToString();
                       
                        if (reader["Link"] != null)
                            menu.Link = reader["Link"].ToString();

                        menus.Add(menu);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();   
            }

            return menus;
        }

        #endregion
    }
}
