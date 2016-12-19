namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the base profile.
    /// </summary>
    public interface IBaseProfile
    {
        /// <summary>
        /// Indicates profile type name
        /// </summary>
        ProfileType ProfileType { get; }
    }
}
