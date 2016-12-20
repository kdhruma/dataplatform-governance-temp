using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces.Matching
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMatchProfile
    {
        /// <summary>
        /// 
        /// </summary>
        string ProfileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string RuleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string TargetTableName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        MatchingEngineType EngineType { get; set; }
    }
}
