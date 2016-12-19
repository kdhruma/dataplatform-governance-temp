using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects.Exports;

namespace MDM.JigsawIntegrationManager.DataPackages
{
	public class ExportEventData: IEventData
	{
		#region Properties
		public String ExportProfileName
		{
			get;
			set;
		}

		public Int32 NumberOfEntitiesExported
		{
			get;
			set;
		}

		public IList<String> CollaborationContainerEntityGuidId
		{
			get;
			set;
		}

		public IList<String> ApprovedContainerEntityGuidId
		{
			get;
			set;
		}

		public IList<Int64> CollaborationContainerEntityExternalId
		{
			get;
			set;
		}

		public IList<Int64> ApprovedContainerEntityExternalId
		{
			get;
			set;
		}
		
		public DateTime ExportStartTime
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public DateTime ExportEndTime
		{
			get;
			set;
		}


        public IList<String> ExternalEntityShortNames
        {
            get;
            set;
        }
        #endregion
    }
}
