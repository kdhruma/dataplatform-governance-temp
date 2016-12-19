using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using MDM.BusinessObjects.Diagnostics;
using MDM.Core.Extensions;
using MDM.Interfaces;

namespace MDM.EntityManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.EntityManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Business layer which contains logic for work with Sources
    /// </summary>
    public class SourceBL : BusinessLogicBase
    {
        #region Constants

        private const String SourceCreateUpdateDeleteProcessName = "Process";
        private const String CreateSourceProcessName = "Create";
        private const String UpdateSourceProcessName = "Update";
        private const String DeleteSourceProcessName = "Delete";
        private const String GetSourcesByIdsProcessName = "GetByIds";
        private const String GetSourceByIdProcessName = "GetById";
        private const String GetAllSourcesProcessName = "GetAll";

        private const String EmptyProgramNameReplacementPrefix = "MDM.EntityManager.Business.SourceBL.";
        private const String TracingPrefix = "MDM.EntityManager.Business.SourceBL.";

        private const Int32 UpdatingSourcesCacheAttemptsCount = 3;

        #endregion

        #region Fields

        private readonly CacheBufferManager<SourceCollection> _sourceBufferManager = new CacheBufferManager<SourceCollection>(CacheKeyGenerator.GetAllSourcesCacheKey(), "MDMCenter.EntityManager.SourcesCache.Enabled");

        private SecurityPrincipal _currentSecurityPrincipal = null;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public String UserLogin
        {
            get
            {
                try
                {
                    if (_currentSecurityPrincipal == null)
                    {
                        _currentSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                    }
                    return _currentSecurityPrincipal.CurrentUserName;
                }
                catch
                {
                    TraceError("Unable to fetch user login", MDMTraceSource.EntityProcess);
                }
                return null;
            }
        }

        #endregion

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Get all Sources
        /// </summary>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Collection of Sources</returns>
        public SourceCollection GetAll(CallerContext callerContext)
        {
            SourceCollection result = null;

            StartTraceActivity(GetAllSourcesProcessName, MDMTraceSource.EntityGet);
            try
            {
                ValidateContext(callerContext, "GetAll", MDMTraceSource.EntityGet);

                TraceInformation("Finding Sources in cache.", MDMTraceSource.EntityGet);
                result = _sourceBufferManager.GetAllObjectsFromCache();
                if (result == null || !result.Any())
                {
                    TraceInformation("Sources not found in cache.", MDMTraceSource.EntityGet);
                    TraceInformation("Loading Sources from Database.", MDMTraceSource.EntityGet);

                    SourceDA sourceDA = new SourceDA();

                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    result = sourceDA.GetAll(command);

                    if (result != null && result.Any())
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            TraceInformation(String.Format("Caching {0} Sources.", result.Count), MDMTraceSource.EntityGet);
                        }
                        _sourceBufferManager.SetBusinessObjectsToCache(result.Clone() as SourceCollection, UpdatingSourcesCacheAttemptsCount);
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        TraceInformation(String.Format("Found {0} Sources in cache.", result.Count), MDMTraceSource.EntityGet);
                    }
                }
            }
            finally
            {
                StopTraceActivity(GetAllSourcesProcessName, MDMTraceSource.EntityGet);
            }

            return result;
        }

        /// <summary>
        /// Get Sources by collection of Ids
        /// </summary>
        /// <param name="sourcesIds">Collection of Sources Ids to return</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Collection of Sources</returns>
        public SourceCollection GetByIds(Collection<Int32> sourcesIds, CallerContext callerContext)
        {
            SourceCollection result = new SourceCollection();

            StartTraceActivity(GetSourcesByIdsProcessName, MDMTraceSource.EntityGet);
            try
            {
                SourceCollection allSources = GetAll(PrepareCallerContext(callerContext, "GetByIds"));
                HashSet<Int32> ids = new HashSet<Int32>(sourcesIds);

                foreach (Source source in allSources)
                {
                    if (ids.Contains(source.Id))
                    {
                        result.Add(source);
                    }
                }
            }
            finally
            {
                StopTraceActivity(GetSourcesByIdsProcessName, MDMTraceSource.EntityGet);
            }

            return result;
        }

        /// <summary>
        /// Get Source by Id
        /// </summary>
        /// <param name="sourceId">Source Id</param>    
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Source for the given sourceId</returns>
        public Source GetById(Int32 sourceId, CallerContext callerContext)
        {
            Source result;

            StartTraceActivity(GetSourceByIdProcessName, MDMTraceSource.EntityGet);
            try
            {
                result = GetByIds(new Collection<Int32> { sourceId }, PrepareCallerContext(callerContext, "GetById")).FirstOrDefault();
            }
            finally
            {
                StopTraceActivity(GetSourceByIdProcessName, MDMTraceSource.EntityGet);
            }

            return result;
        }

        /// <summary>
        /// Get System source
        /// </summary>
        /// <param name="systemSource">System source</param>    
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Source for the given sourceId</returns>
        public Source GetSystemSource(SystemSource systemSource, CallerContext callerContext)
        {
            Source result;

            StartTraceActivity(GetSourceByIdProcessName, MDMTraceSource.EntityGet);
            try
            {
                result = GetById((Int32)systemSource, PrepareCallerContext(callerContext, "GetSystemSource"));
            }
            finally
            {
                StopTraceActivity(GetSourceByIdProcessName, MDMTraceSource.EntityGet);
            }

            return result;
        }

        #endregion

        #region CUD operations

        /// <summary>
        /// Create new Source
        /// </summary>
        /// <param name="source">Source to create</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        public OperationResult Create(Source source, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            StartTraceActivity(CreateSourceProcessName, MDMTraceSource.EntityProcess);
            try
            {
                ObjectAction method = ObjectAction.Create;

                ValidateSource(source, method.ToString(), MDMTraceSource.EntityProcess);
                source.Action = method;

                operationResult = Process(new SourceCollection { source }, PrepareCallerContext(callerContext, "Create")).FirstOrDefault();
            }
            finally
            {
                StopTraceActivity(CreateSourceProcessName, MDMTraceSource.EntityProcess);
            }

            return operationResult;
        }

        /// <summary>
        /// Update the existing Source 
        /// </summary>
        /// <param name="source">Source to update</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        public OperationResult Update(Source source, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            StartTraceActivity(UpdateSourceProcessName, MDMTraceSource.EntityProcess);
            try
            {
                ObjectAction method = ObjectAction.Update;

                ValidateSource(source, method.ToString(), MDMTraceSource.EntityProcess);
                source.Action = method;

                operationResult = Process(new SourceCollection { source }, PrepareCallerContext(callerContext, "Update")).FirstOrDefault();
            }
            finally
            {
                StopTraceActivity(UpdateSourceProcessName, MDMTraceSource.EntityProcess);
            }

            return operationResult;
        }

        /// <summary>
        /// Delete the existing Source
        /// </summary>
        /// <param name="source">Source to delete</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        public OperationResult Delete(Source source, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            StartTraceActivity(DeleteSourceProcessName, MDMTraceSource.EntityProcess);
            try
            {
                ObjectAction method = ObjectAction.Delete;

                ValidateSource(source, method.ToString(), MDMTraceSource.EntityProcess);
                source.Action = method;

                operationResult = Process(new SourceCollection(new List<Source> { source }), PrepareCallerContext(callerContext, "Delete")).FirstOrDefault();
            }
            finally
            {
                StopTraceActivity(DeleteSourceProcessName, MDMTraceSource.EntityProcess);
            }

            return operationResult;
        }

        /// <summary>
        /// Supports create, update or delete operations for Sources
        /// </summary>
        /// <param name="sources">Collection of Sources</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Collection of inserted, updated or deleted Sources</returns>
        public OperationResultCollection Process(SourceCollection sources, CallerContext callerContext)
        {
            OperationResultCollection operationResult = null;

            StartTraceActivity(SourceCreateUpdateDeleteProcessName, MDMTraceSource.EntityProcess);
            try
            {
                ValidateContext(callerContext, "Process", MDMTraceSource.EntityProcess);

                ValidateSourcesCollection(sources, "Process", MDMTraceSource.EntityProcess);

                Int32 createRecordsCounter = -1;
                foreach (Source source in sources)
                {
                    if (source.Action == ObjectAction.Create)
                    {
                        source.Id = createRecordsCounter;
                        createRecordsCounter--;
                    }
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResult = new SourceDA().Process(sources, PrepareCallerContext(callerContext, "Process"), command, UserLogin);

                    TraceInformation("Creating, Updating, Deleting Sources ends", MDMTraceSource.EntityProcess);

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        #region Invalidate cache

                        _sourceBufferManager.RemoveBusinessObjectFromCache(true, String.Empty, "Sources");

                        #endregion

                        transactionScope.Complete();
                    }
                }
            }
            finally
            {
                StopTraceActivity(SourceCreateUpdateDeleteProcessName, MDMTraceSource.EntityProcess);
            }

            return operationResult;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Set source of changes to each entity from collection and all it's decendants: attribute values, relationships and it's attribute values
        /// </summary>
        /// <param name="entityCollection">Entity Collection to set source</param>
        /// <param name="source">Source to be set</param>
        public void SetSourceToAllEntityDescendants(IEntityCollection entityCollection, Int32 source)
        {
	        SourceInfo sourceObj = new SourceInfo(source);

            foreach (Entity entity in entityCollection)
            {
                this.SetSourceToAllEntityDescendants(entity, sourceObj);
            }
        }

        /// <summary>
        /// Set source of changes to entity and all it decendants: attribute values, relationships and it's attribute values
        /// </summary>
        /// <param name="entity">Entity to set source</param>
        /// <param name="source">Source to be set</param>
        public void SetSourceToAllEntityDescendants(IEntity entity, Int32 source)
        {
	        SetSourceToAllEntityDescendants(entity, new SourceInfo(source));
        }

		/// <summary>
		/// Set source (source id and source entity) of changes to entity and all it decendants: attribute values, relationships and it's attribute values
		/// </summary>
		/// <param name="entity">Entity to set source</param>
		/// <param name="source">Source to be set</param>
		public void SetSourceToAllEntityDescendants(IEntity entity, SourceInfo source)
		{
			Boolean isBasicTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

			DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

			if (isBasicTracingEnabled)
			{
				diagnosticActivity.Start();
				diagnosticActivity.LogMessage(MessageClassEnum.Information, String.Format("Starting setting source to all descendants of entity with id {0}", entity.Id));
			}

			try
			{
				if (entity.Action == ObjectAction.Create)
				{
					entity.SourceInfo = new SourceInfo(source);
				}
				SetSourceToRelationShips(entity.GetRelationships(), source);
				SetSourceToAttributeValues(entity.GetAttributes(), source);
			}
			catch (Exception ex)
			{
				if (!isBasicTracingEnabled)
				{
					diagnosticActivity.Start();
				}

				diagnosticActivity.LogError(String.Format("Setting source to all the descendants of entity with id {0} failed with following expection: {1} ", entity.Id, ex.Message));
			}
			finally
			{
				if (isBasicTracingEnabled)
				{
					diagnosticActivity.Stop();
				}
			}

		}
		#endregion Helper Methods


		#endregion

		#region Private Methods

		private void ValidateContext(CallerContext callerContext, String methodName, MDMTraceSource traceSource)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.SourceBL." + methodName, String.Empty, methodName);
            }
        }

        private void ValidateSource(Source source, String methodName, MDMTraceSource traceSource)
        {
            if (source == null)
            {
                throw new MDMOperationException(String.Empty, "Source is null", "EntityManager.SourceBL." + methodName, String.Empty, methodName);
            }
        }

        private void ValidateSourcesCollection(SourceCollection sources, String methodName, MDMTraceSource traceSource)
        {
            if (sources == null)
            {
                throw new MDMOperationException(String.Empty, "Sources collection is null", "EntityManager.SourceBL." + methodName, String.Empty, methodName);
            }
            if (!sources.Any())
            {
                throw new MDMOperationException(String.Empty, "Sources collection is empty", "EntityManager.SourceBL." + methodName, String.Empty, methodName);
            }
        }

        private CallerContext PrepareCallerContext(CallerContext callerContext, String methodName)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                CallerContext context = (CallerContext)callerContext.Clone();
                context.ProgramName = EmptyProgramNameReplacementPrefix + methodName;
                return context;
            }
            return callerContext;
        }

        private static Boolean StartTraceActivity(String record, MDMTraceSource traceSource)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StartTraceActivity(PopulateTraceRecord(record), traceSource, false) : true;
        }

        private static Boolean StopTraceActivity(String record, MDMTraceSource traceSource)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StopTraceActivity(PopulateTraceRecord(record), traceSource) : true;
        }

        private static Boolean TraceInformation(String record, MDMTraceSource traceSource)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, PopulateTraceRecord(record), traceSource) : true;
        }

        private static Boolean TraceError(String record, MDMTraceSource traceSource)
        {
            return MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, PopulateTraceRecord(record), traceSource);
        }

        private static String PopulateTraceRecord(String record)
        {
            return TracingPrefix + record;
        }

        #region Setup object change sources

		/// <summary>
		/// Set source info object to collection of relationships
		/// </summary>
		/// <param name="relationshipCollection"></param>
		/// <param name="source"></param>
		private void SetSourceToRelationShips(IRelationshipCollection relationshipCollection, SourceInfo source)
		{
			if (relationshipCollection == null)
			{
				return;
			}
			foreach (Relationship relationship in relationshipCollection)
			{
				if (relationship.Action == ObjectAction.Create)
				{
					relationship.SourceInfo = new SourceInfo(source);
				}
				SetSourceToAttributeValues(relationship.RelationshipAttributes, source);
			}
		}

		/// <summary>
		/// Set source of changes to each value of attribute
		/// </summary>
		/// <param name="attributeCollection">Collection of attributes</param>
		/// <param name="source">Source of changes</param>
		public void SetSourceToAttributeValues(IAttributeCollection attributeCollection, Int32 source)
		{
			SetSourceToAttributeValues(attributeCollection, new SourceInfo(source));
		}

		/// <summary>
		/// Set the source object to the collection of attributes
		/// </summary>
		/// <param name="attributeCollection"></param>
		/// <param name="source"></param>
		public void SetSourceToAttributeValues(IAttributeCollection attributeCollection, SourceInfo source)
		{
			if (attributeCollection == null)
			{
				return;
			}
			foreach (Attribute attribute in attributeCollection)
			{
				if (attribute.IsComplex)
				{
					SetSourceToComplextAttributeValues(attribute, source);
				}
				else
				{
					SetSourceToValueCollection(attribute.OverriddenValues, source);

				}
			}
		}

		/// <summary>
		/// Set source of changes to each value of each instancerecord of complex attributes
		/// </summary>
		/// <param name="complexAttribute"></param>
		/// <param name="source"></param>
		private void SetSourceToComplextAttributeValues(Attribute complexAttribute, SourceInfo source)
		{
			if (complexAttribute.Action == ObjectAction.Create || complexAttribute.Action == ObjectAction.Update && !complexAttribute.OverriddenValues.IsNullOrEmpty())
			{
				if (complexAttribute.OverriddenValues == null)
				{
					return;
				}
				foreach (var overriddenValue in complexAttribute.OverriddenValues)
				{
					overriddenValue.Action = ObjectAction.Update;
				}
				SetSourceToValueCollection(complexAttribute.OverriddenValues, source);
			}
		}

		/// <summary>
		/// Set Source object of changes to collection of values
		/// </summary>
		/// <param name="valueCollection">Collection of values</param>
		/// <param name="source">Source of changes</param>
		private void SetSourceToValueCollection(IValueCollection valueCollection, SourceInfo source)
		{
			if (valueCollection == null)
			{
				return;
			}

			foreach (Value value in valueCollection)
			{
				if (value.Action == ObjectAction.Create || value.Action == ObjectAction.Update)
				{
					if (value.SourceInfo != null && value.SourceInfo.SourceId == (Int32)SystemSource.System)
					{
						continue;
					}

					value.SourceInfo = source;
				}
			}
		}

		#endregion Setup object change sources

		#endregion
	}
}