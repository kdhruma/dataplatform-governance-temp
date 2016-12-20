using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Represents schedule criteria class used for scheduling options selected associated with a job
    /// </summary>
    [DataContract]
    public class ScheduleCriteria : MDMObject, IScheduleCriteria
    {
        #region Fields

        /// <summary>
        /// The underlying field supporting the <see cref="RecurrenceFrequency"/> property.
        /// </summary>
        private RecurrenceFrequencyOptions _recurrenceFrequency = RecurrenceFrequencyOptions.Daily;

        /// <summary>
        /// The underlying field supporting the <see cref="RecurrenceDelay"/> property.
        /// </summary>
        private Int32 _recurrenceDelay = 1;

        /// <summary>
        /// The underlying field supporting the <see cref="DaysOfWeek"/> property.
        /// </summary>
        private Collection<DayOfWeek> _daysOfWeek = new Collection<DayOfWeek>();

        /// <summary>
        /// The underlying field supporting the <see cref="MonthlyFrequency"/> property.
        /// </summary>
        private MonthlyFrequencyOptions _monthlyFrequency = MonthlyFrequencyOptions.Daily;

        /// <summary>
        /// The underlying field supporting the <see cref="DayOfMonth"/> property.
        /// </summary>
        private Int32 _dayOfMonth = 1; // 1..31

        /// <summary>
        /// The underlying field supporting the <see cref="WeekOfMonth"/> property.
        /// </summary>
        private Int32 _weekOfMonth = 1; // 1..5 where 5 = last week of month

        /// <summary>
        /// The underlying field supporting the <see cref="DayOfWeek"/> property.
        /// </summary>
        private DayOfWeek _dayOfWeek = DayOfWeek.Sunday;

        /// <summary>
        /// The underlying field supporting the <see cref="DailyFrequency"/> property.
        /// </summary>
        private DailyFrequencyOptions _dailyFrequency = DailyFrequencyOptions.Once;

        /// <summary>
        /// The underlying field supporting the <see cref="DailyDelay"/> property.
        /// </summary>
        private Int32 _dailyDelay = 1; // 1..24 hours, or 1.. minutes

        /// <summary>
        /// The underlying field supporting the <see cref="StartTime"/> property.
        /// </summary>
        private TimeSpan _startTime = new TimeSpan(0, 0, 0); // Hour and Minutes past midnight

        /// <summary>
        /// The underlying field supporting the <see cref="EndTime"/> property.
        /// </summary>
        private TimeSpan _endTime = new TimeSpan(TimeSpan.TicksPerDay); // Hour and Minutes past midnight

        /// <summary>
        /// The underlying field supporting the <see cref="EffectiveStartDate"/> property.
        /// </summary>
        private DateTime _effectiveStartDate = DateTime.MinValue;

        /// <summary>
        /// The underlying field supporting the <see cref="EffectiveEndDate"/> property.
        /// </summary>
        private DateTime _effectiveEndDate = DateTime.MaxValue;

        /// <summary>
        /// Represents the first occurrence that this will execute or has executed
        /// </summary>
        /// <remarks>
        /// Never access this field directly.  The <see cref="FirstOccurrence"/> property will calculate the value
        /// once and save time on subsequent accesses.
        /// </remarks>
        private DateTime _firstOccurrence = DateTime.MinValue;

        /// <summary>
        /// Indicates Id and name of profile(s) to execute for current schedule
        /// </summary>
        private Collection<KeyValuePair<Int32, String>> _profileIdNamePair = new Collection<KeyValuePair<Int32, String>>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value representing the frequency of job execution.
        /// </summary>
        /// <value>
        /// Valid values include
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="RecurrenceFrequencyOptions.Daily"/></term>
        /// <description>To be executed every so many days.</description>
        /// </item>
        /// <item>
        /// <term><see cref="RecurrenceFrequencyOptions.Weekly"/></term>
        /// <description>To be executed every so many weeks.</description>
        /// </item>
        /// <item>
        /// <term><see cref="RecurrenceFrequencyOptions.Monthly"/></term>
        /// <description>To be executed every so many months.</description>
        /// </item>
        /// </list>
        /// </value>
        /// <remarks>
        /// The default value is <b>RecurrenceFrequency.Daily</b>.
        /// </remarks>
        [DataMember]
        public RecurrenceFrequencyOptions RecurrenceFrequency
        {
            get
            {
                return _recurrenceFrequency;
            }
            set
            {
                _recurrenceFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the delay between executions.
        /// </summary>
        /// <remarks>
        /// The default value is 1.
        /// </remarks>
        [DataMember]
        public Int32 RecurrenceDelay
        {
            get
            {
                return _recurrenceDelay;
            }
            set
            {
                _recurrenceDelay = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the days of the week a job should
        /// be executed on a weekly basis.
        /// </summary>
        /// <remarks>
        /// The default value is Sunday only, but is ignored.  The value of this property
        /// is only used when the <see cref="RecurrenceFrequency"/> is set to 
        /// <see cref="RecurrenceFrequencyOptions.Weekly"/>.
        /// </remarks>
        [DataMember]
        public Collection<DayOfWeek> DaysOfWeek
        {
            get
            {
                return _daysOfWeek;
            }
            set
            {
                if (_daysOfWeek != null)
                {
                    _daysOfWeek.Clear();
                }
                else
                {
                    _daysOfWeek = new Collection<DayOfWeek>();
                }

                if (value != null)
                {
                    foreach (DayOfWeek day in value)
                    {
                        _daysOfWeek.Add(day);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value representing the method used to determine day of the month
        /// a job should be executed.
        /// </summary>
        /// <value>
        /// Valid values include
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="MonthlyFrequencyOptions.Daily"/></term>
        /// <description>To be executed on a specific day of the month.</description>
        /// </item>
        /// <item>
        /// <term><see cref="MonthlyFrequencyOptions.Weekly"/></term>
        /// <description>To be executed on a certain occurence of a particular weekday.</description>
        /// </item>
        /// </list>
        /// </value>
        /// <remarks>
        /// The default value is <b>MonthlyFrequencyOptions.Daily</b> but is ignored.  
        /// The value of this property is only used when the <see cref="RecurrenceFrequency"/>
        /// is set to <see cref="RecurrenceFrequencyOptions.Monthly"/>.
        /// </remarks>
        [DataMember]
        public MonthlyFrequencyOptions MonthlyFrequency
        {
            get
            {
                return _monthlyFrequency;
            }
            set
            {
                _monthlyFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the day of the month a job should be executed.
        /// </summary>
        /// <value>Any value between 1 and 31 is valid.  Internally if the last day of
        /// the month is less than the value of this property, the last day of the month
        /// will be used.</value>
        /// <remarks>
        /// The default value is <b>MonthlyFrequencyOptions.Daily</b> but is ignored.  
        /// The value of this property is only used when the <see cref="RecurrenceFrequency"/>
        /// is set to <see cref="RecurrenceFrequencyOptions.Monthly"/> and the
        /// <see cref="MonthlyFrequency"/> is set to <see cref="MonthlyFrequencyOptions.Daily"/>.
        /// </remarks>
        [DataMember]
        public Int32 DayOfMonth
        {
            get
            {
                return _dayOfMonth;
            }
            set
            {
                _dayOfMonth = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the specific occurrence of a specified day of the week
        ///  a job should be executed.
        /// </summary>
        /// <value>
        /// 1 through 4 represents the 1st through 4th occurrences within the month.
        /// 5 represents the last occurrence within the month.
        /// </value>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="DayOfWeek"/> property. <br/>
        /// The default value is <b>MonthlyFrequencyOptions.Daily</b> but is ignored.  
        /// The value of this property is only used when the <see cref="RecurrenceFrequency"/>
        /// is set to <see cref="RecurrenceFrequencyOptions.Monthly"/> and the
        /// <see cref="MonthlyFrequency"/> is set to <see cref="MonthlyFrequencyOptions.Daily"/>.
        /// </remarks>
        [DataMember]
        public Int32 WeekOfMonth
        {
            get
            {
                return _weekOfMonth;
            }
            set
            {
                _weekOfMonth = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the specific day of the week a job should be executed.
        /// </summary>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="WeekOfMonth"/> property. <br/>
        /// The default value is <b>MonthlyFrequencyOptions.Daily</b> but is ignored.  
        /// The value of this property is only used when the <see cref="RecurrenceFrequency"/>
        /// is set to <see cref="RecurrenceFrequencyOptions.Monthly"/> and the
        /// <see cref="MonthlyFrequency"/> is set to <see cref="MonthlyFrequencyOptions.Daily"/>.
        /// </remarks>
        [DataMember]
        public DayOfWeek DayOfWeek
        {
            get
            {
                return _dayOfWeek;
            }
            set
            {
                _dayOfWeek = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the frequency of job execution within a given day
        /// </summary>
        /// <value>
        /// Valid values include
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="DailyFrequencyOptions.Once"/></term>
        /// <description>To be executed at a specified time of day.</description>
        /// </item>
        /// <item>
        /// <term><see cref="DailyFrequencyOptions.Hourly"/></term>
        /// <description>To be executed every so many hours.</description>
        /// </item>
        /// <item>
        /// <term><see cref="DailyFrequencyOptions.EveryMinute"/></term>
        /// <description>To be executed every so many minutes.</description>
        /// </item>
        /// </list>
        /// </value>
        /// <remarks>
        /// The default value of this property is <b>DailyFrequencyOptions.Once</b>.
        /// </remarks>
        [DataMember]
        public DailyFrequencyOptions DailyFrequency
        {
            get
            {
                return _dailyFrequency;
            }
            set
            {
                _dailyFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the delay between executions within a given day.
        /// </summary>
        /// <value>
        /// If the <see cref="DailyFrequency"/> property is set to <b>Hourly</b>, then the value of
        /// this property represents the number of hours between executions.  If the <b>DailyFrequency</b>
        /// property is set to <b>EveryMinute</b>, then the value of this property represents the
        /// number of minutes between executions.  If the <b>DailyFrequency</b> property is set to
        /// <b>Once</b>, then the value of this property is ignored.
        /// </value>
        /// <remarks>
        /// The default value of this property is 1.
        /// </remarks>
        [DataMember]
        public Int32 DailyDelay
        {
            get
            {
                return _dailyDelay;
            }
            set
            {
                _dailyDelay = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing the first time a job should be executed in a given day.
        /// </summary>
        /// <remarks>
        /// The default value of this property is midnight.
        /// </remarks>
        [DataMember]
        public string StartTime
        {
            get
            {
                return _startTime.ToString();
            }
            set
            {
                _startTime = TimeSpan.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets a value representing the time after which no executions should occur.
        /// </summary>
        /// <remarks>
        /// The default value of this property is TimeSpan.TicksPerDay.  The value of this property
        /// is ignored if <see cref="DailyFrequency"/> is set to <b>Once</b>.
        /// </remarks>
        [DataMember]
        public string EndTime
        {
            get
            {
                return _endTime.ToString();
            }
            set
            {
                _endTime = TimeSpan.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets a value representing a date before which no executions should occur.
        /// </summary>
        /// <remarks>
        /// The default value of this property is DateTime.Today.
        /// </remarks>
        [DataMember]
        public DateTime EffectiveStartDate
        {
            get
            {
                return _effectiveStartDate;
            }
            set
            {
                _effectiveStartDate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value representing a date after which no executions should occur.
        /// </summary>
        /// <value>
        /// The value of this property may be any DateTime on or after <see cref="EffectiveStartDate"/>.  
        /// A value of DateTime.MaxValue represents no end date.
        /// </value>
        /// <remarks>
        /// The default value of this property is DateTime.MaxValue.
        /// </remarks>
        [DataMember]
        public DateTime EffectiveEndDate
        {
            get
            {
                return _effectiveEndDate;
            }
            set
            {
                _effectiveEndDate = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Necessary for Serialization support
        /// </summary>
        public ScheduleCriteria() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleCriteria"/> class.
        /// </summary>
        /// <param name="recurrenceFrequency">value for the <see cref="RecurrenceFrequency"/> property</param>
        /// <param name="recurenceDelay">value for the <see cref="RecurrenceDelay"/> property</param>
        /// <param name="sunday">value for the <see cref="DaysOfWeek"/> property</param>
        /// <param name="monday">value for the <see cref="DaysOfWeek"/> property</param>
        /// <param name="tuesday">value for the <see cref="DaysOfWeek"/> property</param>
        /// <param name="wednesday">value for the <see cref="DaysOfWeek"/> property</param>
        /// <param name="thursday">value for the <see cref="DaysOfWeek"/> property</param>
        /// <param name="friday">value for the <see cref="DaysOfWeek"/> property</param>
        /// <param name="saturday">value for the <see cref="DaysOfWeek"/> property</param>
        /// <param name="monthlyFrequency">value for the <see cref="MonthlyFrequency"/> property</param>
        /// <param name="dayOfMonth">value for the <see cref="DayOfMonth"/> property</param>
        /// <param name="weekOfMonth">value for the <see cref="WeekOfMonth"/> property</param>
        /// <param name="dayOfWeek">value for the <see cref="DayOfWeek"/> property</param>
        /// <param name="dailyFrequency">value for the <see cref="DailyFrequency"/> property</param>
        /// <param name="dailyDelay">value for the <see cref="DailyDelay"/> property</param>
        /// <param name="startTime">value for the <see cref="StartTime"/> property</param>
        /// <param name="endTime">value for the <see cref="EndTime"/> property</param>
        /// <param name="effectiveStartDate">value for the <see cref="EffectiveStartDate"/> property</param>
        /// <param name="effectiveEndDate">value for the <see cref="EffectiveEndDate"/> property</param>
        public ScheduleCriteria(
            RecurrenceFrequencyOptions recurrenceFrequency,
            Int32 recurenceDelay,
            Boolean sunday,
            Boolean monday,
            Boolean tuesday,
            Boolean wednesday,
            Boolean thursday,
            Boolean friday,
            Boolean saturday,
            MonthlyFrequencyOptions monthlyFrequency,
            Int32 dayOfMonth,
            Int32 weekOfMonth,
            DayOfWeek dayOfWeek,
            DailyFrequencyOptions dailyFrequency,
            Int32 dailyDelay,
            TimeSpan startTime,
            TimeSpan endTime,
            DateTime effectiveStartDate,
            DateTime effectiveEndDate)
        {
            this._recurrenceFrequency = recurrenceFrequency;
            this._recurrenceDelay = recurenceDelay;
            this._daysOfWeek = new Collection<DayOfWeek>();
            if (sunday)
                this._daysOfWeek.Add(DayOfWeek.Sunday);
            if (monday)
                this._daysOfWeek.Add(DayOfWeek.Monday);
            if (tuesday)
                this._daysOfWeek.Add(DayOfWeek.Tuesday);
            if (wednesday)
                this._daysOfWeek.Add(DayOfWeek.Wednesday);
            if (thursday)
                this._daysOfWeek.Add(DayOfWeek.Thursday);
            if (friday)
                this._daysOfWeek.Add(DayOfWeek.Friday);
            if (saturday)
                this._daysOfWeek.Add(DayOfWeek.Saturday);
            this._monthlyFrequency = monthlyFrequency;
            this._dayOfMonth = dayOfMonth;
            this._weekOfMonth = weekOfMonth;
            this._dayOfWeek = dayOfWeek;
            this._dailyFrequency = dailyFrequency;
            this._dailyDelay = dailyDelay;
            this._startTime = startTime;
            this._endTime = endTime;
            this._effectiveStartDate = effectiveStartDate;
            this._effectiveEndDate = effectiveEndDate;
        }

        /// <summary>
        /// Load the schedule criteria object from xml
        /// </summary>
        /// <param name="valuesAsXml">Xml representation of schedule criteria object</param>
        public ScheduleCriteria(String valuesAsXml)
        {
            LoadScheduleCriteria(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets a value representing the date and time of the first occurrence within this schedule.
        /// </summary>
        public DateTime FirstOccurrence()
        {
            if (_firstOccurrence != DateTime.MinValue)
                return _firstOccurrence;

            DateTime firstDate = _effectiveStartDate.Date;
            TimeSpan firstTime = GetNextTimeOfOccurrence(new TimeSpan(0));

            switch (_recurrenceFrequency)
            {
                case RecurrenceFrequencyOptions.Daily:
                    // nothing to change The firstDate is the EffectiveStartDate
                    break;
                case RecurrenceFrequencyOptions.Monthly:
                    {
                        switch (_monthlyFrequency)
                        {
                            case MonthlyFrequencyOptions.Daily:
                                // Go to the correct month
                                if (firstDate.Day > _dayOfMonth)
                                {
                                    firstDate = firstDate.AddMonths(1);
                                    firstDate = firstDate.AddDays(1 - firstDate.Day);
                                }

                                // Go to the correct day
                                Int32 targetDay = Math.Min(DateTime.DaysInMonth(firstDate.Year, firstDate.Month), _dayOfMonth);
                                if (firstDate.Day < targetDay)
                                {
                                    firstDate = firstDate.AddDays(targetDay - firstDate.Day);
                                }
                                break;

                            case MonthlyFrequencyOptions.Weekly:
                                firstDate = GetWeekdayOfMonth(firstDate);
                                // Now that we have a target date.. verify that it is after the effective start date...
                                if (firstDate < _effectiveStartDate)
                                {
                                    firstDate = GetWeekdayOfMonth(firstDate.AddMonths(1));
                                }

                                break;
                        }
                        break;
                    }
                case RecurrenceFrequencyOptions.Weekly:
                    // find the first day starting from the effective date that is in the DaysOfWeek list
                    while (!_daysOfWeek.Contains(firstDate.DayOfWeek))
                        firstDate = firstDate.AddDays(1);
                    break;
            }

            _firstOccurrence = firstDate + firstTime;
            return _firstOccurrence;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ScheduleCriteria"/> class with values present in
        /// an Xml document.
        /// </summary>
        /// <param name="xmlText">The Xml text to be used for the properties of <b>ScheduleCriteria</b></param>
        /// <returns>The new instance of <b>ScheduleCriteria</b></returns>
        public static ScheduleCriteria CreateFromXml(string xmlText)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScheduleCriteria));
            StringReader reader = new StringReader(xmlText);
            return ( ScheduleCriteria )serializer.Deserialize(reader);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ScheduleCriteria"/> class with values present in
        /// an Xml document.
        /// </summary>
        /// <param name="filename">The path and name of the Xml file to be used for the properties 
        /// of <b>ScheduleCriteria</b></param>
        /// <returns>The new instance of <b>ScheduleCriteria</b></returns>
        public static ScheduleCriteria CreateFromXmlFile(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScheduleCriteria));
            FileStream file = System.IO.File.OpenRead(filename);
            return ( ScheduleCriteria )serializer.Deserialize(file);
        }

        /// <summary>
        /// Serializes this instance of <see cref="ScheduleCriteria"/> Int32o Xml text.
        /// </summary>
        /// <returns>The Xml text representation of the <b>ScheduleCriteria</b> instance.</returns>
        public string GenerateXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScheduleCriteria));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.ToString();
        }

        /// <summary>
        /// Serializes this instance of <see cref="ScheduleCriteria"/> to an Xml file.
        /// </summary>
        /// <param name="filename">The path and name of the Xml file to be created with the
        /// <b>ScheduleCriteria</b> instance information.</param>
        public void GenerateXml(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScheduleCriteria));
            FileStream file = System.IO.File.Create(filename);
            try
            {
                serializer.Serialize(file, this);
            }
            finally
            {
                file.Close();
            }
        }

        /// <summary>
        /// Gets the date and time of the next occurrence for this schedule.
        /// </summary>
        /// <param name="seed">The date and time on or after which the next occurrence will be calculated.</param>
        /// <remarks>If the <b>seed</b> represents a valid occurrence, then that value will be returned.</remarks>
        /// <returns>The date and time of the next occurrence.</returns>
        public DateTime GetNextOccurrence(DateTime seed)
        {
            // If the Schedule Criteria has expired there is no next occurrence
            if (seed > _effectiveEndDate)
                return DateTime.MinValue;

            // If the seed is before the first occurrence... the "next" will be the first.
            if (seed < FirstOccurrence())
                return FirstOccurrence();

            // First get the new time..
            seed = seed.Date + GetNextTimeOfOccurrence(seed.TimeOfDay);

            // Now get the new date..
            switch (_recurrenceFrequency)
            {
                case RecurrenceFrequencyOptions.Daily:
                    seed = GetNextDailyOccurrence(seed);
                    break;
                case RecurrenceFrequencyOptions.Monthly:
                    seed = GetNextMonthlyOccurrence(seed);
                    break;
                case RecurrenceFrequencyOptions.Weekly:
                    seed = GetNextWeeklyOccurrence(seed);
                    break;
            }
            // if the result is past the EffectiveEndDate, then there are no more valid occurrences of this schedule.
            if (seed.Date > _effectiveEndDate)
                return DateTime.MinValue;
            else
                return seed;
        }

        /// <summary>
        /// Gets the date and time of the next occurrence on or after DateTime.Now.
        /// </summary>
        /// <returns>The date and time of the next occurrence.</returns>
        public DateTime GetNextOccurrence()
        {
            return GetNextOccurrence(DateTime.Now);
        }

        /// <summary>
        /// Convert to XML
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            String scheduleCriteriaXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("ScheduleCriteria");

            #region Write Schedule criteria properties

            xmlWriter.WriteAttributeString("RecurrenceFrequency", this.RecurrenceFrequency.ToString());
            xmlWriter.WriteAttributeString("RecurrenceDelay", this.RecurrenceDelay.ToString());
            xmlWriter.WriteAttributeString("MonthlyFrequency", this.MonthlyFrequency.ToString());
            xmlWriter.WriteAttributeString("DayOfMonth", this.DayOfMonth.ToString());
            xmlWriter.WriteAttributeString("WeekOfMonth", this.WeekOfMonth.ToString());
            xmlWriter.WriteAttributeString("DayOfWeek", this.DayOfWeek.ToString());
            xmlWriter.WriteAttributeString("DailyFrequency", this.DailyFrequency.ToString());
            xmlWriter.WriteAttributeString("DailyDelay", this.DailyDelay.ToString());
            xmlWriter.WriteAttributeString("StartTime", this.StartTime);
            xmlWriter.WriteAttributeString("EndTime", this.EndTime);
            // Always save the dates in en-US format and convert while reading the XML
            String formattedDate = FormatHelper.FormatDate(this.EffectiveStartDate,LocaleEnum.en_US.GetCultureName());
            xmlWriter.WriteAttributeString("EffectiveStartDate", formattedDate);
            formattedDate = FormatHelper.FormatDate(this.EffectiveEndDate,LocaleEnum.en_US.GetCultureName());
            xmlWriter.WriteAttributeString("EffectiveEndDate", formattedDate);
            
            if (DaysOfWeek != null)
            {
                //Start days of week node
                xmlWriter.WriteStartElement("DaysOfWeek");

                foreach (DayOfWeek day in this.DaysOfWeek)
                {
                    xmlWriter.WriteElementString("DayOfWeek", day.ToString());
                }

                //End days of week node
                xmlWriter.WriteEndElement();
            }

            #endregion Write Schedule criteria  properties

            //Schedule criteria node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            scheduleCriteriaXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return scheduleCriteriaXml;
        }

        /// <summary>
        /// Represents current object in xml format
        /// </summary>
        /// <param name="serializationOption">Option for serialization type.</param>
        /// <returns></returns>
        public override String ToXml(ObjectSerialization serializationOption)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Create a clone of this object and copy over the properties
        /// </summary>
        /// <returns>Returns the cloned object</returns>
        public IScheduleCriteria Clone()
        {
            ScheduleCriteria clonedCriteria = new ScheduleCriteria();
            clonedCriteria.Action = this.Action;
            clonedCriteria.DailyDelay = this.DailyDelay;
            clonedCriteria.DailyFrequency = this.DailyFrequency;
            clonedCriteria.DayOfMonth = this.DayOfMonth;
            clonedCriteria.DayOfWeek = this.DayOfWeek;
            clonedCriteria.DaysOfWeek = this.DaysOfWeek;
            clonedCriteria.EffectiveEndDate = this.EffectiveEndDate;
            clonedCriteria.EffectiveStartDate = this.EffectiveStartDate;
            clonedCriteria.EndTime = this.EndTime;
            clonedCriteria.Id = this.Id;
            clonedCriteria.MonthlyFrequency = this.MonthlyFrequency;
            clonedCriteria.RecurrenceDelay = this.RecurrenceDelay;
            clonedCriteria.RecurrenceFrequency = this.RecurrenceFrequency;
            clonedCriteria.StartTime = this.StartTime;
            clonedCriteria.WeekOfMonth = this.WeekOfMonth;

            return clonedCriteria;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load schedule criteria object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        private void LoadScheduleCriteria(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ScheduleCriteria" && reader.IsStartElement())
                        {
                            //Read job schedule metadata
                            #region Read job schedule Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("RecurrenceFrequency"))
                                {
                                    RecurrenceFrequencyOptions option = RecurrenceFrequencyOptions.Daily;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out option))
                                    {
                                        this.RecurrenceFrequency = option;
                                    }
                                }

                                if (reader.MoveToAttribute("RecurrenceDelay"))
                                    this.RecurrenceDelay = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), _recurrenceDelay);

                                if (reader.MoveToAttribute("MonthlyFrequency"))
                                {
                                    {
                                        MonthlyFrequencyOptions option = MonthlyFrequencyOptions.Daily;
                                        if (Enum.TryParse(reader.ReadContentAsString(), out option))
                                        {
                                            this.MonthlyFrequency = option;
                                        }
                                    }
                                }

                                if (reader.MoveToAttribute("DayOfMonth"))
                                    this.DayOfMonth = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), _dayOfMonth);

                                if (reader.MoveToAttribute("WeekOfMonth"))
                                    this.WeekOfMonth = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), _weekOfMonth);

                                if (reader.MoveToAttribute("DayOfWeek"))
                                {
                                    {
                                        System.DayOfWeek option = System.DayOfWeek.Sunday;
                                        if (Enum.TryParse(reader.ReadContentAsString(), out _dayOfWeek))
                                        {
                                            this.DayOfWeek = option;
                                        }
                                    }
                                }

                                if (reader.MoveToAttribute("DailyFrequency"))
                                {
                                    DailyFrequencyOptions option = DailyFrequencyOptions.Once;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out option))
                                    {
                                        this.DailyFrequency = option;
                                    }
                                }

                                if (reader.MoveToAttribute("DailyDelay"))
                                    this.DailyDelay = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), _dailyDelay);

                                if (reader.MoveToAttribute("StartTime"))
                                    this.StartTime = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("EndTime"))
                                    this.EndTime = reader.ReadContentAsString();

                                // Ensure the datetime is always saved in en-US. In this flow it is done by the XMLSerializer and by ToXml()
                                if (reader.MoveToAttribute("EffectiveStartDate"))
                                {
                                   String formattedStartDate= FormatHelper.FormatDate(reader.ReadContentAsString(), LocaleEnum.en_US.GetCultureName(), Thread.CurrentThread.CurrentCulture.Name);
                                   this.EffectiveStartDate = ValueTypeHelper.ConvertToDateTime(formattedStartDate);
                                }

                                if (reader.MoveToAttribute("EffectiveEndDate"))
                                {
                                    String formattedEndDate = FormatHelper.FormatDate(reader.ReadContentAsString(), LocaleEnum.en_US.GetCultureName(), Thread.CurrentThread.CurrentCulture.Name);
                                    this.EffectiveEndDate = ValueTypeHelper.ConvertToDateTime(formattedEndDate);
                                }   
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DaysOfWeek")
                        {
                            String daysXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(daysXml))
                            {
                                ReadDaysOfWeek(daysXml);
                            }
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

        private void ReadDaysOfWeek(string daysXml)
        {
            if (!String.IsNullOrWhiteSpace(daysXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(daysXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DayOfWeek" )
                        {
                            System.DayOfWeek day = System.DayOfWeek.Sunday;
                            if (Enum.TryParse(reader.ReadElementContentAsString(), out day))
                            {
                                this.DaysOfWeek.Add(day);
                            }
                        }
                        reader.Read();
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Gets the first day of a week for a particular month.
        /// </summary>
        /// <param name="dow">The day of the week representing the "first" of the week.</param>
        /// <param name="date">The date representing the month in question.</param>
        /// <remarks>
        /// Any day of the week can represent the first of the week.  This method returns the day of the
        /// month that represents the first occurrence of a particular day of the week. 
        /// </remarks>
        /// <returns>A numeric representation of the day of the month.</returns>
        private Int32 FirstDayOfWeek(DayOfWeek dow, DateTime date)
        {
            return ( date.Day + 7 + dow - date.DayOfWeek ) % 7;
        }

        /// <summary>
        /// Gets the specific occurrence of a day of the week for a particular month.
        /// </summary>
        /// <param name="date">The date representing the month in question.</param>
        /// <returns>The date representing the specified occurence.</returns>
        private DateTime GetWeekdayOfMonth(DateTime date)
        {
            if (_weekOfMonth < 5)
            {
                Int32 firstDay = FirstDayOfWeek(_dayOfWeek, date);
                date = date.AddDays(firstDay - date.Day);
                date = date.AddDays(7 * ( _weekOfMonth - 1 ));
            }
            else
            {
                // lastDay is first of next month - 7 days
                date = date.AddMonths(1);
                Int32 firstDay = FirstDayOfWeek(_dayOfWeek, date);
                date = date.AddDays(firstDay - date.Day - 7);
            }
            return date;
        }

        /// <summary>
        /// Gets the next time on or after the <b>seed</b> that a job should be executed.
        /// </summary>
        /// <param name="seed">The time on or after the return value should be.</param>
        /// <returns>The value representing the next time a job should be executed.</returns>
        protected TimeSpan GetNextTimeOfOccurrence(TimeSpan seed)
        {
            Int32 seedHour = seed.Hours, seedMin = seed.Minutes, seedDay = seed.Days;

            // if prior to start time
            if (seed.TotalHours % 24 < _startTime.TotalHours)
                return _startTime + TimeSpan.FromDays(seedDay);

            // otherwise...
            switch (_dailyFrequency)
            {
                case DailyFrequencyOptions.Once:
                    if (seed.TotalHours % 24 > _startTime.TotalHours)
                        seedDay++;
                    seed = _startTime + TimeSpan.FromDays(seedDay);
                    break;
                case DailyFrequencyOptions.Hourly:
                    // Correct the minutes
                    if (seedMin > _startTime.Minutes)
                        seedHour++;
                    seedMin = _startTime.Minutes;
                    // Go to the next proper hour
                    Int32 hourDiff = GetRecurrenceDiff(( Int32 )( seedHour - _startTime.Hours ), _dailyDelay);
                    seedHour += hourDiff;
                    if (seedHour > 23)
                    {
                        seedDay++;
                        seedHour = _startTime.Hours;
                    }
                    seed = new TimeSpan(seedDay, seedHour, seedMin, 0);
                    break;
                case DailyFrequencyOptions.EveryMinute:
                    // Go to the next proper minute
                    Int32 minuteDiff = GetRecurrenceDiff(( Int32 )( seed.TotalMinutes - _startTime.TotalMinutes ), _dailyDelay);
                    seed = new TimeSpan(seed.Days, seed.Hours, seed.Minutes + minuteDiff, 0);
                    break;
            }
            // if we have gone past the end time, then go to start time of the next day.
            if (seed.TotalHours % 24 > _endTime.TotalHours)
                seed = _startTime + TimeSpan.FromDays(1 + seed.Days);
            // if prior to start time
            if (seed.TotalHours % 24 < _startTime.TotalHours)
                seed = _startTime + TimeSpan.FromDays(seed.Days);

            return seed;
        }

        /// <summary>
        /// Gets the value remaining between the <b>seed</b> and the next multiple of <b>delay</b>.
        /// </summary>
        /// <param name="seed">The value to be tested</param>
        /// <param name="delay">The base multiplier</param>
        /// <returns>Integer value less than <b>delay</b>.</returns>
        protected Int32 GetRecurrenceDiff(Int32 seed, Int32 delay)
        {
            return Math.Abs(delay - ( seed % delay )) % delay;
        }

        /// <summary>
        /// Gets the next daily occurence on this schedule.
        /// </summary>
        /// <param name="seed">The date on or after which the next execution should occur.</param>
        /// <returns>The date of the next occurrence on or after <b>seed</b>.</returns>
        protected DateTime GetNextDailyOccurrence(DateTime seed)
        {
            // Determine which date the next occurrence will be...
            // Note: if the current date is valid, then there will be no change to the date.
            TimeSpan seedDiff = seed.Date - FirstOccurrence().Date;
            seed = seed.AddDays(GetRecurrenceDiff(( Int32 )seedDiff.TotalDays, _recurrenceDelay));

            return seed;
        }

        /// <summary>
        /// Gets the next monthly occurrence on this schedule.
        /// </summary>
        /// <param name="seed">The date on or after which the next execution should occur.</param>
        /// <returns>The date of the next occurrence on or after <b>seed</b>.</returns>
        protected DateTime GetNextMonthlyOccurrence(DateTime seed)
        {
            // Estimate the number of months b/w the FirstOccurrence and the seed.
            Int32 monthDiff = seed.Year - FirstOccurrence().Year - FirstOccurrence().Month + seed.Month;
            seed = seed.AddMonths(GetRecurrenceDiff(monthDiff, _recurrenceDelay));

            switch (_monthlyFrequency)
            {
                case MonthlyFrequencyOptions.Daily:
                    if (seed.Day > _dayOfMonth)
                    {
                        seed = seed.AddMonths(_recurrenceDelay);
                        seed = seed.AddDays(Math.Min(DateTime.DaysInMonth(seed.Year, seed.Month), _dayOfMonth) - seed.Day);
                    }
                    else if (seed.Day < _dayOfMonth)
                    {
                        seed = seed.AddDays(Math.Min(DateTime.DaysInMonth(seed.Year, seed.Month), _dayOfMonth) - seed.Day);
                    }
                    break;

                case MonthlyFrequencyOptions.Weekly:
                    DateTime targetDate = GetWeekdayOfMonth(seed);
                    if (targetDate.Day < seed.Day)
                        seed = GetWeekdayOfMonth(targetDate.AddMonths(_recurrenceDelay));
                    else
                        seed = targetDate;

                    break;
            }
            return seed;
        }

        /// <summary>
        /// Gets the next weekly occurrence on this schedule.
        /// </summary>
        /// <param name="seed">The date on or after which the next execution should occur.</param>
        /// <returns>The date of the next occurrence on or after <b>seed</b>.</returns>
        protected DateTime GetNextWeeklyOccurrence(DateTime seed)
        {
            Int32 weekDiff = ( Int32 )( ( seed.Date - FirstOccurrence().Date ).TotalDays / 7 );
            Int32 recurDiff = GetRecurrenceDiff(weekDiff, _recurrenceDelay);
            // If we are not in a valid week.. go to the first day of the next valid week
            if (recurDiff > 0)
            {
                seed = seed.AddDays(recurDiff * 7);
                while (seed.DayOfWeek != FirstOccurrence().DayOfWeek)
                    seed = seed.AddDays(1);
            }

            // FirstOccurrence.DayOfWeek represents the "first day of the week" for this schedule.
            // If we are already on that day... return
            if (seed.DayOfWeek == FirstOccurrence().DayOfWeek)
                return seed;
            // otherwise, if we encounter the "first day of the week" we have just started the next week
            // and need to add the weekly delay
            while (( seed.DayOfWeek != FirstOccurrence().DayOfWeek ) && ( !_daysOfWeek.Contains(seed.DayOfWeek) ))
            {
                seed = seed.AddDays(1);
            }
            if (seed.DayOfWeek == FirstOccurrence().DayOfWeek)
            {
                seed = seed.AddDays(7 * ( _recurrenceDelay - 1 ));
            }
            return seed;
        }

        #endregion Private Methods

        #endregion
    }
}

