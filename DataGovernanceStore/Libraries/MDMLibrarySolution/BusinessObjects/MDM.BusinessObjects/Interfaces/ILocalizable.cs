namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes interface for working with UI Locale
    /// </summary>
    public interface ILocalizable
    {
        /// <summary>
        /// Represents UI Locale enum value
        /// </summary>
        LocaleEnum UILocale { get; set; }
    }
}
