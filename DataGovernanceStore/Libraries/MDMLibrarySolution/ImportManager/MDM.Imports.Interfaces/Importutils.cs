using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.BusinessObjects;
using MDM.Core;

namespace MDM.Imports.Interfaces
{
    public class Importutils
    {
        /// <summary>
        /// Find if the given entity matches the given import provider context. If it matches add it to the collection and return true. Otherwise return false.
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="entity"></param>
        /// <param name="entityProviderContext"></param>
        /// <returns></returns>
        public static Boolean AddEntityMatchingProviderContext(EntityCollection entityCollection, Entity entity, EntityProviderContext entityProviderContext)
        {
            if (entityProviderContext.EntityProviderContextType == EntityProviderContextType.Container)
            {
                if (String.IsNullOrEmpty(entityProviderContext.ContainerName) == true)
                {
                    // Container name is not given for a container type processing.
                    throw new ArgumentException("Container infomration is not provided for a container type processing.");
                }
            }

            if (entityProviderContext.EntityProviderContextType == EntityProviderContextType.EntityType)
            {
                if (String.IsNullOrEmpty(entityProviderContext.EntityTypeName) == true)
                {
                    // Entity type name is not given for a entity type processing.
                    throw new ArgumentException("Entity type is not provided for a entity type processing.");
                }
            }

            Boolean successFlag = false;
            switch (entityProviderContext.EntityProviderContextType)
            {
                case EntityProviderContextType.All:
                    entityCollection.Add(entity);
                    successFlag = true;
                    break;
                case EntityProviderContextType.Container:
                    if (String.Compare(entityProviderContext.ContainerName, entity.ContainerName, true) == 0)
                    {
                        entityCollection.Add(entity);
                        successFlag = true;
                    }
                    break;
                case EntityProviderContextType.EntityType:
                    if (String.Compare(entityProviderContext.EntityTypeName, entity.EntityTypeName, true) == 0)
                    {
                        entityCollection.Add(entity);
                        successFlag = true;
                    }
                    break;
                default:
                    break;
            }
            return successFlag;
        }
    }
}
