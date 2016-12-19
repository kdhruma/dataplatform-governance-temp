using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Transactions;


namespace MDM.Workflow.TrackingManager.Business
{
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.Workflow.TrackingManager.Data;
    using MDM.Core;
    using MDM.JigsawIntegrationManager;

    /// <summary>
    /// Business Logic for Fault Tracking Record
    /// </summary>
    public class FaultTrackingBL : BusinessLogicBase
    {
        /// <summary>
        /// Adds new Fault Tracking Record.
        /// </summary>
        /// <param name="faultTrackingRecord">Fault Tracking Record to be added</param>
        /// <param name="status">Fault Tracking status</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Create(FaultTracking faultTrackingRecord, String status, CallerContext context)
        {
            Collection<FaultTracking> listFaultRecord = new Collection<FaultTracking>();
            listFaultRecord.Add(faultTrackingRecord);

            return this.Create(listFaultRecord, status, context);
        }

        /// <summary>
        /// Adds collection of Fault Tracking Record.
        /// </summary>
        /// <param name="faultTrackingRecords">Collection of Fault Tracking Record</param>
        /// <param name="status">Fault Tracking status</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Create(Collection<FaultTracking> faultTrackingRecords, String status, CallerContext context)
        {
            Boolean result = false;

            //Get command
            DBCommandProperties command = DBCommandHelper.Get(context, Core.MDMCenterModuleAction.Create);

            FaultTrackingDA faultTrackingRecordDA = new FaultTrackingDA();
            int rowsAffected = faultTrackingRecordDA.Create(faultTrackingRecords, command);

            if (JigsawConstants.IsJigsawIntegrationEnabled)
            {
                foreach (var faultTracking in faultTrackingRecords)
                {
                    JigsawIntegrationHelper.SendToJigsaw(faultTracking.RuntimeInstanceId, status, context);
                }
            }

            if (rowsAffected > 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Updates Fault Tracking Record
        /// </summary>
        /// <param name="faultTrackingRecord">Fault Tracking Record to be updated</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Update(FaultTracking faultTrackingRecord, CallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes existing Fault Tracking Record
        /// </summary>
        /// <param name="faultTrackingRecord">Fault Tracking Record to be deleted</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Delete(FaultTracking faultTrackingRecord, CallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Fault Tracking Record by id
        /// </summary>
        /// <param name="id"> Id of the record to be fetched</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>FaultTrackingRecord</returns>
        public FaultTracking GetById(int id, CallerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
