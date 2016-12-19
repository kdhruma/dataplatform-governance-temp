using System;

namespace MDM.MessageManager.Business
{
	using BusinessObjects;
	using CacheManager.Business;
	using Core;
	using Utility;
	using ExceptionManager;
	using Data.SqlClient;

	/// <summary>
	/// Class to manage Mail Config related configuration
	/// </summary>
	public class MailConfigBL : BusinessLogicBase
	{
		#region Fields

		#endregion Fields

		#region Properties

		#endregion Properties

		#region Constructor

		#endregion Constructor

		#region Methods

		/// <summary>
		/// Get mail configuration for given application and module.
		/// </summary>
		/// <param name="callerContext"></param>
		/// <returns>Mail Config</returns>
		public MailConfig Get(CallerContext callerContext)
		{
			return Get(callerContext.Application, callerContext.Module);
		}

		/// <summary>
		/// Get mail configuration for given application and module.
		/// </summary>
		/// <param name="application">Application for which mail configuration is to be fetched.</param>
		/// <param name="module">Module for which mail configuration is to be fetched.</param>
		/// <returns>Mail Config</returns>
		public MailConfig Get(MDMCenterApplication application, MDMCenterModules module)
		{
			MailConfig mailConfig = null;
			String cacheKey = GetCacheKey(application, module);

			#region Read cached command object

			ICache cache = CacheFactory.GetCache();
			if (cache != null)
			{
				mailConfig = cache.Get<MailConfig>(cacheKey);
			}

			#endregion Read cached command object

			//Get mail config from DB if not found in cache.
			if (mailConfig == null)
			{
				try
				{
					//Get command
					DBCommandProperties command = new DBCommandProperties();
					command.ConnectionString = AppConfigurationHelper.ConnectionString;

					MailConfigDA configDA = new MailConfigDA();
					mailConfig = configDA.Get(application, module, command);

					#region Put mail config back in cache

					if (cache != null && mailConfig != null)
					{
						cache.Set(cacheKey, mailConfig, DateTime.Now.AddHours(24.0));
					}

					#endregion Put mail config back in cache
				}
				catch (Exception ex)
				{
					LogException(ex);
				}
			} 
			return mailConfig;
		}

		#region Private Methods

		private String GetCacheKey(MDMCenterApplication application, MDMCenterModules module)
		{
			String cacheKey = "RS_MailConfig_Application:{0}_Module:{1}";
			cacheKey = String.Format(cacheKey, application, module);
			return cacheKey;
		}

		private void LogException(Exception ex)
		{
			new ExceptionHandler(ex);
		}

		#endregion Private Methods

		#endregion Methods
	}
}
