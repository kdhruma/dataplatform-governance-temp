namespace MDM.ExcelUtility.RSExcelStyling
{
	public class WorksheetStyles
	{
		/// <summary>
		/// Metadata Attribute Style
		/// </summary>
		public AttributesTypeStyle MetadataAttributeStyle { get; set; }

		/// <summary>
		/// Common Attribute Style
		/// </summary>
		public AttributesTypeStyle RequiredCommonAttributeStyle { get; set; }

		/// <summary>
		/// Category Specific Attribute Style
		/// </summary>
		public AttributesTypeStyle RequiredCategorySpecificAttributeStyle { get; set; }
        
        /// <summary>
		/// Relationship Attribute Style
		/// </summary>
		public AttributesTypeStyle RequiredRelationshipAttributeStyle { get; set; }

		/// <summary>
		/// Optional Common Attribute Style
		/// </summary>
		public AttributesTypeStyle OptionalCommonAttributeStyle { get; set; }

		/// <summary>
		/// Optional Category Specific Attribute Style
		/// </summary>
		public AttributesTypeStyle OptionalCategorySpecificAttributeStyle { get; set; }
        
        /// <summary>
		/// Optional Relationship Attribute Style
		/// </summary>
        public AttributesTypeStyle OptionalRelationshipAttributeStyle { get; set; }

		/// <summary>
		/// Common Attribute Collection Style
		/// </summary>
		public AttributesTypeStyle RequiredCommonAttributeCollectionStyle { get; set; }

		/// <summary>
		/// Category Specific Attribute Collection Style
		/// </summary>
		public AttributesTypeStyle RequiredCategorySpecificAttributeCollectionStyle { get; set; }
        
        /// <summary>
        /// Relationship Attribute Collection Style
		/// </summary>
        public AttributesTypeStyle RequiredRelationshipAttributeCollectionStyle { get; set; }

		/// <summary>
		/// Optional Common Attribute Collection Style
		/// </summary>
		public AttributesTypeStyle OptionalCommonAttributeCollectionStyle { get; set; }

		/// <summary>
		/// Optional Category Specific Attribute Collection Style
		/// </summary>
		public AttributesTypeStyle OptionalCategorySpecificAttributeCollectionStyle { get; set; }
        
        /// <summary>
        /// Optional Relationship Attribute Collection Style
		/// </summary>
        public AttributesTypeStyle OptionalRelationshipAttributeCollectionStyle { get; set; }

        /// <summary>
        /// System Attribute Style
        /// </summary>
        public AttributesTypeStyle SystemAttributeStyle { get; set; }

        /// <summary>
        /// Workflow Attribute Style
        /// </summary>
        public AttributesTypeStyle WorkflowAttributeStyle { get; set; }
	}
}
