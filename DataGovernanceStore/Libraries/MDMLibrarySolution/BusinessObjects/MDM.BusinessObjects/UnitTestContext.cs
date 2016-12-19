using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitTestContext
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<String, Object> _inputDataDictionary = null;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<String, Object> _outputDataDictionary = null;

        #endregion

        #region Constructor
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetInputData<T>(String key)
        {
            T returnVal = default(T);

            if (!String.IsNullOrWhiteSpace(key) && this._inputDataDictionary != null &&this._inputDataDictionary.ContainsKey(key))
            {
                returnVal = (T)this._inputDataDictionary[key];
            }
            else
            {
                throw new Exception("Key : '" + key + "' not found in input data dictionary.");
            }

            return returnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetOutputData<T>(String key)
        {
            T returnVal = default(T);

            if (!String.IsNullOrWhiteSpace(key) && this._outputDataDictionary != null && this._outputDataDictionary.ContainsKey(key))
            {
                returnVal = (T)this._outputDataDictionary[key];
            }
            else
            {
                throw new Exception("Key : '" + key + "' not found in output data dictionary.");
            }

            return returnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDataDictionary"></param>
        public void SetInputData(Dictionary<String, Object> inputDataDictionary)
        {
            if (inputDataDictionary != null && inputDataDictionary.Count > 0)
            {
                this._inputDataDictionary = inputDataDictionary;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputDataDictionary"></param>
        public void SetOutputData(Dictionary<String, Object> outputDataDictionary)
        {
            if (outputDataDictionary != null && outputDataDictionary.Count > 0)
            {
                this._outputDataDictionary = outputDataDictionary;
            }
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}