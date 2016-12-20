using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.CacheManager.UnitTest
{
    using MDM.CacheManager.Business;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using MDM.ParallelizationManager.Processors;
    using MDM.BusinessObjects;

    [TestClass]
    public class AppFabricCacheTest
    {
        private Int32 _cacheMissCount = 0;

        private Object _lockObj = new Object();

        private IDistributedCache _cacheManager = null;

        String _cacheKey = "TestParallelGet_SameCacheKey";

        Int32 _noOfThreads = 20;

        Int32 _totalIterations = 0;

        [TestMethod]
        public void TestParallelGet()
        {
            _cacheManager = CacheFactory.GetDistributedCache();

            Entity entity = new Entity();
            entity.Id = 1234;
            entity.Name = "Entity 1234";

            DateTime expiryTime = DateTime.Now.AddDays(365);

            _cacheManager.Set(_cacheKey, entity, expiryTime);

            Collection<Int32> ids = new Collection<Int32>();

            for (int i = 0; i < 100000;i++)
            {
                ids.Add(i);
            }

            //#region Test using parallel for with 

            //Parallel.For(0, _noOfThreads, i =>
            //    {
            //        GetCache(i);
            //    }
            //);

            //Console.WriteLine(String.Format("AFTER Parallel.For: Cache miss count is: {0}", _cacheMissCount));
            //Assert.AreEqual(_cacheMissCount, 0);

            //#endregion

            #region Test using Run In Parallel TPL based libarary

            _cacheMissCount = 0;

            ParallelTaskProcessor taskProcessor = new ParallelTaskProcessor();
            taskProcessor.RunInParallel(ids, GetCache, null, _noOfThreads);

            Console.WriteLine(String.Format("AFTER RunInParallel: Total iterations are: {0}", _totalIterations));
            Console.WriteLine(String.Format("AFTER RunInParallel: Cache miss count is: {0}", _cacheMissCount));
            
            Assert.AreEqual(_cacheMissCount, 0);

            #endregion
        }

        public void GetCache(Int32 id)
        {
            Object valueAsObj = _cacheManager.Get(_cacheKey);

            if (valueAsObj == null)
            {
                lock (_lockObj)
                {
                    _cacheMissCount++;
                }
            }
            
            lock(_lockObj)
            {
                _totalIterations++;
            }
        }
    }
}
