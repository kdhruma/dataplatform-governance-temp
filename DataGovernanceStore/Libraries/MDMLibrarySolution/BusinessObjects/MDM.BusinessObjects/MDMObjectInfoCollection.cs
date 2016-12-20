using System;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MDMObjectInfoCollection : InterfaceContractCollection<IMDMObjectInfo, MDMObjectInfo>, IMDMObjectInfoCollection
    {
        /// <summary>
        /// Determines whether the mdm object info collection contains a specific mdm object info
        /// </summary>
        /// <param name="id">Indicates identifier to locate in the mdm object info</param>
        /// <returns>
        /// <para>true : if mdm object info found in mdm object info collection</para>
        /// <para>false : If mdm object info found not in mdm object info collection</para>
        /// </returns>
        public Boolean Contains(Int32 id)
        {
            return GetMDMObjectInfo(id) != null;
        }

        /// <summary>
        /// Remove mdm object info object from mdm object info collection
        /// </summary>
        /// <param name="mdmObjectInfoId">Indicates identifier of mdm object info which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 mdmObjectInfoId)
        {
            MDMObjectInfo mdmObjectInfo = GetMDMObjectInfo(mdmObjectInfoId);

            if (mdmObjectInfo == null)
                throw new ArgumentException("No MDMObjectInfo found for given Id :" + mdmObjectInfoId);

            return this.Remove(mdmObjectInfo);
        }

        /// <summary>
        /// Get Xml representation of mdm object info collection object
        /// </summary>
        /// <returns>Returns xml string representing the mdm object info collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<MDMObjectInfoes>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMObjectInfo mdmObjectInfo in this._items)
                {
                    returnXml = String.Concat(returnXml, mdmObjectInfo.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</MDMObjectInfoes>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of mdm object info collection object
        /// </summary>
        /// <param name="serialization">Indicates serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Returns xml string representing the mdm object info collection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<MDMObjectInfoes>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMObjectInfo mdmObjectInfo in this._items)
                {
                    returnXml = String.Concat(returnXml, mdmObjectInfo.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</MDMObjectInfoes>");

            return returnXml;
        }

        #region Private methods

        private MDMObjectInfo GetMDMObjectInfo(Int32 id)
        {
            var filteredMDMObjectInfoes = from mdmObjectInfo in this._items
                                       where mdmObjectInfo.Id == id
                                       select mdmObjectInfo;

            return filteredMDMObjectInfoes.Any() ? filteredMDMObjectInfoes.First() : null;
        }

        #endregion

    }
}
