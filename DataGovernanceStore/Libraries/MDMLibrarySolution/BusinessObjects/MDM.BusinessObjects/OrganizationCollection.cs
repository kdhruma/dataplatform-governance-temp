using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent Collection of Organization Collection Object
    /// </summary>
    [DataContract]
    public class OrganizationCollection : InterfaceContractCollection<IOrganization, Organization>, IOrganizationCollection , IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field for allowed user actions on the object
        /// </summary>
        private Collection<UserAction> _permissionSet = null;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates allowed user actions on the object
        /// </summary>
        [DataMember]
        public Collection<UserAction> PermissionSet
        {
            get { return _permissionSet; }
            set { _permissionSet = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.Organization;
            }
        }

        #endregion Properties

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the Organization Collection
        /// </summary>
        public OrganizationCollection() { }

        /// <summary>
        /// Initialize Organization Collection from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Organization Collection in xml format</param>
        public OrganizationCollection(String valuesAsXml)
        {
            LoadOrganizationCollection(valuesAsXml);
        }

        /// <summary>
        /// Constructor which takes List of type Organizations
        /// </summary>
        /// <param name="organizations"></param>
        public OrganizationCollection(IList<Organization> organizations)
        {
            this._items = new Collection<Organization>(organizations);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Check if OrganizationCollection contains Organization with given Id
        /// </summary>
        /// <param name="id">Id using which Organization is to be searched from collection</param>
        /// <returns>
        /// <para>true : If Organization found in OrganizationCollection</para>
        /// <para>false : If Organization not found in OrganizationCollection</para>
        /// </returns>
        public Boolean Contains(Int32 id)
        {
            return GetOrganization(id) != null;
        }

        /// <summary>
        /// Remove Organization object from OrganizationCollection
        /// </summary>
        /// <param name="organizationId">organizationId of Organization which is to be removed from collection</param>
        /// <returns>true if Organization is successfully removed; otherwise, false. This method also returns false if Organization was not found in the original collection</returns>
        public Boolean Remove(Int32 organizationId)
        {
            Organization organization = GetOrganization(organizationId);

            if (organization == null)
                throw new ArgumentException("No Organization found for given Id :" + organizationId);

            return this.Remove(organization);
        }

        /// <summary>
        /// Get Xml representation of OrganizationCollection
        /// </summary>
        /// <returns>Xml representation of OrganizationCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<Organizations>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Organization organization in this._items)
                {
                    returnXml = String.Concat(returnXml, organization.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</Organizations>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of OrganizationCollection
        /// </summary>
        /// <returns>Xml representation of OrganizationCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<Organizations>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Organization organization in this._items)
                {
                    returnXml = String.Concat(returnXml, organization.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</Organizations>");

            return returnXml;
        }

        /// <summary>
        /// Clone Organization collection.
        /// </summary>
        /// <returns>cloned Organization collection object.</returns>
        public IOrganizationCollection Clone()
        {
            OrganizationCollection clonedOrganizations = new OrganizationCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Organization organization in this._items)
                {
                    IOrganization clonedIOrganization = organization.Clone();
                    clonedOrganizations.Add(clonedIOrganization);
                }
            }

            return clonedOrganizations;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is OrganizationCollection)
            {
                OrganizationCollection objectToBeCompared = obj as OrganizationCollection;

                Int32 organizationUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 organizationIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (organizationUnion != organizationIntersect)
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

            foreach (Organization organization in this._items)
            {
                hashCode += organization.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Gets the organization for given name.
        /// </summary>
        /// <param name="organizationName">name of organization to be searched in the collection</param>
        /// <returns>Organization having given name</returns>
        public Organization Get(String organizationName)
        {
            organizationName = organizationName.ToLowerInvariant();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Organization organization in this._items)
                {
                    if (organization.Name.ToLowerInvariant().Equals(organizationName))
                    {
                        return organization;
                    }
                }
            }

            return null;
        }

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            OrganizationCollection organizations = GetOrganizations(referenceIds);

            if (organizations != null && organizations.Count > 0)
            {
                foreach (Organization organization in organizations)
                {
                    result = result && this.Remove(organization);
                }
            }

            return result;
        }


        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> organizationsInBatch = null;

            if (this._items != null)
            {
                organizationsInBatch = Utility.Split(this, batchSize);
            }

            return organizationsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as Organization);
        }

        #endregion

        /// <summary>
        /// Add Organizations in collection
        /// </summary>
        /// <param name="organizations">OrganizationCollection to add in collection</param>
        public void AddRange(IOrganizationCollection organizations)
        {
            if (organizations == null)
            {
                throw new ArgumentNullException("Organizations");
            }

            foreach (Organization organization in organizations)
            {
                if (!this.Contains(organization))
                {
                    this.Add(organization);
                }
            }
        }

        /// <summary>
        /// Gets the organization based on the parent organization short name.
        /// </summary>
        /// <param name="organizationName">Indicates the organization short name</param>
        /// <returns>Returns the organization</returns>
        public Organization GetOrganizationByParentName(String organizationName)
        {
            if (String.IsNullOrWhiteSpace(organizationName))
            {
                throw new ArgumentNullException("organizationName");
            }

            Organization result = null;

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Organization org in this._items)
                {
                    if (String.Compare(org.ParentOrganizationName, organizationName, true) == 0)
                    {
                        result = org;
                        break;
                    }
                }
            }
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadOrganizationCollection(string valuesAsXml)
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Organization")
                        {
                            String organizationXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(organizationXml))
                            {
                                var organization = new Organization(organizationXml);
                                if (organization != null)
                                {
                                    this.Add(organization);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Organization GetOrganization(Int32 id)
        {
            var filteredOrganization = from organization in this._items
                where organization.Id == id
                select organization;

            return filteredOrganization.Any() ? filteredOrganization.First() : null;
        }

        /// <summary>
        ///  Gets the organizations using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an organization which is to be fetched.</param>
        /// <returns>Returns filtered organizations</returns>
        private OrganizationCollection GetOrganizations(Collection<String> referenceIds)
        {
            OrganizationCollection organizations = new OrganizationCollection();
            Int32 counter = 0;

            if (this._items != null && this._items.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (Organization organization in this._items)
                {
                    if (referenceIds.Contains(organization.ReferenceId))
                    {
                        organizations.Add(organization);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return organizations;
        }

        #endregion

        #endregion
    }
}