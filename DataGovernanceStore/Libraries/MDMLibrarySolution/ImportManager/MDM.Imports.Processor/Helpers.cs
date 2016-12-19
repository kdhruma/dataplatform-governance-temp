using System;
using System.Text.RegularExpressions;

namespace MDM.Imports.Processor
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Static Helpers Methods
    /// </summary>
    public static class Helpers
    {
        #region Fields

        /// <summary>
        /// Indicates RegEx options.
        /// </summary>
        private static RegexOptions _options = RegexOptions.IgnoreCase | RegexOptions.RightToLeft | RegexOptions.Compiled;

        /// <summary>
        /// Indicates regex format.
        /// </summary>
        private static String _regFormat = string.Format("[{0}{1} .,+\\/_#''-]", @"\n", @"\r");

        #endregion Fields

        #region Methods

        #region Public Methods

        /// <summary>
        /// Computes the key value for the given attribute value based on its data type.
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeDataType"></param>
        /// <param name="itemValue"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String ComputeKeyValue(Int32 attributeId, AttributeDataType attributeDataType, Value itemValue, Int32 locale)
        {
            String keyVal = String.Empty;

            switch (attributeDataType)
            {
                case AttributeDataType.Decimal:
                    break;
                case AttributeDataType.Fraction:
                    break;
                case AttributeDataType.Integer:
                    break;
                case AttributeDataType.Date:
                    keyVal = Convert.ToDateTime(itemValue.AttrVal).ToString("yyyyMMdd");
                    break;
                case AttributeDataType.DateTime:
                    keyVal = Convert.ToDateTime(itemValue.AttrVal).ToString("yyyyMMdd");
                    break;
                default:
                    keyVal = Regex.Replace(itemValue.AttrVal.ToString(), _regFormat, " ", _options);
                    break;
            }
            return keyVal;
        }

        /// <summary>
        /// Computes the search value for the attribute values based on the attribute type.
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeDataType"></param>
        /// <param name="itemValue"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String ComputeSearchValue(Int32 attributeId, AttributeDataType attributeDataType, Value itemValue, Int32 locale)
        {
            String searchVal = String.Empty;

            switch (attributeDataType)
            {
                case AttributeDataType.Decimal:
                    break;
                case AttributeDataType.Fraction:
                    break;
                case AttributeDataType.Integer:
                    break;
                case AttributeDataType.Date:
                    searchVal = string.Format("q{0}L{1}z{2}", attributeId, locale, Helpers.ComputeKeyValue(attributeId, attributeDataType, itemValue, locale));
                    break;
                case AttributeDataType.DateTime:
                    searchVal = string.Format("q{0}L{1}z{2}", attributeId, locale, Helpers.ComputeKeyValue(attributeId, attributeDataType, itemValue, locale));
                    break;
                default:
                    string replacementString = String.Format(" q{0}L{1}z", attributeId, locale);
                    searchVal = Regex.Replace(itemValue.AttrVal.ToString(), _regFormat, replacementString, _options);
                    searchVal = String.Format("q{0}L{1}z{2}", attributeId, locale, searchVal);
                    break;
            }
            return searchVal;
        }

        /// <summary>
        /// For the DN search compute the key value with header information.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static String ComputeKeyValueWithHeader(Entity entity, String keyValue)
        {
            String shortName = Regex.Replace(entity.Name, _regFormat, " ", _options);
            String longName = Regex.Replace(entity.LongName, _regFormat, " ", _options);
            String sku = Regex.Replace(entity.SKU, _regFormat, " ", _options);
            String header = String.Format(" {0} {1} {2}", shortName, longName, sku);
            return String.Concat(keyValue, header);
        }

        /// <summary>
        /// For the DN search compute the search value with header information.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static String ComputeSearchValueWithHeader(Entity entity, String searchValue)
        {
            String shortName = Regex.Replace(entity.Name, _regFormat, " q22z", _options);
            String longName = Regex.Replace(entity.LongName, _regFormat, " q23z", _options);
            String sku = Regex.Replace(entity.SKU, _regFormat, " q24z", _options);

            String idPath = String.Empty;

            //Suppose entity.IdPath is 1 9 41
            //If Key is enabled, then id path will be q1z1c q1zc9c q1zc41 in searchVal
            //If Key is disabled, then id path will be q1z1c9c41 in searchVal

            if (!String.IsNullOrWhiteSpace(entity.IdPath))
            {
                    idPath = entity.IdPath.Trim().Replace(" ", "c q1z") + "c";
                }

            String header = String.Format("q1z{0} q3z{1} q27z{2} q22z{3} q23z{4} q24z{5} q4z{6} ", idPath, entity.EntityTypeId, entity.Id, shortName, longName, sku, entity.ContainerId);
            return String.Concat(header, searchValue);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public static Int32 GetMatchProfileIdFromImportProfile(ImportProfile importProfile)
        {
            Int32 profileId = 0;

            String matchProcessProfileId = String.Empty;

            if (importProfile.InputSpecifications.ReaderSettings["MatchProfileId"] != null && importProfile.InputSpecifications.ReaderSettings["MatchProfileId"].Value != "")
            {
                matchProcessProfileId = importProfile.InputSpecifications.ReaderSettings["MatchProfileId"].Value;
            }

            if (String.IsNullOrEmpty(matchProcessProfileId))
                return profileId;

            // Try to get file id from the job data parameter..
            profileId = ValueTypeHelper.ConvertToInt32(matchProcessProfileId);

            return profileId;
        }

        /// <summary>
        /// Gets the job status based on successful, failed and partial success item count.
        /// </summary>
        /// <param name="successfulItemCount">Indicates successful item count.</param>
        /// <param name="failedItemCount">Indicates failed item count.</param>
        /// <param name="partialSuccessItemCount">Indicates partial success item count.</param>
        /// <returns>Returns the job status based on given input.</returns>
        public static JobStatus GetJobStatus(Int64 successfulItemCount, Int64 failedItemCount, Int64 partialSuccessItemCount)
        {
            JobStatus finalStatus = JobStatus.UnKnown;

            if (successfulItemCount > 0 && failedItemCount == 0 && partialSuccessItemCount == 0)
            {
                finalStatus = JobStatus.Completed;
            }
            else if ((failedItemCount > 0 && successfulItemCount > 0 && partialSuccessItemCount == 0) || (failedItemCount > 0 && successfulItemCount == 0 && partialSuccessItemCount == 0))
            {
                finalStatus = JobStatus.CompletedWithErrors;
            }
            else if (partialSuccessItemCount > 0 && failedItemCount > 0)
            {
                finalStatus = JobStatus.CompletedWithWarningsAndErrors;
            }
            else if (partialSuccessItemCount > 0 && failedItemCount == 0)
            {
                finalStatus = JobStatus.CompletedWithWarnings;
            }
            else
            {
                finalStatus = JobStatus.Completed;
            }

            return finalStatus;
        }

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}