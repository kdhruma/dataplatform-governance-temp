using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility.RSExcelValidation
{
    using Interfaces;
    using MDM.Utility;

    internal static class DataValidationExtensions
    {
        /// <summary>
        /// Updated the dataValidation object with predefined values validation restrictions
        /// </summary>
        /// <param name="dataValidation">The target object </param>
        /// <param name="columnName">The column name </param>
        /// <param name="formulaCollectionName">The formula Collection Name.</param>
        /// <param name="messages">The messages collection. </param>
        public static void AddPredefinedLookupValidation(this DataValidation dataValidation, String columnName, String formulaCollectionName, IDictionary<String, String> messages)
        {
            String predefinedValuesMessage = messages[RSExcelConstants.PredefinedValuesMessageCode];

            dataValidation.Type = DataValidationValues.List;
            dataValidation.AddDefaultConfigurationToDataValidation(predefinedValuesMessage, predefinedValuesMessage, columnName, messages);
            dataValidation.SequenceOfReferences = new ListValue<StringValue>
            {
                InnerText = String.Format(RSExcelConstants.ColumnRangeFormat, columnName)
            };
            
            Formula1 formula1 = new Formula1
            {
                Text = formulaCollectionName
            };
            dataValidation.Append(formula1);
        }

        /// <summary>
        /// Updated the dataValidation object with integer type validation restrictions
        /// </summary>
        /// <param name="dataValidation"> The target object  </param>
        /// <param name="columnName"> The column name  </param>
        /// <param name="attributeModel"> The attributeModel  </param>
        /// <param name="messages"> The messages. </param>
        public static void AddIntegerTypeValidation(this DataValidation dataValidation, String columnName, IAttributeModel attributeModel, IDictionary<String, String> messages)
        {
            Int32 minValue = Int32.MinValue;
            Int32 maxValue = Int32.MaxValue;
            Boolean isRangeSpecified = false;

            if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom))
            {
                isRangeSpecified |= Int32.TryParse(attributeModel.RangeFrom, out minValue);
            }

            if (!String.IsNullOrWhiteSpace(attributeModel.RangeTo))
            {
                isRangeSpecified |= Int32.TryParse(attributeModel.RangeTo, out maxValue);
            }

            dataValidation.DefineOperatorForDataValidation(attributeModel);
            dataValidation.Type = DataValidationValues.Whole;

            String validationMessage = isRangeSpecified
                ? GetRangeValidationMessage(minValue.ToString(CultureInfo.InvariantCulture), maxValue.ToString(CultureInfo.CurrentCulture), dataValidation.Operator, messages)
                : messages[RSExcelConstants.ValidIntegerValueMessageCode];

            dataValidation.AddDefaultConfigurationToDataValidation(validationMessage, validationMessage, columnName, messages);

            Formula1 formula1 = dataValidation.Operator == DataValidationOperatorValues.LessThanOrEqual 
                ? new Formula1 { Text = maxValue.ToString(CultureInfo.InvariantCulture) } 
                : new Formula1 { Text = minValue.ToString(CultureInfo.InvariantCulture) };
            dataValidation.Append(formula1);

            if (dataValidation.Operator == DataValidationOperatorValues.Between)
            {
                Formula2 formula2 = new Formula2 { Text = maxValue.ToString(CultureInfo.InvariantCulture) };
                dataValidation.Append(formula2);
            }
        }

        /// <summary>
        /// Updated the dataValidation object with decimal type restrictions
        /// </summary>
        /// <param name="dataValidation"> The target object </param>
        /// <param name="columnName"> The column name </param>
        /// <param name="attributeModel"> The attributeModel </param>
        /// <param name="messages"> The messages. </param>
        public static void AddDecimalTypeValidation(this DataValidation dataValidation, String columnName, IAttributeModel attributeModel, IDictionary<String, String> messages)
        {
            Single minValue = Single.MinValue;
            Single maxValue = Single.MaxValue;
            Boolean isRangeSpecified = false;

            if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom))
            {
                isRangeSpecified |= Single.TryParse(attributeModel.RangeFrom, out minValue);
            }

            if (!String.IsNullOrWhiteSpace(attributeModel.RangeTo))
            {
                isRangeSpecified |= Single.TryParse(attributeModel.RangeTo, out maxValue);
            }

            dataValidation.DefineOperatorForDataValidation(attributeModel);
            dataValidation.Type = DataValidationValues.Decimal;

            String validationMessage = isRangeSpecified
                ? GetRangeValidationMessage(minValue.ToString(CultureInfo.InvariantCulture), maxValue.ToString(CultureInfo.CurrentCulture), dataValidation.Operator, messages)
                : messages[RSExcelConstants.ValidDecimalValueMessageCode];

            dataValidation.AddDefaultConfigurationToDataValidation(validationMessage, validationMessage, columnName, messages);

            Formula1 formula1 = dataValidation.Operator == DataValidationOperatorValues.LessThanOrEqual
                ? new Formula1 { Text = maxValue.ToString(CultureInfo.InvariantCulture) }
                : new Formula1 { Text = minValue.ToString(CultureInfo.InvariantCulture) };
            dataValidation.Append(formula1);

            if (dataValidation.Operator == DataValidationOperatorValues.Between)
            {
                Formula2 formula2 = new Formula2 { Text = maxValue.ToString(CultureInfo.InvariantCulture) };
                dataValidation.Append(formula2);
            }
        }

        /// <summary>
        /// Updated the dataValidation object with decimal type restrictions
        /// </summary>
        /// <param name="dataValidation"> The target object </param>
        /// <param name="columnName"> The column name </param>
        /// <param name="attributeModel"> The attributeModel </param>
        /// <param name="messages"> The messages. </param>
        public static void AddDateTimeTypeValidation(this DataValidation dataValidation, String columnName, IAttributeModel attributeModel, IDictionary<String, String> messages)
        {
            DateTime minValue = DateTime.Today.AddYears(-100).Date;
            DateTime maxValue = DateTime.Today.AddYears(500).Date;
            Boolean isRangeSpecified = false;

            if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom))
            {
                isRangeSpecified |= DateTime.TryParse(attributeModel.RangeFrom, out minValue);
            }

            if (!String.IsNullOrWhiteSpace(attributeModel.RangeTo))
            {
                isRangeSpecified |= DateTime.TryParse(attributeModel.RangeTo, out maxValue);
            }

            dataValidation.DefineOperatorForDataValidation(attributeModel);
            dataValidation.Type = DataValidationValues.Date;

            String validationMessage = isRangeSpecified
                ? GetRangeValidationMessage(minValue.ToString(CultureInfo.InvariantCulture), maxValue.ToString(CultureInfo.CurrentCulture), dataValidation.Operator, messages)
                : messages[RSExcelConstants.ValidDateValueMessageCode];

            dataValidation.AddDefaultConfigurationToDataValidation(validationMessage, validationMessage, columnName, messages);

            Formula1 formula1 = dataValidation.Operator == DataValidationOperatorValues.LessThanOrEqual
                ? new Formula1 { Text = maxValue.ToOADate().ToString(CultureInfo.InvariantCulture) }
                : new Formula1 { Text = minValue.ToOADate().ToString(CultureInfo.InvariantCulture) };
            dataValidation.Append(formula1);

            if (dataValidation.Operator == DataValidationOperatorValues.Between)
            {
                Formula2 formula2 = new Formula2 { Text = maxValue.ToOADate().ToString(CultureInfo.InvariantCulture) };
                dataValidation.Append(formula2);
            }
        }

        /// <summary>
        /// Updated the dataValidation object with precision validation restrictions
        /// </summary>
        /// <param name="dataValidation"> The target object </param>
        /// <param name="columnName"> The column name </param>
        /// <param name="attributeModel"> The attributeModel </param>
        /// <param name="messages"> The messages. </param>
        public static void AddValueLengthValidation(this DataValidation dataValidation, String columnName, IAttributeModel attributeModel, IDictionary<String, String> messages)
        {
            String validationMessage = String.Format(messages[RSExcelConstants.ValueLengthMessageCode], attributeModel.MinLength, attributeModel.MaxLength);

            dataValidation.Type = DataValidationValues.TextLength;
            dataValidation.AddDefaultConfigurationToDataValidation(validationMessage, validationMessage, columnName, messages);

            Formula1 formula1 = new Formula1 { Text = attributeModel.MinLength.ToString(CultureInfo.InvariantCulture) };
            Formula2 formula2 = new Formula2 { Text = attributeModel.MaxLength.ToString(CultureInfo.InvariantCulture) };
            dataValidation.Append(formula1);
            dataValidation.Append(formula2);
        }

        private static String GetRangeValidationMessage(String rangeFrom, String rangeTo, DataValidationOperatorValues operatorValue, IDictionary<String, String> messages)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(messages[RSExcelConstants.ValueCriteriaMessageCode]);
            if (operatorValue == DataValidationOperatorValues.GreaterThanOrEqual || operatorValue == DataValidationOperatorValues.Between)
            {
                stringBuilder.AppendLine(messages[RSExcelConstants.ValueGreaterOrEqualMessageCode] + rangeFrom);
            }

            if (operatorValue == DataValidationOperatorValues.LessThanOrEqual || operatorValue == DataValidationOperatorValues.Between)
            {
                stringBuilder.AppendLine(messages[RSExcelConstants.ValueLessOrEqualMessageCode] + rangeTo);
            }

            return stringBuilder.ToString();
        }
 
        private static void AddDefaultConfigurationToDataValidation(this DataValidation dataValidation, String notificationMessage, String errorMessage, String columnName, IDictionary<String, String> messages)
        {
            Boolean showInputMessage = AppConfigurationHelper.GetAppConfig<Boolean>("MDM.Exports.RSExcelFormatter.ShowInputMessage", false);

            dataValidation.ShowErrorMessage = true;
            dataValidation.ErrorTitle = messages[RSExcelConstants.ErrorCaptionMessageCode];
            dataValidation.Error = errorMessage;

            dataValidation.ShowInputMessage = showInputMessage;
            if (showInputMessage)
            {
                dataValidation.PromptTitle = messages[RSExcelConstants.NotificationCaptionMessageCode];
                dataValidation.Prompt = notificationMessage;
            }

            dataValidation.SequenceOfReferences = new ListValue<StringValue>
            {
                InnerText = String.Format(RSExcelConstants.ColumnRangeFormat, columnName)
            };
        }

        private static void DefineOperatorForDataValidation(this DataValidation dataValidation, IAttributeModel attributeModel)
        {
            DataValidationOperatorValues result = DataValidationOperatorValues.Between;

            if (!String.IsNullOrWhiteSpace(attributeModel.RangeFrom) && String.IsNullOrWhiteSpace(attributeModel.RangeTo))
            {
                // Greater case
                result = DataValidationOperatorValues.GreaterThanOrEqual;
            }
            else if (String.IsNullOrWhiteSpace(attributeModel.RangeFrom) && !String.IsNullOrWhiteSpace(attributeModel.RangeTo))
            {
                // Less case
                result = DataValidationOperatorValues.LessThanOrEqual;
            }

            dataValidation.Operator = result;
        }
    }
}
