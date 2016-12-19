// *  Purpose:    This file contains utilities for processing XML.
// *  Maintenance History:
// *  Date        Programmer      Description
// *              Vu Le           Initial creation
// **********************************************************************************

using System; 
using System.Text.RegularExpressions;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace MDM.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlSerializationHelper
    {
        #region Constants

        private const String InvalidXmlCharactersPattern = @"[^\x09\x0A\x0D\u0020-\uD7FF\uE000-\uFFFD]";

        #endregion Constants

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(Object obj)
        {
            Stream myStream = null; // memory stream to hold xml
            TextReader reader = null; // used to read the memory stream
            XmlSerializer mySerializer = null;


            try
            {
                // create new serializer for the object type
                mySerializer = new XmlSerializer(obj.GetType());
                myStream = new MemoryStream();
                // serialize object into stream
                mySerializer.Serialize(myStream, obj);
                reader = new StreamReader(myStream);
                // reset stream pointer
                myStream.Seek(0, SeekOrigin.Begin);
                // get entire string from reader
                return reader.ReadToEnd();

            }
            catch (Exception e)
            {
                throw (new Exception("Error in XmlManager.Serialize(Object): " + e.Message));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                reader = null;
                myStream = null;
                mySerializer = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static bool Serialize(Object obj, XmlDocument xmlDoc)
        {
            string strXml = Serialize(obj);
            if (xmlDoc == null)
            {
                xmlDoc = new XmlDocument();
            }
            xmlDoc.LoadXml(strXml);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlStream"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Serialize(Stream xmlStream, Object obj)
        {
            XmlSerializer mySerializer = null;
            try
            {
                // create new serializer for the object type
                mySerializer = new XmlSerializer(obj.GetType());
                // serialize object into an xml file
                mySerializer.Serialize(xmlStream, obj);
                return true;
            }
            catch (Exception e)
            {
                throw (new Exception("Error in XmlManager.Serialize(Stream, Object): " + e.Message));
            }
            finally
            {
                mySerializer = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlString"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public static Object Deserialize(string XmlString, Type pType)
        {
            // Will Smith - The old way
            // Dim myStream As Stream 'hold the xml string
            // Dim writer As TextWriter 'used to write to the stream
            // Try
            //     myStream = New MemoryStream
            //     writer = New StreamWriter(myStream)
            //     'write string to memory stream
            //     writer.Write(XmlString)
            //     writer.Flush()
            //     'call overloaded method with stream 
            //     Return Deserialize(myStream, pType)
            // Catch e As Exception
            //     Throw (New Exception("Error in XmlManager.Deserialize(String, Object): " & e.Message))
            // Finally
            //     If Not (myStream Is Nothing) Then
            //         myStream.Close()
            //         myStream = Nothing
            //     End If
            //     If Not (writer Is Nothing) Then
            //         writer.Close()
            //         writer = Nothing
            //     End If
            // End Try

            // Will Smith - The new way
            XmlSerializer serializer = new XmlSerializer(pType);
            StringReader reader = new StringReader(XmlString);
            return serializer.Deserialize(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlStream"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public static Object Deserialize(Stream xmlStream, Type pType)
        {
            XmlReader reader = null;
            XmlSerializer mySerializer = null;
            Object result = null;

            try
            {
                // make sure we're at the beginning and create xml reader
                xmlStream.Seek(0, SeekOrigin.Begin);
                reader = new XmlTextReader(xmlStream);

                // create serializer for specified type, and serialize
                mySerializer = new XmlSerializer(pType);
                if ((mySerializer.CanDeserialize(reader)))
                {
                    xmlStream.Seek(0, SeekOrigin.Begin);
                    result = mySerializer.Deserialize(xmlStream);
                }

            }
            catch (Exception e)
            {
                throw (new Exception("Error in XmlManager.Deserialize(Stream, Object): " + e.Message));
            }
            finally
            {
                mySerializer = null;

                if (reader != null)
                    reader.Close();
            }

            return result;

        }

        /// <summary>
        /// This function will extract an xml fragment from the xmlString.  Given the name of the xml element, it will extract all the children of that element and return as a string
        /// </summary>
        /// <param name="xmlTag"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public string ReadXmlFragment(string xmlTag, string xmlString)
        {
            StringReader stream = new StringReader(xmlString);
            XmlTextReader reader = new XmlTextReader(stream);
            string result = string.Empty;

            try
            {
                // loop through xml until we see xmlTag.  once found, save it to string
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element & reader.Name == xmlTag))
                    {
                        result = "<" + reader.Name;
                        // get attributes if any
                        if ((reader.HasAttributes))
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                result += " " + reader.Name + @"=""" + reader.Value + @"""";
                            }
                        }
                        result += ">";
                        break;
                    }
                }

                if ((result == string.Empty))
                {
                    throw new Exception("Please check XML document...Missing " + xmlTag + " tag.");
                }

                bool isEnd = false;
                // read the rest of the data, exit when end of xmlTag found
                while (!isEnd && reader.Read())
                {
                    string emptyTag = null;
                    switch ((reader.NodeType))
                    {
                        case XmlNodeType.Element:
                            result += "<" + reader.Name;
                            if (reader.IsEmptyElement)
                            {
                                emptyTag = "/>";
                            }
                            else
                            {
                                emptyTag = ">";
                            }
                            // get attributes if any
                            if ((reader.HasAttributes))
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    result += " " + reader.Name + "='" + reader.Value + "'";
                                }
                            }
                            result += emptyTag;

                            break;
                        case XmlNodeType.EndElement:
                            result += "</" + reader.Name + ">";
                            // once the end xmlTag is found, get out                        
                            isEnd = (reader.Name == xmlTag);

                            break;
                        case XmlNodeType.Text:
                            result += reader.Value;

                            break;
                    }

                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
            }
        }

        /// <summary>
        /// This function will extract an xml fragment from the xmlStream.  Given the name of the xml element, it will extract all the children of that element and return as a string
        /// </summary>
        /// <param name="xmlTag">Indicates name of the xml tag</param>
        /// <param name="xmlStream">Indicates xml Stream from which xmlTextReader read the data</param>
        /// <returns>Returns string as an output</returns>        
        public string ReadXmlFragment(string xmlTag, Stream xmlStream)
        {
            XmlTextReader reader = null;
            string result = null;

            try
            {
                // reset the stream
                xmlStream.Seek(0, System.IO.SeekOrigin.Begin);

                reader = new XmlTextReader(xmlStream);
                result = string.Empty;

                // loop through xml until we see xmlTag.  once found, save it to string
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element & reader.Name == xmlTag))
                    {
                        result = "<" + reader.Name;
                        // get attributes if any
                        if ((reader.HasAttributes))
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                result += " " + reader.Name + @"=""" + reader.Value + @"""";
                            }
                        }
                        result += ">";
                        break;
                    }
                }

                if ((result == string.Empty))
                {
                    throw new Exception("Please check XML document...Missing " + xmlTag + " tag.");
                }

                bool isEnd = false;
                // read the rest of the data, exit when end of xmlTag found
                while (!isEnd && reader.Read())
                {
                    string emptyTag = null;
                    switch ((reader.NodeType))
                    {
                        case XmlNodeType.Element:
                            result += "<" + reader.Name;
                            if (reader.IsEmptyElement)
                            {
                                emptyTag = "/>";
                            }
                            else
                            {
                                emptyTag = ">";
                            }
                            // get attributes if any
                            if ((reader.HasAttributes))
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    result += " " + reader.Name + "='" + reader.Value + "'";
                                }
                            }
                            result += emptyTag;

                            break;
                        case XmlNodeType.EndElement:
                            result += "</" + reader.Name + ">";
                            // once the end xmlTag is found, get out                        
                            isEnd = (reader.Name == xmlTag);

                            break;
                        case XmlNodeType.Text:
                            result += reader.Value;

                            break;
                    }

                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string XmlEncode(string inString)
        {
            if (inString != null && inString != string.Empty && inString != "&amp;" && inString != "&quot;" && inString != "&apos;" && inString != "&lt;" && inString != "&gt;")
            {
                inString = inString.Replace("&", "&amp;");
                inString = inString.Replace("<", "&lt;");
                inString = inString.Replace(">", "&gt;");
                inString = inString.Replace(@"""", "&quot;");
                inString = inString.Replace("'", "&apos;");
                inString = inString.Replace("&#xD;", "").Replace("&#xA;", ""); // remove linefeed and character return
                // s = S1.Replace(Strings.Chr(63) + Strings.Chr(32), Strings.Chr(174))
                char[] achr = System.Text.Encoding.ASCII.GetChars(new byte[] { 174 });
                inString = inString.Replace('�', achr[0]);
                // s = s.Replace('�', Strings.Chr(169))
            }

            return inString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string XmlDecode(string inString)
        {
            if (!String.IsNullOrWhiteSpace(inString))
            {
                inString = inString.Replace("&amp;", "&");
                inString = inString.Replace("&apos;", "'");
                inString = inString.Replace("&quot;", @"""");
                inString = inString.Replace("&gt;", ">");
                inString = inString.Replace("&lt;", "<");
            }

            return inString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string XmlValidate(string inString)
        {
            String specialChars = String.Empty;
            //AppConstants.SpecialCharacterToRemove
            return System.Text.RegularExpressions.Regex.Replace(inString, specialChars, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rootTag"></param>
        /// <param name="attrTag"></param>
        /// <returns></returns>
        public static string EncodeAttributeCollectionXml(string source, string rootTag, string attrTag)
        {
            String xml = String.Empty;
            XmlTextReader xmlRdr = new XmlTextReader(source, XmlNodeType.Element, null);
            StringWriter strWtr = new StringWriter();
            XmlTextWriter xmlWtr = new XmlTextWriter(strWtr);

            /* Read and copy data from xmlRdr to xmlWtr
                 When xmlRdr <attrTag> has collection children, <CollectionXml ValSource=""> 
                 then encode and put in appropriate val attribute.
              <rootTag>
                  <attrTag>
                      <CollectionXml>
                          <SimpleAttribute/>
                      </CollectionXml>
                  </attrTag>
              </rootTag>			  
            */
            string ValSource = string.Empty;
            Hashtable values = new Hashtable();
            Hashtable valStrWriters = new Hashtable();
            Hashtable valWriters = new Hashtable();
            bool OpenCollectionXml = false;

            while (xmlRdr.Read())
            {
                //values.Clear();
                if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == rootTag)
                {
                    xmlWtr.WriteStartElement(rootTag);
                    while (xmlRdr.MoveToNextAttribute())
                    {
                        xmlWtr.WriteAttributeString(xmlRdr.Name, xmlRdr.Value);
                    }
                    xmlRdr.MoveToElement();
                    if (xmlRdr.IsEmptyElement)
                        xmlWtr.WriteEndElement(); //</rootTag>
                }
                else if (xmlRdr.NodeType == XmlNodeType.EndElement && xmlRdr.Name == rootTag)
                {
                    xmlWtr.WriteEndElement(); //</attrTag>
                }
                else if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == attrTag)
                {
                    xmlWtr.WriteStartElement(attrTag);

                    while (xmlRdr.MoveToNextAttribute())
                    {
                        if (xmlRdr.Name == "Val" || xmlRdr.Name == "InheritVal" || xmlRdr.Name == "MasterVal")
                            values[xmlRdr.Name] = xmlRdr.Value;
                        else
                            xmlWtr.WriteAttributeString(xmlRdr.Name, xmlRdr.Value);
                    }
                    xmlRdr.MoveToElement();
                    if (xmlRdr.IsEmptyElement)
                    {
                        xmlWtr.WriteAttributeString("Val", values["Val"] as string);
                        xmlWtr.WriteAttributeString("InheritVal", values["InheritVal"] as string);
                        xmlWtr.WriteAttributeString("MasterVal", values["MasterVal"] as string);
                        xmlWtr.WriteEndElement(); //</attrTag>
                        values.Clear();
                    }
                }
                else if (xmlRdr.NodeType == XmlNodeType.EndElement && xmlRdr.Name == attrTag)
                {
                    xmlWtr.WriteAttributeString("Val", values["Val"] as string);
                    xmlWtr.WriteAttributeString("InheritVal", values["InheritVal"] as string);
                    xmlWtr.WriteAttributeString("MasterVal", values["MasterVal"] as string);
                    xmlWtr.WriteEndElement(); //</attrTag>
                    values.Clear();
                }
                else if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == "CollectionXml")
                {
                    xmlRdr.MoveToAttribute("ValSource");
                    ValSource = xmlRdr.Value;
                    xmlRdr.MoveToElement();
                    if (xmlRdr.IsEmptyElement)
                    {
                        values[ValSource] = string.Empty;
                    }
                }
                else if (xmlRdr.NodeType == XmlNodeType.EndElement && xmlRdr.Name == "CollectionXml")
                {
                    if (valWriters.Count > 0)
                    {
                        XmlTextWriter xmlWtrVal = valWriters[ValSource] as XmlTextWriter;
                        if (xmlWtrVal != null)
                            xmlWtrVal.WriteEndElement(); //</CollectionXml>
                    }
                    if (valStrWriters.Count > 0)
                    {
                        values[ValSource] = (valStrWriters[ValSource] as StringWriter).ToString();
                    }
                    OpenCollectionXml = false;
                }
                else if (xmlRdr.NodeType == XmlNodeType.Element && xmlRdr.Name == "SimpleAttribute")
                {
                    XmlTextWriter xmlWtrVal;
                    if (!OpenCollectionXml)
                    {
                        OpenCollectionXml = true;
                        StringWriter strWtrVal = new StringWriter();
                        valStrWriters[ValSource] = strWtrVal;
                        xmlWtrVal = new XmlTextWriter(strWtrVal);
                        valWriters[ValSource] = xmlWtrVal;
                        xmlWtrVal.WriteStartElement("CollectionXml");
                    }
                    else
                    {
                        xmlWtrVal = valWriters[ValSource] as XmlTextWriter;
                    }
                    //Placed a condition here for a check for "xmlWtrVal" in case if it will be null.
                    if (valWriters.Count > 0)
                    {
                        if (xmlWtrVal != null)
                        {
                            xmlWtrVal.WriteStartElement("SimpleAttribute");
                            while (xmlRdr.MoveToNextAttribute())
                            {
                                xmlWtrVal.WriteAttributeString(xmlRdr.Name, xmlRdr.Value);
                            }
                            xmlWtrVal.WriteEndElement(); //</SimpleAttribute>
                        }
                    }


                }
            }

            xml = strWtr.ToString();

            xmlWtr.Close();
            xmlRdr.Close();
            strWtr.Close();

            return xml;
        }

        /// <summary>
        /// Calls DataSet.GetXml after changing all ColumnMappings to Attribute.
        /// </summary>
        /// <param name="dataSet">The dataset to be converted to Xml</param>
        /// <param name="upperCaseColumns">Indicates that the columns should be rendered in all UPPER-CASE</param>
        /// <returns>The Xml as a string</returns>
        public static string CreateXmlFromDataSet(DataSet dataSet, bool upperCaseColumns)
        {
            if (upperCaseColumns)
            {
                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }
                }
            }
            else
            {
                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dc.ColumnName = dc.ColumnName.ToUpper();
                        dc.ColumnMapping = MappingType.Attribute;
                    }
                }
            }

            return dataSet.GetXml();
        }

        /// <summary>
        /// Removes unsupported XML chars from string
        /// </summary>
        /// <param name="text">xml string</param>
        /// <returns>xml string without unsupported xml chars</returns>
        public static String CleanInvalidXmlChars(String text)
        {
            return Regex.Replace(text, InvalidXmlCharactersPattern, String.Empty);
        }
    }
} 
