using System;
using System.Drawing;
using System.Linq;
using System.Activities;
using System.ComponentModel;
using MDM.LookupManager.Business;

namespace MDM.Workflow.Activities.Entity
{
    using MDMBO = MDM.BusinessObjects;
    using MDMC = MDM.Core;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.Workflow.Activities.Core;
    using MDM.EntityManager.Business;
    using MDM.Workflow.Activities.Designer;
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using System.Collections.ObjectModel;
    using MDM.Core;

    /// <summary>
    /// Compares the requested attribute value with the given value and the returns the result.
    /// </summary>
    [Designer(typeof(CompareAttributeValueDesigner))]
    [ToolboxBitmap(typeof(CompareAttributeValueDesigner), "Images.CompareAttributeValue.bmp")]
    public class CompareAttributeValue : MDMCodeActivitiyBase<Boolean>
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
		[DisplayName(@"Attribute Model Type")]
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

		/// <summary>
		/// </summary>
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Boolean Execute(CodeActivityContext context)
        {
            Object dataContext = MDMDataContext.Get(context);

            if (dataContext == null)
            {
                return false;
            }

            //Get the entity participating in the workflow
            MDMBOW.WorkflowDataContext wfDataContext = (MDMBOW.WorkflowDataContext) dataContext;
            MDMBOW.WorkflowMDMObject mdmObject = null;

			if (wfDataContext.MDMObjectCollection.Count > 0)
			{
				mdmObject = wfDataContext.MDMObjectCollection.First();
			}

			if (mdmObject == null)
			{
				return false;
			}

            //Get the Attribute Name and Group name for which value needs to be checked
            String attributeName = AttributeName.Get(context);
            String attributeGroupName = AttributeGroupName.Get(context);
            Object attributeValue = AttributeValue.Get(context);

            if (!String.IsNullOrEmpty(attributeName) && !String.IsNullOrEmpty(attributeGroupName))
            {
                //Get attribute id from attribute name and attribute group name
                Int32 attributeId = new AttributeModelBL().GetAttributeId(attributeName, attributeGroupName);

                Int64 entityId = mdmObject.MDMObjectId;

                if (attributeId > 0)
                {
                    String strLocale = Locale.Get(context);
                    LocaleEnum locale;
                    Enum.TryParse(strLocale, out locale);

                    EntityContext entityContext = new EntityContext(0, 0, 0, locale, new Collection<LocaleEnum>() {locale} ,false, true, false, false, null, new Collection<Int32> { attributeId }, false, false, null, false, false, MDMC.AttributeModelType.All);

                    Entity entity = new EntityBL().Get(entityId, entityContext, wfDataContext.Application, wfDataContext.Module, false, false);

                    if (entity != null)
                    {
	                    Attribute attribute = (Attribute) entity.GetAttribute(attributeId, locale);

                        if(attribute != null)
                        {

                            Object origVal = attribute.GetCurrentValue();// current value gives values for either overridden or inherited value

                            //Logic to get valued from lookup table.  Need to specify the value in the display format
                            if (attribute.IsLookup && (origVal != null))
                            {
                                Collection<Int32> lookupValues = new Collection<Int32>();
                                Lookup lookup = new LookupBL().Get(attribute.Id, locale, -1, new CallerContext(MDMCenterApplication.WindowsWorkflow, MDMCenterModules.MDMAdvanceWorkflow));
                                Row row = (Row) lookup.GetRecordById(Int32.Parse(origVal.ToString()));
                                if (row != null)
                                {
                                    Int32 rowId = (Int32) row.Id;
                                    attributeValue = lookup.GetDisplayFormatById(rowId);
                                }
                            }

                            if (origVal != null && attributeValue != null) // do comparison only if both objects have values..
                            {
								if (origVal == attributeValue) // do object comparison
								{
									return true;
								}
								if (origVal.ToString().ToLower() == attributeValue.ToString().ToLower()) // if objects are not same, try to do string comparison of both the objects
								{
									return true;
								}
                            }
                        }
                    }
                }
            }

            return false;
        }

		#endregion
	}
}
