using System;

namespace MDM.ExpressionParser
{
	using MDM.BusinessObjects;
	using MDM.BusinessObjects.Diagnostics;
	using MDM.Core;
	using MDM.CacheManager.Business;
	using MDM.Utility;
	
	public class ExpressionParserBufferManager
    {
        #region Fields

        /// <summary>
        /// field denoting cache manager of cache
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// field denoting lookup cache is enabled or not.
        /// </summary>
        private Boolean _isExpressionCacheEnabled = false;
		
		/// <summary>
		/// 
		/// </summary>
		private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

		private readonly TraceSettings _traceSettings;

		private DiagnosticActivity _diagnosticActivity;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public ExpressionParserBufferManager()
		{
			try
			{
				_isExpressionCacheEnabled = true;

				_isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.DistributedCacheWithNotification.Enabled", false);

				if (_isDistributedCacheWithNotificationEnabled)
				{
					_cacheSynchronizationHelper = new CacheSynchronizationHelper();
				}

				if (_isExpressionCacheEnabled)
				{
					_cacheManager = CacheFactory.GetCache();
					_traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
					_diagnosticActivity = new DiagnosticActivity();
				}

				if (_cacheManager == null)
				{
					this._isExpressionCacheEnabled = false;
				}
			}
			catch
			{
				this._isExpressionCacheEnabled = false;
			}
		}

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
        /// Add an Expression to Cache
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="processor"></param>
        /// <param name="numberOfRetry"></param>
        /// <param name="isDataUpdated"></param>
		public void AddExpressionToCache(String exp, ExpressionProcessor processor, Int32 numberOfRetry, Boolean isDataUpdated = false)
        {
            if (this._isExpressionCacheEnabled)
            {
				if (exp == null)
				{
					throw new ArgumentException("Expression is null");
				}

				if (_traceSettings.IsBasicTracingEnabled)
				{
					_diagnosticActivity.ActivityName = "MDM.ExpressionParser.ExpressionParserBufferManager.AddExpressionToCache";
					_diagnosticActivity.Start();
				}

				String cacheKey = CacheKeyGenerator.GetExpressionDataCacheKey(exp);

                DateTime expiryTime = DateTime.Now.AddDays(5);

				try
				{
					if (_isDistributedCacheWithNotificationEnabled)
					{
						_cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
					}

					_cacheManager.Set(cacheKey, processor, expiryTime);
				}
				catch (Exception ex)
				{
					//Retry < 0 means just ignore update if failed..
					if (numberOfRetry < 0)
					{
						return;
					}

					Boolean retrySuccess = false;

					for (Int32 i = 0; i < numberOfRetry; i++)
					{
						try
						{
							if (_isDistributedCacheWithNotificationEnabled)
							{
								_cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, isDataUpdated);
							}

							_cacheManager.Set(cacheKey, processor, expiryTime);

							retrySuccess = true;
							break;
						}
						catch
						{
						}
					}

					if (!retrySuccess)
					{
						//TODO:: What to do if update fails after retry too..
						_diagnosticActivity.LogError(String.Format("Error occurred : {0}", ex.Message));
						throw;
					}
				}
				finally
				{
					if (_traceSettings.IsBasicTracingEnabled)
					{
						_diagnosticActivity.Stop();
					}
				}
            }
        }

        /// <summary>
        /// Remove an Expression from Cache
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
		public Boolean RemoveExpressionFromCache(String exp)
        {
            Boolean success = false;

            if (this._isExpressionCacheEnabled)
            {
				if (_traceSettings.IsBasicTracingEnabled)
				{
					_diagnosticActivity.ActivityName = "MDM.ExpressionParser.ExpressionParserBufferManager.RemoveExpressionFromCache";
					_diagnosticActivity.Start();
				}

				String cacheKey = CacheKeyGenerator.GetExpressionDataCacheKey(exp);

				try
				{
					success = _cacheManager.Remove(cacheKey);
					if (_isDistributedCacheWithNotificationEnabled)
					{
						_cacheSynchronizationHelper.NotifyLocalCacheRemoval(cacheKey);
					}
				}
				catch (Exception ex)
				{
					_diagnosticActivity.LogError(String.Format("Error occurred : {0}", ex.Message));
					throw;
				}
				finally
				{
					if (_traceSettings.IsBasicTracingEnabled)
					{
						_diagnosticActivity.Stop();
					}
				}
            }

            return success;
        }

        /// <summary>
        /// Find an Expression
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
		public ExpressionProcessor FindExpression(String exp)
        {
            ExpressionProcessor expTree = null;

            if (this._isExpressionCacheEnabled)
            {
				if (_traceSettings.IsBasicTracingEnabled)
				{
					_diagnosticActivity.ActivityName = "MDM.ExpressionParser.ExpressionParserBufferManager.FindExpression";
					_diagnosticActivity.Start();
				}

				String cacheKey = CacheKeyGenerator.GetExpressionDataCacheKey(exp);

				try
				{
					expTree = _cacheManager.Get(cacheKey) as ExpressionProcessor;
				}
				catch (Exception ex)
				{
					_diagnosticActivity.LogError(String.Format("Error occurred : {0}", ex.Message));
					throw;
				}
				finally
				{
					if (_traceSettings.IsBasicTracingEnabled)
					{
						_diagnosticActivity.Stop();
					}
				}
            }

            return expTree;
        }

		#endregion Public Methods

		#region Private Methods

		#endregion Private Methods

		#endregion Methods
	}
}
