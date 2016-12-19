using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MDM.WCFServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWorkflowRestService" in both code and config file together.
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IWorkflowRestService
    {
        #region Start Workflow

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract(Name = "StartWorkflow")]
        [WebInvoke(Method = "POST", UriTemplate = "StartWorkflow", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        String StartWorkflow(String input);

        #endregion Start Workflow

        #region Resume Workflow

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract(Name = "ResumeWorkflow")]
        [WebInvoke(Method = "POST", UriTemplate = "ResumeWorkflow", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        String ResumeWorkflow(String input);

        #endregion Resume Workflow

        #region Get Workflow Details

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetWorkflowDetails")]
        [WebInvoke(Method = "POST", UriTemplate = "GetWorkflowDetails", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        String GetWorkflowDetails(String input);

        #endregion Get Workflow Details

        #region User Assignment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract(Name = "ChangeAssignment")]
        [WebInvoke(Method = "POST", UriTemplate = "ChangeAssignment", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        String ChangeAssignment(String input);

        #endregion User Assignment
    }
}
