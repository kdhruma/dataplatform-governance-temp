namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Represents class for AppManageMessage.
    /// </summary>
    internal class AppConfigManageMessage : MessageBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the attributes information.
        /// </summary>
        public new EventExtendedAttributesInfo ExtendedAttributesInfo { get; set; }

        #endregion
    }
}