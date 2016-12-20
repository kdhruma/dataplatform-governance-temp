using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace MDM.Interfaces
{

	using MDM.BusinessObjects;
	using MDM.BusinessObjects.DQM;
	using MDM.Core;

	/// <summary>
	/// Matching Interface
	/// </summary>
	public interface IMatchingManager 
	{
		/// <summary>
		/// This method runs matching and saves the results to db, given a entity and profile Id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="profileId"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResult MatchEntity(Entity entity, Int32 profileId, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity and profile Id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="profileGroupId"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResult MatchEntityByProfileGroup(Entity entity, Int32 profileGroupId, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity and profile Id.
		/// </summary>
		/// <param name="entityCollection"></param>
		/// <param name="profileGroupId"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResultCollection MatchEntityCollectionByProfileGroup(EntityCollection entityCollection, Int32 profileGroupId, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity collection and profile Id.
		/// </summary>
		/// <param name="entityCollection"></param>
		/// <param name="profileId"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResultCollection MatchEntities(EntityCollection entityCollection, Int32 profileId, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity and profile Id.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="profileId"></param>
		/// <returns>Returns matching result</returns>
		MatchingResult MatchEntityAndReturnResult(Entity entity, Int32 profileId);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity collection and profile Id.
		/// </summary>
		/// <param name="entityCollection"></param>
		/// <param name="profileId"></param>
		/// <returns>Returns matching results collection</returns>
		MatchingResultCollection MatchEntitiesAndReturnResults(EntityCollection entityCollection, Int32 profileId);

		/// <summary>
		/// This method will run the matching for a given entityId and profileId.
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileId"></param>
		/// <param name="jobId"></param>
		/// <returns>Operation Result that will return a Success or Failure. If it is a failure, caller can check the Errors to get the failure message.</returns>
		OperationResult MatchEntity(Int64 entityId, Int32 profileId, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity and profile Id.
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileGroupId"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResult MatchEntityByProfileGroup(Int64 entityId, Int32 profileGroupId, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity and profile Id.
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileGroupId"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResultCollection MatchEntitiesByProfileGroup(Int64[] entityIds, Int32 profileGroupId, Int64? jobId = null);

		/// <summary>
		/// This method will run the matching for the list of entityIds and for given profileId.
		/// JobId is the optional parameter. Any Job that runs a matching operation as a batch will
		/// send its ID for reference while getting the result.
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileId"></param>
		/// <param name="jobId"></param>
		/// <returns>Operation Result collection, there will a OperationResult object for each entityIds  that will return a Success or Failure. 
		/// If it is a failure, caller can check the Errors to get the failure message.</returns>
		OperationResultCollection MatchEntities(Int64[] entityIds, Int32 profileId, Int64? jobId = null);

		/// <summary>
		/// This method will run the matching for a given entityId and profileId.
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileId"></param>
		/// <returns>Operation Result that will return a Success or Failure. If it is a failure, caller can check the Errors to get the failure message.</returns>
		MatchingResult MatchEntityAndReturnResult(Int64 entityId, Int32 profileId);

		/// <summary>
		/// This method will run the matching for the list of entityIds and for given profileId. 
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileId"></param>
		/// <returns>
		/// Collection of Matching Results. The Matching Result Object in the collection will have the error message
		/// in case failure for a entityId in the collection.
		/// </returns>
		MatchingResultCollection MatchEntitiesAndReturnResults(Int64[] entityIds, Int32 profileId);

		/// <summary>
		/// This  method is obsolete. Do Not Use.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="profileId"></param>
		/// <returns></returns>
		OperationResult MatchEntitiesInJob(Int64 jobId, Int32 profileId);

		/// <summary>
		/// This method will return the result object for a given entityId and profileId.
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileId"></param>
		/// <returns>Matching Result. If the Status property is null, it means we have results otherwise the matching operation is failed</returns>
		IMatchingResult GetMatchingResult(Int64 entityId, Int32 profileId);

		/// <summary>
		/// This method will return the result object for a given entityId.
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="jobId"></param>
		/// <returns>Matching Result. If the Status property is null, it means we have results otherwise the matching operation is failed</returns>
		IMatchingResult GetMatchingResult(Int64 entityId, Int64? jobId);

		/// <summary>
		/// This method will get the matching results for the matching that was previously run for entityId, profileId earlier.
		/// The optional parameter of JobId can be also given if the matching was run using a Scheduled Job.
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileId"></param>
		/// <param name="jobId"></param>
		/// <returns>Matching Result Collection. If result is not found, Status of the Matching Result will have a message.</returns>
		IMatchingResultCollection GetMatchingResults(Int64[] entityIds, Int32 profileId, Int64? jobId = null);

		/// <summary>
		/// This method will return the result object for a given entityId and profileGroupId.
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileGroupId"></param>
		/// <returns>Matching Result. If the Status property is null, it means we have results otherwise the matching operation is failed</returns>
		IMatchingResult GetMatchingResultByProfileGroup(Int64 entityId, Int32 profileGroupId);

		/// <summary>
		/// This method will return the result object for a given entityId and profileGroupId.
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileGroupId"></param>
		/// <param name="jobId"></param>
		/// <returns>Matching Result. If the Status property is null, it means we have results otherwise the matching operation is failed</returns>
		IMatchingResultCollection GetMatchingResultsByProfileGroup(Int64[] entityIds, Int32 profileGroupId, Int64? jobId = null);

		/// <summary>
		/// This method will get the matching results for the matching that was previously run for entityId, profileId earlier.
		/// The optional parameter of JobId can be also given if the matching was run using a Scheduled Job.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="entityIds"></param>
		/// <returns>Matching Result Collection. If result is not found, Status of the Matching Result will have a message.</returns>
		IMatchingResultCollection GetMatchingResults(Int64 jobId, Int64[] entityIds);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity and profile name.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="profileName"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResult MatchEntity(Entity entity, string profileName, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and saves the results to db, given a entity collection and profile name, .
		/// </summary>
		/// <param name="entityCollection"></param>
		/// <param name="profileName"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResultCollection MatchEntities(EntityCollection entityCollection, string profileName, Int64? jobId = null);

		/// <summary>
		/// This method runs matching and return results to caller, given a entity and profile name.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="profileName"></param>
		/// <returns>Returns matching result</returns>
		MatchingResult MatchEntityAndReturnResult(Entity entity, string profileName);

		/// <summary>
		/// This method runs matching and returns the results to caller, given a entity collection and profile name.
		/// </summary>
		/// <param name="entityCollection"></param>
		/// <param name="profileName"></param>
		/// <returns>Returns matching results collection</returns>
		MatchingResultCollection MatchEntitiesAndReturnResults(EntityCollection entityCollection, string profileName);

		/// <summary>
		/// This method will  run the match for a given entity and profile.  The method will 
		/// load the profile based on the profileName. 
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileName"></param>
		/// <param name="jobId"></param>
		/// <returns>The result will provide the success or failure. On failure there Error object will the message.</returns>
		OperationResult MatchEntity(Int64 entityId, string profileName, Int64? jobId = null);

		/// <summary>
		/// This method run the matching for given entity Ids and profile. The profile is loaded based on the 
		/// profilename.
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileName"></param>
		/// <param name="jobId"></param>
		/// <returns>The result collection will have result object for each of the entity Ids. Each result object will have the status for the  matching ran for a entityId</returns>
		OperationResultCollection MatchEntities(Int64[] entityIds, string profileName, Int64? jobId = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="profileName"></param>
		/// <returns></returns>
		OperationResult MatchEntitiesInJob(Int64 jobId, string profileName);

		/// <summary>
		/// This method runs matching and return results to caller, given a entityId and profile name.
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileName"></param>
		/// <returns>Returns matching result</returns>
		MatchingResult MatchEntityAndReturnResult(Int64 entityId, string profileName);

		/// <summary>
		/// This method runs matching and returns the results to caller, given a entityIds and profile name.
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileName"></param>
		/// <returns>Returns matching results collection</returns>
		MatchingResultCollection MatchEntitiesAndReturnResults(Int64[] entityIds, string profileName);

		/// <summary>
		/// This method will return the Matching Result for the given entityId and profilename. 
		/// If the result is not found, the status property will have the error message. 
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="profileName"></param>
		/// <returns>Matching Result</returns>
		IMatchingResult GetMatchingResult(Int64 entityId, string profileName);

		/// <summary>
		/// This method will get the results for the list of entityIds and given profile name. 
		/// Each item in the result collection is the result status for each of the Id, profile name 
		/// combination in the entityIds list. 
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="profileName"></param>
		/// <returns></returns>
		IMatchingResultCollection GetMatchingResults(Int64[] entityIds, string profileName);

		/// <summary>
		/// This method will get the prrofile for a given profile id. 
		/// </summary>
		/// <param name="profileId"></param>
		/// <returns>MatchingProfile. Returns NULL if the profile doesnt exists.</returns>
		MatchingProfile GetMatchingProfile(Int32 profileId);

		/// <summary>
		/// This  method will get the profile for a given profile name. 
		/// </summary>
		/// <param name="profileName"></param>
		/// <returns>MatchingProfile. NULL if the profile doesn't exist.</returns>
		MatchingProfile GetMatchingProfileByName(string profileName);

		/// <summary>
		/// This  method returns the Matching Profiles for the list of given profile ids.
		/// Profile for each given ids will be return in the collection.
		/// </summary>
		/// <param name="profileIds"></param>
		/// <returns>MatchingProfileCollection.</returns>
		MatchingProfileCollection GetMatchingProfiles(Int32[] profileIds);

		/// <summary>
		/// The  method return all matching profiles available in the MDMCenter.
		/// </summary>
		/// <returns>MatchingProfileCollection</returns>
		MatchingProfileCollection GetAllMatchingProfiles();

		/// <summary>
		/// This method will process the given matching profile. 
		/// </summary>
		/// <param name="profile"></param>
		/// <returns>OperationResult</returns>
		OperationResult ProcessProfile(IMatchingProfile profile);

		/// <summary>
		/// This method will process all the matching profiles in the collection.
		/// </summary>
		/// <param name="profiles"></param>
		/// <returns>OperationResultCollection</returns>
		OperationResultCollection ProcessProfiles(IMatchingProfileCollection profiles);

		/// <summary>
		/// ets the best matching profile for the entity
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="callerContext"></param>
		/// <returns></returns>
		MatchingProfile GetBestMatchProfile(Entity entity, CallerContext callerContext);

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