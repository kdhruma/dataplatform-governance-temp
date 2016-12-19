using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MDM.DataModelManager.UnitTest
{
    public class DataReader
    {
        /// <summary>
        /// Fetch out the <Data> xml chunk for given method from full xml.
        /// </summary>
        /// <param name="fileName">Name of file in which we want to search. This file has Xml having input and output data information for given method</param>
        /// <param name="methodName">Method name for which we are looking the Xml. Meaning, we are looking for input and output information for this method</param>
        /// <returns><![CDATA[<Data>]]> Xml having input and output sections 
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Data MethodName="GetAppConfig_KeyNameExist_ProperValue">
        ///     <Input>
        ///         <Parameters>
        ///             <Parameter Name="appConfigKey">MDMCenter.Workflow.Enabled</Parameter>
        ///         </Parameters>
        ///     </Input>
        ///     <Output>
        ///         True
        ///     </Output>
        /// </Data>
        /// ]]>
        /// </para>
        /// </returns>
        public static String ReadMethodData(String fileName, String methodName )
        {
            if ( String.IsNullOrWhiteSpace(fileName) )
            {
                throw new Exception("fileName is not provided");
            }

            if ( String.IsNullOrWhiteSpace(methodName) )
            {
                throw new Exception("methodName is not provided");
            }

            //Get file path
            DirectoryInfo projectDir = new DirectoryInfo(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath));
            String path = projectDir.FullName;

            //Merge the file path and file name to get actual file url
            String fileURL = path + "\\" + fileName;
            String methodInfoXml = String.Empty;

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(fileURL);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while ( !reader.EOF )
                {
                    if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Data" )
                    {
                        if ( reader.HasAttributes )
                        {
                            //Make sure we reached to the method which we are looking for.
                            String method = reader.GetAttribute("MethodName");
                            if(!String.IsNullOrWhiteSpace(method) && method.ToLower().Equals(methodName.ToLower()))
                            {
                                //Once we are reached at the method we are looking at, no need to keep on looping. So just break.
                                methodInfoXml = reader.ReadOuterXml();
                                break;
                            }
                        }
                    }
                    reader.Read();
                }
            }
            finally
            {
                if ( reader != null )
                    reader.Close();
            }

            if ( String.IsNullOrWhiteSpace(methodInfoXml) )
            {
                throw new Exception(String.Concat("No <Data> found for method : ", methodName, " in file : ", fileURL));
            }
            return methodInfoXml;
        }
    }
}
