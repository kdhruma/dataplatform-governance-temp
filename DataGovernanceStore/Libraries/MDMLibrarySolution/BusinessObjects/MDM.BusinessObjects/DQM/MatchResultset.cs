using MDM.Core;
using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// 
    /// </summary>
    public class MatchResultSet
    {
        /// <summary>
        /// Max records to output
        /// </summary>
        public Int32 MaxRecords { get; set; }

        /// <summary>
        /// Match results collection
        /// </summary>
        public ICollection<MatchResult> MatchResults { get; set; }

        /// <summary>
        /// Updates Field Value with Locale
        /// </summary>
        public void UpdateFieldValueWithLocale()
        {
            foreach (var result in MatchResults)
            {
                LocaleEnum le = LocaleEnum.UnKnown;
                LocaleEnum.TryParse(result.Locale, out le);

                if(!result.FieldValue.Contains("_"))
                    result.FieldValue += "_" + ((Int32)le).ToString();
            }
        }
    }
}