using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for lookup model collection
    /// </summary>
    public class LookupModelCollection : InterfaceContractCollection<ILookupModel, LookupModel>, IEnumerable<LookupModel>
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the lookup model Collection
        /// </summary>
        public LookupModelCollection() { }

        /// <summary>
        /// Initialize lookup model from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Lookup model in xml format</param>
        public LookupModelCollection(String valuesAsXml)
        {
            LoadLookupModels(valuesAsXml);
        }

        /// <summary>
        /// Initialize lookup model from list of lookup model list.
        /// </summary>
        /// <param name="lookupModelList">Indicates list of lookup model.</param>
        public LookupModelCollection(IList<LookupModel> lookupModelList)
        {
            if (lookupModelList != null)
            {
                this._items = new Collection<LookupModel>(lookupModelList);
            }
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of LookupModelCollection.
        /// </summary>
        /// <returns>Xml representation of LookupModelCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<LookupModels>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (LookupModel lookupModel in this._items)
                {
                    returnXml = String.Concat(returnXml, lookupModel.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</LookupModels>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is LookupModelCollection)
            {
                LookupModelCollection objectToBeCompared = obj as LookupModelCollection;

                Int32 lookupModelsUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 lookupModelsntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (lookupModelsUnion != lookupModelsntersect)
                {
                    return false;
                }

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

            foreach (LookupModel lookupModel in this._items)
            {
                hashCode += lookupModel.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Indicates given lookup table name is view based or not.
        /// </summary>
        /// <param name="lookupName">Indicates lookup table name.</param>
        /// <returns>true if lookup is created based on view; otherwise, false. </returns>
        public Boolean IsViewBasedLookup(String lookupName)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (LookupModel lookupModel in this._items)
                {
                    if ((String.Compare(lookupModel.DisplayTableName, lookupName, StringComparison.InvariantCultureIgnoreCase) == 0)
                        && lookupModel.IsViewBasedLookup)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get all lookup models which are not view based.
        /// </summary>
        /// <returns>Collection of lookup modes which are not view based lookup.</returns>
        public LookupModelCollection GetNonViewLookupModels()
        {
            LookupModelCollection lookupModels = null;

            if (this._items != null && this._items.Count > 0)
            {
                lookupModels = new LookupModelCollection();

                foreach (LookupModel lookupModel in this._items)
                {
                    if (!lookupModel.IsViewBasedLookup)
                    {
                        lookupModels.Add(lookupModel);
                    }
                }
            }

            return lookupModels;
        }

        /// <summary>
        /// Gets list of lookup display table names.
        /// </summary>
        /// <returns>Returns list of lookup display table names.</returns>
        public Collection<String> GetAllLookupDisplayTableName()
        {
            Collection<String> lookupDisplayTableNames = new Collection<String>();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (LookupModel lookupModel in _items)
                {
                    lookupDisplayTableNames.Add(lookupModel.DisplayTableName);
                }
            }

            return lookupDisplayTableNames;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current Lookup models from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current LookupModels</param>
        private void LoadLookupModels(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * <LookupModels>
                <LookupModel Id="284" TableName="tblk_Color" DisplayTableName="Color" IsViewBasedLookup="false"  </LookupModel>
               </LookupModels>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupModel")
                        {
                            String lookupModelXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(lookupModelXml))
                            {
                                LookupModel lookupModel = new LookupModel(lookupModelXml);

                                if (lookupModel != null)
                                {
                                    this.Add(lookupModel);
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

        #endregion
    }
}