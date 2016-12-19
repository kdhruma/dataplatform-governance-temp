using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies Security User Collection Class
    /// </summary>
    [DataContract]
    public class SecurityUserCollection : ICollection<SecurityUser>, IEnumerable<SecurityUser>, ISecurityUserCollection, IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Indicates instance of security User collection
        /// </summary>
        [DataMember]
        private Collection<SecurityUser> _securityUsers = new Collection<SecurityUser>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public SecurityUserCollection()
            : base()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public SecurityUserCollection(String valueAsXml)
        {
            LoadSecurityUserCollection(valueAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userList"></param>
        public SecurityUserCollection(IList<SecurityUser> userList)
        {
            if (userList != null)
            {
                this._securityUsers = new Collection<SecurityUser>(userList);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// SecurityUser for a given index from the SecurityUserCollection
        /// </summary>
        /// <param name="index">Index for the Security User to be fetched</param>
        /// <returns>SecurityUser for a given index from the SecurityUserCollection</returns>
        public SecurityUser this[Int32 index]
        {
            get { return this._securityUsers[index]; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return ObjectType.User;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SecurityUserCollection)
            {
                SecurityUserCollection objectToBeCompared = obj as SecurityUserCollection;

                Int32 securityUsersUnion = this._securityUsers.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 securityUsersIntersect = this._securityUsers.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (securityUsersUnion != securityUsersIntersect)
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
            foreach (SecurityUser user in this._securityUsers)
            {
                hashCode += user.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetSecurityUserCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>        
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(SecurityUserCollection subsetSecurityUserCollection, Boolean compareIds = false)
        {
            if (subsetSecurityUserCollection != null && subsetSecurityUserCollection.Count > 0)
            {
                foreach (SecurityUser securityUser in subsetSecurityUserCollection)
                {
                    //Get sub set security user from super security user collection.
                    SecurityUser iSourceSecurityUser = this.GetSecurityUser(securityUser.SecurityUserLogin);

                    if (iSourceSecurityUser != null)
                    {
                        SecurityUser sourceSecurityUser = (SecurityUser)iSourceSecurityUser;

                        if (!iSourceSecurityUser.IsSuperSetOf(securityUser, compareIds))
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clone security user collection
        /// </summary>
        /// <returns>Cloned security user collection</returns>
        public ISecurityUserCollection Clone()
        {
            SecurityUserCollection clonedSecurityUsers = new SecurityUserCollection();

            if (this._securityUsers != null)
            {
                foreach (SecurityUser securityUser in this._securityUsers)
                {
                    SecurityUser clonedSecurityUser = (SecurityUser)securityUser.Clone();
                    clonedSecurityUsers.Add(clonedSecurityUser);
                }
            }

            return (SecurityUserCollection)clonedSecurityUsers;
        }

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of security user which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            SecurityUserCollection securityUsers = GetSecurityUsers(referenceIds);

            if (securityUsers != null && securityUsers.Count > 0)
            {
                foreach (SecurityUser securityUser in securityUsers)
                {
                    result = result && this.Remove(securityUser);
                }
            }

            return result;
        }

        /// <summary>
        /// Find security user from current security user collection with given user login.
        /// </summary>
        /// <param name="userLogin">Name of user login which is to be searched in current collection</param>
        /// <returns>Security user with given user login.</returns>
        public SecurityUser Get(String userLogin)
        {
            Int32 userCount = _securityUsers.Count;
            SecurityUser securityUser = null;
            userLogin = userLogin.ToLowerInvariant();

            for (Int32 index = 0; index < userCount; index++)
            {
                securityUser = _securityUsers[index];

                if (securityUser.SecurityUserLogin.ToLowerInvariant().Equals(userLogin))
                    return securityUser;
            }

            return null;
        }

        #endregion

        #region Private Methods

        private SecurityUser GetSecurityUser(Int32 id)
        {
            var filteredSecurityUser = from securityUser in this._securityUsers
                                       where securityUser.Id == id
                                       select securityUser;

            return filteredSecurityUser.Any() ? filteredSecurityUser.First() : null;
        }

        private SecurityUser GetSecurityUser(String securityUserLogin)
        {
            var filteredSecurityUser = from securityUser in this._securityUsers
                                       where securityUser.SecurityUserLogin == securityUserLogin
                                       select securityUser;

            return filteredSecurityUser.Any() ? filteredSecurityUser.First() : null;
        }
        #endregion

        #region ICollection<SecurityUser> Members

        /// <summary>
        /// Add SecurityUser object in collection
        /// </summary>
        /// <param name="item">SecurityUser to add in collection</param>
        public void Add(SecurityUser item)
        {
            this._securityUsers.Add(item);
        }

        /// <summary>
        /// Add ISecurityUser object in collection
        /// </summary>
        /// <param name="iSecurityUser">ISecurityUser to add in collection</param>
        public void Add(ISecurityUser iSecurityUser)
        {
            this._securityUsers.Add((SecurityUser)iSecurityUser);
        }

        /// <summary>
        /// Add SecurityUserCollection object in collection
        /// </summary>
        /// <param name="securityUsers">SecurityUserCollection to add in existing collection</param>
        public void AddRange(SecurityUserCollection securityUsers)
        {
            if (securityUsers != null)
            {
                foreach (SecurityUser user in securityUsers)
                {
                    this._securityUsers.Add(user);
                }
            }
        }
        
        /// <summary>
        /// Removes all SecurityUsers from collection
        /// </summary>
        public void Clear()
        {
            this._securityUsers.Clear();
        }

        /// <summary>
        /// Determines whether the SecurityUserCollection contains a specific SecurityUser.
        /// </summary>
        /// <param name="item">The SecurityUser object to locate in the SecurityUserCollection.</param>
        /// <returns>
        /// <para>true : If SecurityUser found in SecurityUserCollection</para>
        /// <para>false : If SecurityUser found not in SecurityUserCollection</para>
        /// </returns>
        public bool Contains(SecurityUser item)
        {
            return this._securityUsers.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the SecurityUserCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from SecurityUserCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(SecurityUser[] array, int arrayIndex)
        {
            this._securityUsers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of SecurityUsers in SecurityUserCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._securityUsers.Count;
            }
        }

        /// <summary>
        /// Check if SecurityUserCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the SecurityUserCollection.
        /// </summary>
        /// <param name="item">The SecurityUser object to remove from the SecurityUserCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original SecurityUserCollection</returns>
        public bool Remove(SecurityUser item)
        {
            return this._securityUsers.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the SecurityUserCollection.
        /// </summary>
        /// <param name="iSecurityUser">The SecurityUser object to remove from the SecurityUserCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original SecurityUserCollection</returns>
        public Boolean Remove(ISecurityUser iSecurityUser)
        {
            return this.Remove((SecurityUser)iSecurityUser);
        }

        #endregion

        #region IEnumerable<SecurityUser> Members

        /// <summary>
        /// Returns an enumerator that iterates through a SecurityUserCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<SecurityUser> GetEnumerator()
        {
            return this._securityUsers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a SecurityUserCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._securityUsers.GetEnumerator();
        }

        #endregion

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> spliteSecurityUsers = null;

            if (this._securityUsers != null)
            {
                spliteSecurityUsers = Utility.Split(this, batchSize);
            }

            return spliteSecurityUsers;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as SecurityUser);
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Security User Collection object
        /// </summary>
        /// <returns>Xml string representing the Security User Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<SecurityUsers>";

            if (this._securityUsers != null && this._securityUsers.Count > 0)
            {
                foreach (SecurityUser user in this._securityUsers)
                {
                    returnXml = String.Concat(returnXml, user.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</SecurityUsers>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Security User Collection
        /// </summary>
        /// <returns>Xml representation of Security User Collection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<SecurityUsers>";

            if (this._securityUsers != null && this._securityUsers.Count > 0)
            {
                foreach (SecurityUser user in this._securityUsers)
                {
                    returnXml = String.Concat(returnXml, user.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</SecurityUsers>");

            return returnXml;
        }

        #endregion ToXml methods

        #region Private methods

        /// <summary>
        /// Load the Security USer Collection Object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates Xml of Security User</param>
        public void LoadSecurityUserCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * 
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "User")
                        {
                            String userXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(userXml))
                            {
                                SecurityUser user = new SecurityUser(userXml);
                                if (user != null)
                                {
                                    this.Add(user);
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
        ///  Gets the security user using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of security user which is to be fetched.</param>
        /// <returns>Returns filtered security users</returns>
        private SecurityUserCollection GetSecurityUsers(Collection<String> referenceIds)
        {
            SecurityUserCollection securityUsers = new SecurityUserCollection();
            Int32 counter = 0;

            if (this._securityUsers != null && this._securityUsers.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (SecurityUser securityUser in this._securityUsers)
                {
                    if (referenceIds.Contains(securityUser.ReferenceId))
                    {
                        securityUsers.Add(securityUser);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return securityUsers;
        }


        #endregion Private Methods

        #endregion
    }
}