using System;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Data of a Imported Word List
    /// </summary>
    public interface IImportWordListDTO
    {
        /// <summary>
        /// Specifies if Flush and Fill mode is used for Word List Import
        /// </summary>
        Boolean IsFlushAndFillMode { get; set; }

        /// <summary>
        /// Represents original Word List from Database
        /// </summary>
        WordList OriginalWordList { get; set; }

        /// <summary>
        /// Delta Merge of WordList Values
        /// </summary>
        /// <param name="deltaWordList">WordList that needs to be merged</param>
        void MergeDelta(IWordList deltaWordList);
    }
}
