using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using System.Threading.Tasks;

namespace MDM.CachedDataModelManager
{
    using MDM.Interfaces;

    public sealed class CacheDataModelLoader
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion
        
        #region Properties

        #endregion

        #region Methods

        public static IAsyncResult BeginLoad(AsyncCallback callBack, object state)
        {
            Task<Boolean> task = new Task<Boolean>((obj) =>
            {
                return Load();
            },
            state);

            if (callBack != null)
            {
                task.ContinueWith((tsk) => callBack(tsk));
            }

            task.Start();

            return task;
        }

        private static Boolean Load()
        {
            Boolean successFlag = false;

            #region Load Cached Data Model

            try
            {
                //This will boot strap and load all data models into application start only.
                ICachedDataModel cachedDataModel = CachedDataModel.GetSingleton();

                if (cachedDataModel != null)
                    successFlag = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
            }

            #endregion

            return successFlag;
        }

        public static Boolean EndLoad(IAsyncResult result)
        {
            Boolean data = false;

            Task<Boolean> task = result as Task<Boolean>;

            if (task != null)
            {
                data = task.Result;
            }

            return data;
        }

        #endregion

    }
}
