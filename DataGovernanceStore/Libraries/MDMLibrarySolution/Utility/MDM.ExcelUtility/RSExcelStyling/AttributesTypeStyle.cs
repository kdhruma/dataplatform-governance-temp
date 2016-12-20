using System;

using DocumentFormat.OpenXml;

namespace MDM.ExcelUtility.RSExcelStyling
{
    public class AttributesTypeStyle
    {
        /// <summary>
        /// Background Color RGB value
        /// </summary>
        public HexBinaryValue BackgroundRgb { get; set; }
        
        /// <summary>
        /// Background Color Indexed value
        /// </summary>
        public UInt32Value BackgroundIndexed { get; set; }
        
        /// <summary>
        /// Font Color RGB value
        /// </summary>
        public HexBinaryValue FontRgb { get; set; }

        /// <summary>
        /// Font Color Indexed value
        /// </summary>
        public UInt32Value FontIndexed { get; set; }

        /// <summary>
        /// Font Name
        /// </summary>
        public String FontName { get; set; }

        /// <summary>
        /// Font Size val
        /// </summary>
        public DoubleValue FontSizeVal { get; set; }

        /// <summary>
        /// Is Text Bold
        /// </summary>
        public Boolean IsBold { get; set; }

        /// <summary>
        /// Is Text Italic
        /// </summary>
        public Boolean IsItalic { get; set; }
    }
}
