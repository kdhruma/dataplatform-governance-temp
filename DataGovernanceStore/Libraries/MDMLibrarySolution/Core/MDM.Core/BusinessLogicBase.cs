using System;
using System.Transactions;

namespace MDM.Core
{
    /// <summary>
    /// Base class for all BusinessLogic classes
    /// </summary>
    public abstract class BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Logged in user
        /// </summary>
        private String _user = String.Empty;

        /// <summary>
        /// Indicates which system called the class - Web system or Job service system
        /// </summary>
        private MDMCenterSystem _system;

        /// <summary>
        /// Field denoting locale for model display
        /// </summary>
        private LocaleEnum _modelDisplayLocale = LocaleEnum.en_WW;

        /// <summary>
        /// Field denoting locale for data formatting
        /// </summary>
        private LocaleEnum _dataFormattingLocale = LocaleEnum.en_WW;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less Constructor
        /// </summary>
        public BusinessLogicBase()
        {
        }

        /// <summary>
        /// Constructor to initialize user and system
        /// </summary>
        /// <param name="user">Logged in user</param>
        /// <param name="system">System which is calling the constructor</param>
        public BusinessLogicBase(String user, MDMCenterSystem system)
        {
            _user = user;
            _system = system;
        }

        #endregion

        #region Properties 
        
        /// <summary>
        /// User who is accessing BusinessLogic
        /// </summary>
        public String User
        {
            get
            {
                return _user;
            }
        }

        /// <summary>
        /// Indicates which system called the class. Web system or Job service system
        /// </summary>
        public MDMCenterSystem System
        {
            get
            {
                return _system;
            }
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Gets transaction options as per the configuration
        /// </summary>
        /// <param name="processingMode">Mode of the object processing which can be Sync or Async</param>
        /// <returns>Transaction Options</returns>
        public static TransactionOptions GetTransactionOptions(ProcessingMode processingMode)
        {
            TransactionOptions transactionOptions = new TransactionOptions();

            transactionOptions.IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL;

            if (processingMode == ProcessingMode.Sync)
            {
                transactionOptions.Timeout = Constants.TRANSACTION_TIMEOUT_SYNCPROCESS;
            }
            else
            {
                transactionOptions.Timeout = Constants.TRANSACTION_TIMEOUT_ASYNCPROCESS;
            }

            return transactionOptions;
        }

        /// <summary>
        /// Gets locale which needs to be used for Model display
        /// </summary>
        /// <returns></returns>
        public LocaleEnum GetModelDisplayLocale()
        {
            return _modelDisplayLocale;
        }

        /// <summary>
        /// Gets locale to be used for Data Formatting
        /// </summary>
        /// <returns></returns>
        public LocaleEnum GetDataFormattingLocale()
        {
            return _dataFormattingLocale;
        }

        /// <summary>
        /// Sets model display locale and data formatting locales
        /// </summary>
        /// <param name="modelDisplayLocale">Model display locale</param>
        /// <param name="dataFormattingLocale">Data formatting locale</param>
        protected void SetLocaleDetails(LocaleEnum modelDisplayLocale, LocaleEnum dataFormattingLocale)
        {
            this._modelDisplayLocale = modelDisplayLocale;
            this._dataFormattingLocale = dataFormattingLocale;
        }

        #endregion 
    }
}
