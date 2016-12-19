using System;
using System.Runtime.Serialization;
using MDM.Core;
using MDM.Interfaces;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies an Import Template
    /// </summary>
    [DataContract]
    public class Template : MDMObject, ITemplate
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public Template()
            :base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="templateName">Contains Name of Template</param>
        /// <param name="fileName">Contains Name of File</param>
        /// <param name="fileType">Indicates Mime Type of file</param>
        /// <param name="fileData">Contains content of file in byte array</param>
        public Template(String templateName, String fileName, String fileType, byte[] fileData)
            : base(0, fileName, templateName)
        {
            this.FileType = fileType;
            this.FileData = fileData;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="templateName">Contains Name of Template</param>
        /// <param name="fileName">Contains Name of File</param>
        /// <param name="fileType">Indicates Mime Type of file</param>
        /// <param name="fileData">Contains content of file in byte array</param>
        /// <param name="templateType">Contains type of Template</param>
        public Template(String templateName, String fileName, String fileType, byte[] fileData, TemplateType templateType)
            : base(0, fileName, templateName)
        {
            this.FileType = fileType;
            this.FileData = fileData;
            this.TemplateType = templateType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Data Content of the File
        /// </summary>
        [DataMember]
        public Byte[] FileData { get; set; }

        /// <summary>
        /// Property denoting the File Type
        /// </summary>
        [DataMember]
        public String FileType { get; set; }

        /// <summary>
        /// Type of Template [Export/Import]
        /// </summary>
        [DataMember]
        public TemplateType TemplateType
        {
            get;
            set;
        }

        #endregion
    }
}