using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Vendor
    /// </summary>
    [DataContract]
    public class Vendor : MDMObject, IVendor
    {
        #region Fields

        /// <summary>
        /// field denoting whether the vendor is disable.
        /// </summary>
        private Boolean _disable = false;

        /// <summary>
        /// field denoting description of vendor.
        /// </summary>
        private String _description = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor.
        /// </summary>
        public Vendor()
        { }

        /// <summary>
        ///  Constructor with Vendor Id, Vendor Name, description and disable as input parameter.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="disable"></param>
        public Vendor(Int32 vendorId, String name, String description, Boolean disable)
            : base(vendorId)
        {
            base.Name = name;
            this._description = description;
            this._disable = disable;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Vendor's disable status
        /// </summary>
        [DataMember]
        public Boolean Disable
        {
            get
            {
                return this._disable;
            }
            set
            {
                this._disable = value;
            }
        }

        /// <summary>
        /// Property denoting the Description of the Vendor
        /// </summary>
        [DataMember]
        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        #endregion
    }
}
