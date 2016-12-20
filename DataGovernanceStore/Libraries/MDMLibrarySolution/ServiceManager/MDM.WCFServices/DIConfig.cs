using CommonServiceLocator.StructureMapAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace MDM.WCFServices
{
    using CategoryManager.Business;
    using ContainerManager.Business;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Business;
    using MDM.EntityWorkflowManager.Business;
    using MDM.Interfaces;
    using MDM.LookupManager.Business;
    using MDM.OrganizationManager.Business;
    using MDM.WorkflowRuntimeEngine;

	public class DIConfig
    {
        /// <summary>
        /// Register the configuration for Structure map
        /// </summary>
        public static void RegisterConfiguration()
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(GetStructureMapContainer()));
        }

        private static StructureMap.IContainer GetStructureMapContainer()
        {
            StructureMap.Container container = new StructureMap.Container(x => x.AddRegistry(new WCFRegistry()));

            return container;
        }
    }

    /// <summary>
    /// Class for setting WCF registry for structure map
    /// </summary>
    public class WCFRegistry : Registry
    {
        #region Constructors

        /// <summary>
        /// Class for setting WCF registry
        /// </summary>
        public WCFRegistry()
        {
            For<IEntityManager>().Use<EntityBL>();
            For<IWorkflowRuntimeManager>().Use<WorkflowRuntimeBL>();
            For<IEntityWorkflowManager>().Use<EntityWorkflowBL>();
            For<ILookupManager>().Use<LookupBL>();
            For<IWorkflowRuntimeManager>().Use<WorkflowRuntimeBL>();
            For<IContainerManager>().Use<ContainerBL>();
            For<ICategoryManager>().Use(new CategoryBL());
            For<IContainerEntityTypeMappingManager>().Use<ContainerEntityTypeMappingBL>();
            For<IOrganizationManager>().Use<OrganizationBL>();
            For<IEntityTypeManager>().Use<EntityTypeBL>();
		}

        #endregion Constructors
    }
}
