using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using IO = System.IO;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.AdminManager.Data;

    public class BuildInfoBL : BusinessLogicBase
    {
        #region Fields

        private FileDA _fileDA = new FileDA();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Gets the latest build details
        /// </summary>
        /// <returns>BuildDetail object</returns>
        public BuildDetail GetLatestBuildDetail()
        {
            BuildInfoDA buildInfoDa = new BuildInfoDA();
            return buildInfoDa.GetLatestBuildDetail();
        }



        /// <summary>
        /// Gets the corresponding build feature Id
        /// </summary>
        /// <param name="buildDetailId">Indicates the path of the file</param>
        /// <param name="featureName">Indicates the build server</param>
        /// <returns>Int</returns>
        public Int32 GetBuildFeatureId(Int32 buildDetailId, String featureName)
        {
            BuildInfoDA buildInfoDa = new BuildInfoDA();
            return buildInfoDa.GetBuildFeatureId(buildDetailId, featureName);
        }

        /// <summary>
        /// Process FileCheckSum
        /// </summary>
        /// <param name="buildDetailContext">Indicates the BuildContext</param>
        /// <returns>OperationResult</returns>

        public OperationResult ProcessFileCheckSum(BuildDetailContext buildDetailContext)
        {
            BuildInfoDA buildInfoDa = new BuildInfoDA();
            return buildInfoDa.ProcessFileCheckSum(buildDetailContext);
        }

        /// <summary>
        /// Update Build Status 
        /// </summary>
        /// <param name="buildDetailContext">Indicates the BuildContext</param>

        /// <returns>OperationResult</returns>

        public OperationResult UpdateBuildStatus(BuildDetailContext buildDetailContext)
        {
            BuildInfoDA buildInfoDa = new BuildInfoDA();
            return buildInfoDa.UpdateBuildStatus(buildDetailContext);

        }

        /// <summary>
        /// Save Build Details 
        /// </summary>
        /// <param name="version">Indicates the BuildContext</param>
        /// <returns></returns>

        public OperationResult SaveBuildDetails(BuildDetailContext buildDetailContext)
        {
            BuildInfoDA buildInfoDa = new BuildInfoDA();
            return buildInfoDa.SaveBuildDetails(buildDetailContext);
        }

        public RoleType GetUserRoleType(String userName)
        {
            BuildInfoDA buildInfoDA = new BuildInfoDA();
            return buildInfoDA.GetUserRoleType(userName);
        }

        #endregion
    }
}
