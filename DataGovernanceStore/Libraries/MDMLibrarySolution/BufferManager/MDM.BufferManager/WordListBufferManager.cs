using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace MDM.BufferManager
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies WordList Buffer Manager
    /// </summary>
    public class WordListBufferManager : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Field denoting application configuration item cache is enabled or not
        /// </summary>
        private Boolean _isWordListCacheEnabled = false;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate WordList Buffer Manager
        /// </summary>
        public WordListBufferManager()
        {
            try
            {
                //Get AppConfig which specify whether cache is enabled for application configuration item or not
                this._isWordListCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DataQualityManagement.WordListsCache.Enabled", true);
                this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

                if (_isWordListCacheEnabled)
                {
                    _cacheManager = CacheFactory.GetCache();
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                }

                if (_cacheManager == null)
                    this._isWordListCacheEnabled = false;
            }
            catch
            {
                this._isWordListCacheEnabled = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting application configuration item cache is enabled or not
        /// </summary>
        public Boolean IsWordListCacheEnabled
        {
            get
            {
                return _isWordListCacheEnabled;
            }
        }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Gets word lists by ids from cache
        /// </summary>
        /// <param name="wordListIds"></param>
        /// <returns></returns>
        public WordListsCollection GetWordListsFromCacheByIds(IList<Int32> wordListIds)
        {
            if (wordListIds.IsNullOrEmpty())
            {
                throw new ArgumentException("Provided WordList ids collection is null or empty");
            }

            WordListsCollection wordLists = new WordListsCollection();
            foreach (Int32 wordListId in wordListIds)
            {
                WordListsCollection currentWordLists = GetWordListsFromCache(CacheKeyGenerator.GetWordListsCacheKey(wordListId.ToString(CultureInfo.InvariantCulture)));
                if (currentWordLists.IsNullOrEmpty())
                {
                    return null;
                }
                else
                {
                    if (currentWordLists.Count > 1)
                    {
                        throw new Exception(String.Format("Expected 1 WordList to be cached by Id {0}, returned {1} WordLists",
                            wordListId.ToString(CultureInfo.InvariantCulture), currentWordLists.Count.ToString(CultureInfo.InvariantCulture)));
                    }
                    if (currentWordLists.Count == 1)
                    {
                        wordLists.Add(currentWordLists.First());
                    }
                }
            }
            return wordLists;
        }

        /// <summary>
        /// Gets all word lists from cache
        /// </summary>
        /// <returns></returns>
        public WordListsCollection GetAllWordListsFromCache()
        {
            return GetWordListsFromCache(CacheKeyGenerator.GetAllWordListsCacheKey());
        }

        /// <summary>
        /// Puts Word Lists to cache
        /// </summary>
        /// <param name="wordLists"></param>
        public void PutWordListsToCacheByIds(WordListsCollection wordLists)
        {
            foreach (WordList wordList in wordLists)
            {
                if (wordList != null)
                {
                    PutWordListsToCache(new WordListsCollection { wordList }, CacheKeyGenerator.GetWordListsCacheKey(wordList.Id.ToString(CultureInfo.InvariantCulture)));
                }   
            }
        }

        /// <summary>
        /// Puts all word lists to cache
        /// </summary>
        /// <param name="wordLists"></param>
        public void PutAllWordListsToCache(WordListsCollection wordLists)
        {
            if (!wordLists.IsNullOrEmpty())
            {
                PutWordListsToCache(wordLists, CacheKeyGenerator.GetAllWordListsCacheKey());   
            }
        }
        
        /// <summary>
        /// Removes word lists from cache by their id
        /// </summary>
        /// <param name="wordLists"></param>
        public void RemoveWordListsFromCacheByIds(WordListsCollection wordLists)
        {
            if (wordLists.IsNullOrEmpty())
                throw new ArgumentException("Provided WordLists collection is null or empty");

            foreach (WordList wordList in wordLists)
            {
                if (wordList != null)
                {
                    RemoveWordListsFromCache(CacheKeyGenerator.GetWordListsCacheKey(wordList.Id.ToString(CultureInfo.InvariantCulture)));   
                }
            }
        }

        /// <summary>
        /// Removes word lists from cache for general key without removing WordLists specified with Id cache key
        /// </summary>
        public void RemoveAllWordListsFromCache()
        {
            RemoveWordListsFromCache(CacheKeyGenerator.GetAllWordListsCacheKey());
        }
        
        #endregion

        #region Private
        
        /// <summary>
        /// Finds available word lists from cache
        /// </summary>
        /// <returns>Collection of word lists if found in internal cache otherwise null</returns>
        private WordListsCollection GetWordListsFromCache(String key)
        {
            WordListsCollection result = null;

            if (this._isWordListCacheEnabled)
            {
                String cacheKey = key;

                object itemsObj = null;

                try
                {
                    itemsObj = _cacheManager.Get(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                }

                WordListsCollection wordListCollection = itemsObj as WordListsCollection;
                if (wordListCollection != null)
                {
                    result = (WordListsCollection)wordListCollection.Clone();
                }
            }

            return result;
        }

        /// <summary>
        /// Puts WordLists in internal cache
        /// </summary>
        /// <param name="wordLists">This parameter is specifying collection of word lists to be set in cache</param>
        /// <param name="numberOfRetry">This parameter is specifying number of retry to update object in cache</param>
        /// <param name="key">The cache key.</param>
        /// <param name="isDataUpdated">Specifies whether the cache data object is updated and inserted to local cache</param>
        private void PutWordListsToCache(WordListsCollection wordLists, String key, Int32 numberOfRetry = 3, Boolean isDataUpdated = false)
        {
            if (this._isWordListCacheEnabled)
            {
                if (wordLists.IsNullOrEmpty())
                    throw new ArgumentException("WordLists are not available or empty.");

                DateTime expiryTime = DateTime.Now.AddMonths(1);

                //Clone the object before set to cache
                WordListsCollection clonedWordLists = (WordListsCollection)wordLists.Clone();
                
                String cacheKey = key;

                try
                {
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                    _cacheManager.Set(cacheKey, clonedWordLists, expiryTime);
                }
                catch (Exception ex)
                {
                    //Retry < 0 means just ignore update if failed..
                    if (numberOfRetry < 0)
                        return;

                    Boolean retrySuccess = false;

                    for (Int32 i = 0; i < numberOfRetry; i++)
                    {
                        try
                        {
                            if (_isDistributedCacheWithNotificationEnabled)
                                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);

                            _cacheManager.Set(cacheKey, wordLists, expiryTime);

                            retrySuccess = true;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (!retrySuccess)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                        new ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Removes application configuration items (using specified cache key) from cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="publishCacheChangeEvent">This parameter is specifying publish cache change event</param>
        private void RemoveWordListsFromCache(String cacheKey, Boolean publishCacheChangeEvent = true)
        {
            if (this._isWordListCacheEnabled)
            {
                try
                {
                    _cacheManager.Remove(cacheKey);
                    if (_isDistributedCacheWithNotificationEnabled)
                        _cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}", ex.Message));
                    new ExceptionHandler(ex);
                }
            }
        }
        
        #endregion

        #endregion
    }
}