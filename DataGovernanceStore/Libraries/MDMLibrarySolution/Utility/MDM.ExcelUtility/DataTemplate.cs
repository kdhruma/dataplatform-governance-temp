using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.ExcelUtility
{
    using MDM.Core;
    using MDM.Interfaces;

    public class DataTemplate
    {
        public IAttributeModelCollection EntityAttributeModels = MDMObjectFactory.GetIAttributeModelCollection();

        public IAttributeModelCollection RelationshipAttributeModels = MDMObjectFactory.GetIAttributeModelCollection();

        public Collection<LocaleEnum> RequestedLocales = new Collection<LocaleEnum>();

        public Dictionary<Int32, String> ExcelValidationInfo = new Dictionary<Int32,String>();

        public HashSet<Int32> LookupAttributeModelsContextPresence = new HashSet<Int32>();
    }
}
