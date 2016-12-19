using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects.Workflow
{
    /// <summary>
    /// Specifies the tracked Instance Records
    /// </summary>
    [DataContract]
    public class InstanceTracking : MDMObject
    {
        #region Fields

        /// <summary>
        /// Id of the running workflow which uniquely identifies running instance
        /// </summary>
        private String _runtimeInstanceId = String.Empty;

        /// <summary>
        /// Field for the execution Status of the workflow instance
        /// </summary>
        private String _status = String.Empty;

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
        /// Property denoting the execution Status of the workflow instance
        /// </summary>
        [DataMember]
        public String Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        /// <summary>
        /// Property denoting the name
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
        /// Property denoting the Long name
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
        /// Parameterless Constructor
        /// </summary>
        public InstanceTracking()
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

            xml = "<InstanceTrackingRecord Id=\"{0}\" RuntimeInstanceId = \"{1}\" Status=\"{2}\" />";

            String retXML = String.Format(xml, this.Id, this.RuntimeInstanceId, this.Status);

            return retXML;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ActivityTracking object which needs to be compared.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is InstanceTracking)
            {
                InstanceTracking objectToBeCompared = obj as InstanceTracking;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.RuntimeInstanceId != objectToBeCompared.RuntimeInstanceId)
                    return false;

                if (this.Status != objectToBeCompared.Status)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves  the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.RuntimeInstanceId.GetHashCode() ^ this.Status.GetHashCode();
        }

        #endregion
    }
}
