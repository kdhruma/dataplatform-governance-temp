using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Imports.Processor
{
    using MDM.Imports.Interfaces;
    /// <summary>
    /// 
    /// </summary>
    public class ComplexAttributeProcessor
    {
        #region Fields

        private String _stagingTableName = String.Empty;

        private String _attributeName = String.Empty;

        private String _attributeParentName = String.Empty;

        private Int32 _attributeId = 0;

        private Int64 _startingPk = 0;

        private Int64 _endingPk = 0;

        private Int32 _batchSize = 0;

        private IImportComplexAttribute _attributeSourceData = null;

        #endregion
        #region Constructors

        public ComplexAttributeProcessor()
        {
            _attributeSourceData = new ComplexAttributeSourceData();
        }
        #endregion

        #region Properties
        public String StagingTableName
        {
            get { return _stagingTableName; }
            set { _stagingTableName = value; }
        }

        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        public String AttributeParentName
        {
            get { return _attributeParentName; }
            set { _attributeParentName = value; }
        }

        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        public Int64 StartingPk
        {
            get { return _startingPk; }
            set { _startingPk = value; }
        }

        public Int64 EndingPk
        {
            get { return _endingPk; }
            set { _endingPk = value; }
        }

        public Int32 BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }
        #endregion

        #region Methods

        #region Public methods

        public bool Process()
        {
            bool returnStatus = true;
            long batchNumber = 0;
            string message = string.Empty;
            // Within the given boundaries, run the individual processing in batches..
            for (Int64 ctr = StartingPk; ctr <= EndingPk; )
            {
                #region Prepare for batch Process
                batchNumber++;
                long startBatchTime = DateTime.Now.Ticks;
                Int64 batchStart = ctr;
                Int64 batchEnd = ctr + BatchSize - 1;
                // make sure we dont cross the boundary..
                if (batchEnd > EndingPk)
                    batchEnd = EndingPk;
                #endregion

                BusinessObjects.Table attributeData = null;
                try
                {
                    attributeData = _attributeSourceData.GetDataBatch(StagingTableName, batchStart, batchEnd, Core.MDMCenterApplication.JobService, Core.MDMCenterModules.Import);
                }
                catch (Exception ex)
                {
                    message = string.Format("Entity processing for batch {0} and {1} failed with the following exception {2}.", batchStart, batchEnd, ex.Message);
                    break;
                }
            }
            return returnStatus;
        }
        #endregion
        #endregion

    }
}