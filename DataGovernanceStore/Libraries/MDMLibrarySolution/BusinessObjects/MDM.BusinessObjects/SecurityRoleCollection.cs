using System;
using System.Collections;
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
    /// Specifies Security Role Collection Class
    /// </summary>
    [DataContract]
    public class SecurityRoleCollection : ICollection<SecurityRole>, IEnumerable<SecurityRole>, ISecurityRoleCollection, IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Indicates instance of security role collection
        /// </summary>
        [DataMember]
        private Collection<SecurityRole> _securityRoleCollection = new Collection<SecurityRole>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public SecurityRoleCollection()
            : base()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public SecurityRoleCollection(String valueAsXml)
        {
            LoadSecurityRoleCollection(valueAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleList"></param>
        public SecurityRoleCollection(IList<SecurityRole> roleList)
        {
            if (roleList != null)
            {
                this._securityRoleCollection = new Collection<SecurityRole>(roleList);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return ObjectType.Role;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of security role which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            SecurityRoleCollection securityRoles = GetSecurityRoles(referenceIds);

            if (securityRoles != null && securityRoles.Count > 0)
            {
                foreach (SecurityRole securityRole in securityRoles)
                {
                    result = result && this.Remove(securityRole);
                }
            }

            return result;
        }

        /// <summary>
        /// Clone security role collection
        /// </summary>
        /// <returns>Cloned security role collection</returns>
        public ISecurityRoleCollection Clone()
        {
            SecurityRoleCollection clonedSecurityRoles = new SecurityRoleCollection();

            if (this._securityRoleCollection != null)
            {
                foreach (SecurityRole securityRole in this._securityRoleCollection)
                {
                    SecurityRole clonedSecurityRole = (SecurityRole)securityRole.Clone();
                    clonedSecurityRoles.Add(clonedSecurityRole);
                }
            }

            return (SecurityRoleCollection)clonedSecurityRoles;
        }

        /// <summary>
        /// Find security role from current security role collection with given role name.
        /// </summary>
        /// <param name="roleName">Name of role which is to be searched in current collection</param>
        /// <returns>Security role with given name</returns>
        public SecurityRole Get(String roleName)
        {
            roleName = roleName.ToLowerInvariant();

            Int32 roleCount = _securityRoleCollection.Count;
            SecurityRole securityRole = null;

            for (Int32 index = 0; index < roleCount; index++)
            {
                securityRole = _securityRoleCollection[index];

                if (securityRole.Name.ToLowerInvariant() == roleName)
                    return securityRole;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SecurityRole GetPrivateRole()
        {
            Int32 roleCount = _securityRoleCollection.Count;
            SecurityRole securityRole = null;

            for (Int32 index = 0; index < roleCount; index++)
            {
                securityRole = _securityRoleCollection[index];

                if (securityRole.IsPrivateRole)
                    return securityRole;
            }

            return null;
        }

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SecurityRoleCollection)
            {
                SecurityRoleCollection objectToBeCompared = obj as SecurityRoleCollection;
                Int32 SecurityRoleUnion = this._securityRoleCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 SecurityRolesIntersect = this._securityRoleCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (SecurityRoleUnion != SecurityRolesIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (SecurityRole securityRole in this._securityRoleCollection)
            {
                hashCode += securityRole.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region ICollection<SecurityRole> Members

        /// <summary>
        /// Add SecurityRole object in collection
        /// </summary>
        /// <param name="item">SecurityRole to add in collection</param>
        public void Add(SecurityRole item)
        {
            this._securityRoleCollection.Add(item);
        }

        /// <summary>
        /// Add SecurityRole object in collection
        /// </summary>
        /// <param name="item">SecurityRole to add in collection</param>
        public void Add(ISecurityRole item)
        {
            if (item != null)
            {
                this.Add(( SecurityRole )item);
            }
        }

        /// <summary>
        /// Removes all SecurityRole from collection
        /// </summary>
        public void Clear()
        {
            this._securityRoleCollection.Clear();
        }

        /// <summary>
        /// Determines whether the SecurityRoleCollection contains a specific SecurityRole.
        /// </summary>
        /// <param name="item">The SecurityRole object to locate in the SecurityRoleCollection.</param>
        /// <returns>
        /// <para>true : If SecurityRole found in mappingCollection</para>
        /// <para>false : If SecurityRole found not in mappingCollection</para>
        /// </returns>
        public bool Contains(SecurityRole item)
        {
            return this._securityRoleCollection.Contains(item);
        }

        /// <summary>
        /// Determines whether the SecurityRoleCollection contains a specific SecurityRole.
        /// </summary>
        /// <param name="item">The SecurityRole object to locate in the SecurityRoleCollection.</param>
        /// <returns>
        /// <para>true : If SecurityRole found in mappingCollection</para>
        /// <para>false : If SecurityRole found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ISecurityRole item)
        {
            if (item != null)
            {
                return this.Contains(( SecurityRole )item);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the SecurityRoleCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from SecurityPermissionDefinitionCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(SecurityRole[] array, int arrayIndex)
        {
            this._securityRoleCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of SecurityRoles in SecurityRoleCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._securityRoleCollection.Count;
            }
        }

        /// <summary>
        /// Check if SecurityRoleCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the SecurityRoleCollection.
        /// </summary>
        /// <param name="item">The SecurityRole object to remove from the SecurityRoleCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original SecurityRoleCollection</returns>
        public bool Remove(SecurityRole item)
        {
            return this._securityRoleCollection.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the SecurityRoleCollection.
        /// </summary>
        /// <param name="item">The SecurityRole object to remove from the SecurityRoleCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original SecurityRoleCollection</returns>
        public bool Remove(ISecurityRole item)
        {
            if (item != null)
            {
                return this.Remove(( SecurityRole )item);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<SecurityRole> Members

        /// <summary>
        /// Returns an enumerator that iterates through a SecurityRoleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<SecurityRole> GetEnumerator()
        {
            return this._securityRoleCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a SecurityRoleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._securityRoleCollection.GetEnumerator();
        }

        #endregion

        #region ISecurityRoleCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of SecurityRoleCollection object
        /// </summary>
        /// <returns>Xml string representing the SecurityRoleCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<SecurityRoles>";

            if (this._securityRoleCollection != null && this._securityRoleCollection.Count > 0)
            {
                foreach (SecurityRole role in this._securityRoleCollection)
                {
                    returnXml = String.Concat(returnXml, role.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</SecurityRoles>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of ExportSubscriberCollection
        /// </summary>
        /// <returns>Xml representation of ExportSubscriberCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<SecurityRoles>";

            if (this._securityRoleCollection != null && this._securityRoleCollection.Count > 0)
            {
                foreach (SecurityRole role in this._securityRoleCollection)
                {
                    returnXml = String.Concat(returnXml, role.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</SecurityRoles>");

            return returnXml;
        }

        #endregion ToXml methods

        #endregion ISecurityRoleCollection Members

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> securityRolesInBatch = null;

            if (this._securityRoleCollection != null)
            {
                securityRolesInBatch = Utility.Split(this, batchSize);
            }

            return securityRolesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as SecurityRole);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Load the Security Role Collection Object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates Xml of Security Role</param>
        private void LoadSecurityRoleCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * <SecurityRoles>
                  <Role Id="4" Name="Unit Test - Role1" LongName="Unit Test - Role1_LN" SecurityUserType="Internal" Action="Read">
                    <Users />
                  </Role>
                </SecurityRoles>
			 */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Role")
                        {
                            String roleXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(roleXml))
                            {
                                SecurityRole role = new SecurityRole(roleXml);
                                if (role != null)
                                {
                                    this.Add(role);
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
        ///  Gets the security role using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of security role which is to be fetched.</param>
        /// <returns>Returns filtered security roles</returns>
        private SecurityRoleCollection GetSecurityRoles(Collection<String> referenceIds)
        {
            SecurityRoleCollection securityRoles = new SecurityRoleCollection();
            Int32 counter = 0;

            if (this._securityRoleCollection != null && this._securityRoleCollection.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (SecurityRole securityRole in this._securityRoleCollection)
                {
                    if (referenceIds.Contains(securityRole.ReferenceId))
                    {
                        securityRoles.Add(securityRole);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return securityRoles;
        }

        #endregion

        #endregion
    }
}