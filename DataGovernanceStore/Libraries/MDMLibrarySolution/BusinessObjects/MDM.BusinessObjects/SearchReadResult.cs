using System;
using System.Runtime.Serialization;
using System.Data;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies read result of an search operations
    /// </summary>
    [DataContract]
    public class SearchReadResult : ISearchReadResult
    {
        #region Fields

        /// <summary>
        /// Field denoting list of entities, which were retrieved
        /// </summary>
        private EntityCollection _entities = null;

        /// <summary>
        /// Field denoting search result in form of data table, which were retrieved
        /// </summary>
        private DataTable _dataTable = null;

        /// <summary>
        /// Field denoting list if failed operation result
        /// </summary>
        private OperationResult _operationResult = null;

        /// <summary>
        /// Indicates count of results fetched
        /// </summary>
        private Int32 _totalCount = 0;

        #endregion Fields

        #region Constructors
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denoting list of entities, which were retrieved
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public EntityCollection Entities
        {
            get
            {
                if (this._entities == null)
                {
                    this._entities = new EntityCollection();
                }

                return this._entities;
            }
            set
            {
                this._entities = value;
            }
        }

        /// <summary>
        /// Property denoting search result in form of data table, which were retrieved
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public DataTable DataTable
        {
            get
            {
                if (this._dataTable == null)
                {
                    this._dataTable = new DataTable();
                }

                return this._dataTable;
            }
            set
            {
                this._dataTable = value;
            }
        }

        /// <summary>
        /// Property denoting list if failed result collection
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public OperationResult OperationResult
        {
            get
            {
                if (this._operationResult == null)
                {
                    this._operationResult = new OperationResult();
                }

                return this._operationResult;
            }
            set
            {
                this._operationResult = value;
            }
        }

        /// <summary>
        /// Indicates count of results fetched
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Int32 TotalCount
        {
            get
            {

                return this._totalCount;
            }
            set
            {
                this._totalCount = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets collection of entity
        /// </summary>
        /// <returns>Collection of entities from the search read result</returns>
        public IEntityCollection GetEntities()
        {
            return this._entities;
        }

        /// <summary>
        /// Gets collection of entity operation result
        /// </summary>
        /// <returns>Collection of entity operation result from the entity read result</returns>
        public IOperationResult GetOperationResult()
        {
            return this._operationResult;
        }

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}