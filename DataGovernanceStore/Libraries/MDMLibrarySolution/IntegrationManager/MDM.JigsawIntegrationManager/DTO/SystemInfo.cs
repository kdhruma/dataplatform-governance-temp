using System;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Represents class for SystemInfo.
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// 
        /// </summary>
        private String _tenantId;

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        public String TenantId
        {
            get { return _tenantId; }
            set { _tenantId = value; }
        }
    }
}
