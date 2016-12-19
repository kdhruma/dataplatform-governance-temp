using System;
using System.Collections.Generic;


namespace MDM.Interfaces
{

    /// <summary>
    ///  Exposes methods or properties used for defining the collection of matching result objects. 
    /// </summary>
    public interface IMatchingResultCollection : ICollection<MDM.BusinessObjects.DQM.MatchingResult>, ICloneable
    {

    }
}
