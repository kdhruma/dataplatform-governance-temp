using System;


namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies EntityProviderContext which indicates what all information is to be loaded in Entity object
    /// </summary>
    public class EntityProviderContext : ObjectBase, IEntityProviderContext
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private String entityTypeName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private String containerName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private String organizationName = String.Empty;
        
        /// <summary>
        /// 
        /// </summary>
        private EntityProviderContextType entityproviderContexttype = EntityProviderContextType.All;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityProviderContext()
            : base()
        {

        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public String EntityTypeName
        {
            get
            {
                return entityTypeName;
            }
            set
            {
                entityTypeName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ContainerName
        {
            get
            {
                return containerName;
            }
            set
            {
                containerName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String OrganizationName
        {
            get
            {
                return organizationName;
            }
            set
            {
                organizationName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityProviderContextType EntityProviderContextType
        {
            get
            {
                return entityproviderContexttype;
            }
            set
            {
                entityproviderContexttype = value;
            }
        }

        #endregion Properties

        #region Methods
         
        #region Public methods

        /// <summary>
        /// Get XML representation of EntityProviderContext object
        /// </summary>
        /// <returns>XML representation of EntityProviderContext object</returns>
        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion Public methods

        #endregion Methods
    }
}
