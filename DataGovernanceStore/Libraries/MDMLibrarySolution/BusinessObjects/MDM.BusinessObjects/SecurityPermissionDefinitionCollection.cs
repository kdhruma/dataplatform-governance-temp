using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for Security Permission Definition Collection
    /// </summary>
    [DataContract]
    public class SecurityPermissionDefinitionCollection : ICollection<SecurityPermissionDefinition>, IEnumerable<SecurityPermissionDefinition>, ISecurityPermissionDefinitionCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting instance of Security Permission Definition
        /// </summary>
        [DataMember]
        private Collection<SecurityPermissionDefinition> _securityPermissionDefinitions = new Collection<SecurityPermissionDefinition>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public SecurityPermissionDefinitionCollection()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public SecurityPermissionDefinitionCollection(String valueAsXml)
        {
            LoadSecurityPermissionDefinitionCollection(valueAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region Load Methods

        /// <summary>
        /// Load Security Permission Definition Collection from Xml
        /// <param name="valuesAsXml"></param>
        /// </summary>
        public void LoadSecurityPermissionDefinitionCollection(String valuesAsXml)
        {

            #region SampleXml
            //      <SecurityPermissionDefinitions>
            //         <SecurityPermissionDefinition Id="-1" Name="Edit And View Security Permissions" LongName="Edit And View Security Permissions" RoleId="55" RoleName="Vendor01" PermissionValues="1"  PermissionSet="Edit,View">
            //             <ApplicationContext
            //                 OrganizationId="1"
            //                 OrganizationName="river Works Workspace"
            //                 ContainerId="4"
            //                 ContainerName="staging Master"
            //                 AttributeId="4053"
            //                 AttributeName="vendor Name">
            //             </ApplicationContext
            //         </SecurityPermissionDefinition>
            //</SecurityPermissionDefinitions>
            #endregion

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "SecurityPermissionDefinition")
                    {
                        String securityPermissionDefinitionXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(securityPermissionDefinitionXml))
                        {
                            SecurityPermissionDefinition securityPermissionDefinition = new SecurityPermissionDefinition(securityPermissionDefinitionXml);
                            if (securityPermissionDefinition != null)
                            {
                                this.Add(securityPermissionDefinition);
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


        #endregion

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SecurityPermissionDefinitionCollection)
            {
                SecurityPermissionDefinitionCollection objectToBeCompared = obj as SecurityPermissionDefinitionCollection;
                Int32 securityPermissionsUnion = this._securityPermissionDefinitions.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 securityPermissionsIntersect = this._securityPermissionDefinitions.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (securityPermissionsUnion != securityPermissionsIntersect)
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

            foreach (SecurityPermissionDefinition securityPermissionDefinition in this._securityPermissionDefinitions)
            {
                hashCode += securityPermissionDefinition.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region ICollection<SecurityPermissionDefinition> Members

        /// <summary>
        /// Add SecurityPermissionDefinition object in collection
        /// </summary>
        /// <param name="item">SecurityPermissionDefinition to add in collection</param>
        public void Add(SecurityPermissionDefinition item)
        {
            this._securityPermissionDefinitions.Add(item);
        }

        /// <summary>
        /// Add SecurityPermissionDefinition object in collection
        /// </summary>
        /// <param name="item">ISecurityPermissionDefinition to add in collection</param>
        public void Add(ISecurityPermissionDefinition item)
        {
            if (item != null)
            {
                this._securityPermissionDefinitions.Add((SecurityPermissionDefinition)item);
            }
        }

        /// <summary>
        /// Removes all SecurityPermissions from collection
        /// </summary>
        public void Clear()
        {
            this._securityPermissionDefinitions.Clear();
        }

        /// <summary>
        /// Determines whether the SecurityPermissionDefinitionCollection contains a specific SecurityPermission.
        /// </summary>
        /// <param name="item">The SecurityPermissionDefinition object to locate in the SecurityPermissionDefinitionCollection.</param>
        /// <returns>
        /// <para>true : If SecurityPermissionDefinition found in mappingCollection</para>
        /// <para>false : If SecurityPermissionDefinition found not in mappingCollection</para>
        /// </returns>
        public bool Contains(SecurityPermissionDefinition item)
        {
            return this._securityPermissionDefinitions.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the SecurityPermissionDefinitionCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from SecurityPermissionDefinitionCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(SecurityPermissionDefinition[] array, int arrayIndex)
        {
            this._securityPermissionDefinitions.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of SecurityPermissions in SecurityPermissionDefinitionCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._securityPermissionDefinitions.Count;
            }
        }

        /// <summary>
        /// Check if SecurityPermissionDefinitionCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the SecurityPermissionDefinitionCollection.
        /// </summary>
        /// <param name="item">The SecurityPermissionDefinition object to remove from the SecurityPermissionDefinitionCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original SecurityPermissionDefinitionCollection</returns>
        public bool Remove(SecurityPermissionDefinition item)
        {
            return this._securityPermissionDefinitions.Remove(item);
        }

        #endregion

        #region IEnumerable<SecurityPermissionDefinition> Members

        /// <summary>
        /// Returns an enumerator that iterates through a SecurityPermissionDefinitionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<SecurityPermissionDefinition> GetEnumerator()
        {
            return this._securityPermissionDefinitions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a SecurityPermissionDefinitionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._securityPermissionDefinitions.GetEnumerator();
        }

        #endregion

        #region ISecurityPermissionCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Security Permission Definition Collection object
        /// </summary>
        /// <returns>Xml string representing the Security Permission Definition Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<SecurityPermissionDefinitionCollection>";

            if (this._securityPermissionDefinitions != null && this._securityPermissionDefinitions.Count > 0)
            {
                foreach (SecurityPermissionDefinition securityPermissionDefinition in this._securityPermissionDefinitions)
                {
                    returnXml = String.Concat(returnXml, securityPermissionDefinition.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</SecurityPermissionDefinitionCollection>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Security Permission Definition Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization"></param>
        /// <returns></returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<SecurityPermissionDefinitionCollection>";

            if (this._securityPermissionDefinitions != null && this._securityPermissionDefinitions.Count > 0)
            {
                foreach (SecurityPermissionDefinition securityPermissionDefinition in this._securityPermissionDefinitions)
                {
                    returnXml = String.Concat(returnXml, securityPermissionDefinition.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</SecurityPermissionDefinitionCollection>");

            return returnXml;
        }

        #endregion ToXml methods

        #endregion ISecurityPermissionDefinitionCollection Members

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}