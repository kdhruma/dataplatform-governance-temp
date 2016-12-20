using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace MDM.InstrumentationManager.Utility.Objects
{
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;

    /// <summary>
    /// Class is used to load DiagnosticActivityCollection from Stream using XmlReader and filter Deserialized Data using DiagnosticReportSettings
    /// </summary>
    public class DiagnosticReportFilterUtility
    {
        #region Fields

        private DiagnosticReportSettings _settings = null;

        private Dictionary<SearchColumn, Collection<String>> selectedKeywordValues; 

        private Collection<SearchColumn> _scopeToKeywordCheck;

        private Collection<SearchColumn> _scopeToEqualityCheck;

        private Collection<SearchColumn> _scopeToNameCheck; 

        private Dictionary<SearchColumn, Collection<Int64>> _selectedFiltersValues;

        private Dictionary<SearchColumn, Collection<String>> _selectedFiltersByNameValues; 

        private Boolean _searchByKeywordsRequested;

        private Boolean _activitiesMessageClassSelected;

        private Boolean _recordsKeywordsSearchRequested;

        private Boolean _durationFiltrationRequired;

        private Func<Double, Boolean> _durationFilterFunction;

        private Int32 _duration;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public DiagnosticReportFilterUtility(DiagnosticReportSettings settings)
        {
            _settings = settings;

            if (_settings == null)
            {
                return;
            }

            _searchByKeywordsRequested = settings.SearchKeywords != null && settings.SearchKeywords.Any() &&
                                       settings.SearchColumns != null && settings.SearchColumns.Any();

            _recordsKeywordsSearchRequested = _searchByKeywordsRequested && settings.SearchColumns != null &&
                                              (settings.SearchColumns.Contains(SearchColumn.All) ||
                                               settings.SearchColumns.Contains(SearchColumn.Message) ||
                                               settings.SearchColumns.Contains(SearchColumn.ThreadId));

            _scopeToKeywordCheck = settings.SearchColumns;

            _activitiesMessageClassSelected = settings.MessageClasses.Count == 0 || settings.MessageClasses.Contains(MessageClassEnum.Information);

            selectedKeywordValues = new Dictionary<SearchColumn, Collection<String>>();

            _scopeToEqualityCheck = new Collection<SearchColumn>();
            _selectedFiltersValues = new Dictionary<SearchColumn, Collection<Int64>>();

            _scopeToNameCheck = new Collection<SearchColumn>();
            _selectedFiltersByNameValues = new Dictionary<SearchColumn, Collection<String>>();

            if (settings.ThreadIds.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.ThreadId);
                _selectedFiltersValues.Add(SearchColumn.ThreadId, CastCollection(settings.ThreadIds));
            }

            if (settings.LegacyMDMTraceSources.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.LegacyTraceSource);
                _selectedFiltersValues.Add(SearchColumn.LegacyTraceSource, CastEnumCollectionToLongCollection(settings.LegacyMDMTraceSources));
            }

            FillSearchScopeByKeywords(settings);

            FillSearchScopeByCallerContext(settings);

            FillSearchScopeByCallDataContext(settings);

            FillSearchScopeBySecurityContext(settings);

            InitializeDurationCheckMethod(settings);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads Diagnostic Activity Collection from Stream using Xml Reader
        /// </summary>
        /// <param name="reader"></param>
        public DiagnosticActivityCollection LoadFromStream(XmlReader reader)
        {
            DiagnosticActivityCollection collection = new DiagnosticActivityCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DiagnosticActivity")
                    {
                        DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

                        diagnosticActivity.LoadFromStream(reader);

                        Boolean appropriate = true;

                        if (_settings != null)
                        {
                            appropriate = FilterActivity(diagnosticActivity);
                        }

                        if (appropriate)
                        {
                            collection.Add(diagnosticActivity);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "DiagnosticActivities")
                    {
                        break;
                    }
                }
            }

            return collection;
        }

        #endregion

        #region Private Methods

        private void FilterActivities(DiagnosticActivityCollection activityCollection)
        {
            if (!activityCollection.Any())
            {
                return;
            }

            Int32 index = 0;

            while (activityCollection.Count > index)
            {
                DiagnosticActivity item = activityCollection.ElementAt(index);

                if (!FilterActivity(item))
                {
                    activityCollection.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        private Boolean FilterActivity(DiagnosticActivity activity)
        {
            Boolean searchByKeywordsPassed = true;

            Boolean searchByContextFiltersPassed = true;

            Boolean searchByContextFiltersByNamesPassed = true;

            FilterActivities(activity.DiagnosticActivities);

            FilterRecords(activity.DiagnosticRecords);

            Boolean childLoaded = activity.DiagnosticActivities.Any() || activity.DiagnosticRecords.Any();

            Boolean searchByKeyWordsRequired = _searchByKeywordsRequested && !childLoaded;

            Boolean durationFilterPassed = true;

            if (searchByKeyWordsRequired)
            {
                Dictionary<SearchColumn, String> dictionary = PrepareActivityData(activity, _scopeToKeywordCheck);

                searchByKeywordsPassed = CheckBySubstring(dictionary, _scopeToKeywordCheck, selectedKeywordValues, true);
            }

            if (!childLoaded && searchByKeywordsPassed && _durationFiltrationRequired)
            {
                durationFilterPassed = _durationFilterFunction(activity.DurationInMilliSeconds);
            }

            if (!childLoaded && searchByKeywordsPassed && durationFilterPassed && _scopeToEqualityCheck.Any())
            {
                Dictionary<SearchColumn, Collection<Int64>> activityContextData = PrepareActivityContextData(activity, _scopeToEqualityCheck);

                searchByContextFiltersPassed = CheckByEquality(activityContextData, _scopeToEqualityCheck, _selectedFiltersValues);
            }

            if (!childLoaded && searchByKeywordsPassed && durationFilterPassed && searchByContextFiltersPassed && _scopeToNameCheck.Any())
            {
                Dictionary<SearchColumn, String> activityContextDataWithNames = PrepareActivityContextDataWithNames(activity, _scopeToNameCheck);

                searchByContextFiltersByNamesPassed = CheckBySubstring(activityContextDataWithNames, _scopeToNameCheck, _selectedFiltersByNameValues);
            }

            return childLoaded || (searchByKeywordsPassed && durationFilterPassed && searchByContextFiltersPassed && searchByContextFiltersByNamesPassed && _activitiesMessageClassSelected);
        }

        private Dictionary<SearchColumn, String> PrepareActivityData(DiagnosticActivity activity, Collection<SearchColumn> scopeToCheck)
        {
            Dictionary<SearchColumn, String> result = new Dictionary<SearchColumn, String>();

            foreach (SearchColumn columnToCheck in scopeToCheck)
            {
                String value = null;

                switch (columnToCheck)
                {
                    case SearchColumn.ActivityName:
                        value = activity.ActivityName;
                        break;
                }

                if (!String.IsNullOrEmpty(value))
                {
                    result.Add(columnToCheck, value);
                }
            }

            return result;
        }

        private Dictionary<SearchColumn, Collection<Int64>> PrepareActivityContextData(DiagnosticActivity activity, Collection<SearchColumn> scopeToCheck)
        {
            Dictionary<SearchColumn, Collection<Int64>> result = new Dictionary<SearchColumn, Collection<Int64>>();

            Collection<Int64> value = new Collection<Int64>();

            foreach (SearchColumn columnToCheck in scopeToCheck)
            {
                switch (columnToCheck)
                {
                    case SearchColumn.ThreadId:
                        value.Add(activity.ThreadId);
                        break;

                    case SearchColumn.LegacyTraceSource:
                        value = CastEnumCollectionToLongCollection(activity.ExecutionContext.LegacyMDMTraceSources);
                        break;

                        // Caller Context

                    case SearchColumn.ApplicationId:
                        value.Add((Int64) activity.ExecutionContext.CallerContext.Application);
                        break;

                    case SearchColumn.MDMPublisherId:
                        value.Add((Int64) activity.ExecutionContext.CallerContext.MDMPublisher);
                        break;

                    case SearchColumn.ModuleId:
                        value.Add((Int64) activity.ExecutionContext.CallerContext.Module);
                        break;

                    case SearchColumn.MDMSourceId:
                        value.Add((Int64) activity.ExecutionContext.CallerContext.MDMSource);
                        break;

                    case SearchColumn.MDMSubscriberId:
                        value.Add((Int64) activity.ExecutionContext.CallerContext.MDMSubscriber);
                        break;

                    case SearchColumn.ServerId:
                        value.Add(activity.ExecutionContext.CallerContext.ServerId);
                        break;

                    case SearchColumn.JobId:
                        value.Add(activity.ExecutionContext.CallerContext.JobId);
                        break;

                    case SearchColumn.ProfileId:
                        value.Add(activity.ExecutionContext.CallerContext.ProfileId);
                        break;

                        // CallDataContext

                    case SearchColumn.EntityId:
                        value = activity.ExecutionContext.CallDataContext.EntityIdList;
                        break;

                    case SearchColumn.EntityTypeId:
                        value = CastCollection(activity.ExecutionContext.CallDataContext.EntityTypeIdList);
                        break;

                    case SearchColumn.OrganizationId:
                        value = CastCollection(activity.ExecutionContext.CallDataContext.OrganizationIdList);
                        break;

                    case SearchColumn.ContainerId:
                        value = CastCollection(activity.ExecutionContext.CallDataContext.ContainerIdList);
                        break;

                    case SearchColumn.RelationshipTypeId:
                        value = CastCollection(activity.ExecutionContext.CallDataContext.RelationshipTypeIdList);
                        break;

                    case SearchColumn.CategoryId:
                        value = activity.ExecutionContext.CallDataContext.CategoryIdList;
                        break;

                    case SearchColumn.AttributeId:
                        value = CastCollection(activity.ExecutionContext.CallDataContext.AttributeIdList);
                        break;

                    case SearchColumn.Locale:
                        value = CastEnumCollectionToLongCollection(activity.ExecutionContext.CallDataContext.LocaleList);
                        break;

                        // Security Context

                    case SearchColumn.UserId:
                        value.Add(activity.ExecutionContext.SecurityContext.UserId);
                        break;

                    case SearchColumn.UserRoleId:
                        value.Add(activity.ExecutionContext.SecurityContext.UserRoleId);
                        break;

                }

                if (value.Count > 0)
                {
                    result.Add(columnToCheck, value);
                }
            }

            return result;
        }
        
        private Dictionary<SearchColumn, String> PrepareActivityContextDataWithNames(DiagnosticActivity activity, Collection<SearchColumn> scopeToCheck)
        {
            Dictionary<SearchColumn, String> result = new Dictionary<SearchColumn, String>();

            foreach (SearchColumn columnToCheck in scopeToCheck)
            {
                String value = null;

                switch (columnToCheck)
                {
                    // Caller Context

                    case SearchColumn.ProgramName:
                        value = activity.ExecutionContext.CallerContext.ProgramName;
                        break;

                    case SearchColumn.ServerName:
                        value = activity.ExecutionContext.CallerContext.ServerName;
                        break;

                    case SearchColumn.ProfileName:
                        value = activity.ExecutionContext.CallerContext.ProfileName;
                        break;

                    // Security Context

                    case SearchColumn.UserName:
                        value = activity.ExecutionContext.SecurityContext.UserLoginName;
                        break;

                    case SearchColumn.UserRoleName:
                        value = activity.ExecutionContext.SecurityContext.UserRoleName;
                        break;

                }

                if (!String.IsNullOrEmpty(value))
                {
                    result.Add(columnToCheck, value);
                }
            }

            return result;
        }
        
        private void FilterRecords(DiagnosticRecordCollection records)
        {
            if (!records.Any())
            {
                return;
            }

            Int32 index = 0;

            while (records.Count > index)
            {
                DiagnosticRecord item = records.ElementAt(index);

                if (!FilterRecord(item))
                {
                    records.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        private Boolean FilterRecord(DiagnosticRecord record)
        {
            Boolean searchByKeywordsPassed = true;
            Boolean searchByMessageClassPassed = true;
            Boolean searchByContextPassed = true;
            Boolean searchByContextFiltersByNamePassed = true;
            Boolean searchByDurationFilterPassed = true;

            if (_recordsKeywordsSearchRequested)
            {
                Dictionary<SearchColumn, String> recordData = PrepareRecordData(record, _scopeToKeywordCheck);

                searchByKeywordsPassed = CheckBySubstring(recordData, _scopeToKeywordCheck, selectedKeywordValues, true);
            }

            if (searchByKeywordsPassed)
            {
                searchByMessageClassPassed = _settings.MessageClasses.Count == 0 || _settings.MessageClasses.Contains(record.MessageClass);

                if (searchByMessageClassPassed && _durationFiltrationRequired)
                {
                    searchByDurationFilterPassed = _durationFilterFunction(record.DurationInMilliSeconds);
                }

                if (searchByMessageClassPassed && searchByDurationFilterPassed && _scopeToEqualityCheck.Any())
                {
                    Dictionary<SearchColumn, Collection<Int64>> recordContextData = PrepareRecordContextData(record, _scopeToEqualityCheck);

                    searchByContextPassed = CheckByEquality(recordContextData, _scopeToEqualityCheck, _selectedFiltersValues);
                }

                if (searchByMessageClassPassed && searchByDurationFilterPassed && searchByContextPassed && _scopeToNameCheck.Any())
                {
                    Dictionary<SearchColumn, String> recordContextData = PrepareRecordContextDataWithNames(record, _scopeToNameCheck);

                    searchByContextFiltersByNamePassed = CheckBySubstring(recordContextData, _scopeToNameCheck, _selectedFiltersByNameValues);
                }
            }

            return searchByKeywordsPassed && searchByDurationFilterPassed && searchByMessageClassPassed && (_searchByKeywordsRequested == _recordsKeywordsSearchRequested) && searchByContextPassed && searchByContextFiltersByNamePassed;
        }

        private Dictionary<SearchColumn, String> PrepareRecordData(DiagnosticRecord record, Collection<SearchColumn> scopeToCheck)
        {
            Dictionary<SearchColumn, String> result = new Dictionary<SearchColumn, String>();

            foreach (SearchColumn columnToCheck in scopeToCheck)
            {
                String value = null;

                switch (columnToCheck)
                {
                    case SearchColumn.Message:
                        value = record.Message;
                        break;
                }

                if (!String.IsNullOrEmpty(value))
                {
                    result.Add(columnToCheck, value);
                }
            }

            return result;
        }

        private Dictionary<SearchColumn, String> PrepareRecordContextDataWithNames(DiagnosticRecord record, Collection<SearchColumn> scopeToCheck)
        {
            Dictionary<SearchColumn, String> result = new Dictionary<SearchColumn, String>();

            foreach (SearchColumn columnToCheck in scopeToCheck)
            {
                String value = null;

                switch (columnToCheck)
                {
                    // Caller Context

                    case SearchColumn.ProgramName:
                        value = record.ExecutionContext.CallerContext.ProgramName;
                        break;

                    case SearchColumn.ServerName:
                        value = record.ExecutionContext.CallerContext.ServerName;
                        break;

                    case SearchColumn.ProfileName:
                        value = record.ExecutionContext.CallerContext.ProfileName;
                        break;

                    // Security Context

                    case SearchColumn.UserName:
                        value = record.ExecutionContext.SecurityContext.UserLoginName;
                        break;

                    case SearchColumn.UserRoleName:
                        value = record.ExecutionContext.SecurityContext.UserRoleName;
                        break;

                }

                if (!String.IsNullOrEmpty(value))
                {
                    result.Add(columnToCheck, value);
                }
            }

            return result;
        }

        private Dictionary<SearchColumn, Collection<Int64>> PrepareRecordContextData(DiagnosticRecord record, Collection<SearchColumn> scopeToCheck)
        {
            Dictionary<SearchColumn, Collection<Int64>> result = new Dictionary<SearchColumn, Collection<Int64>>();

            Collection<Int64> value = new Collection<Int64>();

            foreach (SearchColumn columnToCheck in scopeToCheck)
            {
                switch (columnToCheck)
                {
                    case SearchColumn.ThreadId:
                        value.Add(record.ThreadId);
                        break;

                    case SearchColumn.LegacyTraceSource:
                        value = CastEnumCollectionToLongCollection(record.ExecutionContext.LegacyMDMTraceSources);
                        break;

                        // Caller Context

                    case SearchColumn.ApplicationId:
                        value.Add((Int64) record.ExecutionContext.CallerContext.Application);
                        break;

                    case SearchColumn.MDMPublisherId:
                        value.Add((Int64) record.ExecutionContext.CallerContext.MDMPublisher);
                        break;

                    case SearchColumn.ModuleId:
                        value.Add((Int64) record.ExecutionContext.CallerContext.Module);
                        break;

                    case SearchColumn.MDMSourceId:
                        value.Add((Int64) record.ExecutionContext.CallerContext.MDMSource);
                        break;

                    case SearchColumn.MDMSubscriberId:
                        value.Add((Int64) record.ExecutionContext.CallerContext.MDMSubscriber);
                        break;

                    case SearchColumn.ServerId:
                        value.Add(record.ExecutionContext.CallerContext.ServerId);
                        break;

                    case SearchColumn.JobId:
                        value.Add(record.ExecutionContext.CallerContext.JobId);
                        break;

                    case SearchColumn.ProfileId:
                        value.Add(record.ExecutionContext.CallerContext.ProfileId);
                        break;

                        // CallDataContext

                    case SearchColumn.EntityId:
                        value = record.ExecutionContext.CallDataContext.EntityIdList;
                        break;

                    case SearchColumn.EntityTypeId:
                        value = CastCollection(record.ExecutionContext.CallDataContext.EntityTypeIdList);
                        break;

                    case SearchColumn.OrganizationId:
                        value = CastCollection(record.ExecutionContext.CallDataContext.OrganizationIdList);
                        break;

                    case SearchColumn.ContainerId:
                        value = CastCollection(record.ExecutionContext.CallDataContext.ContainerIdList);
                        break;

                    case SearchColumn.RelationshipTypeId:
                        value = CastCollection(record.ExecutionContext.CallDataContext.RelationshipTypeIdList);
                        break;

                    case SearchColumn.CategoryId:
                        value = record.ExecutionContext.CallDataContext.CategoryIdList;
                        break;

                    case SearchColumn.AttributeId:
                        value = CastCollection(record.ExecutionContext.CallDataContext.AttributeIdList);
                        break;

                    case SearchColumn.Locale:
                        value = CastEnumCollectionToLongCollection(record.ExecutionContext.CallDataContext.LocaleList);
                        break;

                        // Security Context

                    case SearchColumn.UserId:
                        value.Add(record.ExecutionContext.SecurityContext.UserId);
                        break;

                    case SearchColumn.UserRoleId:
                        value.Add(record.ExecutionContext.SecurityContext.UserRoleId);
                        break;

                }

                if (value.Count > 0)
                {
                    result.Add(columnToCheck, value);
                }
            }

            return result;
        }

        private Boolean CheckBySubstring(Dictionary<SearchColumn, String> data, Collection<SearchColumn> scopeToCheck, Dictionary<SearchColumn, Collection<String>> searchFiltersString, Boolean useLogicOR = false)
        {
            Int32 counter = 0;
            Int32 success = 0;

            foreach (SearchColumn column in scopeToCheck)
            {
                counter++;

                String dataValue;
                Collection<String> searchStrings;

                if (data.TryGetValue(column, out dataValue) && searchFiltersString.TryGetValue(column, out searchStrings))
                {
                    foreach (String searchString in searchStrings)
                    {
                        if (dataValue.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            if (useLogicOR)
                            {
                                return true;
                            }

                            success++;
                            break;
                        }
                    }

                    if (success < counter && !useLogicOR)
                    {
                        break;
                    }
                }
            }

            return counter > 0 && counter == success;
        }

        private Boolean CheckByEquality(Dictionary<SearchColumn, Collection<Int64>> data, Collection<SearchColumn> scopeToCheck, Dictionary<SearchColumn, Collection<Int64>> searchFiltersNumbers)
        {
            Int32 counter = 0;
            Int32 success = 0;

            foreach (SearchColumn column in scopeToCheck)
            {
                counter++;

                Collection<Int64> value;
                Collection<Int64> searchNumbers;
                if (data.TryGetValue(column, out value) && searchFiltersNumbers.TryGetValue(column, out searchNumbers))
                {
                    IEnumerable<Int64> intersection = value.Intersect(searchNumbers);

                    if (intersection.Any())
                    {
                        success++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return counter > 0 && counter == success;
        }

        private Collection<Int64> CastCollection(Collection<Int32> collectionToCast)
        {
            Collection<Int64> result = new Collection<Int64>();

            foreach (Int32 value in collectionToCast)
            {
                result.Add(value);
            }

            return result;
        }

        private Collection<Int64> CastEnumCollectionToLongCollection<TEnum>(Collection<TEnum> collectionToCast)
            where TEnum : struct, IConvertible
        {
            Collection<Int64> result = new Collection<Int64>();

            if (!typeof (TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            foreach (TEnum enumValue in collectionToCast)
            {
                result.Add((Int64)Convert.ChangeType(enumValue, typeof(Int64)));
            }

            return result;
        }

        private void FillSearchScopeByKeywords(DiagnosticReportSettings settings)
        {
            foreach (SearchColumn searchColumn in _scopeToKeywordCheck)
            {
                selectedKeywordValues.Add(searchColumn, settings.SearchKeywords);
            }
        }

        private void FillSearchScopeByCallerContext(DiagnosticReportSettings settings)
        {
            #region Scope to search by Ids

            if (settings.CallerContextFilter.ApplicationList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.ApplicationId);
                _selectedFiltersValues.Add(SearchColumn.ApplicationId, CastEnumCollectionToLongCollection(settings.CallerContextFilter.ApplicationList));
            }

            if (settings.CallerContextFilter.MDMPublisherList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.MDMPublisherId);
                _selectedFiltersValues.Add(SearchColumn.MDMPublisherId, CastEnumCollectionToLongCollection(settings.CallerContextFilter.MDMPublisherList));
            }

            if (settings.CallerContextFilter.ModuleList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.ModuleId);
                _selectedFiltersValues.Add(SearchColumn.ModuleId, CastEnumCollectionToLongCollection(settings.CallerContextFilter.ModuleList));
            }

            if (settings.CallerContextFilter.MDMSourceList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.MDMSourceId);
                _selectedFiltersValues.Add(SearchColumn.MDMSourceId, CastEnumCollectionToLongCollection(settings.CallerContextFilter.MDMSourceList));
            }

            if (settings.CallerContextFilter.MDMSubscriberList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.MDMSubscriberId);
                _selectedFiltersValues.Add(SearchColumn.MDMSubscriberId, CastEnumCollectionToLongCollection(settings.CallerContextFilter.MDMSubscriberList));
            }

            if (settings.CallerContextFilter.ServerIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.ServerId);
                _selectedFiltersValues.Add(SearchColumn.ServerId, CastCollection(settings.CallerContextFilter.ServerIdList));
            }

            if (settings.CallerContextFilter.JobIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.JobId);
                _selectedFiltersValues.Add(SearchColumn.JobId, settings.CallerContextFilter.JobIdList);
            }

            if (settings.CallerContextFilter.ProfileIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.ProfileId);
                _selectedFiltersValues.Add(SearchColumn.ProfileId, CastCollection(settings.CallerContextFilter.ProfileIdList));
            }
            
            #endregion

            #region Scope to search By Names

            if (settings.CallerContextFilter.ProgramNameList.Any())
            {
                _scopeToNameCheck.Add(SearchColumn.ProgramName);
                _selectedFiltersByNameValues.Add(SearchColumn.ProgramName, settings.CallerContextFilter.ProgramNameList);
            }

            if (settings.CallerContextFilter.ServerNameList.Any())
            {
                _scopeToNameCheck.Add(SearchColumn.ServerName);
                _selectedFiltersByNameValues.Add(SearchColumn.ServerName, settings.CallerContextFilter.ServerNameList);
            }

            if (settings.CallerContextFilter.ProfileNameList.Any())
            {
                _scopeToNameCheck.Add(SearchColumn.ProfileName);
                _selectedFiltersByNameValues.Add(SearchColumn.ProfileName, settings.CallerContextFilter.ProfileNameList);
            }

            #endregion

        }

        private void FillSearchScopeByCallDataContext(DiagnosticReportSettings settings)
        {
            // todo[dd]: add search by Lookup Table Name 

            if (settings.CallDataContext.OrganizationIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.OrganizationId);
                _selectedFiltersValues.Add(SearchColumn.OrganizationId, CastCollection(settings.CallDataContext.OrganizationIdList));
            }

            if (settings.CallDataContext.ContainerIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.ContainerId);
                _selectedFiltersValues.Add(SearchColumn.ContainerId, CastCollection(settings.CallDataContext.ContainerIdList));
            }

            if (settings.CallDataContext.EntityIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.EntityId);
                _selectedFiltersValues.Add(SearchColumn.EntityId, settings.CallDataContext.EntityIdList);
            }

            if (settings.CallDataContext.EntityTypeIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.EntityTypeId);
                _selectedFiltersValues.Add(SearchColumn.EntityTypeId, CastCollection(settings.CallDataContext.EntityTypeIdList));
            }

            if (settings.CallDataContext.RelationshipTypeIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.RelationshipTypeId);
                _selectedFiltersValues.Add(SearchColumn.RelationshipTypeId, CastCollection(settings.CallDataContext.RelationshipTypeIdList));
            }

            if (settings.CallDataContext.CategoryIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.CategoryId);
                _selectedFiltersValues.Add(SearchColumn.CategoryId, settings.CallDataContext.CategoryIdList);
            }

            if (settings.CallDataContext.AttributeIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.AttributeId);
                _selectedFiltersValues.Add(SearchColumn.AttributeId, CastCollection(settings.CallDataContext.AttributeIdList));
            }

            if (settings.CallDataContext.LocaleList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.Locale);
                _selectedFiltersValues.Add(SearchColumn.Locale, CastEnumCollectionToLongCollection(settings.CallDataContext.LocaleList));
            }
        }

        private void FillSearchScopeBySecurityContext(DiagnosticReportSettings settings)
        {
            #region Scope to search by Ids
            
            if (settings.SecurityContextFilter.UserIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.UserId);
                _selectedFiltersValues.Add(SearchColumn.UserId,
                    CastCollection(settings.SecurityContextFilter.UserIdList));
            }

            if (settings.SecurityContextFilter.UserRoleIdList.Any())
            {
                _scopeToEqualityCheck.Add(SearchColumn.UserRoleId);
                _selectedFiltersValues.Add(SearchColumn.UserRoleId,
                    CastCollection(settings.SecurityContextFilter.UserRoleIdList));
            }

            #endregion

            #region Scope to search by Names

            if (settings.SecurityContextFilter.UserLoginNameList.Any())
            {
                _scopeToNameCheck.Add(SearchColumn.UserName);
                _selectedFiltersByNameValues.Add(SearchColumn.UserName, settings.SecurityContextFilter.UserLoginNameList);
            }

            if (settings.SecurityContextFilter.UserRoleNameList.Any())
            {
                _scopeToNameCheck.Add(SearchColumn.UserRoleName);
                _selectedFiltersByNameValues.Add(SearchColumn.UserRoleName, settings.SecurityContextFilter.UserRoleNameList);
            }

            #endregion

        }

        private void InitializeDurationCheckMethod(DiagnosticReportSettings settings)
        {
            _durationFiltrationRequired = settings.Duration.HasValue && settings.DurationOperator.HasValue;

            if (_durationFiltrationRequired)
            {
                _duration = settings.Duration.Value;

                switch (settings.DurationOperator)
                {
                    case SearchOperator.EqualTo:
                        _durationFilterFunction = EqualTo;
                        break;
                    case SearchOperator.GreaterThan:
                        _durationFilterFunction = GreaterThan;
                        break;
                    case SearchOperator.GreaterThanOrEqualTo:
                        _durationFilterFunction = GreaterThanOrEqualTo;
                        break;
                    case SearchOperator.LessThan:
                        _durationFilterFunction = LessThan;
                        break;
                    case SearchOperator.LessThanOrEqualTo:
                        _durationFilterFunction = LessThanOrEqualTo;
                        break;
                    default:
                        _durationFiltrationRequired = false;
                        break;
                }
            }
        }

        private Boolean LessThanOrEqualTo(Double durationInMilliSeconds)
        {
            return (Int32) durationInMilliSeconds <= _duration;
        }

        private Boolean LessThan(Double durationInMilliSeconds)
        {
            return (Int32) durationInMilliSeconds < _duration;
        }

        private Boolean GreaterThanOrEqualTo(Double durationInMilliSeconds)
        {
            return (Int32) durationInMilliSeconds >= _duration;
        }

        private Boolean GreaterThan(Double durationInMilliSeconds)
        {
            return (Int32) durationInMilliSeconds > _duration;
        }

        private Boolean EqualTo(Double durationInMilliSeconds)
        {
            return (Int32) durationInMilliSeconds == _duration;
        }

        #endregion
    }
}
