using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects.Workflow
{
    /// <summary>
    /// Specifies the tracked Fault Records
    /// </summary>
    [DataContract]
    public class FaultTracking : MDMObject
    {
        #region Fields

        /// <summary>
        /// Id of the running workflow which uniquely identifies running instance
        /// </summary>
        private String _runtimeInstanceId = String.Empty;

        /// <summary>
        /// The name of the workflow where exception has occurred
        /// </summary>
        private String _applicationSource = String.Empty;

        /// <summary>
        /// The Activity name in the workflow where exception has occurred
        /// </summary>
        private String _faultActivitySourceName = String.Empty;

        /// <summary>
        /// Message that describes the exception
        /// </summary>
        private String _faultMessage = String.Empty;

        /// <summary>
        /// Represents the immediate frames on the call stack.
        /// </summary>
        private String _stackTrace = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        ///Property denoting the Id of the running workflow which uniquely identifies running instance
        /// </summary>
        [DataMember]
        public String RuntimeInstanceId
        {
            get
            {
                return _runtimeInstanceId;
            }
            set
            {
                _runtimeInstanceId = value;
            }
        }

        /// <summary>
        /// Property denoting the name of the workflow where exception has occurred
        /// </summary>
        [DataMember]
        public String ApplicationSource
        {
            get
            {
                return _applicationSource;
            }
            set
            {
                _applicationSource = value;
            }
        }

        /// <summary>
        /// Property denoting the Activity name in the workflow where exception has occurred
        /// </summary>
        [DataMember]
        public String FaultActivitySourceName
        {
            get
            {
                return _faultActivitySourceName;
            }
            set
            {
                _faultActivitySourceName = value;
            }
        }

        /// <summary>
        ///Property denoting the Message that describes the exception
        /// </summary>
        [DataMember]
        public String FaultMessage
        {
            get
            {
                return _faultMessage;
            }
            set
            {
                _faultMessage = value;
            }
        }

        /// <summary>
        /// Property denoting the immediate frames on the call stack.
        /// </summary>
        [DataMember]
        public String StackTrace
        {
            get
            {
                return _stackTrace;
            }
            set
            {
                _stackTrace = value;
            }
        }

        /// <summary>
        /// Property denoting the Name
        /// </summary>
        [DataMember]
        public override String Name
        {
            get
            {
                throw new NotImplementedException("Name property is not implemented in this context");
            }
            set
            {
                throw new NotImplementedException("Name property is not implemented in this context");
            }
        }

        /// <summary>
        /// Property denoting the Long Name
        /// </summary>
        [DataMember]
        public override string LongName
        {
            get
            {
                throw new NotImplementedException("LongName property is not implemented in this context");
            }
            set
            {
                throw new NotImplementedException("LongName property is not implemented in this context");
            }
        }

        /// <summary>
        /// Property denoting the Locale
        /// </summary>
        [DataMember]
        public new String Locale
        {
            get
            {
                throw new NotImplementedException("Locale property is not implemented in this context");
            }
            set
            {
                throw new NotImplementedException("Locale property is not implemented in this context");
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public FaultTracking()
        {
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXML()
        {
            String xml = String.Empty;

            xml = "<FaultTrackingRecord Id=\"{0}\" RuntimeInstanceId = \"{1}\" ApplicationSource = \"{2}\" FaultActivitySourceName=\"{3}\" FaultMessage=\"{4}\" StackTrace=\"{5}\" />";

            String retXML = String.Format(xml, this.Id, this.RuntimeInstanceId, this.ApplicationSource, this.FaultActivitySourceName, this.FaultMessage, this.StackTrace);

            return retXML;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ActivityTracking object which needs to be compared.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is FaultTracking)
            {
                FaultTracking objectToBeCompared = obj as FaultTracking;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.RuntimeInstanceId != objectToBeCompared.RuntimeInstanceId)
                    return false;

                if (this.ApplicationSource != objectToBeCompared.ApplicationSource)
                    return false;

                if (this.FaultActivitySourceName != objectToBeCompared.FaultActivitySourceName)
                    return false;

                if (this.FaultMessage != objectToBeCompared.FaultMessage)
                    return false;

                if (this.StackTrace != objectToBeCompared.StackTrace)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.RuntimeInstanceId.GetHashCode() ^ this.ApplicationSource.GetHashCode() ^ this.FaultActivitySourceName.GetHashCode() ^ this.FaultMessage.GetHashCode() ^ this.StackTrace.GetHashCode();
        }

        #endregion
    }
}
