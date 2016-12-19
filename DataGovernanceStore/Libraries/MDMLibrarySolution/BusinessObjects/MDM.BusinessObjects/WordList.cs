using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies WordList
    /// </summary>
    [DataContract]
    public class WordList : MDMObject, IWordList, IEquatable<WordList>
    {
        #region Fields

        private WordElementsCollection _wordElements = new WordElementsCollection();
        private Int32? _wordListTypeId;
        private Int32? _wordBreakerSetId;
        private DateTime? _createDateTime;
        private DateTime? _modDateTime;
        private String _createUser = String.Empty;
        private String _modUser = String.Empty;
        private String _createProgram = String.Empty;
        private String _modProgram = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Specifies parameterless constructor
        /// </summary>
        public WordList()
        {
        }

        /// <summary>
        /// Initialize WordList from xml
        /// </summary>
        /// <param name="xml"></param>
        public WordList(String xml)
        {
            LoadFromXmlWithOuterNode(xml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Denotes word elements
        /// </summary>
        [DataMember]
        public WordElementsCollection WordElements
        {
            get { return _wordElements; }
            set { _wordElements = value; }
        }

        /// <summary>
        /// Denotes word list type id
        /// </summary>
        [DataMember]
        public Int32? WordListTypeId
        {
            get { return _wordListTypeId; }
            set { _wordListTypeId = value; }
        }

        /// <summary>
        /// Denotes word breaker set id
        /// </summary>
        [DataMember]
        public Int32? WordBreakerSetId
        {
            get { return _wordBreakerSetId; }
            set { _wordBreakerSetId = value; }
        }

        /// <summary>
        /// Denotes create date time
        /// </summary>
        [DataMember]
        public DateTime? CreateDateTime
        {
            get { return _createDateTime; }
            set { _createDateTime = value; }
        }

        /// <summary>
        /// Denotes last modification date time
        /// </summary>
        [DataMember]
        public DateTime? ModDateTime
        {
            get { return _modDateTime; }
            set { _modDateTime = value; }
        }

        /// <summary>
        /// Denotes user that created current word list
        /// </summary>
        [DataMember]
        public String CreateUser
        {
            get { return _createUser; }
            set { _createUser = value; }
        }

        /// <summary>
        /// Denotes the last user that modified current word list
        /// </summary>
        [DataMember]
        public String ModUser
        {
            get { return _modUser; }
            set { _modUser = value; }
        }

        /// <summary>
        /// Denotes create program
        /// </summary>
        [DataMember]
        public String CreateProgram
        {
            get { return _createProgram; }
            set { _createProgram = value; }
        }

        /// <summary>
        /// Denotes the last program that modified current word list
        /// </summary>
        [DataMember]
        public String ModProgram
        {
            get { return _modProgram; }
            set { _modProgram = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Denotes method for cloning current word list
        /// </summary>
        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Denotes method for comparing word lists
        /// </summary>
        public Boolean Equals(WordList other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) &&
                   _wordListTypeId == other._wordListTypeId && _wordBreakerSetId == other._wordBreakerSetId &&
                   _createDateTime.Equals(other._createDateTime) && _modDateTime.Equals(other._modDateTime) &&
                   String.Equals(_createUser, other._createUser) && String.Equals(_modUser, other._modUser) &&
                   String.Equals(_createProgram, other._createProgram) && String.Equals(_modProgram, other._modProgram) &&
                   ((_wordElements == null && other._wordElements == null) || (_wordElements != null && _wordElements.Equals(other._wordElements)));
        }

        /// <summary>
        /// Denotes method for comparing word lists
        /// </summary>
        public override Boolean Equals(Object obj)
        {
            return Equals(obj as WordList);
        }

        /// <summary>
        /// Denotes method for generating hashcode
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_wordElements != null ? _wordElements.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _wordListTypeId.GetHashCode();
                hashCode = (hashCode * 397) ^ _wordBreakerSetId.GetHashCode();
                hashCode = (hashCode * 397) ^ _createDateTime.GetHashCode();
                hashCode = (hashCode * 397) ^ _modDateTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (_createUser != null ? _createUser.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_modUser != null ? _modUser.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_createProgram != null ? _createProgram.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_modProgram != null ? _modProgram.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        /// Denotes method for xml serialization
        /// </summary>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("WordList");

            xmlWriter.WriteAttributeString("Id", Id.ToString());

            xmlWriter.WriteStartElement("Action");
            xmlWriter.WriteRaw(Action.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Name");
            xmlWriter.WriteRaw(Name);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("LongName");
            xmlWriter.WriteRaw(LongName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("WordElements");
            xmlWriter.WriteRaw(WordElements.ToXml());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("WordListTypeId");
            xmlWriter.WriteRaw(WordListTypeId.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("WordBreakerSetId");
            xmlWriter.WriteRaw(WordBreakerSetId.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CreateDateTime");
            xmlWriter.WriteRaw(CreateDateTime.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ModDateTime");
            xmlWriter.WriteRaw(ModDateTime.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CreateUser");
            xmlWriter.WriteRaw(CreateUser);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ModUser");
            xmlWriter.WriteRaw(ModUser);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CreateProgram");
            xmlWriter.WriteRaw(CreateProgram);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ModProgram");
            xmlWriter.WriteRaw(ModProgram);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Denotes method for xml deserialization from string
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode node = doc.SelectSingleNode("WordList");

                LoadFromXml(node);
            }
        }

        /// <summary>
        /// Denotes method for xml deserialization
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            if (node != null)
            {
                if (node.Attributes != null)
                {
                    if (node.Attributes["Id"] != null)
                    {
                        Id = ValueTypeHelper.ConvertToInt32(node.Attributes["Id"].Value);
                    }
                }

                foreach (XmlNode child in node.ChildNodes)
                {
                    String nodeName = child.Name;
                    String nodeValue = child.InnerXml;
                    if (!String.IsNullOrEmpty(nodeValue))
                    {
                        switch (nodeName)
                        {
                            case "Name":
                                Name = nodeValue;
                                break;
                            case "Action":
                                ObjectAction tempAction;
                                ValueTypeHelper.EnumTryParse(nodeValue, true, out tempAction);
                                Action = tempAction;
                                break;
                            case "LongName":
                                LongName = nodeValue;
                                break;
                            case "WordElements":
                                WordElements.LoadFromXmlWithOuterNode(nodeValue);
                                break;
                            case "WordListTypeId":
                                WordListTypeId = ValueTypeHelper.ConvertToInt32(nodeValue);
                                break;
                            case "WordBreakerSetId":
                                WordBreakerSetId = ValueTypeHelper.ConvertToInt32(nodeValue);
                                break;
                            case "CreateDateTime":
                                CreateDateTime = ValueTypeHelper.ConvertToNullableDateTime(nodeValue);
                                break;
                            case "ModDateTime":
                                ModDateTime = ValueTypeHelper.ConvertToNullableDateTime(nodeValue);
                                break;
                            case "CreateUser":
                                CreateUser = nodeValue;
                                break;
                            case "ModUser":
                                ModUser = nodeValue;
                                break;
                            case "CreateProgram":
                                CreateProgram = nodeValue;
                                break;
                            case "ModProgram":
                                ModProgram = nodeValue;
                                break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}