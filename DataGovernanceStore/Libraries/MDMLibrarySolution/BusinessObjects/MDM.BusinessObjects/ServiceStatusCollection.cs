using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using System.Collections;
    using System.Runtime.Serialization;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Class denoting collection of Service Object
    /// </summary>
    [DataContract]
    public class ServiceStatusCollection : ICollection<ServiceStatus>, IEnumerable<ServiceStatus>, IServiceStatusCollection
    {
        #region Fields

        /// <summary>
        /// Collection of ServiceStatus Object
        /// </summary>
        [DataMember]
        private Collection<ServiceStatus> _serverStatusCollection = new Collection<ServiceStatus>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ServiceStatusCollection() : base() { }

        /// <summary>
        /// Constructor with the list of ServiceStatus Object
        /// </summary>
        /// <param name="serviceStatusList">List of ServiceStatus Object</param>
        public ServiceStatusCollection(IList<ServiceStatus> serviceStatusList)
		{
            this._serverStatusCollection = new Collection<ServiceStatus>(serviceStatusList);
		}

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region IServiceStatusCollection Methods

        /// <summary>
        /// XML representation of ServiceStatusCollection
        /// </summary>
        /// <returns>ServiceStatus XML</returns>
        public string ToXml()
        {
            String serviceStatusXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ServiceStatus serviceStatus in this._serverStatusCollection)
            {
                builder.Append(serviceStatus.ToXml());
            }

            serviceStatusXml = String.Format("<ServiceStatusCollection>{0}</ServiceStatusCollection>", builder.ToString());

            return serviceStatusXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ServiceStatusCollection)
            {
                ServiceStatusCollection objectToBeCompared = obj as ServiceStatusCollection;
                Int32 serverStatusUnion = this._serverStatusCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 serviceStatusIntersect = this._serverStatusCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (serverStatusUnion != serviceStatusIntersect)
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
            foreach (ServiceStatus serviceStatus in this._serverStatusCollection)
            {
                hashCode += serviceStatus.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region IEnumerable<ServiceStatus> Methods

        /// <summary>
        /// Returns an enumerator that iterates through a ServerStatusCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ServiceStatus> GetEnumerator()
        {
            return this._serverStatusCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a ServerStatusCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._serverStatusCollection.GetEnumerator();
        }

        #endregion

        #region ICollection<ServiceStatus> Methods

        /// <summary>
        /// Add ServiceStatus object in collection
        /// </summary>
        /// <param name="item">ServiceStatus to add in collection</param>
        public void Add(ServiceStatus item)
        {
            this._serverStatusCollection.Add(item);
        }

        /// <summary>
        /// Removes all ServiceStatus from collection
        /// </summary>
        public void Clear()
        {
            this._serverStatusCollection.Clear();
        }

        /// <summary>
        /// Determines whether the ServiceStatusCollection contains a specific ServiceStatus.
        /// </summary>
        /// <param name="item">The ServiceStatus object to locate in the ServiceStatusCollection.</param>
        /// <returns>
        /// <para>true : If ServiceStatus found in mappingCollection</para>
        /// <para>false : If ServiceStatus found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ServiceStatus item)
        {
            return this._serverStatusCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ServiceStatusCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ServiceStatusCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ServiceStatus[] array, int arrayIndex)
        {
            this._serverStatusCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ServiceStatus in ServiceStatusCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._serverStatusCollection.Count;
            }
        }

        /// <summary>
        /// Check if ServiceStatusCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ServiceStatusCollection.
        /// </summary>
        /// <param name="item">The Locale object to remove from the ServiceStatusCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ServiceStatusCollection</returns>
        public bool Remove(ServiceStatus item)
        {
            return this._serverStatusCollection.Remove(item);
        }

        #endregion

        #endregion
    }
}
