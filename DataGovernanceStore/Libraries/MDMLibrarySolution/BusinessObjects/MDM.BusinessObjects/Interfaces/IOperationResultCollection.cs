using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of operation results.
    /// </summary>
    public interface IOperationResultCollection : ICollection<OperationResult>
    {
        /// <summary>
        /// Indicates OperationResultStatus
        /// </summary>
        OperationResultStatusEnum OperationResultStatus  { get; }
        
        /// <summary>
        /// Adds item to the collection
        /// </summary>
        /// <param name="iOperationResult">Object which implements interface <see cref="IOperationResult"/></param>
        void Add(IOperationResult iOperationResult);

        /// <summary>
        /// Verifies whether or not sequence contain required item
        /// </summary>
        /// <param name="operationResult">Id item</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(Int32 operationResult);

        /// <summary>
        /// Compares collection with another collection
        /// </summary>
        /// <param name="obj">Collection to compare</param>
        /// <returns>[True] if collections equal and [False] otherwise</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Gets hash code of the item
        /// </summary>
        /// <returns>Hash code of the item</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="id">Id of item to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(Int32 id);

        /// <summary>
        /// Convert items to XML
        /// </summary>
        /// <returns>XML string</returns>
        String ToXml();

        /// <summary>
        /// Convert items to XML with specific rule
        /// </summary>
        /// <param name="serialization">Rules of serialization</param>
        /// <returns>Convert item to XML with specific rule</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Gets item by Id
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <returns>Cloned item</returns>
        IOperationResult Get(Int32 id);
    }
}
