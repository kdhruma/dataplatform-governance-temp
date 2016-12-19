using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("TranslationStep")]
    [Serializable()]
    public sealed class TranslationStep : RS.MDM.Object
    {
        #region Field

        /// <summary>
        /// Field indicates source language for translation
        /// </summary>
        private string _sourceLanguage = string.Empty;

        /// <summary>
        /// field indicates target language for translation
        /// </summary>
        private string _targetLanguage = string.Empty;

        /// <summary>
        /// Indicates steps for translation (i.e - 1,2,3) 
        /// </summary>
        private int _sequence = 1;

        #endregion Field

        #region Properties

        /// <summary>
        /// Specifies source language for translation
        /// </summary>
        [XmlAttribute("SourceLanguage")]
        [Description("Indicates Source Language For Translation")]
        [Category("Translation")]
        [TrackChanges()]
        public string SourceLanguage
        {
            get
            {
                return _sourceLanguage;
            }
            set
            {
                _sourceLanguage = value;
            }
        }

        /// <summary>
        /// Specifies target language for translation
        /// </summary>
        [XmlAttribute("TargetLanguage")]
        [Description("Indicates Target Language For Translation")]
        [Category("Translation")]
        [TrackChanges()]
        public string TargetLanguage
        {
            get
            {
                return _targetLanguage;
            }
            set
            {
                _targetLanguage = value;
            }
        }

        /// <summary>
        /// Specifies steps sequence for translation
        /// </summary>
        [XmlAttribute("Sequence")]
        [Description("Indicates Step Number of the Translation")]
        [Category("Translation")]
        [TrackChanges()]
        public int Sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
            }
        }

        #endregion Properties

        #region Constructor
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public TranslationStep()
            : base()
        {
        }

        #endregion Constructor

        #region Method

        #region Serialization and deserialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Additional Context used for deserialization</param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion Serialization and deserialization

        #region Overrides

        /// <summary>
        /// Get a tree node that reprents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "UIColumn";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            return _treeNode;
        }

        #endregion

        #endregion
    }
}
