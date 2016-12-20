using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace MDM.DataModelExport.Business
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;

    public sealed class DataModelHelper
    {
        /// <summary>
        /// Export the Data Model  excel file
        /// </summary>
        /// <param name="callerContext">Callercontext to denote application and module information</param>
        /// <returns>Returns Excel File</returns>
        public static String GetTempFileLocation(Boolean isJobServiceRequest)
        {
            string tempFileLocation = String.Empty;
            if (isJobServiceRequest)
                tempFileLocation = AppConfigurationHelper.GetAppConfig<String>("Jobs.TemporaryFileRoot");
            else
                tempFileLocation = AppConfigurationHelper.GetAppConfig<String>("WCF.TemporaryFileRoot");

            return tempFileLocation;
        }
        
        /// <summary>
        /// Export the Data Model  excel file
        /// </summary>
        /// <param name="callerContext">Callercontext to denote application and module information</param>
        /// <returns>Returns Excel File</returns>
        public static String GetTemplateFileName(String templateName, String tempFileLocation, CallerContext callerContext)
        {
            String filePath = String.Empty;

            if (!String.IsNullOrEmpty(templateName))
            {
                ExportTemplateBL exportTemplateBL = new ExportTemplateBL();

                // Get a template based on the profile.
                Template template = exportTemplateBL.GetExportTemplateByName(templateName, callerContext);

                if (template != null)
                {
                    File file = new File(template.Name, template.FileType, false, template.FileData);
                    filePath = String.Format("{0}\\{1}.{2}", tempFileLocation, file.Name, file.FileType);

                    FileInfo fileInfo = new FileInfo(filePath);

                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }

                    // Create a file to write to.
                    using (FileStream fileStream = fileInfo.Create())
                    {
                        fileStream.Write(file.FileData, 0, file.FileData.Length);
                    }
                }
            }

            return filePath;
        }

        /// <summary>
        /// Export the Data Model  excel file
        /// </summary>
        /// <param name="callerContext">Callercontext to denote application and module information</param>
        /// <returns>Returns Excel File</returns>
        public static File GetFileFromStream(Stream stream, String fileType)
        {
            File result = null;
            if (stream != null)
            {
                MemoryStream memoryStream = (MemoryStream)stream;
                byte[] binary = memoryStream.ToArray();
                result = new File(String.Empty, fileType, false, binary);
            }
            return result;
        }

        /// <summary>
        /// Export the Data Model  excel file
        /// </summary>
        /// <param name="callerContext">Callercontext to denote application and module information</param>
        /// <returns>Returns Excel File</returns>
        public static void WriteFileToTargetStream(String fileName, Stream targetStream)
        {
            Stream inStream = System.IO.File.OpenRead(fileName);

            const int BufferSize = 1024 * 10;
            byte[] buffer = new byte[BufferSize];

            int bytesRead;
            do
            {
                bytesRead = inStream.Read(buffer, 0, buffer.Length);
                targetStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead == BufferSize);
            inStream.Close();
        }
    }
}
