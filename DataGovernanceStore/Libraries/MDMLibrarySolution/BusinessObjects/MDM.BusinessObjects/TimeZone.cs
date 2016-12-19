using System;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies TimeZone
    /// </summary>
    [DataContract]
    public class TimeZone : MDMObject
    {
        #region Fields

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public TimeZone()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a TimeZone</param>
        public TimeZone(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a TimeZone as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a TimeZone</param>
        /// <param name="name">Indicates the Name of a TimeZone</param>
        /// <param name="longName">Indicates the Description of a TimeZone</param>
        public TimeZone(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// constructor with Object array as input parameter
        /// </summary>
        /// <param name="objectArray">Indicates array containing value for TimeZone</param>
        public TimeZone(object[] objectArray)
        {
            Int32 intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);

            this.Id = intId;

            if (objectArray[1] != null)
                this.Name = System.Security.SecurityElement.Escape(objectArray[1].ToString());

            if (objectArray[2] != null)
                this.LongName = System.Security.SecurityElement.Escape(objectArray[2].ToString());

        }
        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of TimeZone object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXML()
        {
            String xml = string.Empty;
            xml = String.Format("<TimeZone PK_TimeZone=\"{0}\" ShortName=\"{1}\" LongName=\"{2}\" />", this.Id, this.Name, this.LongName);

            return xml;

        }

        #endregion
    }
}
