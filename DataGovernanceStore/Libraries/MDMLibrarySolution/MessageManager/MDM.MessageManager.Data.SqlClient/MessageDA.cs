using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using MDM.Core;
using MDM.Utility;
using MDM.BusinessObjects;

namespace MDM.MessageManager.Data
{
    public class MessageDA : SqlClientDataAccessBase
    {
        public List<Object[]> getMessages(String LoginUser, string IsReadCondition, string StateCondition, string MsgType)
        {
            List<Object[]> data = new List<Object[]>();
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

                parameters = generator.GetParameters("Messagemanager_getMessages_ParametersArray");

                parameters[0].Value = LoginUser;
                parameters[1].Value = IsReadCondition;
                parameters[2].Value = StateCondition;
                parameters[3].Value = MsgType;

                storedProcedureName = "Usp_Messagemanager_getMessages";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    data.Add(values);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return data;
        }

        public Object[] getBody(int Id, String LoginUser, String ProgramName)
        {
            Object[] values = new Object[0];
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

                parameters = generator.GetParameters("Messagemanager_getBody_ParametersArray");

                parameters[0].Value = Id;
                parameters[1].Value = LoginUser;
                parameters[2].Value = ProgramName;

                storedProcedureName = "Usp_Messagemanager_getBody";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader.Read())
                {
                    values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return values;
        }

        public void getKeyStatistics(String LoginUser, out int ReadMessages, out int UnreadMessages, out int PendingMessages)
        {
            ReadMessages = 0;
            UnreadMessages = 0;
            PendingMessages = 0;

            Object[] values = new Object[0];
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

                parameters = generator.GetParameters("Messagemanager_getKeyStatistics_ParametersArray");

                parameters[0].Value = LoginUser;

                storedProcedureName = "Usp_Messagemanager_getKeyStatistics";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader.Read())
                {
                    values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    UnreadMessages = (int)values[0];
                }
                if (reader.Read())
                {
                    values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    ReadMessages = (int)values[0];
                }
                if (reader.Read())
                {
                    values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    PendingMessages = (int)values[0];
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public bool markComplete(int Id, String LoginUser, String ProgramName, String State)
        {
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

                parameters = generator.GetParameters("Messagemanager_markComplete_ParametersArray");

                parameters[0].Value = Id;
                parameters[1].Value = LoginUser;
                parameters[2].Value = ProgramName;
                parameters[3].Value = State;

                storedProcedureName = "Usp_Messagemanager_markComplete";

                ExecuteProcedureSingleRow(connectionString, parameters, storedProcedureName);

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return true;
        }

        public Boolean ProcessMessageDetail(Message Message, Boolean processDetailOnly, string action, String LoginUser, String ProgramName)
        {
            Boolean result = false;
            string connectionString = AppConfigurationHelper.ConnectionString;

            String MessageDataXml = Message.ToXml();

            SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

            SqlParameter[] parameters = generator.GetParameters("MessageManager_Message_Process_ParametersArray");

            parameters[0].Value = MessageDataXml;
            parameters[1].Value = action;
            parameters[2].Value = LoginUser;
            parameters[3].Value = ProgramName;

            string storedProcedureName = "usp_MessageManager_Message_Process";

            object[] values = ExecuteProcedureSingleRow(connectionString, parameters, storedProcedureName);
            if (values.Length == 1)
            {
                Int32 MessageId = ValueTypeHelper.Int32TryParse(values[0].ToString(), 0);

                if (MessageId > 0)
                {
                    Message.Id = MessageId;
                    result = true;
                }
            }

            return result;
        }
    }
}
