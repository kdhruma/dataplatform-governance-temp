using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;
using ProtoBuf.Meta;

namespace MDM.Utility
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Provides utility methods to Serialize and De-serialize objects using ProtoBuf.
    /// </summary>
    public static class ProtoBufSerializationHelper
    {
        #region Fields

        private static Boolean _isInitialized = false;

        /// <summary>
        /// Field representing the Entity type strong type map
        /// </summary>
        public static Dictionary<Int32, Type> EntityTypeStrongTypeMap = new Dictionary<Int32, Type>();

        /// <summary>
        /// Field representing the Strong type property map
        /// </summary>
        public static Dictionary<Type, Dictionary<String, PropertyInfo>> StrongTypePropertyMap = new Dictionary<Type, Dictionary<String, PropertyInfo>>();

        #endregion Fields

        #region Public Methods

        /// <summary>
        /// Initializes the ProtoBuf Serialization Runtime Meta types along with its sub types. 
        /// </summary>
        public static void InitializeMetaTypesWithSubTypes(String assemblyName = "")
        {
            try
            {
                if (!_isInitialized)
                {
                    InitializeMetaTypeForObjectBase();
                    InitializeMetaTypeForMDMObject();
                    InitializeMetaTypeForStrongTypes(assemblyName);
                    _isInitialized = true;
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Serializes the specified data into a byte array using ProtoBuf serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Byte[] Serialize<T>(T data)
        {
            Byte[] byteData = null;
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, data);
                byteData = stream.ToArray();
            }
            return byteData;
        }

        /// <summary>
        /// De-serializes the specified byte array to a requested type.
        /// </summary>
        /// <typeparam name="T">The type to which the data should be De-serialized.</typeparam>
        /// <param name="byteData">The input byte array which is to be De-serialized.</param>
        /// <returns></returns>
        public static T Deserialize<T>(Byte[] byteData)
        {
            T data = default(T);
            using (Stream stream = new MemoryStream(byteData))
            {
                data = Serializer.Deserialize<T>(stream);
            }
            return data;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adding all objectbase class properties into derived class
        /// </summary>
        private static void InitializeMetaTypeForObjectBase()
        {
            MetaType objectBaseMetaType = RuntimeTypeModel.Default.Add(typeof(ObjectBase), true);

            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 1, typeof(MDMObject));
            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 2, typeof(WorkflowState));
            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 3, typeof(EntityContext));
            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 4, typeof(AttributeUniqueIdentifier));
            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 5, typeof(CallerContext));
            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 6, typeof(AttributeModelContext));
            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 7, typeof(EntityChangeContext));
            objectBaseMetaType.AddSubType(ProtoBufConstants.OBJECTBASE + 8, typeof(EntityMoveContext));
        }

        /// <summary>
        /// Adding all mdmobject class properties into derived class
        /// </summary>
        private static void InitializeMetaTypeForMDMObject()
        {
            MetaType mdmObjectMetaType = RuntimeTypeModel.Default.Add(typeof(MDMObject), true);

            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 1, typeof(Attribute));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 2, typeof(Value));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 3, typeof(RelationshipBase));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 4, typeof(WorkflowActionContext));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 5, typeof(Entity));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 6, typeof(AttributeModel));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 7, typeof(AttributeModelBaseProperties));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 8, typeof(AttributeModelMappingProperties));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 9, typeof(AttributeModelLocaleProperties));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 10, typeof(DependentAttribute));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 11, typeof(ApplicationContext));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 12, typeof(RelationshipType));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 13, typeof(RelationshipContext));
            mdmObjectMetaType.AddSubType(ProtoBufConstants.MDMOBJECT + 14, typeof(EntityHierarchyContext));
        }


        private static void InitializeMetaTypeForStrongTypes(String assemblyName)
        {
            if (!String.IsNullOrWhiteSpace(assemblyName))
            {
                MetaType strongTypeBaseMetaType = RuntimeTypeModel.Default.Add(typeof(StronglyTypedEntityBase), true);
                Assembly assembly = Assembly.LoadFrom(String.Format("{0}\\{1}", Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), assemblyName));
                if (assembly != null)
                {
                    Int32 protobufIndex = 1;
                    Type stronglyTypedMetaDataAttribute = typeof(StronglyTypedMetaDataAttribute);
                    Type stronglyTypedClassMetaDataAttribute = typeof(StronglyTypedClassMetaDataAttribute);
                    foreach (Type strongType in assembly.GetTypes())
                    {
                        StronglyTypedClassMetaDataAttribute metaAttribute = strongType.GetCustomAttributes(stronglyTypedClassMetaDataAttribute, true).Cast<StronglyTypedClassMetaDataAttribute>().FirstOrDefault();
                        if (metaAttribute != null)
                        {
                            strongTypeBaseMetaType.AddSubType(ProtoBufConstants.STRONGLYTYPEDENTITYBASE + protobufIndex++, strongType);
                            if (!EntityTypeStrongTypeMap.ContainsKey(metaAttribute.EntityTypeId))
                            {
                                EntityTypeStrongTypeMap.Add(metaAttribute.EntityTypeId, strongType);
                            }
                        }
                        if (!StrongTypePropertyMap.ContainsKey(strongType))
                        {
                            Dictionary<String, PropertyInfo> keyPropertyMap = new Dictionary<string, PropertyInfo>();

                            foreach (PropertyInfo property in strongType.GetProperties())
                            {
                                StronglyTypedMetaDataAttribute propertyAttribute = property.GetCustomAttributes(stronglyTypedMetaDataAttribute, true).Cast<StronglyTypedMetaDataAttribute>().FirstOrDefault();
                                if (propertyAttribute != null)
                                {
                                    keyPropertyMap.Add(propertyAttribute.ShortName + "##@##" + propertyAttribute.ParentName, property);
                                }
                            }
                            if (keyPropertyMap.Keys.Count > 0)
                            {
                                StrongTypePropertyMap.Add(strongType, keyPropertyMap);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
