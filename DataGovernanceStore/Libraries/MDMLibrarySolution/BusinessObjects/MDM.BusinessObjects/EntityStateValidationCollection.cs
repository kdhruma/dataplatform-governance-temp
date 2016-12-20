using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies collection of entity state validation
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityStateValidationCollection : InterfaceContractCollection<IEntityStateValidation, EntityStateValidation>, IEntityStateValidationCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the entity state validation collection
        /// </summary>
        public EntityStateValidationCollection()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityStateValidationCollection(String valuesAsXml)
        {
            LoadEntityStateValidationCollection(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="entityStateValidationList">List of Entity State Validation</param>
        public EntityStateValidationCollection(IList<EntityStateValidation> entityStateValidationList)
        {
            if (entityStateValidationList != null)
            {
                this._items = new Collection<EntityStateValidation>(entityStateValidationList);
            }
        }

        /// <summary>
        /// Adds entity state validations into existing collection
        /// </summary>
        /// <param name="entityStateValidations">Indicates the Entity State Validation to add in collection</param>
        public void AddRange(EntityStateValidationCollection entityStateValidations)
        {
            if (entityStateValidations != null && entityStateValidations.Count > 0)
            {
                foreach (EntityStateValidation entityStateValidation in entityStateValidations)
                {
                    base.Add(entityStateValidation);
                }
            }
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EntityStateValidation Collection
        /// </summary>
        /// <returns>Xml representation of EntityStateValidation Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //EntityStateValidations node start
                    xmlWriter.WriteStartElement("EntityStateValidations");

                    #region Write EntityStateValidations

                    if (_items != null)
                    {
                        foreach (EntityStateValidation entityValidationState in this._items)
                        {
                            xmlWriter.WriteRaw(entityValidationState.ToXml());
                        }
                    }

                    #endregion

                    //EntityStateValidations node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of EntityStateValidation Collection
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of EntityStateValidation Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    if (objectSerialization == ObjectSerialization.External)
                    {
                        //ValidationMessages node start
                        xmlWriter.WriteStartElement("ValidationMessages");

                        #region Write EntityStateValidations

                        if (_items != null)
                        {
                            foreach (EntityStateValidation entityValidationState in this._items)
                            {
                                xmlWriter.WriteRaw(entityValidationState.ToXml(objectSerialization));
                            }
                        }

                        #endregion

                        //ValidationMessages node end
                        xmlWriter.WriteEndElement();

                    }

                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Get State of an Entity for a Business Rule
        /// </summary>
        /// <param name="entityId">Indicates the entity id</param>
        /// <param name="ruleName">Indicates the rule name</param>
        /// <returns>Returns Entity State</returns>
        public EntityStateValidation GetEntityStateForBusinessRule(Int64 entityId, String ruleName)
        {
            EntityStateValidation entityStateToReturn = null;

            if (entityId > 0 && !String.IsNullOrWhiteSpace(ruleName))
            {
                foreach (EntityStateValidation entityState in this._items)
                {
                    if (entityState.EntityId == entityId && String.Compare(entityState.RuleName, ruleName) == 0)
                    {
                        entityStateToReturn = entityState;
                        break;
                    }
                }
            }

            return entityStateToReturn;
        }

        /// <summary>
        /// Gets the specified Entity State Validation collection according to the condition.
        /// </summary>
        /// <param name="booleanCondition">Indicates the Boolean condition for getting entity state validation collection </param>
        /// <returns>Returns collection of Entity State Validation if the condition get passed </returns>
        public EntityStateValidationCollection Get(Func<EntityStateValidation, Boolean> booleanCondition)
        {
            EntityStateValidationCollection returnCollection = new EntityStateValidationCollection();

            foreach (EntityStateValidation item in this._items)
            {
                if (booleanCondition(item))
                {
                    returnCollection.Add(item);
                }
            }

            return returnCollection;
        }

        /// <summary>
        /// Gets the Entity State Validation according to the matching parameters
        /// </summary>
        /// <param name="entityName">Indicates the entity name</param>
        /// <param name="attributeName">Indicates the attribute name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="messageCode">Indicates the message code</param>
        /// <returns>Returns the EntityStateValidation object</returns>
        public EntityStateValidation GetEntityStateValidationByParams(String entityName, String attributeName,  LocaleEnum locale, String messageCode)
        {
            EntityStateValidation entityStateValidation = null;

            foreach (EntityStateValidation stateValidation in this._items)
            {
                if ((String.Compare(stateValidation.EntityName, entityName) == 0) && (String.Compare(stateValidation.AttributeName, attributeName) == 0) &&
                     locale == stateValidation.Locale && (String.Compare(stateValidation.MessageCode, messageCode) == 0))
                {
                    entityStateValidation = stateValidation;
                    break;
                }
            }

            return entityStateValidation;
        }


        /// <summary>
        /// Checks whether actual output is a superset of expected output or not
        /// </summary>
        /// <param name="subsetEntityStateValidations">Indicates EntityStateValidationCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(EntityStateValidationCollection subsetEntityStateValidations)
        {
            if (subsetEntityStateValidations != null)
            {
                foreach (EntityStateValidation subSetEntityStateValidation in subsetEntityStateValidations)
                {                  
                    EntityStateValidation sourceEntityStateValidation = this.GetEntityStateValidationByParams(subSetEntityStateValidation.EntityName, subSetEntityStateValidation.AttributeName,
                                                                                                                subSetEntityStateValidation.Locale, subSetEntityStateValidation.MessageCode);

                    //If it doesn't return, that means super set doesn't contain that entity state validation.
                    //So return false, else do further comparison
                    if (sourceEntityStateValidation != null)
                    {
                        if (!sourceEntityStateValidation.IsSuperSetOf(subSetEntityStateValidation))
                        {
                            return false;
                        }                        
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load Entity State Validation Collection
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityStateValidationCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityStateValidation")
                        {
                            String entityStateValidationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(entityStateValidationXml))
                            {
                                EntityStateValidation entityStateValidation = new EntityStateValidation(entityStateValidationXml);

                                this.Add(entityStateValidation);
                            }
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

        #endregion
    }
}