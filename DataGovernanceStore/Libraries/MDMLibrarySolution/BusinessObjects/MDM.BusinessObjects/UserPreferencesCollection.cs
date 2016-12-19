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
    /// Specifies User Preferences Collection
    /// </summary>
    [DataContract]
    public class UserPreferencesCollection : ICollection<UserPreferences>, IEnumerable<UserPreferences>, IUserPreferencesCollection, IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of User Preferences
        /// </summary>
        [DataMember]
        private Collection<UserPreferences> _userPreferencesCollection = new Collection<UserPreferences>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the User Preferences Collection
        /// </summary>
        public UserPreferencesCollection() : base() { }

        /// <summary>
        /// Initialize UserPreferencesCollection from IList of value
        /// </summary>
        /// <param name="userPreferencesList">List of UserPreferences object</param>
        public UserPreferencesCollection(IList<UserPreferences> userPreferencesList)
        {
            this._userPreferencesCollection = new Collection<UserPreferences>(userPreferencesList);
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public UserPreferencesCollection(String valueAsXml)
        {
            LoadUserPreferencesCollection(valueAsXml);
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
                return ObjectType.UserPreferences;
            }
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Clone UserPreferences collection.
        /// </summary>
        /// <returns>Cloned UserPreferences collection object.</returns>
        public IUserPreferencesCollection Clone()
        {
            UserPreferencesCollection clonedUserPreferencesCollection = new UserPreferencesCollection();

            if (this._userPreferencesCollection != null && this._userPreferencesCollection.Count > 0)
            {
                foreach (UserPreferences userPreferences in this._userPreferencesCollection)
                {
                    IUserPreferences clonedIUserPreferences = userPreferences.Clone();
                    clonedUserPreferencesCollection.Add(clonedIUserPreferences);
                }
            }

            return clonedUserPreferencesCollection;
        }



        /// <summary>
        /// Get Xml representation of User Preferences Collection
        /// </summary>
        /// <returns>Xml representation of User Preferences Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<UserPreferencesCollection>";

            if (this._userPreferencesCollection != null && this._userPreferencesCollection.Count > 0)
            {
                foreach (UserPreferences userPreferences in this._userPreferencesCollection)
                {
                    returnXml = String.Concat(returnXml, userPreferences.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</UserPreferencesCollection>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is UserPreferencesCollection)
            {
                UserPreferencesCollection objectToBeCompared = obj as UserPreferencesCollection;

                Int32 userPreferencesCollectionUnion = this._userPreferencesCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 userPreferencesCollectionIntersect = this._userPreferencesCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (userPreferencesCollectionUnion != userPreferencesCollectionIntersect)
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

            foreach (UserPreferences userPreferences in this._userPreferencesCollection)
            {
                hashCode += userPreferences.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region ICollection<UserPreferences> Members

        /// <summary>
        /// Add user preferences object in collection
        /// </summary>
        /// <param name="userPreferences">User Preferences to add in collection</param>
        public void Add(UserPreferences userPreferences)
        {
            this._userPreferencesCollection.Add(userPreferences);
        }

        /// <summary>
        /// Add user preferences object in collection
        /// </summary>
        /// <param name="userPreferences">user preferences to add in collection</param>
        public void Add(IUserPreferences userPreferences)
        {
            if (userPreferences != null)
            {
                Add((UserPreferences)userPreferences);
            }
        }

        /// <summary>
        /// Add passed user preferences to the current collection
        /// </summary>
        /// <param name="userPreferencesCollection">Collection of user preferences collection which needs to be added</param>
        public void AddRange(UserPreferencesCollection userPreferencesCollection)
        {
            if (userPreferencesCollection != null && userPreferencesCollection.Count > 0)
            {
                foreach (UserPreferences userPreferences in userPreferencesCollection)
                {
                    this.Add(userPreferences);
                }
            }
        }

        /// <summary>
        /// Removes all user preferences from collection
        /// </summary>
        public void Clear()
        {
            this._userPreferencesCollection.Clear();
        }

        /// <summary>
        /// Copies the elements of the UserPreferencesCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(UserPreferences[] array, int arrayIndex)
        {
            this._userPreferencesCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of user preferences in UserPreferencesCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._userPreferencesCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityTypeCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Check if UserPreferencesCollection contains given user preferences
        /// </summary>
        /// <param name="userPreferences">user preferences which is to be searched from collection</param>
        /// <returns>
        /// <para>true : If user preferences found in UserPreferencesCollection</para>
        /// <para>false : If user preferences found not in UserPreferencesCollection</para>
        /// </returns>
        public bool Contains(UserPreferences userPreferences)
        {
            return this._userPreferencesCollection.Contains(userPreferences);
        }

        /// <summary>
        /// Remove user preferences object from UserPreferencesCollection
        /// </summary>
        /// <param name="userPreferences">user preferences which is to be searched from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(UserPreferences userPreferences)
        {
            return this._userPreferencesCollection.Remove(userPreferences);
        }

        #endregion

        #region IEnumerable<UserPreferences> Members

        /// <summary>
        /// Returns an enumerator that iterates through a UserPreferencesCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<UserPreferences> GetEnumerator()
        {
            return this._userPreferencesCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a UserPreferencesCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._userPreferencesCollection.GetEnumerator();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetUserPreferencesCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>        
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(UserPreferencesCollection subsetUserPreferencesCollection, Boolean compareIds = false)
        {
            if (subsetUserPreferencesCollection != null && subsetUserPreferencesCollection.Count > 0)
            {
                foreach (UserPreferences userPreferences in subsetUserPreferencesCollection)
                {
                    //Get subset user preferences from super user preferences collection.
                    UserPreferences sourceUserPreferences = this.GetUserPreferences(userPreferences.Name);

                    if (sourceUserPreferences != null)
                    {
                        if (!sourceUserPreferences.IsSuperSetOf(userPreferences, compareIds))
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

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            UserPreferencesCollection userPreferencesCollection = GetUserPreferencesCollection(referenceIds);

            if (userPreferencesCollection != null && userPreferencesCollection.Count > 0)
            {
                foreach (UserPreferences userPreferences in userPreferencesCollection)
                {
                    result = result && this.Remove(userPreferences);
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
            Collection<IDataModelObjectCollection> spliteUserPreferences = null;

            if (this._userPreferencesCollection != null)
            {
                spliteUserPreferences = Utility.Split(this, batchSize);
            }

            return spliteUserPreferences;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as UserPreferences);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize UserPreferences Collection from Xml
        /// </summary>
        /// <param name="valuesAsXml">>Xml having value for User Preferences Collection</param>
        private void LoadUserPreferencesCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "UserPreferences")
                        {
                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                UserPreferences userPreferences = new UserPreferences(attributeXml);
                                if (userPreferences != null)
                                {
                                    this.Add(userPreferences);
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
        /// <param name="userPreferences"></param>
        /// <returns></returns>
        private UserPreferences GetUserPreferences(String userPreferences)
        {
            var filteredUserPreferences = from type in this._userPreferencesCollection
                                          where type.Name.Equals(userPreferences)
                                          select type;

            if (filteredUserPreferences.Any())
                return filteredUserPreferences.First();
            else
                return null;
        }

        /// <summary>
        ///  Gets the user preferences using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an user preferences which is to be fetched.</param>
        /// <returns>Returns filtered user preference collection</returns>
        private UserPreferencesCollection GetUserPreferencesCollection(Collection<String> referenceIds)
        {
            UserPreferencesCollection userPreferencesCollection = new UserPreferencesCollection();
            Int32 counter = 0;

            if (this._userPreferencesCollection != null && this._userPreferencesCollection.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (UserPreferences userPreferences in this._userPreferencesCollection)
                {
                    if (referenceIds.Contains(userPreferences.ReferenceId))
                    {
                        userPreferencesCollection.Add(userPreferences);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return userPreferencesCollection;
        }

        #endregion

        #endregion
    }
}