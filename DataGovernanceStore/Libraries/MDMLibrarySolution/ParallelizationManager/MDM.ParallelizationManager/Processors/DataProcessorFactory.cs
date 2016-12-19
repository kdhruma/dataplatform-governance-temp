using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Collections.Concurrent;

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MDM.ParallelizationManager.Processors
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.ParallelizationManager.Objects;
    using MDM.ParallelizationManager.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DataProcessorFactory
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private ConcurrentDictionary<String, IDataProcessor> _dataProcessors = new ConcurrentDictionary<String, IDataProcessor>();

        /// <summary>
        /// Singleton instance of factory.
        /// </summary>
        private static DataProcessorFactory _instance = null;

        /// <summary>
        /// lock object
        /// </summary>
        private static Object lockObj = new Object();

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataProcessorFactory GetSingleton()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new DataProcessorFactory();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processorName"></param>
        /// <returns></returns>
        public static IDataProcessor CreateProcessor(CoreDataProcessorList processorName)
        {
            return CreateProcessor(processorName.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processorName"></param>
        /// <returns></returns>
        public static IDataProcessor CreateProcessor(String processorName)
        {
            Boolean successFlag = true;
            //create new data processor
            IDataProcessor dataProcessor = new DataProcessor(processorName);
            successFlag = DataProcessorFactory.AddProcessor(dataProcessor);

            return dataProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processorName"></param>
        /// <returns></returns>
        public static IDataProcessor GetProcessor(CoreDataProcessorList processorName)
        {
            return GetProcessor(processorName.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processorName"></param>
        /// <returns></returns>
        public static IDataProcessor GetProcessor(String processorName)
        {
            Boolean successFlag = true;
            IDataProcessor dataProcessor = null;

            DataProcessorFactory factory = DataProcessorFactory.GetSingleton();

            if (factory._dataProcessors.ContainsKey(processorName))
                successFlag = factory._dataProcessors.TryGetValue(processorName, out dataProcessor);
            else
                throw new KeyNotFoundException("Requested data processer with name:" + processorName + " not found or is not initialized.");

            return dataProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <returns></returns>
        public static Boolean AddProcessor(IDataProcessor dataProcessor)
        {
            DataProcessorFactory factory = DataProcessorFactory.GetSingleton();

            if (!factory._dataProcessors.ContainsKey(dataProcessor.Name))
            {
                factory._dataProcessors.TryAdd(dataProcessor.Name, dataProcessor);
            }
            else
                throw new ArgumentException("Data processor with name:" + dataProcessor.Name + " already exist.");

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <returns></returns>
        public static Boolean RemoveProcessor(IDataProcessor dataProcessor)
        {
            DataProcessorFactory factory = DataProcessorFactory.GetSingleton();

            if (factory._dataProcessors.ContainsKey(dataProcessor.Name))
            {
                factory._dataProcessors.TryRemove(dataProcessor.Name, out dataProcessor);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Boolean Shutdown()
        {
            Boolean successFlag = true;

            DataProcessorFactory factory = DataProcessorFactory.GetSingleton();

            foreach (IDataProcessor dataProcessor in factory._dataProcessors.Values)
            {
                Boolean processorSuccessFlag = false;
                processorSuccessFlag = dataProcessor.Complete();
                successFlag = successFlag & processorSuccessFlag;
            }

            return successFlag;
        }
       
        /// <summary>
        /// Clears all the processors from the facotry
        /// </summary>
        public static void ClearProcessors()
        {
            DataProcessorFactory factory = DataProcessorFactory.GetSingleton();
            factory._dataProcessors.Clear();
        }

        /// <summary>
        /// Get all the processors available in factory
        /// </summary>
        /// <returns>Data processors</returns>
        public static ConcurrentDictionary<String, IDataProcessor> GetProcessors()
        {
            DataProcessorFactory factory = DataProcessorFactory.GetSingleton();
            return factory._dataProcessors;
        }

        #endregion
    }
}