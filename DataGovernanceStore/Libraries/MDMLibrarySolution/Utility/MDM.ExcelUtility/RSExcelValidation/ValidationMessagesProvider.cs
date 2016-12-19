using System;
using System.Collections.Generic;

namespace MDM.ExcelUtility.RSExcelValidation
{
    using BusinessObjects;
    using Core;
    using MessageManager.Business;
    using Utility;

    internal class ValidationMessagesProvider
    {
        private readonly List<String> _messageCodes = new List<String>
        {
            RSExcelConstants.ErrorCaptionMessageCode, 
            RSExcelConstants.ExpectedValuesMessageCode, 
            RSExcelConstants.MandatoryFieldMessageCode,
            RSExcelConstants.NotificationCaptionMessageCode,
            RSExcelConstants.OptionalFieldMessageCode,
            RSExcelConstants.PredefinedValuesMessageCode,
            RSExcelConstants.ProvideValueMessageCode,
            RSExcelConstants.ValidDateValueMessageCode,
            RSExcelConstants.ValidDecimalValueMessageCode,
            RSExcelConstants.ValidIntegerValueMessageCode,
            RSExcelConstants.ValueCriteriaMessageCode,
            RSExcelConstants.ValueGreaterOrEqualMessageCode,
            RSExcelConstants.ValueLengthMessageCode,
            RSExcelConstants.ValueLessOrEqualMessageCode,
            RSExcelConstants.ContextLookupValueWarningMessageCode
        };

        private readonly LocaleEnum _locale = LocaleEnum.UnKnown;
        private readonly SecurityPrincipal _securityPrincipal;
        private readonly LocaleMessageBL _localeMessageBL;
        private readonly CallerContext _callerContext;

        public ValidationMessagesProvider()
        {
            if (_locale == LocaleEnum.UnKnown)
            {
                if (_securityPrincipal == null)
                {
                    _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                }

                _locale = (_securityPrincipal.UserPreferences == null || _securityPrincipal.UserPreferences.DataLocale == LocaleEnum.UnKnown)
                    ? GlobalizationHelper.GetSystemDataLocale() : _securityPrincipal.UserPreferences.DataLocale;
            }

            _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.UIProcess);
            _localeMessageBL = new LocaleMessageBL();
        }

        /// <summary>
        /// Get Locale Message
        /// </summary>
        /// <param name="messageCode">The message code</param>
        /// <returns>The message</returns>
        public String GetLocaleMessage(String messageCode)
        {
            LocaleMessage localeMessage = _localeMessageBL.Get(_locale, messageCode, false, _callerContext);
            return localeMessage.Message;
        }

        /// <summary>
        /// Get All Validation Messages Dictionary
        /// </summary>
        /// <returns>Dictionary with values</returns>
        public IDictionary<String, String> GetAllValidationMessagesDictionary()
        {
            IDictionary<String, String> result = new Dictionary<String, String>();

            foreach (String messageCode in _messageCodes)
            {
                String message = GetLocaleMessage(messageCode);
                result.Add(messageCode, message);
            }

            return result;
        }
    }
}
