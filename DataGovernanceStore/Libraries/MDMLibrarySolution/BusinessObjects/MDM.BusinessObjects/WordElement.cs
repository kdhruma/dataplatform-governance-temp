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
    public class WordElement : MDMObject, IWordElement, IEquatable<WordElement>, IDataModelObject
    {
        #region Fields

        private String _word = String.Empty;
        private String _substitute = String.Empty;
        private Int32 _wordListId = -1;
        private Int32? _sequence;
        private DateTime? _createDateTime;
        private DateTime? _modDateTime;
        private String _createUser = String.Empty;
        private String _modUser = String.Empty;
        private String _createProgram = String.Empty;
        private String _modProgram = String.Empty;
        private String _encodedWord = String.Empty;
        private String _wordListName = String.Empty;
        private String _externalId = String.Empty;
        private WordElement _originalWordElement;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public WordElement() { }

        /// <summary>
        /// Initialize WordElement from String in Xml format
        /// </summary>
        /// <param name="valueAsXml"></param>
        public WordElement(String valueAsXml)
        {
            LoadFromXmlWithOuterNode(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Denotes the Word
        /// </summary>
        [DataMember]
        public String Word
        {
            get { return _word; }
            set { _word = value; }
        }

        /// <summary>
        /// Denotes the Substitute
        /// </summary>
        [DataMember]
        public String Substitute
        {
            get { return _substitute; }
            set { _substitute = value; }
        }

        /// <summary>
        /// Denotes the WordList id
        /// </summary>
        [DataMember]
        public Int32 WordListId
        {
            get { return _wordListId; }
            set { _wordListId = value; }
        }

        /// <summary>
        /// Denotes the WordList ShortName
        /// </summary>
        [DataMember]
        public String WordListName
        {
            get { return _wordListName; }
            set { _wordListName = value; }
        }

        /// <summary>
        /// Denotes sequence
        /// </summary>
        [DataMember]
        public Int32? Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
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
        /// Denotes modification date time
        /// </summary>
        [DataMember]
        public DateTime? ModDateTime
        {
            get { return _modDateTime; }
            set { _modDateTime = value; }
        }

        /// <summary>
        /// Denotes user that created word element
        /// </summary>
        [DataMember]
        public String CreateUser
        {
            get { return _createUser; }
            set { _createUser = value; }
        }

        /// <summary>
        /// Denotes the last user that modified word element
        /// </summary>
        [DataMember]
        public String ModUser
        {
            get { return _modUser; }
            set { _modUser = value; }
        }

        /// <summary>
        /// Denotes the program that created word element
        /// </summary>
        [DataMember]
        public String CreateProgram
        {
            get { return _createProgram; }
            set { _createProgram = value; }
        }

        /// <summary>
        /// Denotes the last program that modified word element
        /// </summary>
        [DataMember]
        public String ModProgram
        {
            get { return _modProgram; }
            set { _modProgram = value; }
        }

        /// <summary>
        /// Denotes encoded word
        /// </summary>
        [DataMember]
        public String EncodedWord
        {
            get { return _encodedWord; }
            set { _encodedWord = value; }
        }

        /// <summary>
        /// Property denoting the external id for an DataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        /// <summary>
        /// Property denoting a ObjectType for DataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return Core.ObjectType.WordElement;
            }
        }

        /// <summary>
        /// Property denoting the original Word Element
        /// </summary>
        public WordElement OriginalWordElement
        {
            get
            {
                return _originalWordElement;
            }
            set
            {
                _originalWordElement = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Delta Merge of WordElements Values
        /// </summary>
        /// <param name="deltaWordElement">WordElement that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged WordElement instance</returns>
        public IWordElement MergeDelta(IWordElement deltaWordElement, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IWordElement mergedWordElement = returnClonedObject ? (IWordElement)deltaWordElement.Clone() : deltaWordElement;

            mergedWordElement.Action = mergedWordElement.WordListId == WordListId && mergedWordElement.Word.Equals(Word) && mergedWordElement.Substitute.Equals(Substitute) ?
                ObjectAction.Read : ObjectAction.Update;

            return mergedWordElement;
        }

        /// <summary>
        /// Denotes method for cloning
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Denotes method for equality comparison
        /// </summary>
        public Boolean Equals(WordElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && String.Equals(_word, other._word) &&
                   String.Equals(_substitute, other._substitute) && _wordListId == other._wordListId &&
                   _createDateTime.Equals(other._createDateTime) && _sequence == other._sequence &&
                   _modDateTime.Equals(other._modDateTime) && String.Equals(_createProgram, other._createProgram) &&
                   String.Equals(_modUser, other._modUser) && String.Equals(_createUser, other._createUser) &&
                   String.Equals(_encodedWord, other._encodedWord) && String.Equals(_modProgram, other._modProgram);
        }

        /// <summary>
        /// Denotes method for equality comparison
        /// </summary>
        public override Boolean Equals(Object obj)
        {
            return Equals(obj as WordElement);
        }

        /// <summary>
        /// Denotes method for getting hashcode
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (_word != null ? _word.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_substitute != null ? _substitute.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _wordListId;
                hashCode = (hashCode*397) ^ _createDateTime.GetHashCode();
                hashCode = (hashCode*397) ^ _sequence.GetHashCode();
                hashCode = (hashCode*397) ^ _modDateTime.GetHashCode();
                hashCode = (hashCode*397) ^ (_createProgram != null ? _createProgram.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_modUser != null ? _modUser.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_createUser != null ? _createUser.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_encodedWord != null ? _encodedWord.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_modProgram != null ? _modProgram.GetHashCode() : 0);
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

            xmlWriter.WriteStartElement("WordElement");

            xmlWriter.WriteAttributeString("Id", Id.ToString());

            xmlWriter.WriteStartElement("Action");
            xmlWriter.WriteRaw(Action.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Word");
            xmlWriter.WriteRaw(Word);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Substitute");
            xmlWriter.WriteRaw(Substitute);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("WordListId");
            xmlWriter.WriteRaw(WordListId.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Sequence");
            xmlWriter.WriteRaw(Sequence.ToString());
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

            xmlWriter.WriteStartElement("EncodedWord");
            xmlWriter.WriteRaw(EncodedWord);
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
                XmlNode node = doc.SelectSingleNode("WordElement");

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
                            case "Action":
                                ObjectAction tempAction;
                                ValueTypeHelper.EnumTryParse(nodeValue, true, out tempAction);
                                Action = tempAction;
                                break;
                            case "Word":
                                Word = nodeValue;
                                break;
                            case "Substitute":
                                Substitute = nodeValue;
                                break;
                            case "WordListId":
                                WordListId = ValueTypeHelper.ConvertToInt32(nodeValue);
                                break;
                            case "WordListName":
                                WordListName = nodeValue;
                                break;
                            case "Sequence":
                                Sequence = ValueTypeHelper.ConvertToNullableInt32(nodeValue);
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
                            case "EncodedWord":
                                EncodedWord = nodeValue;
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion
    }
}