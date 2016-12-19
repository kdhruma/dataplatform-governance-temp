using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;
    using MDM.BusinessObjects.Interfaces.Exports;


    /// <summary>
    /// Specifies the exportdataformatter collection object
    /// </summary>
    [DataContract]
    public class ExportDataFormatterCollection : InterfaceContractCollection<IExportDataFormatter, ExportDataFormatter>, IExportDataFormatterCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ExportDataFormatterCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public ExportDataFormatterCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadExportDataFormatterCollection(valueAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize export data formatter collection from IList
        /// </summary>
        /// <param name="exportDataFormattersList">IList of exportdataformattercollection</param>
        public ExportDataFormatterCollection(IList<ExportDataFormatter> exportDataFormattersList)
        {
            this._items = new Collection<ExportDataFormatter>(exportDataFormattersList);
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatterId"></param>
        /// <returns></returns>
        public ExportDataFormatter GetExportDataFormatter(Int32 formatterId)
        {
            ExportDataFormatter formatterToReturn = null;

            foreach (ExportDataFormatter formatter in this._items)
            {
                if (formatter.Id.Equals(formatterId))
                {
                    formatterToReturn = formatter;
                    break;
                }
            }

            return formatterToReturn;
        }

        /// <summary>
        /// Get Xml representation of export data formatter collection object
        /// </summary>
        /// <returns>Xml string representing the export data formatter collection</returns>
        public String ToXml()
        {
            String exportDataFormattersXml = String.Empty;

            exportDataFormattersXml = "<ExportDataFormatters>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExportDataFormatter formatter in this._items)
                {
                    exportDataFormattersXml = String.Concat(exportDataFormattersXml, formatter.ToXml());
                }
            }

            exportDataFormattersXml = String.Concat(exportDataFormattersXml, "</ExportDataFormatters>");

            return exportDataFormattersXml;
        }

        /// <summary>
        /// Get Xml representation of export data formatter collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the export data formatter collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            return ToXml();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the export data formatter collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExportDataFormatterCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <ExportDataFormatters>
			        <ExportDataFormatter Id="1" Name="ExecutionType" LongName="" Type="RSXml" ExportType="MDM.ExportFormatters.RSXml.RSXmlFormatter40, RS.MDM.ExportFormatters.RSXml" DisplayName="" DiscriminatorValue="" FileExtension=""/> 
			        <ExportDataFormatter Id="2" Name="ExecutionType" LongName="" Type="RSExcel" ExportType="MDM.ExportFormatters.RSExcel.RSExcelFormatter11, RS.MDM.ExportFormatters.RSExcel" DisplayName="" DiscriminatorValue="" FileExtension=""/> 
		        </ExportDataFormatters>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportDataFormatter")
                        {
                            #region Read ExportDataFormatters Collection

                            String exportDataFormattersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(exportDataFormattersXml))
                            {
                                ExportDataFormatter exportDataFormatter = new ExportDataFormatter(exportDataFormattersXml, objectSerialization);
                                if (exportDataFormatter != null)
                                {
                                    this.Add(exportDataFormatter);
                                }
                            }

                            #endregion Read ExportDataFormatter Collection
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
