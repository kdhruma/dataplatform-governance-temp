using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.DynamicTableSchema
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class DBConstraintCollection : ICollection<DBConstraint>, IEnumerable<DBConstraint>, IDBConstraintCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Constraint
        /// </summary>
        [DataMember]
        private Collection<DBConstraint> _dbConstraints = new Collection<DBConstraint>();

        #endregion

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DBConstraintCollection() : base() { }

        /// <summary>
        /// Initialize DBConstraintCollection from IList
        /// </summary>
        /// <param name="constraintList">IList of DBConstraints</param>
        public DBConstraintCollection(IList<DBConstraint> constraintList)
        {
            if (constraintList != null)
            {
                this._dbConstraints = new Collection<DBConstraint>(constraintList);
            }
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DBConstraintCollection(String valueAsXml)
        {
            LoadDBConstraintCollection(valueAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is DBConstraintCollection)
            {
                DBConstraintCollection objectToBeCompared = obj as DBConstraintCollection;
                Int32 DBConstraintsUnion = this._dbConstraints.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 DBConstraintsIntersect = this._dbConstraints.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (DBConstraintsUnion != DBConstraintsIntersect)
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
            foreach (DBConstraint DBConstraint in this._dbConstraints)
            {
                hashCode += DBConstraint.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Clone DBConstraintCollection object
        /// </summary>
        /// <returns>Cloned copy of IDBConstraintCollection object.</returns>
        public IDBConstraintCollection Clone()
        {
            DBConstraintCollection clonedDBConstraints = new DBConstraintCollection();

            if (this._dbConstraints != null && this._dbConstraints.Count > 0)
            {
                foreach (DBConstraint dbConstraint in this._dbConstraints)
                {
                    clonedDBConstraints.Add((DBConstraint)dbConstraint.Clone());
                }
            }

            return clonedDBConstraints;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public DBConstraintCollection Get(String columnName)
        {
            columnName = columnName.ToLowerInvariant();
            DBConstraintCollection dbContraints = new DBConstraintCollection();

            if (this._dbConstraints != null && this._dbConstraints.Count > 0)
            {
                foreach (DBConstraint dbConstraint in this._dbConstraints)
                {
                    if (dbConstraint.ColumnName.ToLowerInvariant().Equals(columnName))
                    {
                        dbContraints.Add(dbConstraint);
                    }
                }
            }

            return dbContraints;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="constraintType"></param>
        /// <returns></returns>
        public DBConstraint Get(String columnName, ConstraintType constraintType)
        {
            columnName = columnName.ToLowerInvariant();

            if (this._dbConstraints != null && this._dbConstraints.Count > 0)
            {
                foreach (DBConstraint dbConstraint in this._dbConstraints)
                {
                    if (dbConstraint.ColumnName.ToLowerInvariant().Equals(columnName) && dbConstraint.ConstraintType == constraintType)
                    {
                        return dbConstraint;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Set provided object action to each instance of constraint in the collection
        /// </summary>
        /// <param name="objectAction">Specifies object action to be update</param>
        public void SetAction(ObjectAction objectAction)
        {
            foreach (DBConstraint dbContraint in this._dbConstraints)
            {
                dbContraint.Action = objectAction;
            }
        }

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of DBConstraintCollection object
        /// </summary>
        /// <returns>Xml string representing the DBConstraintCollection</returns>
        public String ToXml()
        {
            String dbConstraintsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DBConstraint dbConstraint in this._dbConstraints)
            {
                builder.Append(dbConstraint.ToXml());
            }

            dbConstraintsXml = String.Format("<Constraints>{0}</Constraints>", builder.ToString());

            return dbConstraintsXml;
        }

        #endregion ToXml Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadDBConstraintCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <Columns></Columns>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Constraint")
                        {
                            String DBConstraintXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(DBConstraintXml))
                            {
                                DBConstraint DBConstraint = new DBConstraint(DBConstraintXml);
                                this.Add(DBConstraint);
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

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBConstraintCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._dbConstraints.GetEnumerator();
        }

        #endregion

        #region IEnumerable<DBConstraint> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DBConstraintCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DBConstraint> GetEnumerator()
        {
            return this._dbConstraints.GetEnumerator();
        }

        #endregion

        #region ICollection<DBConstraint> Members

        /// <summary>
        /// Add DBConstraint object in collection
        /// </summary>
        /// <param name="item">DBConstraint to add in collection</param>
        public void Add(DBConstraint item)
        {
            this._dbConstraints.Add(item);
        }

        /// <summary>
        /// Removes all DBConstraints from collection
        /// </summary>
        public void Clear()
        {
            this._dbConstraints.Clear();
        }

        /// <summary>
        /// Determines whether the DBConstraintCollection contains a specific DBConstraint.
        /// </summary>
        /// <param name="item">The DBConstraint object to locate in the DBConstraintCollection.</param>
        /// <returns>
        /// <para>true : If DBConstraint found in mappingCollection</para>
        /// <para>false : If DBConstraint found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DBConstraint item)
        {
            return this._dbConstraints.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DBConstraintCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DBConstraintCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DBConstraint[] array, int arrayIndex)
        {
            this._dbConstraints.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DBConstraints in DBConstraintCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._dbConstraints.Count;
            }
        }

        /// <summary>
        /// Check if DBConstraintCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DBConstraintCollection.
        /// </summary>
        /// <param name="item">The DBConstraint object to remove from the DBConstraintCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DBConstraintCollection</returns>
        public bool Remove(DBConstraint item)
        {
            return this._dbConstraints.Remove(item);
        }

        #endregion
    }
}