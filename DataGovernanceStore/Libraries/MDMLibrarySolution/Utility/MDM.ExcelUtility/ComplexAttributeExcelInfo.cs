using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace MDM.ExcelUtility
{
    /// <summary>
    /// Represents ComplexAttributeExcelInfo class
    /// </summary>
    internal class ComplexAttributeExcelInfo
    {
        public String SheetName { get; set; }
        public SheetData SheetData { get; set; }
        public String ReplacementId { get; set; }
        public Worksheet WorkSheet { get; set; }
        public OpenXmlWriter Writer { get; set; }
        public OpenXmlReader Reader { get; set; }
        public uint RowIndex { get; set; }
    }
}
