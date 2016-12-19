using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using ProtoBuf;


    /// <summary>
    /// Specifies read result of an entityget operations
    /// </summary>
    [DataContract]
    public class EntityReadResult : IEntityReadResult
    {
        #region Fields

        /// <summary>
        /// Field denoting list of entities, which were retrieved
        /// </summary>
        private EntityCollection _entityCollection = null;

        /// <summary>
        /// Field denoting list if failed entity result collection
        /// </summary>
        private EntityOperationResultCollection _entityOperationResults = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting list of entities, which were retrieved
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public EntityCollection EntityCollection
        {
            get
            {
                if (_entityCollection == null)
                {
                    _entityCollection = new EntityCollection();
                }
                return _entityCollection;
            }
            set
            {
                _entityCollection = value;
            }
        }

        /// <summary>
        /// Property denoting list if failed result collection
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public EntityOperationResultCollection EntityOperationResultCollection
        {
            get
            {
                if (_entityOperationResults == null)
                {
                    _entityOperationResults = new EntityOperationResultCollection();
                }
                return _entityOperationResults;
            }
            set
            {
                _entityOperationResults = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes new instance of entity read result
        /// </summary>
        public EntityReadResult()
        {

        }

        /// <summary>
        /// Initialize new instance of entity read result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for entity read result</param>
        public EntityReadResult(String valuesAsXml)
        {
            LoadEntityReadResult(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Entity Read Result
        /// </summary>
        /// <returns>Xml representation of Entity Read Result object</returns>
        public String ToXml()
        {
            String entityReadResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Read result node start
            xmlWriter.WriteStartElement("EntityReadResult");

            #region Write EntityCollection

            xmlWriter.WriteRaw(this.EntityCollection.ToXml());

            #endregion Write EntityCollection

            #region Write EntityOperationResult collection

            xmlWriter.WriteRaw(this.EntityOperationResultCollection.ToXml());

            #endregion Write EntityOperationResult collection

            //Read result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            entityReadResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityReadResultXml;
        }

        /// <summary>
        /// Provides failed operation result from result collection
        /// </summary>
        /// <param name="entityId">EntityId to be used to search operation result</param>
        /// <returns>Single instance of entity operation result</returns>
        public EntityOperationResult GetFailedOperationResult(Int64 entityId)
        {
            if (this.EntityOperationResultCollection.Contains(entityId) == false)
            {
                var failedResult = new EntityOperationResult(entityId, "");
                failedResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                _entityOperationResults.Add((IEntityOperationResult) failedResult);
            }
            return _entityOperationResults[entityId];
        }

        /// <summary>
        /// Gets collection of entity
        /// </summary>
        /// <returns>Returns collection of entity</returns>
        public IEntityCollection GetEntityCollection()
        { 
            return this.EntityCollection;
        }

        /// <summary>
        /// Gets collection of entity operation result
        /// </summary>
        /// <returns>Returns collection of entity operation result</returns>
        public IEntityOperationResultCollection GetEntityOperationResultCollection()
        {
            return this.EntityOperationResultCollection;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetEntityReadResult">Indicates an object to compare with the current Object.</param>
        /// <returns>Returns true if the specified object is equal to the current object, otherwise false.</returns>
        public Boolean IsSuperSetOf(EntityReadResult subsetEntityReadResult)
        {
            if (subsetEntityReadResult != null)
            {

                if (!((EntityCollection)this.EntityCollection).IsSuperSetOf((EntityCollection)subsetEntityReadResult.EntityCollection))
                {
                    return false;
                }

                if (!((EntityOperationResultCollection)this.EntityOperationResultCollection).IsSuperSetOf((EntityOperationResultCollection)subsetEntityReadResult.EntityOperationResultCollection))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadEntityReadResult(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entities")
                        {
                            #region Read entities

                            String entitiesXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(entitiesXml))
                            {
                                this.EntityCollection = new EntityCollection(entitiesXml);
                            }

                            #endregion Read entities

                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityOperationResults")
                        {
                            #region Read operation results

                            String operationResultsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(operationResultsXml))
                            {
                                this.EntityOperationResultCollection = new EntityOperationResultCollection(operationResultsXml);
                            }

                            #endregion Read operation results
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

        #endregion Private Methods

        #endregion Methods
    }
}
