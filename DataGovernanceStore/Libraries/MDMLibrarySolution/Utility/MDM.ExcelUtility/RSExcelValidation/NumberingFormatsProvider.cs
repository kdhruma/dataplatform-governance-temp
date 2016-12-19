using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility.RSExcelValidation
{
    using Core;
    using Interfaces;
    using MDM.BusinessObjects;
    
    internal class NumberingFormatsProvider
    {
        internal const Int32 ExcelPredefinedNumberFormat = 2;
        internal const Int32 ExcelPredefinedIntegerNumberFormat = 1;
        internal const Int32 ExcelPredefinedTextFormat = 49;

        private static NumberingFormatsProvider _numberingFormatsProviderInstance;
        
        /// <summary>
        /// Validation Provider instance
        /// </summary>
        public static NumberingFormatsProvider Provider
        {
            get
            {
                return _numberingFormatsProviderInstance ?? (_numberingFormatsProviderInstance = new NumberingFormatsProvider());
            }
        }

        /// <summary>
        /// Returns number formats
        /// </summary>
        /// <param name="attributes">List of attributes.</param>
        /// <param name="numberFormatMappings">The number Format Mappings.</param>
        /// <param name="skipColumnsCount">Number of columns to skip.</param>
        /// <returns>The NumberingFormats object.</returns>
        public NumberingFormats GetNumberingFormats(List<AttributeModel> attributes, out Dictionary<String, UInt32Value> numberFormatMappings, int skipColumnsCount = 0)
        {
            numberFormatMappings = new Dictionary<String, UInt32Value>();
            int index = skipColumnsCount;
            UInt32Value numberFormatId = 164U;
            NumberingFormats result = new NumberingFormats();

            foreach (var attributeModel in attributes)
            {
                AttributeDataType attributeDataType;
                Enum.TryParse(attributeModel.AttributeDataTypeName, true, out attributeDataType);
                String attrKey = String.Concat(attributeModel.AttributeParentName, "//", attributeModel.Name + "//" + attributeModel.Locale);
                if (attributeDataType == AttributeDataType.Decimal
                    && attributeModel.Precision > 0
                    && String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                {
                    // Excel predefined number format
                    if (attributeModel.Precision == 2)
                    {
                        numberFormatMappings.Add(attrKey, ExcelPredefinedNumberFormat);
                    }
                    else
                    {
                        String format = GetPrecissionFormat(attributeModel.Precision);
                        NumberingFormat numberingFormat = new NumberingFormat
                        {
                            NumberFormatId = numberFormatId,
                            FormatCode = format
                        };
                        result.Append(numberingFormat);
                        numberFormatMappings.Add(attrKey, numberFormatId);
                    }

                    numberFormatId++;
                }
                else if (attributeDataType == AttributeDataType.Integer
                   && String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                {
                    numberFormatMappings.Add(attrKey, ExcelPredefinedIntegerNumberFormat);
                }
                else if (attributeDataType == AttributeDataType.String
                    || attributeDataType == AttributeDataType.Boolean
                    || !String.IsNullOrEmpty(attributeModel.LookUpTableName)
                    || !String.IsNullOrEmpty(attributeModel.AllowableValues))
                {
                    // Excel Predefined text format
                    numberFormatMappings.Add(attrKey, ExcelPredefinedTextFormat);
                }

                index++;
            }

            return result;
        }

        private String GetPrecissionFormat(int precission)
        {
            String result = "0.";
            for (Int32 i = 0; i < precission; i++)
            {
                result += "0";
            }

            return result;
        }
    }
}
