using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using MDM.Core;
using MDMBO = MDM.BusinessObjects;
using MDM.KnowledgeManager.Data;

namespace MDM.KnowledgeManager.Business
{
    public class TimeZoneBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public Collection<MDMBO.TimeZone> GetAll()
        {
            Collection<MDMBO.TimeZone> timeZones = new Collection<MDMBO.TimeZone>();

            TimeZoneDA timeZoneDA = new TimeZoneDA();
            timeZones = timeZoneDA.GetAll();

            return timeZones;
        }

        public String GenerateXML(Collection<MDMBO.TimeZone> timeZones)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<TimeZones>");
            foreach (MDMBO.TimeZone timeZone in timeZones)
                builder.Append(timeZone.ToXML());
            builder.Append("</TimeZones>");
            return builder.ToString();
        }

        #endregion
    }
}
