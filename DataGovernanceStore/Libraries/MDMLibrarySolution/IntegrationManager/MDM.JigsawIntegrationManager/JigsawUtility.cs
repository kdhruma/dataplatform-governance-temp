using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Practices.ServiceLocation;

namespace MDM.JigsawIntegrationManager
{
    using MDM.Services;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;
	using MDM.BusinessObjects.DQM;
	/// <summary>
	/// Represents JigsawUtility class
	/// </summary>
	public static class JigsawUtility
    {
		private static object lockObject = new Object();
		private static readonly Lazy<ConcurrentDictionary<Int32, Boolean>> _entityTypeMatchConfiguration = new Lazy<ConcurrentDictionary<Int32, Boolean>>(JigsawUtility.GetEntityTypeMatchProfiles);

		/// <summary>
		/// Gets the locale message.
		/// </summary>
		/// <param name="messageCode">The message code.</param>
		/// <param name="parameters">The parameters.</param>
		/// <param name="locale">The locale.</param>
		/// <returns></returns>
		public static String GetLocaleMessage(String messageCode, IEnumerable<Object> parameters = null, LocaleEnum locale = LocaleEnum.UnKnown)
        {
            LocaleEnum targetLocale = locale == LocaleEnum.UnKnown ? GlobalizationHelper.GetSystemDataLocale() : locale;
            String localizedMessage = String.Empty;

            var localeMessage =
                new ConfigurationService().GetLocaleMessages(targetLocale, new Collection<String> { messageCode }, MDMObjectFactory.GetICallerContext())
                    .FirstOrDefault();

            if (localeMessage == null)
            {
                return localizedMessage;
            }

            if (parameters != null && parameters.Any())
            {
                localizedMessage = String.Format(localeMessage.Message, parameters.ToArray());
            }

            return localizedMessage;
        }

		public static ConcurrentDictionary<Int32, Boolean> GetEntityTypeMatchProfiles()
		{
			ConcurrentDictionary<Int32, Boolean> entityTypeMatchinProfileExists = new ConcurrentDictionary<Int32, Boolean>();

			IMatchingManager matchingmanager = ServiceLocator.Current.GetInstance(typeof(IMatchingManager)) as IMatchingManager;

			if (matchingmanager != null)
			{
				MatchingProfileCollection profileCollection = matchingmanager.GetAllMatchingProfiles();

				if (profileCollection != null)
				{
					foreach (MatchingProfile profile in profileCollection)
					{
						entityTypeMatchinProfileExists.TryAdd(profile.EntityTypeId,true);
					}
				}
			}

			return entityTypeMatchinProfileExists;
		}

		/// <summary>
		/// Checks to see if the match is enabled for Entity Types
		/// </summary>
		/// <param name="entityTypeIds"></param>
		/// <returns>ICollection</returns>
		public static ICollection<Int32> IsJigsawIntegrationEnabledForEntityTypes(ICollection<Int32> entityTypeIds)
		{
			if (JigsawConstants.IsJigsawInformationGovernanceReportsEnabled)
			{
				return entityTypeIds;
			}


			List<Int32> matchEnabledEntityTypes = new List<Int32>();


			//If matching is true then check if entity  entityType has a matching profile to process the entity to be sent to Jigsaw
			if (JigsawConstants.IsJigsawMatchingEnabled)
			{
				lock (lockObject)
				{
					if (_entityTypeMatchConfiguration.IsValueCreated)
					{
						foreach (Int32 entityTypeId in entityTypeIds)
						{
							if (_entityTypeMatchConfiguration.Value.ContainsKey(entityTypeId))
							{
								matchEnabledEntityTypes.Add(entityTypeId);
							}

						}
					}
				}
			}

			return matchEnabledEntityTypes;
		}

		/// <summary>
		/// Check to see if the entity json message can be sent jigsaw
		/// </summary>
		/// <param name="entityTypeId"></param>
		/// <returns></returns>
		public static Boolean IsJigsawIntegrationEnabledForEntityType(Int32 entityTypeId)
		{
			if (JigsawConstants.IsJigsawInformationGovernanceReportsEnabled)
			{
				return true;
			}

			//If matching is true then check if entity  entityType has a matching profile to process the entity to be sent to Jigsaw
			if (JigsawConstants.IsJigsawMatchingEnabled)
			{
				return _entityTypeMatchConfiguration.Value.ContainsKey(entityTypeId);
			}

			return false;
		}
		
	}
} 