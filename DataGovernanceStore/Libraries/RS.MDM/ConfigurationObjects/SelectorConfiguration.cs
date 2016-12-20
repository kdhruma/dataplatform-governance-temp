using System;
using System.Xml.Serialization;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("SelectorConfiguration")]
    [Serializable()]
    public class SelectorConfiguration : Object, ICloneable
    {
        private Int32 _bulkEditDefaultProfileId = 0;
        private Int32 _exportToExcelDefaultProfileId = 0;
        private Int32 _dataModelExportDefaultProfileId = 0;
        private Int32 _detailViewDefaultProfileId;

        [XmlAttribute("BulkEditDefaultProfileId")]
        [Category("SelectorConfiguration")]
        [Description("Property denoting Bulk Edit default profile Id")]
        public Int32 BulkEditDefaultProfileId
        {
            get
            {
                return _bulkEditDefaultProfileId;
            }
            set
            {
                _bulkEditDefaultProfileId = value;
            }
        }

        [XmlAttribute("ExportToExcelDefaultProfileId")]
        [Category("SelectorConfiguration")]
        [Description("Property denoting Export To Excel default profile Id")]
        public Int32 ExportToExcelDefaultProfileId
        {
            get
            {
                return _exportToExcelDefaultProfileId;
            }
            set
            {
                _exportToExcelDefaultProfileId = value;
            }
        }

        [XmlAttribute("DataModelExportDefaultProfileId")]
        [Category("SelectorConfiguration")]
        [Description("Property denoting DataModel Export default profile Id")]
        public Int32 DataModelExportDefaultProfileId
        {
            get
            {
                return _dataModelExportDefaultProfileId;
            }
            set
            {
                _dataModelExportDefaultProfileId = value;
            }
        }

        [XmlAttribute("DetailViewDefaultProfileId")]
        [Category("SelectorConfiguration")]
        [Description("Property denoting Detail View default profile Id")]
        public Int32 DetailViewDefaultProfileId
        {
            get { return _detailViewDefaultProfileId; }
            set { _detailViewDefaultProfileId = value; }
        }

        /// <summary>
        /// Initializes a new instance of SelectorConfiguration class.
        /// </summary>
        public SelectorConfiguration()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of SelectorConfiguration class.
        /// </summary>
        public SelectorConfiguration(Int32 bulkEditDefaultProfileId, Int32 exportToExcelDefaultProfileId, Int32 dataModelExportDefaultProfileId)
        {
            _bulkEditDefaultProfileId = bulkEditDefaultProfileId;
            _exportToExcelDefaultProfileId = ExportToExcelDefaultProfileId;
            _dataModelExportDefaultProfileId = DataModelExportDefaultProfileId;
        }

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion  

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is SelectorConfiguration)
                {
                    SelectorConfiguration objectToBeCompared = obj as SelectorConfiguration;

                    if (this.BulkEditDefaultProfileId != objectToBeCompared.BulkEditDefaultProfileId)
                        return false;

                    if (this.ExportToExcelDefaultProfileId != objectToBeCompared.ExportToExcelDefaultProfileId)
                        return false;

                    if (this.DataModelExportDefaultProfileId != objectToBeCompared.DataModelExportDefaultProfileId)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.BulkEditDefaultProfileId.GetHashCode() ^ this.ExportToExcelDefaultProfileId.GetHashCode() 
                            ^ this.DataModelExportDefaultProfileId.GetHashCode();

            return hashCode;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}