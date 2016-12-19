using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    public class EntityOperationDiagnosticReport : DiagnosticReportBase, IEntityOperationDiagnosticReport, ICloneable
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private String _inputDataXml;

        #endregion

        #region Constructors

        #endregion

        #region Properties
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(1)]
        public String InputDataXml
        {
            get { return _inputDataXml; }
            set { _inputDataXml = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implemented ICloneable interface
        /// </summary>
        /// <returns>Cloned EntityOperationDiagnosticReport object</returns>
        public object Clone()
        {
            EntityOperationDiagnosticReport report = (EntityOperationDiagnosticReport)this.MemberwiseClone();

            return report;
        }

        #endregion
    }
}
