using System;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.EntityManager.Business;
    using MDM.EntityWorkflowManager.Business;
    using MDM.Workflow.PersistenceManager.Business;
    using Newtonsoft.Json.Linq;
    using System.ServiceModel.Activation;

    /// <summary>
    /// 
    /// </summary>
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WorkflowRestService : IWorkflowRestService
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private CallerContext _callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        /// <summary>
        /// 
        /// </summary>
        private EntityWorkflowBL _entityWorkflowBL = new EntityWorkflowBL(new EntityBL());

        #endregion

        #region Constructor

        #endregion Constructor

        #region Properties

        #endregion Properties

        #region Methods
        
        #region Public Methods

        #region Start Workflow

        #region Entity Workflow Service

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String StartWorkflow(String input)
        {
            String output;
            
            JObject requestJsonObject = JObject.Parse(input);
            JObject responseJsonObject = _entityWorkflowBL.StartWorkflow(requestJsonObject);
            output = responseJsonObject.ToString();
            
            return output;
        }

        #endregion Entity Workflow Service

        #endregion Start Workflow

        #region Resume Workflow

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String ResumeWorkflow(String input)
        {
            String output;

            JObject requestJsonObject = JObject.Parse(input);
            JObject responseJsonObject = _entityWorkflowBL.ResumeWorkflow(requestJsonObject);
            output = responseJsonObject.ToString();

            return output;
        }


        #endregion Resume Workflow

        #region Get Workflow Details

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String GetWorkflowDetails(String input)
        {
            String output;

            WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();

            JObject requestJsonObject = JObject.Parse(input);
            JObject responseJsonObject = workflowInstanceBL.LoadWorkflowDetails(requestJsonObject);
            output = responseJsonObject.ToString();

            return output;
        }

        #endregion Get Workflow Details

        #region User Assignment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ChangeAssignment(string input)
        {
            String output;

            JObject requestJsonObject = JObject.Parse(input);
            JObject responseJsonObject = _entityWorkflowBL.ChangeAssignment(requestJsonObject);
            output = responseJsonObject.ToString();

            return output;
        }

        #endregion User Assignment 

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #endregion Methods


     
    }
}
