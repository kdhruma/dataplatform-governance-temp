using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MDM.EntityManager.Business
{
	using Core;
	using BusinessObjects;
	using AttributeModelManager.Business;
	using CacheManager.Business;

	/// <summary>
	/// Specifies business operations for entity views
	/// </summary>
	public class EntityViewBL : BusinessLogicBase
	{
		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Gets the completion status for entity views based on criterion defined in the entity view xml
		/// </summary>
		/// <param name="entityId">Id of the entity for which status needs to be determined</param>
		/// <param name="userId">User Id for which status needs to be determined</param>
		/// <param name="entityViewXml">Entity editor left panel config xml containing views and the completion criteria</param>
		/// <param name="entityContext">Context parameters of the entity</param>
		/// <param name="isRecalculationRequired">Flag which says whether the completion status needs to be recalculated or needs to be get from the cache</param>
		/// <param name="application">Name of application which is performing action</param>
		/// <param name="module">Name of module which is performing action</param> 
		/// <returns>Collection of entity views with the completion status</returns>
		/// <exception cref="ArgumentNullException">Thrown when EntityViewXml or EntityContext is null</exception>
		/// <exception cref="ArgumentException">Thrown when Entity Id or User Id is not available</exception>
		public EntityViewCollection GetEntityViewCompletionStatus(Int64 entityId, Int32 userId, String entityViewXml, EntityContext entityContext, Boolean isRecalculationRequired, MDMCenterApplication application, MDMCenterModules module)
		{
			EntityViewCollection entityViewCollection = null;

			#region Validation of input parameters

			if (entityId < 1)
				throw new ArgumentException("Entity Id is not available. Completion status cannot be determined for product views.");

			if (userId < 1)
				throw new ArgumentException("User Id is not available. Completion status cannot be determined for product views.");

			if (entityContext == null)
				throw new ArgumentNullException("Entity Context");

			#endregion

			//Prepare completion status cache key
			String completionStatusCacheKey = String.Format("RS_EntityViewCompletionStatus_CatalogId_{0}_EntityTypeId_{1}_CategoryId_{2}_UserId_{3}", entityContext.ContainerId, entityContext.EntityTypeId, entityContext.CategoryId, userId);

			//Prepare cache object
			ICache cache = CacheFactory.GetCache();

			if (cache != null)
			{
				//Get the completion status object from the cache
				Object completionStatuscacheObject = cache[completionStatusCacheKey];

				if (completionStatuscacheObject != null && completionStatuscacheObject is EntityViewCollection)
				{
					entityViewCollection = completionStatuscacheObject as EntityViewCollection;

					//Check whether status recalculation is required..
					if (!isRecalculationRequired)
					{
						//No.. return the thus got cached object
						return entityViewCollection;
					}
				}
			}

			if (entityViewCollection == null)
			{
				//Failed to get the entity view collection completion status from cache
				//Get the entity view details from the entity view xml

				//Validate entity view xml
				if (String.IsNullOrWhiteSpace(entityViewXml))
					throw new ArgumentNullException("Entity View Xml");

				entityViewCollection = LoadEntityViewCollection(entityViewXml, entityContext);

				if (entityViewCollection == null)
					throw new Exception("Failed to get product view details. Completion status cannot be determined for product views.");
			}

			//Concat attribute ids of all the entity views
			List<Int32> attributeIdList = new List<Int32>();

			foreach (EntityView entityView in entityViewCollection)
			{
				if (entityView.AttributeIdList != null)
					attributeIdList = attributeIdList.Concat(entityView.AttributeIdList).ToList();
			}

			//Get attribute models for the above prepared attribute id list
			if (attributeIdList.Count > 0)
			{
				//Get the entity model
				EntityBL entityManager = new EntityBL();
				entityContext.LoadAttributes = true;
				entityContext.AttributeIdList = new Collection<Int32>(attributeIdList);
				Entity entity = entityManager.Get(entityId, entityContext, application, module);

				if (entity == null || entity.Attributes == null || entity.Attributes.Count < 1)
					throw new Exception("Failed to get entity details. Completion status cannot be determined for product views.");

				foreach (EntityView entityView in entityViewCollection)
				{
					//Set completion status as completed by default..
					Boolean completionStatus = true;

					if (entityView.CompletionCriterion != CompletionCriterionEnum.None)
					{
						foreach (Int32 attrId in entityView.AttributeIdList)
						{
							Attribute attribute = (Attribute)entity.GetAttribute(attrId, entity.Locale);

							if (attribute != null)
							{
								//Check for completion criterion..
								//If it is not required.. then continue with the other ids skipping the remaining part
								if (entityView.CompletionCriterion == CompletionCriterionEnum.Required && !attribute.Required)
									continue;

								if (!CheckHasAnyNonEmptyValue(attribute))
								{
									//attribute is not having value..
									//View is not completed
									completionStatus = false;

									//No need to check the status for the other attributes
									break;
								}
							}
						}
					}

					//Assign completion status
					entityView.CompletionStatus = completionStatus;
				}
			}

			if (cache != null)
			{
				//Add entity views completion status into the cache

				//TODO::Come up with cache update logic..
				if (cache[completionStatusCacheKey] != null)
					cache.Remove(completionStatusCacheKey);

				cache.Set(completionStatusCacheKey, entityViewCollection, DateTime.Now.AddHours(12.0));
			}

			return entityViewCollection;
		}

		#endregion

		#region Private Methods

		private EntityViewCollection LoadEntityViewCollection(String entityViewsXml, EntityContext entityContext)
		{
			//-----------------------------------------------------------------------
			//Expected entity view xml
			//-----------------------------------------------------------------------
			/*<TreeView Title="Item Stages" DefaultSelectedNodeId="1" Collapsible="True">
				<TreeNodes>
					<TreeNode Id="1" UniqueIdentifier="9c840faf-9566-41f8-9099-fe4eb590209b" Name="Basic Data" Title="Basic Data" Icon="EntityEditorViewController.OnEntityViews_NodeClick" OnClick="EntityEditorViewController.OnEntityViews_NodeClick">
						<Params>
							<Param Name="AttributeDisplayMode"><![CDATA[AttributeGroupIdList]]></Param>
							<Param Name="AttributeGroupIdList"><![CDATA[343,344]]></Param>
							<Param Name="ExcludeAttributeIdList"><![CDATA[4012,4013]]></Param>
							<Param Name="ShowCategory"><![CDATA[1]]></Param>
							<Param Name="CompletionCriterion"><![CDATA[List]]></Param>
							<Param Name="AttributeIdListForCompletionCriterion"><![CDATA[343]]></Param>
						</Params>
					<TreeNodes />
				</TreeNode>
			  </TreeView*/
			//------------------------------------------------------------------------

			EntityViewCollection entityViewCollection = null;

			XmlTextReader reader = new XmlTextReader(entityViewsXml, XmlNodeType.Element, null);

			if (reader != null)
			{
				entityViewCollection = new EntityViewCollection();

				AttributeModelBL attributeModelManager = new AttributeModelBL();
				//TODO :: AttributeModelContext.Locales : Passing multiple locale value form EntityContext to AttributeModelContext
				AttributeModelContext attributeModelContext = new AttributeModelContext(entityContext.ContainerId, entityContext.EntityTypeId, 0, entityContext.CategoryId, entityContext.DataLocales, 0, AttributeModelType.All, false, false, false);
				attributeModelContext.ApplySorting = false;

				while (!reader.EOF)
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "TreeNode")
					{
						EntityView entityView = new EntityView();

						//Read UniqueIdentifier from TreeNode node
						if (reader.IsStartElement() && reader.HasAttributes)
						{
							if (reader.MoveToAttribute("UniqueIdentifier"))
							{
								entityView.UniqueIdentifier = reader.ReadContentAsString();
							}
						}

						String attributeDisplayMode = String.Empty;
						Collection<Int32> attributeIdList = new Collection<Int32>();
						Collection<Int32> attributeGroupIdList = new Collection<Int32>();
						Int32 customViewId = 0;
						Int32 stateViewId = 0;
						Boolean isAttributeGroupSTA = false;
						Collection<Int32> excludeAttributeIdList = new Collection<Int32>();
						CompletionCriterionEnum completionCriterion = CompletionCriterionEnum.Required;
						Collection<Int32> attributeIdListForCompletionCriterion = new Collection<Int32>();

						//Read Parameters
						if (reader.ReadToFollowing("Params"))
						{
							while (!reader.EOF)
							{
								if (reader.NodeType == XmlNodeType.Element && reader.Name == "Param")
								{
									if (reader.MoveToAttribute("Name"))
									{
										String name = reader.ReadContentAsString();

										reader.Read();

										switch (name)
										{
											case "AttributeDisplayMode":
												attributeDisplayMode = reader.Value;
												break;
											case "AttributeIdList":
												attributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.Value, ',');
												break;
											case "AttributeGroupIdList":
												if (String.Compare(reader.Value, "sta", StringComparison.InvariantCultureIgnoreCase) == 0)
												{
													isAttributeGroupSTA = true;
												}
												else
												{
													attributeGroupIdList = ValueTypeHelper.SplitStringToIntCollection(reader.Value, ',');
												}
												break;
											case "CustomViewId":
												Int32.TryParse(reader.Value, out customViewId);
												break;
											case "StateViewId":
												Int32.TryParse(reader.Value, out stateViewId);
												break;
											case "ExcludeAttributeIdList":
												excludeAttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.Value, ',');
												break;
											case "CompletionCriterion":
												//If completionciteria is empty, set it to required.
												string strCompletionCriterion = reader.Value;
												if (String.IsNullOrWhiteSpace(strCompletionCriterion))
												{
													completionCriterion = CompletionCriterionEnum.Required;
												}
												else
												{
													Enum.TryParse(reader.Value, true, out completionCriterion);
												}
												break;
											case "AttributeIdListForCompletionCriterion":
												attributeIdListForCompletionCriterion = ValueTypeHelper.SplitStringToIntCollection(reader.Value, ',');
												break;
										}
									}
								}
								else
								{
									reader.Read();
								}

								if (reader.NodeType == XmlNodeType.Element && reader.Name == "TreeNode")
									break; //Coming out of the parameters reading loop.
							}
						}

						//Set completion criteria
						entityView.CompletionCriterion = completionCriterion;

						//Get the attribute id list configured for the view
						if (completionCriterion != CompletionCriterionEnum.None)
						{
							Collection<Int32> viewAttributeIdList = new Collection<int>();
							try
							{
								switch (attributeDisplayMode)
								{
									case "AttributeIdList":
										viewAttributeIdList = attributeIdList;
										break;
									case "AttributeGroupIdList":
										viewAttributeIdList = attributeModelManager.GetAttributeIdList(attributeGroupIdList, 0, 0, attributeModelContext);
										break;
									case "CustomViewId":
										viewAttributeIdList = attributeModelManager.GetAttributeIdList(null, customViewId, 0, attributeModelContext);
										break;
									case "StateViewId":
										viewAttributeIdList = attributeModelManager.GetAttributeIdList(null, 0, stateViewId, attributeModelContext);
										break;
									case "Mixed":
										if (isAttributeGroupSTA)
										{
											attributeModelContext.AttributeModelType = AttributeModelType.Category;
											viewAttributeIdList = attributeModelManager.GetAttributeIdList(attributeGroupIdList, customViewId, stateViewId, attributeModelContext);
										}
										else
										{
											viewAttributeIdList = attributeModelManager.GetAttributeIdList(attributeGroupIdList, customViewId, stateViewId, attributeModelContext);
										}
										viewAttributeIdList = ValueTypeHelper.MergeCollections(attributeIdList, viewAttributeIdList);
										break;
								}
							}
							catch
							{
								//Ignore the exception.. Possible exception is when custom view is not configured properly, throws 'No attribute(s) found for the given context'..
							}

							if (viewAttributeIdList != null && viewAttributeIdList.Count > 0)
							{
								IEnumerable<Int32> tempAttributeIdList = viewAttributeIdList;

								if (excludeAttributeIdList != null)
									tempAttributeIdList = tempAttributeIdList.Except(excludeAttributeIdList);

								if (completionCriterion == CompletionCriterionEnum.List && attributeIdListForCompletionCriterion != null)
									tempAttributeIdList = tempAttributeIdList.Intersect(attributeIdListForCompletionCriterion);

								entityView.AttributeIdList = new Collection<Int32>(tempAttributeIdList.Distinct().ToList());
							}
						}

						if (entityView != null)
							entityViewCollection.Add(entityView);
					}
					else
					{
						reader.Read();
					}
				}

				if (reader != null)
				{
					reader.Close();
				}
			}

			return entityViewCollection;
		}

		/// <summary>
		/// Check if attribute has any nonempty value (without considering SourceFlag. overridden or inherited)
		/// </summary>
		/// <returns>True : If value collection (Inherited or Overridden) has any value. False : otherwise</returns>
		public Boolean CheckHasAnyNonEmptyValue(Attribute attribute)
		{
			Boolean hasAnyNonEmptyValue = false;

			//check for each value in OverridenValues
			if (attribute.HasAnyValue())
			{
				foreach (Value value in attribute.GetOverriddenValuesInvariant())
				{
					if (value.AttrVal != null && !String.IsNullOrWhiteSpace(value.AttrVal.ToString()))
					{
						hasAnyNonEmptyValue = true;
					}
				}
			}

			//check for each value in InheritedValues
			if (attribute.HasAnyValue())
			{
				foreach (Value value in attribute.InheritedValues)
				{
					if (value != null && !String.IsNullOrWhiteSpace(value.AttrVal.ToString()))
					{
						hasAnyNonEmptyValue = true;
					}
				}
			}

			return hasAnyNonEmptyValue;
		}

		#endregion

		#endregion
	}
}
