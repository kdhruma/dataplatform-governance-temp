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
    public class ComplexAttributeSourceData : IImportComplexAttribute
    {
        #region Fields
        Int64 _count = 0;

        Int64 _seed = 0;

        Int64 _endpoint = 0;

        Int32 _batchsize = 100;

        #endregion

        #region Properties
        public Int64 Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public Int64 Seed
        {
            get { return _seed; }
            set { _seed = value; }
        }

        public Int64 Endpoint
        {
            get { return _endpoint; }
            set { _endpoint = value; }
        }

        public Int32 Batchsize
        {
            get { return _batchsize; }
            set { _batchsize = value; }
        }
        #endregion
        #region IImportComplexAttribute Methods
        public Int64 GetDataCount()
        {
            return Count;
        }

        public Int64 GetDataSeed()
        {
            return Seed;
        }

        public Int64 GetEndPoint()
        {
            return Endpoint;
        }

        public Int32 GetDataBatchSize()
        {
            return Batchsize;
        }

        public BusinessObjects.Table GetDataBatch(String staginTableName, Int64 startPK, Int64 endPK, Core.MDMCenterApplication application, Core.MDMCenterModules module)
        {
            BusinessObjects.Table attributeData = new BusinessObjects.Table();
            return attributeData;
        }
        #endregion
    }
}
