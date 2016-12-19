using System;
using System.Drawing;
using System.Linq;
using System.Activities;
using System.ComponentModel;

namespace MDM.Workflow.Activities.Entity
{
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
    /// Checks whether requested attribute is having value in the passed entity
    /// </summary>
    [Designer(typeof(HasAttributeValueDesigner))]
    [ToolboxBitmap(typeof(HasAttributeValueDesigner), "Images.HasAttributeValue.bmp")]
    public class HasAttributeValue : MDMCodeActivitiyBase<Boolean>
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
		[DisplayName(@"Attribute Model Type to Check")]
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

                    EntityContext entityContext = new EntityContext(0, 0, 0, locale, new Collection<LocaleEnum>() {locale}, false, true, false, false, null, new Collection<Int32> { attributeId }, false, false, null, false, false, MDMC.AttributeModelType.All);
                    Entity entity = new EntityBL().Get(entityId, entityContext, wfDataContext.Application, wfDataContext.Module, false, false);

                    if (entity != null)
                    {
                        Attribute attribute = ( Attribute ) entity.GetAttribute(attributeId, locale);
                        if (attribute != null && attribute.HasAnyValue()) // Return true if attribute has any value(overridden or inherited)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

		#endregion
	}
}
