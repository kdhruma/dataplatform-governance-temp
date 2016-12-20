using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using ProtoBuf;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;

    /// <summary>
    /// Specifies EntityScope which indicates entity ids along with scope options
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityScope : ObjectBase, IEntityScope
    {
        #region Fields

        /// <summary>
        /// field denoting entity context collection for entities to be fetched
        /// </summary>
        EntityContextCollection _entityContextCollection = null;

        /// <summary>
        /// field denoting Ids of entities for which information needs to be fetched
        /// </summary>
        Collection<Int64> _entityIdList = null;

       
        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityScope()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes entitycontext, entityIdList as input parameters
        /// </summary>
        /// <param name="entityContextCollection">Entity context collection for entities to be fetched</param>
        /// <param name="entityIdList">Ids of entities for which information needs to be fetched</param>
        public EntityScope(EntityContextCollection entityContextCollection, Collection<Int64> entityIdList)
        {
            this._entityContextCollection = entityContextCollection;
            this.EntityIdList = entityIdList;
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityScope(String valuesAsXml)
        {
            LoadEntityScope(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting entity context collection for entities to be fetched
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public EntityContextCollection EntityContextCollection 
        {
            get
            {
                if (_entityContextCollection == null)
                {
                    _entityContextCollection = new EntityContextCollection();
                }
                return _entityContextCollection;
            }
            set
            {
                _entityContextCollection = value;
            }
        }

        /// <summary>
        /// Property denoting Ids of entities for which information needs to be fetched
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Collection<Int64> EntityIdList
        {
            get
            {
                if (_entityIdList == null)
                {
                    _entityIdList = new Collection<Int64>();
                }
                return _entityIdList;
            }
            set
            {
                _entityIdList = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Entity Operation Result
        /// </summary>
        /// <returns>Xml representation of Entity Operation Result object</returns>
        public String ToXml()
        {
            String entityScopeXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityScope node start
            xmlWriter.WriteStartElement("EntityScope");

            #region Write EntityIds

            //Add EntityIds node
            xmlWriter.WriteStartElement("EntityIds");

            if(this._entityIdList!=null)
            {
                xmlWriter.WriteString(String.Join(",", _entityIdList.Select(n => n.ToString()).ToArray()));
            }

            //EntityIds node end
            xmlWriter.WriteEndElement();

            #endregion Write EntityIds

            #region Write EntityContext

            if (this._entityContextCollection != null)
            {
                xmlWriter.WriteRaw(this._entityContextCollection.ToXml());
            }
            
            #endregion Write EntityContext

            //EntityScope node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            entityScopeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityScopeXml;
        }

        /// <summary>
        /// Adds entitycontext assciated with this entityscope
        /// </summary>
        /// <param name="iEntityContext">Indicates an instance of entitycontext</param>
        public void AddContext(IEntityContext iEntityContext)
        {
            if (iEntityContext != null)
            {
                this.EntityContextCollection.Add((EntityContext)iEntityContext);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadEntityScope(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityIds")
                        {
                            #region Read EntityIds

                            String entityIdsAsString = reader.ReadElementContentAsString();

                            if (!String.IsNullOrWhiteSpace(entityIdsAsString))
                            {
                                this._entityIdList = SplitEntityIds(entityIdsAsString);
                            }

                            #endregion Read EntityIds
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityContext")
                        {
                            #region Read EntityContext

                            String entityContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(entityContextXml))
                            {
                                this._entityContextCollection.Add(new EntityContext(entityContextXml));
                            }

                            #endregion Read EntityContext
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        private Collection<Int64> SplitEntityIds(String entityIdsString)
        {
            Boolean checkIntStatus = false;
            Int64 checkInt = 0;
            Collection<Int64> entityIdList = null;

            if (entityIdsString.Length > 0)
            {
                entityIdList = new Collection<long>();
                foreach (string element in entityIdsString.Split(new char[] { ',' }))
                {
                    checkIntStatus = Int64.TryParse(element.Trim(), out checkInt);

                    if (checkIntStatus)
                    {
                        entityIdList.Add(checkInt);
                    }
                }
            }

            return entityIdList;
        }

        #endregion Private Methods
    }
}
