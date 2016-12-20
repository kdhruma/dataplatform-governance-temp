using System;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the category tree item object
    /// </summary>
    [DataContract]
    public class CategoryTreeItem : MDMObject, ICategoryTreeItem
    {
        #region Fields

        /// <summary>
        /// Field for the Id of a Category Tree Item
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field for the Id of a parent Category Tree Item
        /// </summary>
        private Int64 _parentId = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Property for the Id of Category Tree Item
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "CategoryTreeItem";
            }
        }

        /// <summary>
        /// Property denoting the Id of a parent Category Tree Item
        /// </summary>
        [DataMember]
        public Int64 ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        /// <summary>
        /// Property denoting the path of the Category Tree Item
        /// </summary>
        [DataMember]
        public String Path { get; set; }

        /// <summary>
        /// Property denoting the path of the Category Tree Item
        /// </summary>
        [DataMember]
        public String IdPath { get; set; }

        /// <summary>
        /// Property denoting the child items count of the Category Tree Item
        /// </summary>
        [DataMember]
        public Int64 ChildCount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Category Tree Item class
        /// </summary>
        public CategoryTreeItem()
            : base()
        {
            IdPath = null;
        }

        /// <summary>
        /// Initializes a new instance of the Category Tree Item class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="longName"></param>
        /// <param name="path"></param>
        /// <param name="idPath"></param>
        /// <param name="childCount"></param>
        public CategoryTreeItem(Int64 id, Int64 parentId, String name, String longName, String path, String idPath, Int64 childCount)
            : base()
        {
            _id = id;
            _parentId = parentId;
            Name = name;
            LongName = longName;
            Path = path;
            IdPath = idPath;
            ChildCount = childCount;
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsxml">Xml having value for current category tree item object</param>
        public CategoryTreeItem(String valuesAsxml)
        {
            IdPath = null;
            LoadCategory(valuesAsxml);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is CategoryTreeItem)
                {
                    var objectToBeCompared = obj as CategoryTreeItem;
                    return
                        (Id == objectToBeCompared.Id) &&
                        (ParentId == objectToBeCompared.ParentId) &&
                        (Path == objectToBeCompared.Path) &&
                        (IdPath == objectToBeCompared.IdPath) &&
                        (ChildCount == objectToBeCompared.ChildCount);
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override int GetHashCode()
        {
            return 
                base.GetHashCode() ^ Id.GetHashCode() ^ ParentId.GetHashCode() ^ (Path == null ? 0 : Path.GetHashCode()) ^ (IdPath == null ? 0 : IdPath.GetHashCode()) ^ ChildCount.GetHashCode();
        }

        /// <summary>
        /// Get Xml representation of CategoryTreeItem
        /// </summary>
        /// <returns>Xml representation of CategoryTreeItem</returns>
        public override String ToXml()
        {
            using (var sw = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(sw))
            {
                //CategoryTreeItem node start
                xmlWriter.WriteStartElement(ObjectType);
                xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("ParentId", ParentId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("Name", Name);
                xmlWriter.WriteAttributeString("LongName", LongName);
                xmlWriter.WriteAttributeString("Path", Path);
                if (IdPath != null)
                {
                    xmlWriter.WriteAttributeString("IdPath", IdPath);
                }
                xmlWriter.WriteAttributeString("ChildCount", ChildCount.ToString(CultureInfo.InvariantCulture));

                //CategoryTreeItem node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                return sw.ToString();
            }
        }

         /// <summary>
        /// Get Xml representation of CategoryTreeItem based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of CategoryTreeItem</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return ToXml();
        }

        /// <summary>
        /// Initialize current CategoryTreeItem object through Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for current CategoryTreeItem object </param>
        public void LoadCategory(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                using (var reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == ObjectType)
                        {
                            #region Read CategoryTreeItem Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("ParentId"))
                                {
                                    ParentId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongName"))
                                {
                                    LongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Path"))
                                {
                                    Path = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("IdPath"))
                                {
                                    IdPath = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ChildCount"))
                                {
                                    ChildCount = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                            }

                            #endregion Read CategoryTreeItem Properties
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion
    }
}