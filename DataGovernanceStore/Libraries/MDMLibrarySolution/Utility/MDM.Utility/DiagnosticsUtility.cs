using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Microsoft.SqlServer.Server;

namespace MDM.Utility
{
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// DiagnosticsUtility
    /// </summary>
    public class DiagnosticsUtility
    {
        /// <summary>
        /// Logs Information Message of all of the table
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="tableName"></param>
        /// <param name="metadata"></param>
        /// <param name="table"></param>
        public static void LogSqlTVPInformation(DiagnosticActivity activity, String storedProcedureName, String tableName, SqlMetaData[] metadata, List<SqlDataRecord> table)
        {
            if (activity == null || String.IsNullOrEmpty(storedProcedureName) || String.IsNullOrEmpty(tableName) || metadata == null || table == null)
            {
                return;
            }

            try
            {
                String xml = String.Empty;

                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                xmlWriter.WriteStartElement(tableName);

                xmlWriter.WriteAttributeString("StoredProcedure", storedProcedureName);

                // Starting Record loop

                foreach (var record in table)
                {
                    xmlWriter.WriteStartElement("Record");

                    for (int i = 0; i < metadata.Count(); i++)
                    {
                        xmlWriter.WriteStartElement(record.GetName(i).Replace("@", ""));
                        Object value = record.GetValue(i);

                        if (value != null)
                        {
                            xmlWriter.WriteString(value.ToString());
                        }
                        else
                        {
                            xmlWriter.WriteString("NO or NULL VALUE");
                        }

                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();

                activity.LogMessageWithData(String.Format("{0} {1}", storedProcedureName, tableName), xml);
            }
            catch (Exception exception)
            {
                // this is for Diagnostic log - exception shouldn't affect the flow
                activity.LogError(exception.ToString());
            }
        }
    }
}
