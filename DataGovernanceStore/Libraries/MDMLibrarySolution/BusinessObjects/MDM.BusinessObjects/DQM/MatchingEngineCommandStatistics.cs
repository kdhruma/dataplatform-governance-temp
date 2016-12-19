using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MatchingEngineCommandStatistics
    {
        private double _averageTimeTaken;
        private double _averageOverHeadTime;
        private double _averageProcessingTime;
        private String _cmdName;
        private double _duration;
        private double _maxTimeTaken;
        private double _maxOverHeadTime;
        private double _maxProcessingTime;
        private double _minTimeTaken;
        private double _minOverHeadTime;
        private double _minProcessingTime;
        private Int32 _numberOfCommands;


        /// <summary>
        /// The average time taken for these commands completed during the report time period.
        /// </summary>
        [DataMember]
        public double AverageTimeTaken
        {
            get { return _averageTimeTaken; }
            set { _averageTimeTaken = value; }
        }

        /// <summary>
        /// The average overhead time for these commands completed during the report time period.
        /// </summary>
        [DataMember]
        public double AverageOverHeadTime
        {
            get { return _averageOverHeadTime; }
            set { _averageOverHeadTime = value; }
        }


        /// <summary>
        ///The average processing time for these commands completed during the report time period.
        /// </summary>
        [DataMember]
        public double  AverageProcessingTime
        {
            get { return _averageProcessingTime; }
            set { _averageProcessingTime = value; }
        }


        /// <summary>
        /// Name of the command executed in the matching engine. 
        /// </summary>
        [DataMember]
        public String CommandName
        {
            get { return _cmdName; }
            set { _cmdName = value; }
        }

        
        /// <summary>
        /// Number of milliseconds covered.
        /// </summary>
        [DataMember]
        public double  Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        
        /// <summary>
        /// The maximum time taken for this command during the report time period.
        /// </summary>
        [DataMember]
        public double MaxTimeTaken
        {
            get { return _maxTimeTaken; }
            set { _maxTimeTaken = value; }
        }

        
        /// <summary>
        /// The maximum overhead time for this command during the report time period.
        /// </summary>
        [DataMember]
        public double MaxOverHeadTime
        {
            get { return _maxOverHeadTime; }
            set { _maxOverHeadTime = value; }
        }

      
        /// <summary>
        /// The maximum processing time for this command during the report time period.
        /// </summary>
        [DataMember]
        public double MaxProcessingTime
        {
            get { return _maxProcessingTime; }
            set { _maxProcessingTime = value; }
        }

       
        /// <summary>
        /// The minimum time taken for this command during the report time period.
        /// </summary>
        [DataMember]
        public double MinTimeTaken
        {
            get { return _minTimeTaken; }
            set { _minTimeTaken = value; }
        }

        
        /// <summary>
        /// The minimum overhead time for this command during the report time period.
        /// </summary>
        [DataMember]
        public double MinOverHeadTime
        {
            get { return _minOverHeadTime; }
            set { _minOverHeadTime = value; }
        }

        
        /// <summary>
        /// The minimum processing time for this command during the report time period.
        /// </summary>
        [DataMember]
        public double MinProcessingTime
        {
            get { return _minProcessingTime; }
            set { _minProcessingTime = value; }
        }

        
        /// <summary>
        /// The number of these commands completed during the report time period.
        /// </summary>
        [DataMember]
        public Int32 TimesCommandExecuted
        {
            get { return _numberOfCommands; }
            set { _numberOfCommands = value; }
        }

        /// <summary>
        /// Returns all the property values in XML format. 
        /// </summary>
        /// <returns>Returns XML</returns>
        public String ToXml()
        {
            var output = String.Empty;

            var xout = new XmlOutput();

            var mr = xout.Node("MatchingEngineCommandStatistics")
                .Attribute("CommandName", CommandName)
                .Attribute("MaximumTimeTaken", MaxTimeTaken.ToString())
                .Attribute("MaximumOverHeadTime", MaxOverHeadTime.ToString())
                .Attribute("MaximumProcessingTime", MaxProcessingTime.ToString())
                .Attribute("AverageTimeTaken", AverageTimeTaken.ToString())
                .Attribute("AverageOverHeadTime", AverageOverHeadTime.ToString())
                .Attribute("AverageProcessingTime", AverageProcessingTime.ToString())
                .Attribute("MinimumTimeTaken", MinTimeTaken.ToString())
                .Attribute("MinimumOverHeadTime", MinOverHeadTime.ToString())
                .Attribute("MinimumProcessingTime", MinProcessingTime.ToString())
                .Attribute("NumberOfTimesCommandExecuted", TimesCommandExecuted.ToString());

            output = mr.GetOuterXml();

            return output;
        }
    }
}
