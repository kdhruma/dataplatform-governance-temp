using System;
using System.Linq;

namespace MDM.UomManager.Business
{
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.BufferManager;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.UomManager.Data;

    /// <summary>
    /// 
    /// </summary>
    public class UomBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Specifies UOM buffer manager
        /// </summary>
        private UomBufferManager _uomBufferManager = new UomBufferManager();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public UomBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Get UOM  based on UOM context.
        /// </summary>
        /// <param name="uomContext">Specifies UOM context containing short name , UOM type</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>UOM object based on context.</returns>
        public UOM GetUom(UomContext uomContext, CallerContext callerContext)
        {
            UOMCollection uomCollection = this.GetAllUoms(uomContext, callerContext);

            UOM uom = null;

            if (uomCollection != null && uomCollection.Count > 0)
            {
                uom = (UOM)uomCollection.FindUOM(uomContext);
            }

            return uom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uomContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public UOMCollection GetAllUoms(UomContext uomContext, CallerContext callerContext)
        {
            UOMCollection uomCollection = _uomBufferManager.FindAllUoms();

            if (uomCollection == null)
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                UomDA uomDA = new UomDA();

                uomCollection = uomDA.GetAll(uomContext, command);

                if (uomCollection != null && uomCollection.Count > 0)
                {
                    _uomBufferManager.UpdateUoms(uomCollection, uomContext, 3);
                }
            }

            return uomCollection;
        }

        /// <summary>
        /// Returns all Uoms with given type
        /// </summary>
        /// <param name="uomContext">Represents uom context</param>
        /// <param name="callerContext">Represents caller context</param>
        /// <param name="uomType">Represents uom type name</param>
        /// <returns>Returns Uoms with given type</returns>
        public UOMCollection GetAllUomsWithType(UomContext uomContext, CallerContext callerContext, String uomType)
        {
            UOMCollection allUoms = GetAllUoms(uomContext, callerContext);
            return new UOMCollection(allUoms.Where(uom => uom.UnitTypeShortName.Equals(uomType, StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public string GetUomConversionsAsXml(CallerContext callerContext)
        {
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            UomDA uomDA = new UomDA();

            string uomConversionXML = uomDA.GetUomConversionsAsXml(command);
            return uomConversionXML;
        }        

        #endregion Get Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        #endregion

        #endregion
    }
}