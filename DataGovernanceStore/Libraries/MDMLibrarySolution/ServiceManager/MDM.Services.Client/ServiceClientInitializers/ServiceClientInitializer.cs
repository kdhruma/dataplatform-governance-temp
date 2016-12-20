using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Services.ServiceClientInitializers
{
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Represents a utility class that provides methods to initialize applications that use MDM Service client assembly.
    /// </summary>
    public class ServiceClientInitializer
    {
        /// <summary>
        /// Initializes all resources used in MDM Service client assembly
        /// </summary>
        /// <param name="loadConfigsOnInitialization">Specifies whether to load configuration data on initialization</param>
        public static void Initialize(Boolean loadConfigsOnInitialization = true)
        {
            //[Phani]TODO: Need to change this to AppConfig
            String assemblyName = AppConfiguration.GetSetting("MDMCenter.StronglyTypedEntityBO.AssemblyName");
            // Used for ProtoBuf serialization
            ProtoBufSerializationHelper.InitializeMetaTypesWithSubTypes(assemblyName);

            // Initializes the AppConfigProvider which is available as a Utility
            AppConfigurationHelper.InitializeAppConfig(new AppConfigProviderUsingService(), loadConfigsOnInitialization);

            // Initializes the MDMFeatureConfigProvider which is available as a Utility
            MDMFeatureConfigHelper.InitializeMDMFeatureConfig(new MDMFeatureConfigProviderUsingService(), loadConfigsOnInitialization);

        }
    }
}
