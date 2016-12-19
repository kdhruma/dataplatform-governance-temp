using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.WCFServices
{
    using MDM.Core;
    using MDM.WCFServiceInterfaces;
    
    public static class MDMWCFServiceFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T: class
        {
            T mdmServiceBase = null;

            if(typeof(T).Equals(typeof(IDataService)))
            { 
                mdmServiceBase = (T)GetDataService();
            }
            else if (typeof(T).Equals(typeof(IDataModelService)))
            {
                mdmServiceBase = (T)GetDataModelService();
            }
            else if (typeof(T).Equals(typeof(ICoreService)))
            {
                mdmServiceBase = (T)GetCoreService();
            }
            else if (typeof(T).Equals(typeof(IWorkflowService)))
            {
                mdmServiceBase = (T)GetWorkflowService();
            }
            else if (typeof(T).Equals(typeof(IMessageService)))
            {
                mdmServiceBase = (T)GetMessageService();
            }
            else if (typeof(T).Equals(typeof(IConfigurationService)))
            {
                mdmServiceBase = (T)GetConfigurationService();
            }
            else if (typeof(T).Equals(typeof(ISecurityService)))
            {
                mdmServiceBase = (T)GetSecurityService();
            }
            else if (typeof(T).Equals(typeof(IDenormService)))
            {
                mdmServiceBase = (T)GetDenormService();
            }
            else if (typeof(T).Equals(typeof(IIntegrationService)))
            {
                mdmServiceBase = (T)GetIntegrationService();
            }
            else if (typeof(T).Equals(typeof(IKnowledgeBaseService)))
            {
                mdmServiceBase = (T)GetKnowledgeBaseService();
            }
            else if (typeof(T).Equals(typeof(IInternalCommonService)))
            {
                mdmServiceBase = (T)GetInternalCommonService();
            }
            else if (typeof(T).Equals(typeof(ILegacyDataAccessService)))
            {
                mdmServiceBase = (T)GetLegacyDataAccessService();
            }
            else if (typeof(T).Equals(typeof(IDiagnosticService)))
            {
                mdmServiceBase = (T)GetDiagnosticService();
            }

            return mdmServiceBase;
        }

        public static IDataService GetDataService()
        {
            Boolean loadSecurityPrincipal = false;
            DataService dataService = new DataService(loadSecurityPrincipal);
            return dataService;
        }

        public static IDataModelService GetDataModelService()
        {
            Boolean loadSecurityPrincipal = false;
            DataModelService dataModelService = new DataModelService(loadSecurityPrincipal);
            return dataModelService;
        }

        public static ICoreService GetCoreService()
        {
            Boolean loadSecurityPrincipal = false;
            CoreService coreService = new CoreService(loadSecurityPrincipal);
            return coreService;
        }

        public static IWorkflowService GetWorkflowService()
        {
            Boolean loadSecurityPrincipal = false;
            WorkflowService workflowService = new WorkflowService(loadSecurityPrincipal);
            return workflowService;
        }

        public static IConfigurationService GetConfigurationService()
        {
            Boolean loadSecurityPrincipal = false;
            ConfigurationService configurationService = new ConfigurationService(loadSecurityPrincipal);
            return configurationService;
        }

        public static IMessageService GetMessageService()
        {
            Boolean loadSecurityPrincipal = false;
            MessageService messageService = new MessageService(loadSecurityPrincipal);
            return messageService;
        }

        public static ISecurityService GetSecurityService()
        {
            Boolean loadSecurityPrincipal = false;
            SecurityService securityService = new SecurityService(loadSecurityPrincipal);
            return securityService;
        }

        public static IDenormService GetDenormService()
        {
            DenormService denormService = new DenormService();
            return denormService;
        }
        
        public static IIntegrationService GetIntegrationService()
        {
            IntegrationService integrationService = new IntegrationService();
            return integrationService;
        }

        public static IKnowledgeBaseService GetKnowledgeBaseService()
        {
            KnowledgeBaseService knowledgeBaseService = new KnowledgeBaseService();
            return knowledgeBaseService;
        }

        public static IInternalCommonService GetInternalCommonService()
        {
            InternalCommonService internalCommonService = new InternalCommonService();
            return internalCommonService;
        }

        public static ILegacyDataAccessService GetLegacyDataAccessService()
        {
            LegacyDataAccessService legacyDataAccessService = new LegacyDataAccessService();
            return legacyDataAccessService;
        }

        public static IDiagnosticService GetDiagnosticService()
        {
            var diagnosticService = new DiagnosticService(false);
            return diagnosticService;
        }
    }
}
