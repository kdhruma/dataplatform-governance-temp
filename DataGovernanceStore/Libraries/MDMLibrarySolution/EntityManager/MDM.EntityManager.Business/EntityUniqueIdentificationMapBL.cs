using System;


namespace MDM.EntityManager.Business
{
    using Core;
    using Utility;
    using BusinessObjects;
    using Data;
    using ConfigurationManager.Business;
    using BusinessObjects.EntityIdentification;

    /// <summary>
    /// Specifies business operations for EntityUniqueIdentification Maps
    /// </summary>
    public class EntityUniqueIdentificationMapBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;


        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Entity Map BL
        /// </summary>
        public EntityUniqueIdentificationMapBL()
        {
            GetSecurityPrincipal();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Loads entity unique identification maps with the results
        /// </summary>
        /// <param name="entityUniqueIdentificationMapCollection">Collection of entity unique identification maps which needs to be loaded</param>        
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>                
        public void LoadEntityUniqueIdentificationDetails(EntityUniqueIdentificationMapCollection entityUniqueIdentificationMapCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            if (entityUniqueIdentificationMapCollection == null || entityUniqueIdentificationMapCollection.Count < 1)
            {
                throw new ArgumentException("EntityUniqueIdentification Maps are not available");
            }

            //Get command properties
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            //Get entity mappings from DB
            EntityUniqueIdentificationMapDA entityUniqueIdentificationMapDA = new EntityUniqueIdentificationMapDA();
            entityUniqueIdentificationMapDA.LoadEntityUniqueIdentificationDetails(entityUniqueIdentificationMapCollection, command);
        }

        #endregion

        #region Private Methods

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