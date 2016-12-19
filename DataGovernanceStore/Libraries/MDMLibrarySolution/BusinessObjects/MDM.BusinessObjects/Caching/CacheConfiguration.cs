using MDM.BusinessObjects.Interfaces.Caching;
using MDM.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MDM.BusinessObjects.Caching
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CacheConfiguration : ICacheConfiguration
    {
        #region Private Members

        private String _name;
        private String _keyFormat;
        private String _cacheType;
        private Int32 _retentionTime;
        private DateInterval _retentionType;
        private String _groupName;
        private String _displayName;

        private List<Dictionary<String, String>> _parameters;
        #endregion


        #region ICacheConfiguration Members

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string KeyFormat
        {
            get
            {
                return _keyFormat;
            }
            set
            {
                _keyFormat = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public string CacheType
        {
            get
            {
                return _cacheType;
            }
            set
            {
                _cacheType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public int RetentionTime
        {
            get
            {
                return _retentionTime;
            }
            set
            {
                _retentionTime = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public DateInterval RetentionUnit
        {
            get
            {
                return _retentionType;
            }
            set
            {
                _retentionType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Dictionary<String, String>> Parameters
        {
            get { return _parameters; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public CacheConfiguration()
        {
            _parameters = new List<Dictionary<string, string>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void LoadFromXml(string xml)
        {
            var xele = XElement.Parse(xml);

            DisplayName = xele.Attribute("DisplayName") != null ? xele.Attribute("DisplayName").Value : String.Empty;
            Name = xele.Attribute("Name") != null ? xele.Attribute("Name").Value : String.Empty;
            KeyFormat = xele.Attribute("KeyFormat") != null ? xele.Attribute("KeyFormat").Value : String.Empty;

            if (xele.Attribute("RetentionTime") != null)
                Int32.TryParse(xele.Attribute("RetentionTime").Value, out _retentionTime);

            if (xele.Attribute("RetentionUnit") != null)
                Enum.TryParse<DateInterval>(xele.Attribute("RetentionUnit").Value,  true, out _retentionType);

            CacheType = xele.Attribute("CacheType") != null ? xele.Attribute("CacheType").Value : String.Empty;

            var parameters = xele.Descendants("Param");

            foreach (var prm in parameters)
            {
                var prmo = new Dictionary<String, String>();

                foreach (var attr in prm.Attributes())
                {
                    prmo.Add(attr.Name.ToString(), attr.Value);
                }

                _parameters.Add(prmo);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public void LoadFromJson(string json)
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion
    }
}
