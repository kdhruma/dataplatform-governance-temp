using System;
using System.Drawing;
using System.ComponentModel;
using System.Activities;
using System.Linq;
using System.Collections.ObjectModel;

namespace MDM.Workflow.Activities.Entity
{
    using MDM.BusinessObjects;
    using MDMC = MDM.Core;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.EntityManager.Business;
    using MDM.Workflow.Activities.Core;
    using MDM.Workflow.Activities.Designer;
    using MDM.AttributeModelManager.Business;
    using MDM.Core;

    ///<summary>
    ///Saves the entity details in the database 
    ///</summary>
    [Designer(typeof(UpdateEntityDesigner))]
    [ToolboxBitmap(typeof(UpdateEntityDesigner), "Images.UpdateEntity.bmp")]
    public class UpdateEntity : MDMCodeActivitiyBase<Boolean>
    {
		#region Fields

        private InArgument<String> _locale = "en_WW";
        private MDMC.AttributeModelType _attributeModelType = MDMC.AttributeModelType.Common;

		#endregion

		#region Properties

        ///<summary>
        ///</summary>
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<String> Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        ///<summary>
        ///</summary>
        [DisplayName(@"Attribute Model Type to Update")]
        [Category("Input Arguments")]
        public MDMC.AttributeModelType AttributeModelType
        {
            get { return _attributeModelType; }
            set { _attributeModelType = value; }
        }

        /// <summary>
        /// </summary>
        [DisplayName(@"Attribute Group Name")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<String> AttributeGroupName { get; set; }

        /// <summary>
        /// </summary>
        [DisplayName(@"Attribute Name")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public InArgument<String> AttributeName { get; set; }

        ///<summary>
        ///</summary>
        [DisplayName(@"Attribute Value Source")]
        [Category("Input Arguments")]
        public MDMC.AttributeValueSource AttributeValueSource { get; set; }

		///<summary>
		///</summary>
		[DisplayName(@"Attribute Value")]
		[Category("Input Arguments")]
		[RequiredArgument]
		public InArgument<Object> AttributeValue { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [Browsable(false)]
        public new OutArgument<MDMBOW.WorkflowActionContext> MDMActionContext { get; set; }

		#endregion

		#region Methods

    	/// <summary>
    	/// When implemented in a derived class, performs the execution of the activity.
    	/// </summary>
    	/// <returns>
    	/// The result of the activity’s execution.
    	/// </returns>
    	/// <param name="context">The execution context under which the activity executes.</param>
    	protected override Boolean Execute(CodeActivityContext context)
        {
			Object dataContext = MDMDataContext.Get(context);

            if (dataContext == null)
            {
                return false;
            }

			//Get the entity participating in the workflow
            MDMBOW.WorkflowMDMObject mdmObject = null;
			MDMBOW.WorkflowDataContext wfDataContext = (MDMBOW.WorkflowDataContext) dataContext;

            if (wfDataContext.MDMObjectCollection != null && wfDataContext.MDMObjectCollection.Count > 0)
            {
                mdmObject = wfDataContext.MDMObjectCollection.First();
            }

            if (mdmObject == null)
            {
                return false;
            }

            var entityBL = new EntityBL();

    		//Get the attribute properties
            String attributeName = AttributeName.Get(context);
            String attributeGroupName = AttributeGroupName.Get(context);
            Object attributeValue = AttributeValue.Get(context);

            Boolean isUpdateFailed = false;
			
            if (!String.IsNullOrEmpty(attributeName) && !String.IsNullOrEmpty(attributeGroupName))
            {
                //Get attribute id from attribute name and attribute group name
                Int32 attributeId = new AttributeModelBL().GetAttributeId(attributeName, attributeGroupName);
                Int64 entityId = mdmObject.MDMObjectId;

                if (attributeId > 0)
                {
                    String strLocale = Locale.Get(context);
                    LocaleEnum localeVal;
                    Enum.TryParse(strLocale, out localeVal);

                    // Create entity context with given attribute id
                    EntityContext entityContext = new EntityContext(0, 0, 0, localeVal, new Collection<LocaleEnum>() {localeVal},  false, true, false, false, null, new Collection<Int32> { attributeId }, false, false, null, false, false, MDMC.AttributeModelType.All);

                    Entity entity = new EntityBL().Get(entityId, entityContext, wfDataContext.Application, wfDataContext.Module, false, false);

                    if (entity != null)
                    {
	                    Attribute attribute = (Attribute) entity.GetAttribute(attributeId, localeVal);
                        if (attribute != null) // Return true if attribute has any value(overridden or inherited)
                        {
                            attribute.OverriddenValues.Clear();

	                        var val = new Value
	                        {
		                        AttrVal = attributeValue,
		                        Locale = localeVal,
		                        Action = ObjectAction.Update
	                        };

	                        //attribute.OverriddenValues.Add(val);
                            attribute.AppendValue(val, localeVal);
                            attribute.SourceFlag = AttributeValueSource;
                            attribute.Action = MDM.Core.ObjectAction.Update;
                            entity.Action = MDM.Core.ObjectAction.Update;

	                        EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions
	                        {
		                        ApplyAVS = false,
		                        PublishEvents = false
	                        };

	                        var entityOperationResult = entityBL.Update(entity, entityProcessingOptions, "UpdateEntityActivity", wfDataContext.Application, wfDataContext.Module);

                            if (entityOperationResult != null && entityOperationResult.OperationResultStatus == MDMC.OperationResultStatusEnum.Failed)
                            {
                                isUpdateFailed = true;
                            }
                        }
                    }
                }
            }

            return !isUpdateFailed;
        }

		#endregion
	}
}
