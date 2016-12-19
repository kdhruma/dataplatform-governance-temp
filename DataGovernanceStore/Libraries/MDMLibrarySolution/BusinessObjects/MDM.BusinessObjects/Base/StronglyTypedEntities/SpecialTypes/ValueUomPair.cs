using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Represents the class representing the UOM and Value pair
    /// </summary>
    /// <typeparam name="T">Represents the type of value (Decimal, Int, String)</typeparam>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class ValueUomPair<T>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the uom.
        /// </summary>
        /// <value>
        /// The uom.
        /// </value>
        public String Uom { get; set; }
    }
}
