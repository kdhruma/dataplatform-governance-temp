using System;
using System.Runtime.Serialization;

namespace MDM.ExcelUtility
{
    using MDM.Core;
  
    /// <summary>
    /// Defines the options for the RSExcel creation.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public class RSExcelCreationOptions
    {
        #region Fields

        private Boolean _populateEntityStateInfo = true;

        private Boolean _populateBusinessConditionInfo = true;

        private Boolean _populateEntitySheet = true;

        private Boolean _populateRelationshipSheet = true;

        private String _profileName = String.Empty;

        private Boolean _isApprovedCopy = false;

        private Boolean _populateCategoryLongNamePath = false;

        /// <summary>
        /// 
        /// </summary>
        private String _collectionSeparator = ",";

        /// <summary>
        /// 
        /// </summary>
        private String _uomSeparator = "||";

        /// <summary>
        /// Indicates column header format for attributes.
        /// </summary>
        private RSExcelAttributeColumnHeaderType _attributeColumnHeaderType = RSExcelAttributeColumnHeaderType.AttributeAndAttributeParentLongName;

        /// <summary>
        /// Indicates category path format - Short name or long name path
        /// </summary>
        private RSExcelCategoryPathType _categoryPathType = RSExcelCategoryPathType.ShortNamePath;

        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _formattingLocaleForNonLocalizableAttributes = LocaleEnum.UnKnown;

        /// <summary>
        /// 
        /// </summary>
        private String _fileType = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RSExcelCreationOptions()
        {
            HideRelationshipsSheet = false;
        }

        public RSExcelCreationOptions(Boolean populateEntitySheet, Boolean populateRelationshipSheet)
        {
            _populateEntitySheet = populateEntitySheet;
            _populateRelationshipSheet = populateRelationshipSheet;
            HideRelationshipsSheet = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="populateEntitySheet"></param>
        /// <param name="populateRelationshipSheet"></param>
        /// <param name="populateConfigSheet"></param>
        /// <param name="populateReadMeSheet"></param>
        public RSExcelCreationOptions(Boolean populateEntitySheet, Boolean populateRelationshipSheet, String collectionSeparator, String uomSeparator)
        {
            this._populateEntitySheet = populateEntitySheet;
            this._populateRelationshipSheet = populateRelationshipSheet;
            this._collectionSeparator = collectionSeparator;
            this._uomSeparator = uomSeparator;
        }

        #endregion

        #region Properties

        [DataMember]
        public Boolean HideRelationshipsSheet { get; set; }

        /// <summary>
        /// Populate Entity State Info
        /// </summary>
        [DataMember]
        public Boolean PopulateEntityStateInfo
        {
            get { return _populateEntityStateInfo; }
            set { _populateEntityStateInfo = value; }
        }

        /// <summary>
        /// Populate Entity Business Condition Info
        /// </summary>
        [DataMember]
        public Boolean PopulateBusinessConditionInfo
        {
            get { return _populateBusinessConditionInfo; }
            set { _populateBusinessConditionInfo = value; }
        }

        /// <summary>
        /// Populate Entity Sheet
        /// </summary>
        [DataMember]
        public Boolean PopulateEntitySheet
        {
            get { return _populateEntitySheet; }
            set { _populateEntitySheet = value; }
        }

        /// <summary>
        /// Populate Relationship Sheet
        /// </summary>
        [DataMember]
        public Boolean PopulateRelationshipSheet
        {
            get { return _populateRelationshipSheet; }
            set { _populateRelationshipSheet = value; }
        }

        /// <summary>
        /// Populate Relationship Sheet
        /// </summary>
        [DataMember]
        public String ProfileName
        {
            get { return _profileName; }
            set { _profileName = value; }
        }

        /// <summary>
        /// Populate Relationship Sheet
        /// </summary>
        [DataMember]
        public Boolean IsApprovedCopy
        {
            get { return _isApprovedCopy; }
            set { _isApprovedCopy = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean PopulateCategoryLongNamePath
        {
            get { return _populateCategoryLongNamePath; }
            set { _populateCategoryLongNamePath = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String CollectionSeparator
        {
            get { return _collectionSeparator; }
            set { _collectionSeparator = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String UomSeparator
        {
            get { return _uomSeparator; }
            set { _uomSeparator = value; }
        }

        /// <summary>
        /// Indicates column header format for attributes.
        /// </summary>
        [DataMember]
        public RSExcelAttributeColumnHeaderType AttributeColumnHeaderType
        {
            get { return _attributeColumnHeaderType; }
            set { _attributeColumnHeaderType = value; }
        }

        /// <summary>
        /// Indicates category path format - Short name or long name path
        /// </summary>
        [DataMember]
        public RSExcelCategoryPathType CategoryPathType
        {
            get { return _categoryPathType; }
            set { _categoryPathType = value; }
        }

        /// <summary>
        /// Indicates formatting locale for non-localizable attributes
        /// </summary>
        [DataMember]
        public LocaleEnum FormattingLocaleForNonLocalizableAttributes
        {
            get { return _formattingLocaleForNonLocalizableAttributes; }
            set { _formattingLocaleForNonLocalizableAttributes = value; }
        }

        /// <summary>
        /// Indicates file type of created file
        /// </summary>
        [DataMember]
        public String FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        #endregion
    }
}
