using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Represents class for impacted entity collection
    /// </summary>
    [DataContract]
    public class ImpactedEntityCollection : ICollection<ImpactedEntity>, IEnumerable<ImpactedEntity>, IImpactedEntityCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<ImpactedEntity> _impactedEntities = new Collection<ImpactedEntity>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ImpactedEntityCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ImpactedEntityCollection(String valueAsXml)
        {
            LoadImpactedEntityCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize ImpactedEntityCollection from IList
        /// </summary>
        /// <param name="impactedEntitysList">IList of impactedEntitys</param>
        public ImpactedEntityCollection(IList<ImpactedEntity> impactedEntitysList)
        {
            this._impactedEntities = new Collection<ImpactedEntity>(impactedEntitysList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find impactedEntity from ImpactedEntityCollection based on impactedEntityId
        /// </summary>
        /// <param name="impactedEntityId">ImpactedEntityID to search</param>
        /// <returns>ImpactedEntity object having given impactedEntityID</returns>
        public ImpactedEntity this[Int64 impactedEntityId]
        {
            get
            {
                ImpactedEntity impactedEntity = Get(impactedEntityId);

                if (impactedEntity == null)
                    throw new ArgumentException(String.Format("No impactedEntity found for impactedEntity id: {0}", impactedEntityId), "impactedEntityId");
                else
                    return impactedEntity;
            }
            set
            {
                ImpactedEntity impactedEntity = Get(impactedEntityId);

                if (impactedEntity == null)
                    throw new ArgumentException(String.Format("No impactedEntity found for impactedEntity id: {0} ", impactedEntityId), "impactedEntityId");

                impactedEntity = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if ImpactedEntityCollection contains impactedEntity with given impactedEntityId
        /// </summary>
        /// <param name="impactedEntityId">ImpactedEntityId to search in ImpactedEntityCollection</param>
        /// <returns>
        /// <para>true : If impactedEntity found in impactedEntityCollection</para>
        /// <para>false : If impactedEntity found not in impactedEntityCollection</para>
        /// </returns>
        public bool Contains(Int32 impactedEntityId)
        {
            if(Get(impactedEntityId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove impactedEntity object from ImpactedEntityCollection
        /// </summary>
        /// <param name="impactedEntityId">Id of impactedEntity which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 impactedEntityId)
        {
            ImpactedEntity impactedEntity = Get(impactedEntityId);

            if (impactedEntity == null)
                throw new ArgumentException("No impactedEntity found for given impactedEntity id");
            else
                return this.Remove(impactedEntity);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ImpactedEntityCollection)
            {
                ImpactedEntityCollection objectToBeCompared = obj as ImpactedEntityCollection;
                Int32 impactedEntitysUnion = this._impactedEntities.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 impactedEntitysIntersect = this._impactedEntities.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (impactedEntitysUnion != impactedEntitysIntersect)
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
            foreach (ImpactedEntity entity in this._impactedEntities)
            {
                hashCode += entity.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Initialize impactedEntityCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for ImpactedEntityCollection
        /// <para>Sample Xml : </para>
        /// <![CDATA[
        /// <ImpactedEntity Id="1125" Name="CORE_TECH_TXTBX_Small" LongName="CORE_TECH_Textbox ImpactedEntity Small" ImpactedEntityParentId="1124" ImpactedEntityParentName="CORE_TECH_String ImpactedEntity Group" ImpactedEntityParentLongName="CORE_TECH_String ImpactedEntity Group" IsCollection="False" IsComplex="False" isLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" ImpactedEntityType="TechImpactedEntity" ImpactedEntityDataType="String" ImpactedEntityModelType="Category" SourceFlag="O" Action="Create">
        ///     <Values SourceFlag="O">
        ///         <Value ValueRefId="0" Sequence="-1.00" Locale="en_WW">CORE_TECH_Textbox ImpactedEntity Small</Value>
        ///     </Values>
        ///     <Values SourceFlag="I" />
        /// </ImpactedEntity>
        /// ]]>
        /// </param>
        public void LoadImpactedEntityCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             * <ImpactedEntity Id="1125" Name="CORE_TECH_TXTBX_Small" LongName="CORE_TECH_Textbox ImpactedEntity Small" ImpactedEntityParentId="1124" ImpactedEntityParentName="CORE_TECH_String ImpactedEntity Group" ImpactedEntityParentLongName="CORE_TECH_String ImpactedEntity Group" IsCollection="False" IsComplex="False" isLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" ImpactedEntityType="TechImpactedEntity" ImpactedEntityDataType="String" ImpactedEntityModelType="Category" SourceFlag="O" Action="Create">
                <Values SourceFlag="O">
                  <Value ValueRefId="0" Sequence="-1.00" Locale="en_WW">CORE_TECH_Textbox ImpactedEntity Small</Value>
                </Values>
                <Values SourceFlag="I" />
              </ImpactedEntity>
             */
            #endregion Sample Xml

            if ( !String.IsNullOrWhiteSpace(valuesAsXml) )
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while ( !reader.EOF )
                    {
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "ImpactedEntity" )
                        {
                            String impactedEntityXml = reader.ReadOuterXml();
                            if ( !String.IsNullOrEmpty(impactedEntityXml) )
                            {
                                ImpactedEntity impactedEntity = new ImpactedEntity(impactedEntityXml);
                                if ( impactedEntity != null )
                                {
                                    this.Add(impactedEntity);
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
                    if ( reader != null )
                    {
                        reader.Close();
                    }
                }
            }
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Get impactedEntity from current impactedEntity collection based on ImpactedEntityId 
        /// </summary>
        /// <param name="impactedEntityId">Id of impactedEntity which is to be searched</param>
        /// <returns>ImpactedEntity having given ImpactedEntityId </returns>
        private ImpactedEntity Get(Int64 impactedEntityId)
        {
            ImpactedEntity expectedImpactedEntity = null;

            if(this._impactedEntities != null && this._impactedEntities.Count > 0)
            {
                var result = ( from impactedEntity in this._impactedEntities
                               where impactedEntity.EntityId == impactedEntityId
                               select impactedEntity );

                if(result != null && result.Count() > 0)
                {
                    expectedImpactedEntity = ( ImpactedEntity ) result;
                }
            }

            return expectedImpactedEntity;
        }

        #endregion

        #region ICollection<ImpactedEntity> Members

        /// <summary>
        /// Add impactedEntity object in collection
        /// </summary>
        /// <param name="item">impactedEntity to add in collection</param>
        public void Add(ImpactedEntity item)
        {
            this._impactedEntities.Add(item);
        }

        /// <summary>
        /// Add impactedEntitys object in collection
        /// </summary>
        /// <param name="items">impactedEntitys to add in collection</param>
        public void AddRange(IImpactedEntityCollection items)
        {
            if ( items == null )
            {
                throw new ArgumentNullException("ImpactedEntitys");
            }

            foreach (ImpactedEntity impactedEntity in items)
            {
                this.Add(impactedEntity);
            }
        }

        /// <summary>
        /// Removes all impactedEntitys from collection
        /// </summary>
        public void Clear()
        {
            this._impactedEntities.Clear();
        }

        /// <summary>
        /// Determines whether the ImpactedEntityCollection contains a specific impactedEntity.
        /// </summary>
        /// <param name="item">The impactedEntity object to locate in the ImpactedEntityCollection.</param>
        /// <returns>
        /// <para>true : If impactedEntity found in impactedEntityCollection</para>
        /// <para>false : If impactedEntity found not in impactedEntityCollection</para>
        /// </returns>
        public bool Contains(ImpactedEntity item)
        {
            return this._impactedEntities.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ImpactedEntityCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ImpactedEntityCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ImpactedEntity[] array, int arrayIndex)
        {
            this._impactedEntities.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of impactedEntitys in ImpactedEntityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._impactedEntities.Count;
            }
        }

        /// <summary>
        /// Check if ImpactedEntityCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ImpactedEntityCollection.
        /// </summary>
        /// <param name="item">The impactedEntity object to remove from the ImpactedEntityCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ImpactedEntityCollection</returns>
        public bool Remove(ImpactedEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("ImpactedEntity");
            }

            return this._impactedEntities.Remove(item);
        }

        ///// <summary>
        ///// Removes the first occurrence of a specific object from the ImpactedEntity collection
        ///// </summary>
        ///// <param name="item">The object to remove from the ImpactedEntity collection</param>
        ///// <returns>true if item is successfully removed; otherwise, false</returns>
        //public Boolean Remove(IImpactedEntity item)
        //{
        //    return this.Remove((ImpactedEntity)item);
        //}

        #endregion

        #region IEnumerable<ImpactedEntity> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ImpactedEntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ImpactedEntity> GetEnumerator()
        {
            return this._impactedEntities.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ImpactedEntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._impactedEntities.GetEnumerator();
        }

        #endregion

        #region IImpactedEntityCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ImpactedEntityCollection object
        /// </summary>
        /// <returns>Xml string representing the ImpactedEntityCollection</returns>
        public String ToXml()
        {
            String impactedEntitysXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( ImpactedEntity impactedEntity in this._impactedEntities )
            {
                builder.Append(impactedEntity.ToXml());
            }

            impactedEntitysXml = String.Format("<ImpactedEntities>{0}</ImpactedEntities>", builder.ToString());
            return impactedEntitysXml;
        }

        /// <summary>
        /// Get Xml representation of ImpactedEntityCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String impactedEntitysXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( ImpactedEntity impactedEntity in this._impactedEntities )
            {
                builder.Append(impactedEntity.ToXml(serialization));
            }

            impactedEntitysXml = String.Format("<ImpactedEntities>{0}</ImpactedEntities>", builder.ToString());
            return impactedEntitysXml;
        }

        #endregion ToXml methods

        #region ImpactedEntity Get

        /// <summary>
        /// Gets impactedEntity with specified impactedEntity Id from current entity's impactedEntitys
        /// </summary>
        /// <param name="impactedEntityId">Id of an impactedEntity to search in current entity's impactedEntitys</param>
        /// <returns>ImpactedEntity interface</returns>
        /// <exception cref="ArgumentException">ImpactedEntity Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no impactedEntitys to search in</exception>        
        public ImpactedEntity GetImpactedEntity(Int64 impactedEntityId)
        {
            if (this._impactedEntities == null)
            {
                throw new NullReferenceException("There are no impactedEntitys to search in");
            }

            if (impactedEntityId <= 0)
            {
                throw new ArgumentException("ImpactedEntity Id must be greater than 0", impactedEntityId.ToString());
            }

            ImpactedEntity impactedEntity = Get(impactedEntityId);

            return impactedEntity;
        }

        
        #endregion ImpactedEntity Get

        #endregion IImpactedEntityCollection Memebers
    }
}
