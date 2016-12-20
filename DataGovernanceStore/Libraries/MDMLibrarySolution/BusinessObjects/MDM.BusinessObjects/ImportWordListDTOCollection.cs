using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Data of a Imported Word List Collection
    /// </summary>
    public class ImportWordListDTOCollection : InterfaceContractCollection<IImportWordListDTO, ImportWordListDTO>, IDataModelObjectCollection
    {
        #region Fields

        private const String NodeName = "ImportWordListDTO";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ImportWordListDTOCollection() { }

        /// <summary>
        /// Initialize WordListsCollection from IEnumerable
        /// </summary>
        /// <param name="wordLists">IList of WordLists</param>
        public ImportWordListDTOCollection(IEnumerable<WordList> wordLists)
        {
            _items = new Collection<ImportWordListDTO>(wordLists.Select(wl => new ImportWordListDTO
            {
                Id = wl.Id,
                Name = wl.Name,
                LongName = wl.LongName
            }).ToList());
        }

        /// <summary>
        /// Initializes a new instance of the ImportWordListDTO Collection by xml
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public ImportWordListDTOCollection(String valuesAsXml)
        {
            LoadImportWordListDTOCollection(valuesAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wordList"></param>
        public ImportWordListDTOCollection(IList<ImportWordListDTO> wordList)
        {
            if (wordList != null)
            {
                this._items = new Collection<ImportWordListDTO>(wordList);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for DataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return ObjectType.WordList;
            }
        }

        #endregion

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Remove DataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an Word Lists which is to be removed</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            ImportWordListDTOCollection wordLists = GetWordLists(referenceIds);

            if (!wordLists.IsNullOrEmpty())
            {
                foreach (ImportWordListDTO wordList in wordLists)
                {
                    result = result && Remove(wordList);
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
            Collection<IDataModelObjectCollection> importWordListDTOInBatch = null;

            if (this._items != null)
            {
                importWordListDTOInBatch = Utility.Split(this, batchSize);
            }

            return importWordListDTOInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as ImportWordListDTO);
        }

        #endregion

        #region Private Members

        /// <summary>
        ///  Gets the word lists using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of word lists which is to be fetched</param>
        /// <returns>Returns filtered word lists</returns>
        private ImportWordListDTOCollection GetWordLists(Collection<String> referenceIds)
        {
            ImportWordListDTOCollection wordLists = new ImportWordListDTOCollection();

            if (!_items.IsNullOrEmpty() && !referenceIds.IsNullOrEmpty())
            {
                HashSet<String> ids = new HashSet<String>(referenceIds);
                foreach (ImportWordListDTO wordList in _items)
                {
                    if (ids.Contains(wordList.ReferenceId))
                    {
                        wordLists.Add(wordList);
                    }
                }
            }

            return wordLists;
        }

        private void LoadImportWordListDTOCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(NodeName))
                        {
                            String importWordListXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(importWordListXml))
                            {
                                ImportWordListDTO importWordListDto = new ImportWordListDTO(importWordListXml);
                                Add(importWordListDto);
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
    }
}
