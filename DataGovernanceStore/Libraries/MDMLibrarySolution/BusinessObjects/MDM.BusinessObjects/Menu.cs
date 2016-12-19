using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;  

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;      
    
    /// <summary>
    /// Class denoting Menu Object
    /// </summary>    
    [DataContract]
    public class Menu : MDMObject, IMenu
    {
        #region Fields

        /// <summary>
        /// Field Denoting Id of ParentMenu
        /// </summary>
        private Int32 _menuParentId = 0;

        /// <summary>
        /// Field Denoting Sequence of Menu
        /// </summary>
        private Int32 _sequence = 0;

        /// <summary>
        /// Field Denoting Link of the Menu
        /// </summary>
        private String _link = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter Less Constructor
        /// </summary>
        public Menu()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Menu Instance</param>
        public Menu(Int32 id)
            : base(id)
        {
            
        }

        /// <summary>
        /// Contructor with All properties
        /// </summary>
        /// <param name="id">Indicates the Identity of a Menu Instance</param>
        /// <param name="name">Indicates the Name of a Menu Instance</param>
        /// <param name="longName">Indicates the LongName of a Menu Instance</param>
        /// <param name="sequence">Indicates the sequence of a Menu Instance</param>
        /// <param name="menuParentId">Indicates the Parent Menu Id of a Menu Instance</param>
        /// <param name="link">Indicates the Link of a Menu Instance</param>
        public Menu(Int32 id, String name, String longName, Int32 sequence, Int32 menuParentId, String link)
            : base(id, name, longName)
        {
            this._sequence = sequence;
            this._menuParentId = menuParentId;
            this._link = link;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property Denoting Id of ParentMenu
        /// </summary>
        [DataMember]
        public Int32 MenuParentId
        {
            get { return _menuParentId; }
            set { _menuParentId = value; }
        }

        /// <summary>
        /// Property Denoting Sequence of Menu
        /// </summary>
        [DataMember]
        public Int32 Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        /// <summary>
        /// Property Denoting Link of Menu
        /// </summary>
        [DataMember]
        public String Link
        {
            get { return _link; }
            set { _link = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj != null && obj is Menu)
                {
                    Menu objectToBeCompared = obj as Menu;

                    if (this.MenuParentId != objectToBeCompared.MenuParentId)
                        return false;

                    if (this.Sequence != objectToBeCompared.Sequence)
                        return false;

                    if (this.Link != objectToBeCompared.Link)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.MenuParentId.GetHashCode() ^ this.Sequence.GetHashCode() ^ this.Link.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>
        /// <returns>XML String of Menu Object</returns>
        public override String ToXml()
        {
            String menuXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Menu node start
            xmlWriter.WriteStartElement("Menu");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("MenuParentId", this.MenuParentId.ToString());
            xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Link", this.Link);

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            menuXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return menuXml;
        }

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String menuXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                menuXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Menu node start
                xmlWriter.WriteStartElement("Menu");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("MenuParentId", this.MenuParentId.ToString());
                xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("LongName", this.LongName);
                xmlWriter.WriteAttributeString("Link", this.Link);

                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                menuXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return menuXml;
        }

        #endregion
    }
}
