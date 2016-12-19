using System;
using System.Diagnostics;
using System.IO;
using System.Globalization;

namespace MDM.DiagnosticManager.TraceListeners
{
    /// <summary>
    /// Trace Listener that writes circular rolling logs depending on the number of files configured
    /// </summary>
    public class RollingTraceListener : XmlWriterTraceListener
    {
        #region Fields

        private static RollingStream _stream = null;
        private bool _maxQuotaInitialized = false;
        private const string FileQuotaAttribute = "maxFileSizeMB";
        private const long DefaultMaxQuota = 8;
        private const string DefaultTraceFile = "MDMCenterTraces.svclog";

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor that gets called when we specify a filename in initializeData
        /// </summary>
        /// <param name="file"></param>
        public RollingTraceListener(string file)
            : base(_stream = new RollingStream(file))
        {
        }

        /// <summary>
        /// The constructor that gets called when no filename is specified
        /// </summary>
        public RollingTraceListener()
            : base(_stream = new RollingStream(DefaultTraceFile))
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The maximum Quota Size for the file. The default is 8 MB
        /// </summary>
        private long MaxQuotaSize
        {
            //Get the MaxQuotaSize from configuration file
            //Set to Default Value if there are any problems

            get
            {
                long MaxFileQuota = 0;
                if (!this._maxQuotaInitialized)
                {
                    try
                    {
                        string MaxQuotaOption = this.Attributes[RollingTraceListener.FileQuotaAttribute];
                        if (MaxQuotaOption == null)
                        {
                            MaxFileQuota = DefaultMaxQuota;
                        }
                        else
                        {
                            MaxFileQuota = int.Parse(MaxQuotaOption, CultureInfo.InvariantCulture);
                        }
                    }
                    catch (Exception)
                    {
                        MaxFileQuota = DefaultMaxQuota;
                    }
                    finally
                    {
                        this._maxQuotaInitialized = true;
                    }
                }

                if (MaxFileQuota <= 0)
                {
                    MaxFileQuota = DefaultMaxQuota;
                }

                //MaxFileQuota is in MB in the configuration file, convert to bytes

                MaxFileQuota = MaxFileQuota * 1024 * 1024;
                return MaxFileQuota;
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// To check if we have reached the quota
        /// </summary>
        private void DetermineOverQuota()
        {

            //Set the MaxQuota on the stream if it hasn't been done

            if (!this._maxQuotaInitialized)
            {
                _stream.MaxQuotaSize = this.MaxQuotaSize;
            }

            //If we're past the Quota, flush, then take next file

            if (_stream.IsOverQuota)
            {
                base.Flush();
                _stream.NextFile();
            }
        }

        #region XmlWriterTraceListener methods that needs to be overridden
        /*
         * 
         * Override all methods and make sure we check the size before writing
         * 
         * */
        /// <summary>
        /// To read xml attribute values configured along with the configurations
        /// </summary>
        /// <returns>maxFileSizeMB settings</returns>
        protected override string[] GetSupportedAttributes()
        {
            return new string[] { RollingTraceListener.FileQuotaAttribute };
        }


        /// <summary>
        /// Refer to the XmlWriterTraceListener documentation
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            DetermineOverQuota();
            base.TraceData(eventCache, source, eventType, id, data);
        }


        /// <summary>
        /// Refer to the XmlWriterTraceListener documentation 
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            DetermineOverQuota();
            base.TraceEvent(eventCache, source, eventType, id);
        }

        /// <summary>
        /// Refer to the XmlWriterTraceListener documentation
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            DetermineOverQuota();
            base.TraceData(eventCache, source, eventType, id, data);
        }

        /// <summary>
        /// Refer to the XmlWriterTraceListener documentation
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            DetermineOverQuota();
            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }


        /// <summary>
        /// Refer to the XmlWriterTraceListener documentation
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            DetermineOverQuota();
            base.TraceEvent(eventCache, source, eventType, id, message);
        }


        /// <summary>
        /// Refer to the XmlWriterTraceListener documentation
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="relatedActivityId"></param>
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            DetermineOverQuota();
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);

        }
        #endregion

        #endregion

    }
} 
