using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get word element.
    /// </summary>
    public interface IWordElement : IMDMObject, ICloneable
    {
        /// <summary>
        /// Denotes the Word
        /// </summary>
        String Word { get; set; }

        /// <summary>
        /// Denotes the Substitute
        /// </summary>
        String Substitute { get; set; }

        /// <summary>
        /// Denotes the WordList id
        /// </summary>
        Int32 WordListId { get; set; }

        /// <summary>
        /// Denotes sequence
        /// </summary>
        Int32? Sequence { get; set; }

        /// <summary>
        /// Denotes create date time
        /// </summary>
        DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// Denotes modification date time
        /// </summary>
        DateTime? ModDateTime { get; set; }

        /// <summary>
        /// Denotes user that created word element
        /// </summary>
        String CreateUser { get; set; }

        /// <summary>
        /// Denotes the last user that modified word element
        /// </summary>
        String ModUser { get; set; }

        /// <summary>
        /// Denotes the program that created word element
        /// </summary>
        String CreateProgram { get; set; }

        /// <summary>
        /// Denotes the last program that modified word element
        /// </summary>
        String ModProgram { get; set; }

        /// <summary>
        /// Denotes encoded word
        /// </summary>
        String EncodedWord { get; set; }

        /// <summary>
        /// Delta Merge of WordElement
        /// </summary>
        /// <param name="deltaWordElement">WordElement that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged WordElement instance</returns>
        IWordElement MergeDelta(IWordElement deltaWordElement, ICallerContext iCallerContext,
            Boolean returnClonedObject = true);
    }
}
