using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Workflow
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class JsonString
    {
        /// <summary>
        /// 
        /// </summary>
        private String _jsonData;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String JsonData
        {
            get
            {
                return this._jsonData;
            }
            set
            {
                this._jsonData = value;
            }
        }
    }
}
