namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get DQM job profile.
    /// </summary>
    public interface IDQMJobProfile : IBaseProfile
    {
        /// <summary>
        /// Property denoting profile type
        /// </summary>
        DQMJobType JobType { get; set; }
    }
}