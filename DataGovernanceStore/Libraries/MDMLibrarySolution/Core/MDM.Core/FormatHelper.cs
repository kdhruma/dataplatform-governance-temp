
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace MDM.Core
{
    /// <summary>
    /// This class contains helper methods for Globalization. eg handling number formats, date format and timezones etc.
    /// </summary>
    public class FormatHelper
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Formatting methods

        /// <summary>
        /// To format decimal values to their locale equivalents
        /// The current UI Thread locale is used for obtaining the Number format
        /// eg. 123.45 in locale fr-FR would be 123,45
        /// You dont need to convert numbers without decimal points
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <returns>The locale formatted number string</returns>
        //TODO Add thousands support when required like 100,000,000
        public static String FormatNumber(Double number)
        {
            CultureInfo currentCulture = null;

            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return FormatNumber(number, currentCulture.Name);
        }

        /// <summary>
        /// To format decimal values(in string) to their locale equivalents given a locale value
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="locale">The locale the number to be formated</param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(String number, String locale)
        {
            Double dblNumber = Double.Parse(number, CultureInfo.InvariantCulture);
            return FormatNumber(dblNumber, locale);
        }

        /// <summary>
        /// To format decimal values(in string) to their locale equivalents given a locale value
        /// Takes a locale in which the values is given, and locale in which value should be formatted
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="fromLocale">The locale the number is in</param>
        /// <param name="toLocale">The locale the number to be formated</param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(String number, String fromLocale, String toLocale)
        {
            Double dblNumber = DeformatNumber(number, fromLocale);
            return FormatNumber(dblNumber, toLocale);
        }

        /// <summary>
        /// To format decimal values to their locale equivalents given a locale value
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="locale">The locale the number to be formated</param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(Double number, String locale)
        {
            String numberString = String.Empty;
            CultureInfo culture = null;
            NumberFormatInfo numberFormat = null;

            //automatically throws and exception if the locale is not in the correct format
            culture = new CultureInfo(locale);

            numberFormat = culture.NumberFormat;

            numberString = number.ToString(numberFormat);

            return numberString;
        }

        /// <summary>
        /// To format decimal values to their locale equivalents
        /// The current UI Thread locale is used for obtaining the Number format
        /// Number is padded according to the precision value given
        /// eg. 123.45 in locale fr-FR would be 123,45, with precision 4 it would be 123,450
        /// You dont need to convert numbers without decimal points
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="precisionAndPadding">no. of decimal points needed</param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(Double number, Int32 precisionAndPadding)
        {
            CultureInfo currentCulture = null;

            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return FormatNumber(number, currentCulture.Name,precisionAndPadding,false);
        }

        /// <summary>
        /// To format decimal values(in string) to their locale equivalents given a locale value
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="locale">The locale the number to be formated</param>
        /// <param name="precisionAndPadding">Rounds the result to specified number of fractional digits</param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(String number, String locale, Int32 precisionAndPadding)
        {
            return FormatNumber(number, locale, precisionAndPadding, false);
        }

        /// <summary>
        /// To format decimal values(in string) to their locale equivalents given a locale value
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="locale">The locale the number to be formatted</param>
        /// <param name="precision">Rounds the result to specified number of fractional digits</param>
        /// <param name="isArbitraryPrecision">Property denoting if arbitrary precision is used.
        /// If value of IsPrecisionArbitrary is true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If value of IsPrecisionArbitrary is false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(String number, String locale, Int32 precision, Boolean isArbitraryPrecision)
        {
            Double dblNumber = Double.Parse(number, CultureInfo.InvariantCulture);

            return FormatNumber(dblNumber, locale, precision, isArbitraryPrecision);
        }

        /// <summary>
        /// Round double number for given precision and apply padding if needed.
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="doubleNumber">The number to be formatted</param>
        /// <param name="locale">The locale the number to be formatted</param>
        /// <param name="precision">Rounds the result to specified number of fractional digits</param>
        /// <param name="isArbitraryPrecision">Property denoting if arbitrary precision is used.
        /// If value of IsPrecisionArbitrary is true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If value of IsPrecisionArbitrary is false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(Double doubleNumber, String locale, Int32 precision, Boolean isArbitraryPrecision)
        {
            String numberString = String.Empty;
            CultureInfo culture = null;
            NumberFormatInfo numberFormat = null;
            String paddingString = String.Empty;

            //automatically throws and exception if the locale is not in the correct format
            culture = new CultureInfo(locale);

            numberFormat = culture.NumberFormat;

            for (Int32 counter = 0; counter < precision; counter++)
            {
                paddingString += "0";
            }

            paddingString = String.Concat("0.", paddingString);
            if (precision > 0)
            {
                doubleNumber = Math.Round(doubleNumber, precision);
                numberString = doubleNumber.ToString(paddingString, numberFormat);
            }
            else
            {
                numberString = doubleNumber.ToString(numberFormat);
            }
            if (isArbitraryPrecision)
            {
                numberString = numberString.TrimEnd('0');
                numberString = numberString.TrimEnd(numberFormat.CurrencyDecimalSeparator.ToCharArray());
            }
            return numberString;
        }

        /// <summary>
        /// To format decimal values to their locale equivalents given a locale value
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="locale">The locale the number to be formated</param>
        /// <param name="precisionAndPadding">Rounds the result to specified number of fractional digits</param>
        /// <returns>The locale formatted number string</returns>
        public static String FormatNumber(Double number, String locale, Int32 precisionAndPadding)
        {
            String numberString = String.Empty;
            CultureInfo culture = null;
            NumberFormatInfo numberFormat = null;
            String paddingString = String.Empty;

            //automatically throws and exception if the locale is not in the correct format
            culture = new CultureInfo(locale);

            numberFormat = culture.NumberFormat;

            for (Int32 counter = 0; counter < precisionAndPadding; counter++)
            {
                paddingString += "0";
            }

            paddingString = String.Concat("0.", paddingString);

            if (precisionAndPadding > 0)
            {
                numberString = number.ToString(paddingString, numberFormat);
            }
            else
            {
                numberString = number.ToString(numberFormat);
            }

            return numberString;
        }

        /// <summary>
        /// Format the date(only) in the current locale
        /// </summary>
        /// <param name="dateString">The date string to be formatted</param>
        /// <returns>The locale specific date string</returns>
        public static String FormatDateOnly(String dateString)
        {
            CultureInfo currentCulture = null;

            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return FormatDateOnly(dateString, currentCulture.Name);
        }

        /// <summary>
        /// Format the date(only) in the specified locale
        /// </summary>
        /// <param name="dateString">The date string to be formatted</param>
        /// <param name="locale">The locale in which the format needs to be done</param>
        /// <returns>The locale specific date string</returns>
        public static String FormatDateOnly(String dateString, String locale)
        {
            String formattedDateString = String.Empty;
            CultureInfo culture = null;
            DateTimeFormatInfo dateTimeFormat = null;

            //automatically throws and exception if the locale is not in the correct format
            culture = new CultureInfo(locale);
            dateTimeFormat = culture.DateTimeFormat;

            formattedDateString = DateTime.Parse(dateString, CultureInfo.InvariantCulture).ToString("d", dateTimeFormat);

            return formattedDateString;
        }

        /// <summary>
        /// To covert the localized number string into a number value 
        /// </summary>
        /// <param name="numberString">The number string to be converted eg. 123,45</param>
        /// <returns>The converted number 123.45</returns>
        public static Double ConvertToNumber(String numberString)
        {
            CultureInfo currentCulture = null;

            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return ConvertToNumber(numberString, currentCulture.Name);
        }

        /// <summary>
        /// To covert the localized number string into a number value 
        /// </summary>
        /// <param name="numberString">The number string to be converted eg. 123,45</param>
        /// <param name="locale">The locale the number string is present in</param>
        /// <returns>The converted number 123.45</returns>
        public static Double ConvertToNumber(String numberString, String locale)
        {
            Double numberValue = 0.0;
            CultureInfo culture = null;
            Boolean isSuccess = false;
            NumberFormatInfo numberFormat = null;

            //automatically throws and exception if the locale is not in the correct format
            culture = new CultureInfo(locale);

            numberFormat = culture.NumberFormat;

            isSuccess = Double.TryParse(numberString, NumberStyles.Number, numberFormat, out numberValue);

            if(!isSuccess)
            {
                throw new ArgumentException("Not able to convert to number", numberString);
            }

            return numberValue;
        }

        /// <summary>
        /// Formats date(only) to its locale equivalent given a locale value
        /// </summary>
        /// <param name="dateString">Date which needs to be formatted</param>
        /// <param name="fromLocale">Locale the date is in</param>
        /// <param name="toLocale">Locale to which the date needs to be formatted</param>
        /// <returns>Formatted date string</returns>
        public static String FormatDateOnly(String dateString, String fromLocale, String toLocale)
        {
            String formattedDateString = String.Empty;
            DateTimeFormatInfo dateTimeFormat = null;

            //automatically throws and exception if the locale is not in the correct format
            CultureInfo fromCulture = new CultureInfo(fromLocale);

            //automatically throws and exception if the locale is not in the correct format
            CultureInfo toCulture = new CultureInfo(toLocale);
            dateTimeFormat = toCulture.DateTimeFormat;


            DateTime tryParseDateTime;
            DateTimeFormatInfo fromDateTimeFormat = fromCulture.DateTimeFormat;

            //If we use DateTime.Parse then it will add the current year if the year is not provided.If user is passing value as 11.5 then .Net will add current year.
            // Example::
            // Input  ::- DateString : 11.5
            // Output ::- formattedDateString : 05/11/2013.

            //If date is in proper format then DateTime.TryParse will return true and DateTime value based on dateString as a tryParseDateTime
            //else pass dateString as it is to formattedDateString

            if (DateTime.TryParse(dateString, fromCulture, DateTimeStyles.None, out tryParseDateTime))
            {
                formattedDateString = tryParseDateTime.ToString("d", dateTimeFormat);
            }
            else
            {
                throw new ArgumentException(String.Format("Unable to format date '{0}'.", dateString));
            }

            return formattedDateString;
        }

        /// <summary>
        /// Format the DateTime in the current locale
        /// </summary>
        /// <param name="date">The date to be formatted</param>
        /// <returns>The locale specific date string eg DD/MM/YY</returns>
        public static String FormatDate(DateTime date)
        {
            CultureInfo currentCulture = null;

            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return FormatDate(date, currentCulture.Name);
        }

        /// <summary>
        /// Format the DateTime in the locale required
        /// </summary>
        /// <param name="date">The date time to be formatter</param>
        /// <param name="locale">The locale in which the format needs to be done</param>
        /// <returns>The locale specific date string eg DD/MM/YY</returns>
        public static String FormatDate(DateTime date, String locale)
        {
            String dateString = String.Empty;
            CultureInfo culture = null;
            DateTimeFormatInfo dateTimeFormat = null;

            //automatically throws and exception if the locale is not in the correct format
            culture = new CultureInfo(locale);

            dateTimeFormat = culture.DateTimeFormat;

            dateString = date.ToString(dateTimeFormat);

            return dateString;
        }

        /// <summary>
        /// Format the date time from a locale to another
        /// </summary>
        /// <param name="date">Datetime value to be formatted</param>
        /// <param name="fromLocale">Locale in which value is given</param>
        /// <param name="toLocale">Locale in which value will be converted</param>
        /// <returns>The locale specific date string eg DD/MM/YY</returns>
        public static String FormatDate(String date, String fromLocale, String toLocale)
        {
            return FormatDate(date, fromLocale, toLocale, "g");
        }

        /// <summary>
        /// Format the date time from a locale to another
        /// </summary>
        /// <param name="date">Datetime value to be formatted</param>
        /// <param name="fromLocale">Locale in which value is given</param>
        /// <param name="toLocale">Locale in which value will be converted</param>
        /// <param name="dateFormat">Specifies date format</param>
        /// <returns>The locale specific date string in specified date format</returns>
        public static String FormatDate(String date, String fromLocale, String toLocale, String dateFormat)
        {
            //declare all variables
            String dateString = String.Empty;
            CultureInfo fromCulture = null;
            CultureInfo toCulture = null;
            DateTimeFormatInfo fromDateTimeFormat = null;
            DateTime dateTimeValue;

            //get from culture format, and parse string datetime value into datetime
            fromCulture = new CultureInfo(fromLocale);
            fromDateTimeFormat = fromCulture.DateTimeFormat;
            dateTimeValue = DateTime.Parse(date, fromDateTimeFormat);

            //automatically throws and exception if the locale is not in the correct format
            //get to culture format, and format datetime value in string
            toCulture = new CultureInfo(toLocale);
            dateString = dateTimeValue.ToString(dateFormat, toCulture);

            return dateString;
        }

        /// <summary>
        /// Format the datetime value from a locale to another, and convert the value from a timezone to another
        /// </summary>
        /// <param name="date">Datetime value to be formatted and converted</param>
        /// <param name="applyLocaleFormat">Whether to format datetime value or not</param>
        /// <param name="fromLocale">Locale in which value is sent</param>
        /// <param name="toLocale">Locale in which value will be converted</param>
        /// <param name="applyTimeZoneConversion">Whether to convert datetime to timezone or not</param>
        /// <param name="fromTimeZone">Timezone in which value is given</param>
        /// <param name="toTimeZone">Timezone in which value will be converted</param>
        /// <returns>The locale specific date string eg DD/MM/YY</returns>
        public static String FormatDate(String date, Boolean applyLocaleFormat, String fromLocale, String toLocale, Boolean applyTimeZoneConversion, String fromTimeZone, String toTimeZone)
        {
            if(applyTimeZoneConversion)
            {
                DateTime dateTimeValue = ConvertToTimeZone(date, fromTimeZone, toTimeZone);
                date = dateTimeValue.ToString("g", Constants.STORAGE_CULTUREINFO);
            }
            if(applyLocaleFormat)
            {
                date = FormatDate(date, fromLocale, toLocale);
            }
            else
            {
                date = FormatDate(date, "", "en-US");
            }
            return date;
        }

        /// <summary>
        /// Converts a UTC time for Storage as a String
        /// </summary>
        /// <param name="value">The UTC date time value</param>
        /// <returns>The UTC date time representation format</returns>
        public static String StoreDateTimeUtc(DateTime value)
        {
            String storageFormat = String.Empty;
            // e.g 2010-05-18T14:33:41.123
            storageFormat = value.ToString("yyyy-MM-ddTHH:mm:ss.fff",
                                     CultureInfo.InvariantCulture);

            return storageFormat;
        }

        private string GetDisplayDateTimeAsString(DateTime dateValue, CultureInfo culture)
        {
            string result = String.Empty;
            if(dateValue != null)
            {
                string dateFormat = culture.DateTimeFormat.ShortDatePattern;
                string timeFormat = culture.DateTimeFormat.ShortTimePattern;

                string strDate = dateValue.ToString(dateFormat);
                string strTime = dateValue.ToString(timeFormat);

                result = string.Format("{0} {1}", strDate, strTime);
            }

            return result;
        }

        #endregion Formatting methods

        #region Deformatting methods

        /// <summary>
        /// To deformat decimal values from their locale equivalents given a locale value
        /// The current UI Thread locale is used for obtaining the Number format
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="numberString">The number to be formatted</param>
        /// <returns>The number parsed from formatted string</returns>
        public static Double DeformatNumber(String numberString)
        {
            CultureInfo currentCulture = null;

            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return DeformatNumber(numberString, currentCulture.Name);
        }

        /// <summary>
        /// To deformat decimal values from their locale equivalents given a locale value
        /// The locale string should be any valid locale format like en-US, fr-FR etc
        /// </summary>
        /// <param name="numberString">The number to be formatted</param>
        /// <param name="locale">The locale the number to be formated</param>
        /// <returns>The number parsed from formatted string</returns>
        public static Double DeformatNumber(String numberString, String locale)
        {
            Double number = 0;
            CultureInfo culture = null;
            NumberFormatInfo numberFormat = null;
            
            //automatically throws and exception if the locale is not in the correct format
            culture = new CultureInfo(locale);

            numberFormat = culture.NumberFormat;

            number = Double.Parse(numberString,numberFormat);

            return number;
        }

        /// <summary>
        /// De-Format the DateTime in the locale required
        /// </summary>
        /// <param name="dateTimeValue">The date time value to be de-formatted</param>
        /// <param name="locale">The locale in which the format needs to be done</param>
        /// <returns>De-formatted DateTime object</returns>
        public static DateTime DeformatDate(String dateTimeValue, String locale)
        {
            DateTime dateTime;

            //automatically throws and exception if the locale is not in the correct format
            CultureInfo culture = new CultureInfo(locale);

            dateTime = DateTime.Parse(dateTimeValue, culture);

            return dateTime;
        }

        /// <summary>
        /// Converts a UTC date time representation format to the UTC time
        /// </summary>
        /// <param name="value">The UTC Date Time String e.g 2003-10-26T14:33:41.123</param>
        /// <returns>The UTC Date Time</returns>
        public static DateTime ReadDateTimeUtc(String value)
        {
            DateTime utcTime = DateTime.MinValue;

            utcTime = DateTime.Parse(value, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AdjustToUniversal);

            return utcTime;
        }

        #endregion Deformatting methods

        #region Timezone methods

        /// <summary>
        /// Convert a time from one zone to another
        /// For a list of timezone information support look at http://msdn.microsoft.com/en-us/library/bb384272.aspx
        /// </summary>
        /// <param name="date">The date time value</param>
        /// <param name="sourceTimeZoneId">The source timezone the value is in</param>
        /// <param name="destinationTimeZoneId">The destination timezone to be converted</param>
        /// <returns>The converted date time in the destination timezone</returns>
        public static DateTime ConvertToTimeZone(DateTime date, String sourceTimeZoneId, String destinationTimeZoneId)
        {
            TimeZoneInfo sourceTimeZone = null;
            TimeZoneInfo destinationTimeZone = null;
            DateTime requiredDate = DateTime.MinValue;

            sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
            destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId);

            //Need to set kind of datetime to undefined because if date's kind doesnot match with sourceTimeZone, convertTime method will throw exception.
            DateTime dateTimeToConvert = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            requiredDate = TimeZoneInfo.ConvertTime(dateTimeToConvert, sourceTimeZone, destinationTimeZone);

            return requiredDate;    
        }

        /// <summary>
        /// Convert a time from one zone to another
        /// For a list of timezone information support look at http://msdn.microsoft.com/en-us/library/bb384272.aspx
        /// </summary>
        /// <param name="date">The date time value as a string</param>
        /// <param name="sourceTimeZoneId">The source timezone the value is in</param>        
        /// <param name="destinationTimeZoneId">The destination timezone to be converted</param>
        /// <returns>The converted date time in the destination timezone</returns>
        public static DateTime ConvertToTimeZone(String date, String sourceTimeZoneId, String destinationTimeZoneId)
        {
            DateTime dateValue = DateTime.MinValue;

            if (!String.IsNullOrEmpty(date))
            {
                dateValue = DateTime.Parse(date);
            }

            return ConvertToTimeZone(dateValue, sourceTimeZoneId, destinationTimeZoneId); ;
        }

        /// <summary>
        /// Get the timezone difference between source and destination timezones in ticks
        /// </summary>
        /// <param name="date">The date for which the timezone difference is required, this is made use for daylight saving time calculation which are applicable for this date</param>
        /// <param name="sourceTimeZoneId">The source timezone</param>
        /// <param name="destinationTimeZoneId">The destination timezone</param>
        /// <returns>The timezone difference between the zones</returns>
        public static Int64 GetTimeZoneDifference(DateTime date, String sourceTimeZoneId, String destinationTimeZoneId)
        {
            TimeZoneInfo sourceTimeZone = null;
            TimeZoneInfo destinationTimeZone = null;
            DateTime requiredDate = DateTime.MinValue;
            TimeSpan difference = TimeSpan.Zero;


            sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
            destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId);


            requiredDate = TimeZoneInfo.ConvertTime(date, sourceTimeZone, destinationTimeZone);

            difference = date.Subtract(requiredDate);

            return difference.Ticks;
        }

        #endregion Timezone methods

        #region Format checking methods

        /// <summary>
        /// Validates the Decimal value as per the given locale
        /// </summary>
        /// <param name="decimalValue">Value to be validate</param>
        /// <param name="formatLocale">Locale</param>
        /// <returns>True if Decimal value formate is correct as per the locale</returns>
        public static Boolean IsProperDecimal(String decimalValue, LocaleEnum formatLocale)
        {
            String cultureName = formatLocale.GetCultureName();

            //Get Culture Info
            CultureInfo cultureInfo = new CultureInfo(cultureName);

            String decimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator;
            Regex decimalRegEx = new Regex(@"(?:-- Select --)|^[0-9-]*([\" + decimalSeparator + "][0-9]*(|[eE][-+]?[0-9]+))?$");
            if(!decimalRegEx.IsMatch(decimalValue))
                return false;
            else
                return true;
        }

        //public static Boolean IsProperDateTime(String dateTimeValue, LocaleEnum formatLocale)
        //{
        //    String cultureName = formatLocale.GetCultureName();

        //    //Get Culture Info
        //    CultureInfo cultureInfo = new CultureInfo(cultureName);

        //    Regex dateRegex = new Regex(@"^(?:(?:(?:0?[13578]|1[02])(\/)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{4})$|^(?:0?2(\/)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:(?:0?[1-9])|(?:1[0-2]))(\/)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{4})$");

        //    if(!dateRegex.IsMatch(DateTime))
        //    {
        //        throw new Exception("Date Time is not a valid format");
        //    }

        //    String decimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator;
        //    Regex decimalRegEx = new Regex(@"(?:-- Select --)|^[0-9-]*([\" + decimalSeparator + "][0-9]*)?$");

        //    if(!decimalRegEx.IsMatch(decimalValue))
        //        return false;
        //    else
        //        return true;
        //}

        /// <summary>
        /// Verifies whether the Guid is valid or not
        /// </summary>
        /// <param name="data">Indicates the Guid to be validated</param>
        /// <returns>True - If Guid is valid, False - If the Guid is invalid</returns>
        public static Boolean IsValidGuid(String data)
        {
            Guid result;
            return Guid.TryParse(data, out result);
        }

        #endregion Format checking methods

        #region Format Provider Methods

        /// <summary>
        /// Gets the short date format for current thread culture.
        /// </summary>
        /// <returns>short date format for current thread culture as string</returns>
        public static String GetCurrentCultureShortDateFormat()
        {
            return GetShortDateFormat(Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Gets the short datetime format for current thread culture.
        /// </summary>
        /// <returns>short datetime format for current thread culture as string</returns>
        public static String GetCurrentCultureShortDateTimeFormat()
        {
            DateTimeFormatInfo dateTimeFormatInfo = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
            return String.Format("{0} {1}",
                dateTimeFormatInfo.ShortDatePattern,
                dateTimeFormatInfo.ShortTimePattern).Replace("'", String.Empty);
        }

        /// <summary>
        /// Gets the short time format for current thread culture as per jQuery time picker conventions
        /// http://trentrichardson.com/examples/timepicker/
        /// t -> T
        /// </summary>
        /// <returns>short time format for current thread culture as per jQuery time picker conventions</returns>
        public static String GetCurrentCultureJQueryShortTimeFormat()
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("'", String.Empty).Replace("t", "T");
        }

        /// <summary>
        /// Gets the short date format for current thread culture as per jQuery conventions
        /// Following replacements are needed for jquery date format
        /// dddd -> DD
        /// ddd  -> D
        /// MMMM -> MM
        /// MMM  -> M
        /// MM   -> mm
        /// M    -> m
        /// yyyy -> yy
        /// yy   -> y
        /// </summary>
        /// <returns>short date format for current thread culture as per jQuery conventions</returns>
        public static String GetCurrentCultureJQueryShortDateFormat()
        {
            String format = GetShortDateFormat(Thread.CurrentThread.CurrentCulture);

            #region Year
            if (format.IndexOf("yyyy") >= 0)
            {
                format = format.Replace("yyyy", "yy");
            }
            else if (format.IndexOf("yy") >= 0)
            {
                format = format.Replace("yy", "y");
            }
            #endregion

            #region Day
            if (format.IndexOf("dddd") >= 0)
            {
                format = format.Replace("dddd", "DD");
            }
            else if (format.IndexOf("ddd") >= 0)
            {
                format = format.Replace("ddd", "D");
            }
            #endregion

            #region Month
            if (format.IndexOf("MMMM") >= 0)
            {
                format = format.Replace("MMMM", "MM");
            }
            else if (format.IndexOf("MMM") >= 0)
            {
                format = format.Replace("MMM", "M");
            }
            else if (format.IndexOf("MM") >= 0)
            {
                format = format.Replace("MM", "mm");
            }
            else if (format.IndexOf("M") >= 0)
            {
                format = format.Replace("M", "m");
            }
            #endregion

            return format;
        }

        /// <summary>
        /// Gets short date format for given Locale.
        /// If given locale is Unknown then it will get short date format for current culture.
        /// </summary>
        /// <param name="locale">Locale for which short date format is needed</param>
        /// <returns>short date format for given locale as string</returns>
        public static String GetShortDateFormat(LocaleEnum locale)
        {
            CultureInfo culture = locale == LocaleEnum.UnKnown ?
                Thread.CurrentThread.CurrentCulture :
                new CultureInfo(locale.GetCultureName());

            return GetShortDateFormat(culture);
        }

        /// <summary>
        /// Gets short date format for given culture.
        /// If given culture is null then it will get short date format for current culture.
        /// </summary>
        /// <param name="culture">Culture for which the short format is needed.</param>
        /// <returns>short date format for given culture as string</returns>
        public static String GetShortDateFormat(CultureInfo culture)
        {
            if (culture == null)
            {
                culture = Thread.CurrentThread.CurrentCulture;
            }
            return culture.DateTimeFormat.ShortDatePattern.Replace("'", String.Empty);
        }

        /// <summary>
        /// Gets the display format for grid columns
        /// </summary>
        /// <param name="DataType">datatype for which the display format needs to be fetched</param>
        /// <returns>localized display format</returns>
        public static String GetDisplayFormatForGrid(String DataType)
        {
            String displayFormat = String.Empty;

            if (DataType == "Date" || DataType == "DateTime")
            {
                displayFormat = "m/d/Y h:i A";// Currently the date/datetime display format is hardcoded for all grid datetime columns. This format is not being used for actual data formatting.
            }

            return displayFormat;
        }

        #endregion

        #region Date Type conversions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MM"></param>
        /// <param name="DD"></param>
        /// <param name="YYYY"></param>
        /// <returns></returns>
        public static String ConvertGregorianDatesToJulianDate(int MM, int DD, int YYYY)
        {
            //To convert Gregorian dates to Julian dates:
            String myGregorianDate = MM + "/" + DD + "/" + YYYY;
            //string myGregorianDate = "10/15/1582";
            DateTime dt = DateTime.Parse(myGregorianDate);

            Double A = Math.Floor((Double)dt.Year / 100);
            Double B = Math.Floor(A / 4);
            Double C = 2 - A + B;
            Double D = dt.Day;
            Double E = Math.Floor(365.25 * (dt.Year + 4716));
            Double F = Math.Floor(30.6001 * (dt.Month + 1));

            Double JD = C + D + E + F - 1524.5;
            string myJulianDate = JD.ToString(); // 2299160.5

            return myJulianDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juliandate"></param>
        /// <returns></returns>
        public static String ConvertJulianDatesToGregorianDate(Double juliandate)
        {
            //To convert Julian dates to Gregorian dates:

            //Double JD = 2299160.5;
            Double JD = juliandate;

            Double Z = Math.Floor(JD + 0.5);
            Double W = Math.Floor((Z - 1867216.25) / 36524.25);
            Double X = Math.Floor(W / 4);
            Double AA = Math.Floor(Z + 1 + W - X);
            Double BB = Math.Floor(AA + 1524);
            Double CC = Math.Floor((BB - 122.1) / 365.25);
            Double DD = Math.Floor(365.25 * CC);
            Double EE = Math.Floor((BB - DD) / 30.6001);
            Double FF = Math.Floor(30.6001 * EE);

            Double Day = BB - DD - FF;
            Double Month;
            Double Year;

            if ((EE - 13) <= 12 && (EE - 13) > 0)
                Month = EE - 13;
            else
                Month = EE - 1;

            if (Month == 1 || Month == 2)
                Year = CC - 4715;
            else
                Year = CC - 4716;

            //string GregorianDate = string.format("{0}/{1}/{2}",Month,Day,Year); // 10/15/1582
            String GregorianDate = Month.ToString() + "/" + Day.ToString() + "/" + Year.ToString();
            return GregorianDate;
        }

        #endregion

        #endregion
    }
}