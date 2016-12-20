using System;
using System.Linq;
using MDM.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace MDM.JigsawIntegrationManager.MessageProducers
{
    using Core.Extensions;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;
    using System.Text.RegularExpressions;

    internal class ProducerHelper
    {
        private static Lazy<IEntityManager> _entityManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static DTO.Relationship CreateRelationship(Entity entity)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            bool isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            var relationship = new DTO.Relationship();

            try
            {
                String pattern = JigsawConstants.CategoryPathSeparator;

                

                if (!String.IsNullOrEmpty(entity.CategoryPath) && entity.CategoryPath.Contains(@"#@#"))
                {
                    pattern = @"#@#";
                }

                relationship.EntityId = String.IsNullOrWhiteSpace(entity.ExternalId) ? entity.Name : entity.ExternalId;
                relationship.ParentEntityId = String.IsNullOrWhiteSpace(entity.ParentExternalId) ? entity.ParentEntityName: entity.ParentExternalId;
                relationship.FamilyId = entity.EntityFamilyId.ToString();
                relationship.FamilyTreeId = entity.EntityGlobalFamilyId.ToString();
                relationship.FamilyName = entity.EntityGlobalFamilyLongName;

                relationship.Organization = entity.OrganizationName;

                Entity newReclassifiedEntity;

                switch (entity.EntityMoveContext.ReParentType)
                {
                    case ReParentTypeEnum.CategoryReParent:
                    case ReParentTypeEnum.HiearchyReParent:

                        if (_entityManager == null)
                        {
                            _entityManager = new Lazy<IEntityManager>(() => ServiceLocator.Current.GetInstance(typeof(IEntityManager)) as IEntityManager);
                        }

                        if (_entityManager.Value != null)
                        {
                            EntityContext entityContext = new EntityContext() { LoadEntityProperties = true };
                            newReclassifiedEntity = _entityManager.Value.Get(entity.Id, entityContext, new EntityGetOptions() { PublishEvents = false }, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.JigsawIntegration));
                            SetCategoryPath(newReclassifiedEntity, relationship, pattern);
                        }

                        break;
                    default:
                        SetCategoryPath(entity, relationship, pattern);
                        break;
                }

                

                String segmentAttrName = JigsawConstants.Segment;

                if ((!String.IsNullOrWhiteSpace(segmentAttrName)) && (!segmentAttrName.Equals("NONE")))
                {
                    var attribute = entity.Attributes.FirstOrDefault(attr => attr.Name.Equals(segmentAttrName));

                    if (attribute != null && attribute.CurrentValue != null)
                    {
                        relationship.Segment = attribute.CurrentValue.ToString();
                    }
                }

                relationship.RelatedExternalId = entity.Id.ToString();
                relationship.EntityTypeOfRelatedEntity = entity.EntityTypeName.ToJsCompliant();
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return relationship;
        }

        /// <summary>
        /// Creates the change context.
        /// </summary>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        public static DTO.ChangeContext CreateChangeContext(CallerContext callerContext)
        {
            var changeContext = new DTO.ChangeContext();

            changeContext.User = callerContext.AdditionalProperties != null && callerContext.AdditionalProperties.ContainsKey("UserName") ? callerContext.AdditionalProperties["UserName"].ToString() : String.Empty;

            changeContext.SourceTimestamp = DateTime.Now.ToString("O");
            changeContext.IngestTimestamp = changeContext.SourceTimestamp;

            if (callerContext != null)
            {
                var programName = callerContext.ProgramName;
                
                if(programName.Contains("Job Id"))
                {
                    programName = Regex.Replace(callerContext.ProgramName, @"(, Job Id).*", String.Empty);
                }

                changeContext.ChangeAgent = programName;
                changeContext.ChangeAgentType = callerContext.Application.ToString();
                changeContext.ChangeInterface = callerContext.Module.ToString();
                changeContext.JobId = callerContext.JobId > 0 ? callerContext.JobId.ToString() : String.Empty;
            }

            return changeContext;
        }

        /// <summary>
        /// Sets the categoryPath of the an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationship"></param>
        /// <param name="pattern"></param>
        private static void SetCategoryPath(Entity entity, DTO.Relationship relationship, string pattern)
        {
            if (!String.IsNullOrEmpty(entity.CategoryPath))
            {
                relationship.CategoryPath = String.Join("/", ValueTypeHelper.SplitStringToStringCollection(entity.CategoryPath, pattern, StringSplitOptions.RemoveEmptyEntries)
                        .Select(pathName => pathName.Trim())
                        .ToList());
            }

            if (!String.IsNullOrEmpty(entity.CategoryLongNamePath))
            {
                relationship.CategoryNamePath = String.Join("/", ValueTypeHelper.SplitStringToStringCollection(entity.CategoryLongNamePath, pattern, StringSplitOptions.RemoveEmptyEntries)
                        .Select(pathName => pathName.Trim())
                        .ToList());
            }

            relationship.Category = entity.CategoryName;
            relationship.CategoryName = entity.CategoryLongName;



            if (String.IsNullOrEmpty(relationship.Category))
            {
                var categories = ValueTypeHelper.SplitStringToStringCollection(entity.CategoryPath, @"//", StringSplitOptions.RemoveEmptyEntries);

                if (categories != null && categories.Count > 0)
                {
                    relationship.Category = categories[categories.Count - 1];
                }
                else
                {
                    relationship.Category = entity.CategoryPath;
                }
            }

            relationship.Variant = entity.Id.ToString();

            var variantPathRegex = new Regex("\\s");

            relationship.VariantPath = String.IsNullOrEmpty(entity.IdPath) ? String.Empty : variantPathRegex.Replace(entity.IdPath.Trim(' '), JigsawConstants.JigsawSeparator);

            relationship.Container = entity.ContainerName;
            relationship.ContainerName = entity.ContainerLongName;

            if (!String.IsNullOrEmpty(entity.Path))
            {
                var paths = ValueTypeHelper.SplitStringToStringCollection(entity.Path, "#@#");

                if (paths != null)
                {
                    if (paths.Count > 1)
                    {
                        paths.RemoveAt(paths.Count - 1);
                    }

                    relationship.ContainerPath = ValueTypeHelper.JoinCollection(paths, "/");
                }
            }
        }
    }
}
