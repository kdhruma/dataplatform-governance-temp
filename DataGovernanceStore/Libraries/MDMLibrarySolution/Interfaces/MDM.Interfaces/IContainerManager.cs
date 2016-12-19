using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// 
    /// </summary>
    public interface IContainerManager
    {
        /// <summary>
        /// Gets all containers from all organizations
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="applySecurity"></param>
        /// <returns>ContainerCollection</returns>
        ContainerCollection GetAll(CallerContext callerContext, Boolean applySecurity);

        /// <summary>
        /// Gets all containers from all organizations
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <returns>ContainerCollection</returns>
        ContainerCollection GetAll(ContainerContext containerContext, CallerContext callerContext);

        /// <summary>
        /// Gets container by Id
        /// </summary>
        /// <param name="containerId">Indicates containerId</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        Container Get(Int32 containerId, CallerContext callerContext, Boolean applySecurity);

        /// <summary>
        /// Gets container by Id
        /// </summary>
        /// <param name="containerId">Indicates containerId</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container</returns>
        Container Get(Int32 containerId, ContainerContext containerContext, CallerContext callerContext);

        /// <summary>
        /// Gets container by ShortName
        /// </summary>
        /// <param name="containerName">container Short Name</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        Container Get(String containerName, CallerContext callerContext, Boolean applySecurity);

        /// <summary>
        /// Gets container by ShortName
        /// </summary>
        /// <param name="containerName">container Short Name</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container</returns>
        Container Get(String containerName, ContainerContext containerContext, CallerContext callerContext);

        /// <summary>
        /// Gets container based on container shortname and organization name
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="organizationId"></param>     
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        Container Get(String containerName, Int32 organizationId, CallerContext callerContext, Boolean applySecurity = true);

        /// <summary>
        /// Gets container based on container shortname and organization name
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="organizationName"></param>     
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        Container Get(String containerName, String organizationName, CallerContext callerContext, Boolean applySecurity = true);

        /// <summary>
        /// Gets container based on container shortname and organization name
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="organizationName"></param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container</returns>
        Container Get(String containerName, String organizationName, ContainerContext containerContext, CallerContext callerContext);

        /// <summary>
        /// Gets all containers in a given organization id
        /// </summary>
        /// <param name="orgainzationId">Indicates orgainzation Id</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container Collection</returns>
        ContainerCollection GetByOrgId(Int32 orgainzationId, ContainerContext containerContext, CallerContext callerContext);

        /// <summary>
        /// Gets all containers in given organization
        /// </summary>
        /// <param name="organizationName">organization short Name</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container Collection</returns>
        ContainerCollection GetByOrgName(String organizationName, ContainerContext containerContext, CallerContext callerContext);

        /// <summary>
        /// Processes the list of container(All CRUD operations goes through same process)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="containerOperationResult"></param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        void Process(Container container, OperationResult containerOperationResult, String programName, CallerContext callerContext);

        /// <summary>
        /// Processes the container collection (All CRUD operations goes through same process)
        /// </summary>
        /// <param name="containerCollection">list of containers to be processeds</param>
        /// <param name="command">contains all the db related properties such as connection string etc.</param>
        /// <param name="containerOperationResult"></param>
        /// <param name="programName"></param>
        /// <param name="callerContext"></param>
        void Process(ContainerCollection containerCollection, OperationResult containerOperationResult, String programName, CallerContext callerContext);

        /// <summary>
        /// Processes the supported locales for the container collection
        /// </summary>
        /// <param name="iDataModelObjects">List of container data model objects</param>
        /// <param name="operationResults">Collection of data model operation results</param>
        /// <param name="iCallerContext">caller context indicating who called the API</param>
        void ProcessLocaleMappings(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext);
    }
}