using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent Collection of ApplicationContext Collection Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class ApplicationContextCollection : InterfaceContractCollection<IApplicationContext, ApplicationContext>, IApplicationContextCollection, ICloneable
    {
        #region Contructors

        /// <summary>
        /// Initializes a new instance of the ApplicationContext Collection
        /// </summary>
        public ApplicationContextCollection() { }

        /// <summary>
        /// Initialize ApplicationContext Collection from Xml value
        /// </summary>
        /// <param name="valuesAsXml">ApplicationContext Collection in xml format</param>
        public ApplicationContextCollection(String valuesAsXml)
        {
            LoadApplicationContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Constructor which takes List of type ApplicationContexts
        /// </summary>
        /// <param name="applicationContexts"></param>
        public ApplicationContextCollection(IList<ApplicationContext> applicationContexts)
        {
            this._items = new Collection<ApplicationContext>(applicationContexts);
        }

         /// <summary>
        /// Construct the Application contexts using Entity
        /// </summary>
        /// <param name="entity">Indicates the Entity</param>
        /// <param name="objectTypeId">Indicates the object type Id</param>
        /// <param name="roleIds">Indicates the role Id collection</param>
        public ApplicationContextCollection(Entity entity, Int32 objectTypeId, Collection<Int32> roleIds)
        {
            if (entity != null)
            {
                if (roleIds != null && roleIds.Count > 0)//Each user in the application has atleast one role
                {
                    foreach (Int32 roleId in roleIds)
                    {
                        if (entity.Relationships != null && entity.Relationships.Count > 0)
                        {
                            foreach (Int32 relationId in entity.Relationships.GetRelationshipTypeIds())
                            {
                                this._items.Add(new ApplicationContext(entity, objectTypeId, relationId, roleId));
                            }
                        }
                        else
                        {
                            this._items.Add(new ApplicationContext(entity, objectTypeId, 0, roleId));
                        }
                    }
                }
                else
                {
                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        foreach (Int32 relationId in entity.Relationships.GetRelationshipTypeIds())
                        {
                            this._items.Add(new ApplicationContext(entity, objectTypeId, relationId, 0));
                        }
                    }
                    else
                    {
                        this._items.Add(new ApplicationContext(entity, objectTypeId, 0, 0));
                    }
                }
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Check if ApplicationContextCollection contains ApplicationContext with given Id
        /// </summary>
        /// <param name="id">Id using which ApplicationContext is to be searched from collection</param>
        /// <returns>
        /// <para>true : If ApplicationContext found in ApplicationContextCollection</para>
        /// <para>false : If ApplicationContext not found in ApplicationContextCollection</para>
        /// </returns>
        public Boolean Contains(Int32 id)
        {
            return GetApplicationContext(id) != null;
        }

        /// <summary>
        /// Remove ApplicationContext object from ApplicationContextCollection
        /// </summary>
        /// <param name="applicationContextId">applicationContextId of ApplicationContext which is to be removed from collection</param>
        /// <returns>true if ApplicationContext is successfully removed; otherwise, false. This method also returns false if ApplicationContext was not found in the original collection</returns>
        public Boolean Remove(Int32 applicationContextId)
        {
            IApplicationContext applicationContext = GetApplicationContext(applicationContextId);

            if (applicationContext == null)
                throw new ArgumentException("No ApplicationContext found for given Id :" + applicationContextId);

            return this.Remove(applicationContext);
        }

        /// <summary>
        /// Get Xml representation of ApplicationContextCollection
        /// </summary>
        /// <returns>Xml representation of ApplicationContextCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<ApplicationContexts>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ApplicationContext applicationContext in this._items)
                {
                    returnXml = String.Concat(returnXml, applicationContext.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</ApplicationContexts>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of ApplicationContextCollection
        /// </summary>
        /// <returns>Xml representation of ApplicationContextCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<ApplicationContexts>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ApplicationContext applicationContext in this._items)
                {
                    returnXml = String.Concat(returnXml, applicationContext.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</ApplicationContexts>");

            return returnXml;
        }

        /// <summary>
        /// Gets a cloned instance of the current applicationcontext collection object
        /// </summary>
        /// <returns>Cloned instance of the current applicationcontext collection object</returns>
        public IApplicationContextCollection Clone()
        {
            ApplicationContextCollection clonedApplicationContexts = new ApplicationContextCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ApplicationContext applicationContext in this._items)
                {
                    ApplicationContext clonedIApplicationContext = (ApplicationContext) applicationContext.Clone();
                    clonedApplicationContexts.Add(clonedIApplicationContext);
                }
            }

            return clonedApplicationContexts;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is ApplicationContextCollection)
            {
                ApplicationContextCollection objectToBeCompared = obj as ApplicationContextCollection;

                Int32 applicationContextUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 applicationContextIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (applicationContextUnion != applicationContextIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (ApplicationContext applicationContext in this._items)
            {
                hashCode += applicationContext.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Gets Application Context based on ID
        /// </summary>
        /// <param name="id">Indicates Application Context Id</param>
        /// <returns>Object of type ApplicationContext</returns>
        public IApplicationContext GetApplicationContext(Int32 id)
        {
            var filteredApplicationContext = from applicationContext in this._items
                                             where applicationContext.Id == id
                                             select applicationContext;

            return filteredApplicationContext.Any() ? filteredApplicationContext.First() : null;
        }

        /// <summary>
        /// Return ApplicationContext From List of ApplicationContext
        /// </summary>
        /// <param name="applicationContextName">Name of the ApplicationContext</param>
        /// <returns>ApplicationContext</returns>
        public ApplicationContext GetApplicationContext(String applicationContextName)
        {
            ApplicationContext applicationContextToReturn = null;

            if (!String.IsNullOrWhiteSpace(applicationContextName))
            {
                foreach (ApplicationContext applicationContext in this._items)
                {
                    if (String.Compare(applicationContext.Name, applicationContextName) == 0)
                    {
                        applicationContextToReturn = applicationContext;
                        break;
                    }
                }
            }

            return applicationContextToReturn;
        }

        /// <summary>
        /// Compares ApplicationContextCollection with current collection
        /// </summary>
        /// <param name="subSetApplicationContextCollection">Indicates ApplicationContextCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(ApplicationContextCollection subSetApplicationContextCollection)
        {
            if (subSetApplicationContextCollection != null)
            {
                foreach (ApplicationContext subSetApplicationContext in subSetApplicationContextCollection)
                {
                    ApplicationContext sourceApplicationContext = this.GetApplicationContext(subSetApplicationContext.Name);

                    //If it doesn't return, that means super set doesn't contain that mdmrule.
                    //So return false, else do further comparison
                    if (sourceApplicationContext != null)
                    {
                        if (!sourceApplicationContext.IsSuperSetOf(subSetApplicationContext))
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

        #endregion

        #region Private Methods

        private void LoadApplicationContextCollection(string valuesAsXml)
        {
            #region Sample Xml

            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContext")
                        {
                            String applicationContextXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(applicationContextXml))
                            {
                                var applicationContext = new ApplicationContext(applicationContextXml);
                                if (applicationContext != null)
                                {
                                    this.Add(applicationContext);
                                }
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

       #endregion

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current applicationcontext collection object
        /// </summary>
        /// <returns>Cloned instance of the current applicationcontext collection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #endregion
    }
}
