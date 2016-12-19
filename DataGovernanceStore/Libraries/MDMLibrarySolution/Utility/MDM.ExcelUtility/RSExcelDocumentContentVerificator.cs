using System.Linq;

using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility
{
    /// <summary>
    /// RSExcel Document Content Verification
    /// This class is responsible for removing excel version-specific elements that can cause issues in other versions
    /// </summary>
    internal static class RsExcelDocumentContentVerification
    {
        /// <summary>
        /// Review And Clean Content
        /// </summary>
        /// <param name="document">The document for review</param>
        public static void ReviewAndCleanContent(SpreadsheetDocument document)
        {
            RemoveTimelineStylesExtension(document);
        }

        private static void RemoveTimelineStylesExtension(SpreadsheetDocument document)
        {
            WorkbookPart workbookPart = document.WorkbookPart;
            Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;
            StylesheetExtensionList extensionList = stylesheet.StylesheetExtensionList;
            if (extensionList.Any())
            {
                foreach (StylesheetExtension extension in extensionList.ToList())
                {
                    // TimelineStyles can cause document corruption while saving changes in Excel 2010
                    if (extension.LastChild is TimelineStyles)
                    {
                        extension.Remove();
                    }
                }

                stylesheet.Save();
            }
        }
    }
}
