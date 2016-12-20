using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of validation profiles.
    /// </summary>
    public interface IValidationProfilesCollection : ICollection<ValidationProfile>
    {
    }
}
