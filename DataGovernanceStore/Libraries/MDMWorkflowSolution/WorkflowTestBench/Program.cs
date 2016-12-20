using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using MDMWS = MDM.Services;
using MDMBO = MDM.BusinessObjects;
using MDMBOW = MDM.BusinessObjects.Workflow;
using MDM.Core;
using System.ServiceModel;
using System.Collections;
using MDM.AdminManager.Business;
using MDM.BusinessObjects;
using MDM.Utility;

namespace MDM.Workflow.TestBench 
{
    class Program 
    {
        //static void Main(string[] args)
        //{
        //    BulkWorkflowLoad loadEngine = new BulkWorkflowLoad();
        //    loadEngine.Run();

        //    Console.WriteLine("Press any key to close program...");
        //    string dummy = Console.ReadLine();
        //}

        static void Main(string[] args)
        {
            String invokeAgain = "no";

            do
            {
                Int32 mdmObjectsCount = 0;

                Console.WriteLine("Enter the WorkflowName to be invoked:");
                String workflowName = Console.ReadLine();

                Console.WriteLine();

                Console.WriteLine("Enter the count of MDM Objects required to send to workflow:");
                String strCount = Console.ReadLine();

                Int32.TryParse(strCount, out mdmObjectsCount);

                Console.WriteLine();

                MDMBO.OperationResult operationResult = new MDMBO.OperationResult();

                MDMBOW.Workflow workflow = new MDMBOW.Workflow();
                workflow.LongName = workflowName;

                MDMBOW.WorkflowMDMObjectCollection workflowMDMObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();

                for (Int32 i = 0; i < mdmObjectsCount; i++)
                {
                    MDMBOW.WorkflowMDMObject mdmObject = new MDMBOW.WorkflowMDMObject();

                    Console.WriteLine("Enter MDM Object Id:");
                    String strObjectID = Console.ReadLine();

                    Int32 iMDMObjectID = 0;
                    Int32.TryParse(strObjectID, out iMDMObjectID);

                    mdmObject.MDMObjectId = iMDMObjectID;
                    mdmObject.MDMObjectType = "MDM.BusinessObjects.Job";

                    workflowMDMObjectCollection.Add(mdmObject);
                }

                Console.WriteLine();

                DateTime beforeInvoking = DateTime.Now;
                Console.WriteLine("Invocation started - " + beforeInvoking);

                // Initialize the AppConfigProvider for the application.
                AppConfigurationHelper.InitializeAppConfig(new AppConfigProviderUsingDB());                

                LoadSecurityPrincipal("system");

                MDMBO.CallerContext callerContext = new MDMBO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);


                MDMWS.WorkflowService wfService = new MDMWS.WorkflowService();

                wfService.StartWorkflow(workflow, workflowMDMObjectCollection, "TestBench", 1, "system", MDM.Core.WorkflowInstanceRunOptions.RunAsMultipleInstances, ref operationResult, callerContext);

                DateTime afterInvoking = DateTime.Now;
                Console.WriteLine("Invocation completed - " + afterInvoking);

                Console.WriteLine("Total time taken for invoking : " + afterInvoking.Subtract(beforeInvoking).ToString());

                Console.WriteLine();

                if (operationResult.HasError)
                {
                    Console.WriteLine("The error occurred while invoking. Error: " + operationResult.Errors[0].ErrorMessage);
                }
                else
                {
                    Console.WriteLine("Workflow got invoked successfully.");
                }

                Console.WriteLine();

                Console.WriteLine("Do you want to invoke one more workflow? (yes/no)");
                invokeAgain = Console.ReadLine();
            } while (invokeAgain.ToLower() == "yes");
        }

        private static void LoadSecurityPrincipal(String userName)
        {
            Int32 systemId = 0;

            //the stamp at the time of loging in
            String timeStamp = DateTime.Now.ToString();

            //The cache key to for the user principal to be stored
            string securityPrincipalCacheKey = "SecurityPrincipal_" + userName.ToLower();

            Hashtable loginData = new Hashtable();
            loginData.Add("SystemId", systemId);
            loginData.Add("TimeStamp", timeStamp);

            //obtain the security principal
            SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
            SecurityPrincipal currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, MDM.Core.MDMCenterSystem.Web);

            if (ServiceUserContext.Current == null)
            {
                ServiceUserContext.Initialize(currentUserSecurityPrincipal);
            }
        }

    }
}
