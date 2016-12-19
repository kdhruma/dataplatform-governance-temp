using System;
using System.Collections.ObjectModel;


namespace MDM.Interfaces
{
	using MDM.BusinessObjects;
	using MDM.BusinessObjects.DQMMerging;
	using MDM.Core;

	public interface IMatchReviewProfileManager
	{
		/// <summary>
		/// Property denoting User Login
		/// </summary>
		String UserLogin
		{
			get;
		}

		/// <summary>
		/// User who is accessing BusinessLogic
		/// </summary>
		String User
		{
			get;
		}

		/// <summary>
		/// Indicates which system called the class. Web system or Job service system
		/// </summary>
		MDMCenterSystem System
		{
			get;
		}

		/// <summary>
		/// Get all Match Review Profiles
		/// </summary>
		/// <param name="callerContext">Context details of caller</param>
		/// <returns>Collection of Match Review Profiles</returns>
		MatchReviewProfileCollection GetAll(CallerContext callerContext);

		/// <summary>
		/// Get Match Review Profiles by collection of Ids
		/// </summary>
		/// <param name="matchReviewProfileIds">Collection of Match Review Profiles Ids to return</param>
		/// <param name="callerContext">Context details of caller</param>
		/// <returns>Collection of Match Review Profiles</returns>
		MatchReviewProfileCollection GetByIds(Collection<Int32> matchReviewProfileIds, CallerContext callerContext);

        /// <summary>
        /// Get MatchReviewProfile by Id
        /// </summary>
        /// <param name="matchReviewProfileId">Match Review Profile Id</param>    
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Match Review Profiles for the given MatchReviewProfileId</returns>
        MatchReviewProfile GetById(Int32 matchReviewProfileId, CallerContext callerContext);

		/// <summary>
		/// Get Match Review Profiles by collection of Names
		/// </summary>
		/// <param name="matchReviewProfileNames">Collection of Match Review Profiles Names to return</param>
		/// <param name="callerContext">Context details of caller</param>
		/// <returns>Collection of Merge Planning Profiles</returns>
		MatchReviewProfileCollection GetByNames(Collection<String> matchReviewProfileNames, CallerContext callerContext);

        /// <summary>
        /// Get MatchReviewProfile by Name
        /// </summary>
        /// <param name="matchReviewProfileName">Match Review Profile Name</param>    
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Match Review Profile for the given MatchReviewProfileName</returns>
        MatchReviewProfile GetByName(String matchReviewProfileName, CallerContext callerContext);

		/// <summary>
		/// Create new Match Review Profile
		/// </summary>
		/// <param name="matchReviewProfile">Match Review Profile to create</param>
		/// <param name="callerContext">Context details of caller</param>
		/// <returns>Operation result</returns>
		OperationResult Create(MatchReviewProfile matchReviewProfile, CallerContext callerContext);

		/// <summary>
		/// Update the existing Merge Planning Profile
		/// </summary>
		/// <param name="matchReviewProfile">Match Review Profile to update</param>
		/// <param name="callerContext">Context details of caller</param>
		/// <returns>Operation result</returns>
		OperationResult Update(MatchReviewProfile matchReviewProfile, CallerContext callerContext);

		/// <summary>
		/// Delete the existing Match Review Profile
		/// </summary>
		/// <param name="matchReviewProfile">Match Review Profile to delete</param>
		/// <param name="callerContext">Context details of caller</param>
		/// <returns>Operation result</returns>
		OperationResult Delete(MatchReviewProfile matchReviewProfile, CallerContext callerContext);

		/// <summary>
		/// Supports create, update or delete operations for Match Review Profiles
		/// </summary>
		/// <param name="matchReviewProfiles">Collection of Match Review Profiles</param>
		/// <param name="callerContext">Context details of caller</param>
		/// <returns>Collection of inserted, updated or deleted Match Review Profiles</returns>
		OperationResultCollection Process(MatchReviewProfileCollection matchReviewProfiles, CallerContext callerContext);

		/// <summary>
		/// Gets locale which needs to be used for Model display
		/// </summary>
		/// <returns></returns>
		LocaleEnum GetModelDisplayLocale();

		/// <summary>
		/// Gets locale to be used for Data Formatting
		/// </summary>
		/// <returns></returns>
		LocaleEnum GetDataFormattingLocale();
	}
}