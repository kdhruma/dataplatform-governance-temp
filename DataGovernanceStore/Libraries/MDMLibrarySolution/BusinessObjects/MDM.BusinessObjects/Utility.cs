using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Represents the class for utility
    /// </summary>
    public class Utility
    {
        #region Fields

        private static String _errorMessage = "{0}: Mismatch: Actual: [{1}] Expected: [{2}]"; // error message to display in bvt report - {0} is name of property, {1} is actual, {2} is expected
        
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get relationship permission as string from the specified permission set
        /// </summary>
        /// <param name="permissionSet">Indicates the permission set for the relationship</param>
        /// <returns>Returns permissions as string from the specified permission set</returns>
        public static String GetPermissionsAsString(Collection<UserAction> permissionSet)
        {
            String permission = String.Empty;

            if (permissionSet != null)
            {
                foreach (UserAction userAction in permissionSet)
                {
                    String userPermission = String.Empty;

                    if (userAction == UserAction.View)
                    {
                        userPermission = "V";
                    }
                    else if (userAction == UserAction.Add)
                    {
                        userPermission = "A";
                    }
                    else if (userAction == UserAction.Delete)
                    {
                        userPermission = "D";
                    }
                    else if (userAction == UserAction.Update)
                    {
                        userPermission = "E";
                    }
                    else if (userAction == UserAction.WorkflowActions)
                    {
                        userPermission = "WA";
                    }
                    else if (userAction == UserAction.Reclassify)
                    {
                        userPermission = "R";
                    }

                    permission = (permission.Length > 0) ? String.Concat(permission, "|", userPermission) : userPermission;
                }
            }

            return permission;
        }

        /// <summary>
        /// Get a collection of UserAction based on the specified string
        /// </summary>
        /// <param name="permissionsAsString">Indicates the permission set as string</param>
        /// <returns>Returns a collection of UserAction based on the specified string</returns>
        public static Collection<UserAction> GetPermissionsAsObject(String permissionsAsString)
        {
            Collection<UserAction> permissionSet = new Collection<UserAction>();

            String[] permissions = ValueTypeHelper.SplitStringToStringArray(permissionsAsString, '|');

            if (permissions != null && permissions.Count() > 0)
            {
                foreach (String permission in permissions)
                {
                    switch (permission)
                    {
                        case "V":
                            permissionSet.Add(UserAction.View);
                            break;
                        case "A":
                            permissionSet.Add(UserAction.Add);
                            break;
                        case "D":
                            permissionSet.Add(UserAction.Delete);
                            break;
                        case "E":
                            permissionSet.Add(UserAction.Update);
                            break;
                        case "WA":
                            permissionSet.Add(UserAction.WorkflowActions);
                            break;
                        case "R":
                            permissionSet.Add(UserAction.Reclassify);
                            break;
                    }
                }
            }

            return permissionSet;
        }

        /// <summary>
        /// Set the attributeTypeName property's value based on AttributeTypeName coming from DB.
        /// Mapping of tb_AttributeType to AttributeType is as following.
        /// \nTb_AttributeType.ShortName : MDM.Core.AttributeTypeEnum
        /// \n____________________________________________________
        /// \nAttribute :	Simple
        /// \nComplexAttribute :Complex
        /// \nAttributeGroup : Unknown
        /// \nTechAttribute : Simple
        /// \nTechGroup : Unknown
        /// \nProperty : Unknown
        /// \nTechComplexAttribute : Complex
        /// \nSystemLabel : Unknown
        /// \nSystemObject : Unknown
        /// \nRootGroup : Unknown
        /// \nObjectAttribute : Unknown
        /// \nMetaDataAttribute : Unknown
        /// \nRelationshipAttribute : Simple
        /// \nRelationshipGroup : Unknown
        /// \nSequentialTechAttributes : Unknown
        /// \nFreeFormText : Unknown
        /// </summary>
        /// <param name="attributeTypeName">AttributeTypeName specified in tb_Attribute (FK ok tb_AttributeType)</param>
        /// <param name="isCollection">Indicates if its collection or not</param>
        public static AttributeTypeEnum GetAttributeTypeFromAttributeTypeName(String attributeTypeName, Boolean isCollection)
        {
            AttributeTypeEnum attrType = AttributeTypeEnum.Simple;

            if (!String.IsNullOrWhiteSpace(attributeTypeName))
            {
                //First identify attribute type w/o considering the collection
                switch (attributeTypeName.ToLower())
                {
                    case "attribute":
                    case "techattribute":
                    case "relationshipattribute":
                    case "systemattribute":
                        attrType = AttributeTypeEnum.Simple;
                        break;
                    case "complexattribute":
                    case "techcomplexattribute":
                    case "systemcomplexattribute":
                        attrType = AttributeTypeEnum.Complex;
                        break;
                    case "attributegroup":
                    case "techgroup":
                    case "relationshipgroup":
                        attrType = AttributeTypeEnum.AttributeGroup;
                        break;
                    default:
                        attrType = AttributeTypeEnum.Unknown;
                        break;
                }

                //If attribute is collection then update simple or complex to SimpleCollection or ComplexCollection
                if (isCollection == true)
                {
                    if (attrType == AttributeTypeEnum.Simple)
                    {
                        attrType = AttributeTypeEnum.SimpleCollection;
                    }
                    else if (attrType == AttributeTypeEnum.Complex)
                    {
                        attrType = AttributeTypeEnum.ComplexCollection;
                    }
                }
            }
            return attrType;
        }

        /// <summary>
        /// Set the <see cref="AttributeModelType"/> property's value based on AttributeTypeName coming from DB.
        /// Mapping of tb_AttributeType to AttributeModelType is as following.
        /// \nTb_AttributeType.ShortName : MDM.Core.AttributeModelType
        /// \n____________________________________________________
        /// \nAttribute :	Common
        /// \nComplexAttribute :Common
        /// \nAttributeGroup : Unknown
        /// \nTechAttribute : CategorySpecific
        /// \nTechGroup : Unknown
        /// \nProperty : Unknown
        /// \nTechComplexAttribute : CategorySpecific
        /// \nSystemLabel : Unknown
        /// \nSystemObject : System
        /// \nRootGroup : Unknown
        /// \nObjectAttribute : Unknown
        /// \nMetaDataAttribute : Unknown
        /// \nRelationshipAttribute : Relationship
        /// \nRelationshipGroup : Unknown
        /// \nSequentialTechAttributes : Unknown
        /// \nFreeFormText : Unknown
        /// </summary>
        /// <param name="attributeModelTypeName">AttributeTypeName specified in tb_Attribute (FK ok tb_AttributeType)</param>
        public static AttributeModelType GetAttributeModelTypeFromAttributeTypeName(String attributeModelTypeName)
        {
            AttributeModelType attrModelType = AttributeModelType.Common;

            if (!String.IsNullOrWhiteSpace(attributeModelTypeName))
            {
                switch (attributeModelTypeName.ToLower())
                {
                    case "attribute":
                    case "complexattribute":
                        attrModelType = AttributeModelType.Common;
                        break;
                    case "techattribute":
                    case "techcomplexattribute":
                        attrModelType = AttributeModelType.Category;
                        break;
                    case "relationshipattribute":
                        attrModelType = AttributeModelType.Relationship;
                        break;
                    case "systemattribute":
                    case "systemcomplexattribute":                    
                        attrModelType = AttributeModelType.System;
                        break;
                    case "attributegroup":
                    case "techgroup":
                    case "relationshipgroup":
                        attrModelType = AttributeModelType.AttributeGroup;
                        break;
                    case "metadataattribute":
                        attrModelType = AttributeModelType.MetaDataAttribute;
                        break;
                    default:
                        attrModelType = AttributeModelType.Unknown;
                        break;
                }
            }

            return attrModelType;
        }

        /// <summary>
        /// Methods gives attribute for processing
        /// </summary>
        /// <param name="attributes">Collection of attributes</param>
        /// <param name="hasAttributeChanges">Indicates if attribute has got changed or not</param>
        /// <returns>Collection of attributes</returns>
        public static AttributeCollection GetAttributesForProcessing(AttributeCollection attributes, out bool hasAttributeChanges)
        {
            hasAttributeChanges = false;
            AttributeCollection changedAttributes = new AttributeCollection();

            foreach (MDM.BusinessObjects.Attribute attribute in attributes)
            {
                if (attribute.Action == ObjectAction.Create
                    || attribute.Action == ObjectAction.Update
                    || attribute.Action == ObjectAction.Delete)
                {
                    changedAttributes.Add(attribute);
                    hasAttributeChanges = true;
                }
            }

            return changedAttributes;
        }

        /// <summary>
        /// Returns source flag string value for 'AttributeValueSource' enum
        /// </summary>
        /// <param name="sourceFlag">'AttributeValueSource' enum for which we need coressponding string</param>
        /// <returns>String value for a given 'AttributeValueSource' enum</returns>
        public static String GetSourceFlagString(AttributeValueSource sourceFlag)
        {
            String sourceFlagString = "O";
            if (sourceFlag == AttributeValueSource.Inherited)
                sourceFlagString = "I";

            return sourceFlagString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFlagString"></param>
        /// <returns></returns>
        public static AttributeValueSource GetSourceFlagEnum(String sourceFlagString)
        {
            AttributeValueSource sourceFlag = AttributeValueSource.Overridden;

            if (!String.IsNullOrWhiteSpace(sourceFlagString) && sourceFlagString.ToLower().Equals("i"))
            {
                sourceFlag = AttributeValueSource.Inherited;
            }

            return sourceFlag;
        }

        /// <summary>
        /// Gives the Search Operator for a given 'SearchOperator' Enum
        /// </summary>
        /// <param name="searchOperator">SearchOperator enum for which we need coressponding string</param>
        /// <param name="escapeSpecialCharacters">Flag that specifies whether special characters should be escaped</param>
        /// <returns>String value corresponding to SearchOperator enum</returns>
        public static String GetOperatorString(SearchOperator searchOperator, Boolean escapeSpecialCharacters = true)
        {
            String operatorString = String.Empty;

            switch (searchOperator)
            {
                case SearchOperator.Contains:
                    operatorString = "contains";
                    break;
                case SearchOperator.EqualTo:
                    operatorString = "=";
                    break;
                case SearchOperator.GreaterThan:
                    operatorString = escapeSpecialCharacters ? "&gt;" : ">";
                    break;
                case SearchOperator.GreaterThanOrEqualTo:
                    operatorString = escapeSpecialCharacters ? "&gt;=" : ">=";
                    break;
                case SearchOperator.HasNoValue:
                    operatorString = "has no value";
                    break;
                case SearchOperator.HasValue:
                    operatorString = "has value";
                    break;
                case SearchOperator.In:
                    operatorString = "in";
                    break;
                case SearchOperator.LessThan:
                    operatorString = escapeSpecialCharacters ? "&lt;" : "<";
                    break;
                case SearchOperator.LessThanOrEqualTo:
                    operatorString = escapeSpecialCharacters ? "&lt;=" : "<=";
                    break;
                case SearchOperator.NotContains:
                    operatorString = "not contains";
                    break;
                case SearchOperator.NotIn:
                    operatorString = "not in";
                    break;
                case SearchOperator.Like:
                    operatorString = "like";
                    break;
            }

            return operatorString;
        }

        /// <summary>
        /// Gives the Search Operator for a given 'SearchOperator' Enum without escaping special characters
        /// </summary>
        /// <param name="searchOperator">SearchOperator enum for which we need coressponding string</param>
        /// <returns>String value corresponding to SearchOperator enum</returns>
        public static String GetOperatorStringUnescaped(SearchOperator searchOperator)
        {
            String operatorString = String.Empty;
            
            switch (searchOperator)
            {
                case SearchOperator.GreaterThan:
                    operatorString = ">";
                    break;
                case SearchOperator.GreaterThanOrEqualTo:
                    operatorString = ">=";
                    break;
                case SearchOperator.LessThan:
                    operatorString = "<";
                    break;
                case SearchOperator.LessThanOrEqualTo:
                    operatorString = "<=";
                    break;
                default:
                    operatorString = GetOperatorString(searchOperator);
                    break;
            }

            return operatorString;
        }

        /// <summary>
        /// Returns the Enum value for a specif operator
        /// </summary>
        /// <param name="searchOperatorString">Operator for which we need corresponding enum value</param>
        /// <returns>SearchOperator enum value</returns>
        public static SearchOperator GetOperatorEnum(String searchOperatorString)
        {
            SearchOperator opr = SearchOperator.EqualTo;

            if (!String.IsNullOrWhiteSpace(searchOperatorString))
            {
                switch (searchOperatorString.ToLower())
                {
                    case "contains":
                        opr = SearchOperator.Contains;
                        break;
                    case "=":
                        opr = SearchOperator.EqualTo;
                        break;
                    case "&gt;":
                    case ">":
                        opr = SearchOperator.GreaterThan;
                        break;
                    case "&gt;=":
                    case ">=":
                        opr = SearchOperator.GreaterThanOrEqualTo;
                        break;
                    case "has no value":
                        opr = SearchOperator.HasNoValue;
                        break;
                    case "has value":
                        opr = SearchOperator.HasValue;
                        break;
                    case "in":
                        opr = SearchOperator.In;
                        break;
                    case "&lt;":
                    case "<":
                        opr = SearchOperator.LessThan;
                        break;
                    case "&lt;=":
                    case "<=":
                        opr = SearchOperator.LessThanOrEqualTo;
                        break;
                    case "not contains":
                        opr = SearchOperator.NotContains;
                        break;
                    case "not in":
                        opr = SearchOperator.NotIn;
                        break;
                    case "like":
                        opr = SearchOperator.Like;
                        break;
                }
            }
            return opr;
        }

        /// <summary>
        /// Getting module of MDMCenter.
        /// </summary>
        /// <param name="module">This parameter is specifying module.</param>
        /// <returns>Return the MDMCenterModule name</returns>
        public static MDMCenterModules GetModule(String module)
        {
            MDMCenterModules returnValue = MDMCenterModules.Unknown;

            if (!String.IsNullOrWhiteSpace(module))
            {
                switch (module.ToString().ToLower())
                {
                    case "attributematching":
                        returnValue = MDMCenterModules.AttributeMatching;
                        break;
                    case "dataqualitymanagement":
                        returnValue = MDMCenterModules.DQM;
                        break;
                    case "datavalidation":
                        returnValue = MDMCenterModules.Validation;
                        break;
                    case "attributeextraction":
                        returnValue = MDMCenterModules.AttributeExtraction;
                        break;
                    case "catalogexport":
                    case "exportschedule":
                    case "lookupexport":
                        returnValue = MDMCenterModules.Export;
                        break;
                    default:
                        returnValue = MDMCenterModules.Import;
                        break;
                }

            }
            return returnValue;
        }

        /// <summary>
        /// Getting module of MDMCenter based on JobType. This is mainly for Import / export database separation.
        /// </summary>
        /// <param name="jobType"></param>
        /// <returns>Return the MDMCenterModule name</returns>
        public static MDMCenterModules GetModule(JobType jobType)
        {
            switch (jobType)
            {
                case JobType.EntityExport:
                    return MDMCenterModules.Export;

                case JobType.DataModelImport:
                    return MDMCenterModules.DataModelImport;

                case JobType.DiagnosticReportExport:
                    return MDMCenterModules.Instrumentation;

                case JobType.DDGImport:
                    return MDMCenterModules.DDGImport;

                default:
                    return MDMCenterModules.Import;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static MDMPublisher GetMDMPublisher(MDMCenterApplication application, MDMCenterModules service)
        {
            MDMPublisher publisher = MDMPublisher.Unknown;

            if (application == MDMCenterApplication.PIM && service == MDMCenterModules.Entity)
                publisher = MDMPublisher.MDM_UIProcess;
            else if (application == MDMCenterApplication.PIM && service == MDMCenterModules.Import)
                publisher = MDMPublisher.MDM_Import;
            else if (application == MDMCenterApplication.PIM && service == MDMCenterModules.Export)
                publisher = MDMPublisher.MDM_Export;

            else if (application == MDMCenterApplication.MDMCenter && service == MDMCenterModules.Entity)
                publisher = MDMPublisher.MDM_UIProcess;
            else if (application == MDMCenterApplication.MDMCenter && service == MDMCenterModules.Import)
                publisher = MDMPublisher.MAM_Import;
            else if (application == MDMCenterApplication.MDMCenter && service == MDMCenterModules.Export)
                publisher = MDMPublisher.MDM_Export;

            else if (application == MDMCenterApplication.VendorPortal && service == MDMCenterModules.Entity)
                publisher = MDMPublisher.VendorPortal_UIProcess;
            else if (application == MDMCenterApplication.VendorPortal && service == MDMCenterModules.Import)
                publisher = MDMPublisher.VendorPortal_Import;
            else if (application == MDMCenterApplication.VendorPortal && service == MDMCenterModules.Export)
                publisher = MDMPublisher.VendorPortal_Export;

            else if (application == MDMCenterApplication.MAM && service == MDMCenterModules.Entity)
                publisher = MDMPublisher.MAM_UIProcess;
            else if (application == MDMCenterApplication.MAM && service == MDMCenterModules.Import)
                publisher = MDMPublisher.MAM_Import;
            else if (application == MDMCenterApplication.MAM && service == MDMCenterModules.Export)
                publisher = MDMPublisher.MAM_Export;

            return publisher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="minExclusive"></param>
        /// <param name="attributeDataTypeName"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static String DetermineMinValues(String minInclusive, String minExclusive, String attributeDataTypeName, Int32 precision)
        {
            String rangeFrom = String.Empty;

            if (!String.IsNullOrWhiteSpace(attributeDataTypeName))
            {
                switch (attributeDataTypeName.ToLower())
                {
                    case "integer":

                        if (!String.IsNullOrWhiteSpace(minInclusive))
                        {
                            rangeFrom = minInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(minExclusive))
                        {
                            Int32 iMin = 0;
                            Int32.TryParse(minExclusive, out iMin);
                            iMin++;
                            rangeFrom = iMin.ToString();
                        }
                        break;
                    case "fraction": //Continuing with decimal assuming that database will return min and max in decimals..
                    case "decimal":

                        if (!String.IsNullOrWhiteSpace(minInclusive))
                        {
                            rangeFrom = minInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(minExclusive))
                        {
                            Double dMin = 0;
                            Double.TryParse(minExclusive, NumberStyles.Any, CultureInfo.InvariantCulture, out dMin);
                            String difference = "0.";

                            for (Int32 i = 1; i < precision; i++)
                                difference += "0";
                            difference += "1";

                            Double valueToAddOrSubtract = 0;
                            Double.TryParse(difference, NumberStyles.Any, CultureInfo.InvariantCulture, out valueToAddOrSubtract);
                            dMin += valueToAddOrSubtract;

                            rangeFrom = dMin.ToString(CultureInfo.InvariantCulture);
                        }
                        break;

                    case "datetime":
                        if (!String.IsNullOrWhiteSpace(minInclusive))
                        {
                            rangeFrom = minInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(minExclusive))
                        {
                            DateTime dtMin;
                            if (DateTime.TryParse(minExclusive, out dtMin))
                            {
                                dtMin = dtMin.AddSeconds(1);
                                rangeFrom = dtMin.ToString();
                            }
                        }
                        break;
                    case "date":
                        if (!String.IsNullOrWhiteSpace(minInclusive))
                        {
                            rangeFrom = minInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(minExclusive))
                        {
                            DateTime dateMin;
                            if (DateTime.TryParse(minExclusive, out dateMin))
                            {
                                dateMin = dateMin.AddDays(1);
                                rangeFrom = dateMin.Date.ToString();
                            }
                        }
                        break;
                }
            }

            return rangeFrom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxInclusive"></param>
        /// <param name="maxExclusive"></param>
        /// <param name="attributeDataTypeName"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static String DetermineMaxValues(String maxInclusive, String maxExclusive, String attributeDataTypeName, Int32 precision)
        {
            String rangeTo = String.Empty;

            if (!String.IsNullOrWhiteSpace(attributeDataTypeName))
            {
                switch (attributeDataTypeName.ToLower())
                {
                    case "integer":
                        if (!String.IsNullOrWhiteSpace(maxInclusive))
                        {
                            rangeTo = maxInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(maxExclusive))
                        {
                            Int32 iMax = 0;
                            Int32.TryParse(maxExclusive, out iMax);
                            iMax--;
                            rangeTo = iMax.ToString();
                        }
                        break;
                    case "fraction": //Continuing with decimal assuming that database will return min and max in decimals..
                    case "decimal":
                        if (!String.IsNullOrWhiteSpace(maxInclusive))
                        {
                            rangeTo = maxInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(maxExclusive))
                        {
                            Double dMax = 0;
                            Double.TryParse(maxExclusive, NumberStyles.Any, CultureInfo.InvariantCulture, out dMax);
                            String difference = "0.";

                            for (Int32 i = 1; i < precision; i++)
                                difference += "0";
                            difference += "1";

                            Double valueToAddOrSubtract = 0;
                            Double.TryParse(difference, NumberStyles.Any, CultureInfo.InvariantCulture, out valueToAddOrSubtract);
                            dMax -= valueToAddOrSubtract;
                            rangeTo = dMax.ToString(CultureInfo.InvariantCulture);
                        }
                        break;

                    case "datetime":
                        if (!String.IsNullOrWhiteSpace(maxInclusive))
                        {
                            rangeTo = maxInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(maxExclusive))
                        {
                            DateTime dtMax;
                            if (DateTime.TryParse(maxExclusive, out dtMax))
                            {
                                dtMax = dtMax.Subtract(new TimeSpan(0, 0, 0, 1));
                                rangeTo = dtMax.ToString();
                            }
                        }
                        break;
                    case "date":
                        if (!String.IsNullOrWhiteSpace(maxInclusive))
                        {
                            rangeTo = maxInclusive;
                        }
                        else if (!String.IsNullOrWhiteSpace(maxExclusive))
                        {
                            DateTime dateMax;
                            if (DateTime.TryParse(maxExclusive, out dateMax))
                            {
                                dateMax = dateMax.Subtract(new TimeSpan(1, 0, 0, 0));
                                rangeTo = dateMax.Date.ToString();
                            }
                        }
                        break;
                }
            }

            return rangeTo;
        }

        /// <summary>
        /// Create EntityOperationResultCollection from given entities
        /// </summary>
        /// <param name="entities">Entities for which EntityOperationResultCollection is to be created</param>
        /// <returns>EntityOperationResultCollection initialized with entity and its attributes operation result</returns>
        public static EntityOperationResultCollection PrepareEntityOperationResultsSchema(EntityCollection entities)
        {
            EntityOperationResultCollection entityOperationResults = null;

            if (entities != null)
            {
                entityOperationResults = new EntityOperationResultCollection();

                Int64 entityIdToBeCreated = -1;

                foreach (Entity entity in entities)
                {
                    EntityOperationResult entityOperationResult = Utility.PrepareEntityOperationResultSchema(entity, entityIdToBeCreated);
                    if (entityOperationResult != null)
                    {
                        entityOperationResults.Add(entityOperationResult);
                        entityIdToBeCreated--;
                    }
                }
            }

            return entityOperationResults;
        }

        /// <summary>
        /// Prepares Entity Operation reslut schema 
        /// </summary>
        /// <param name="entity">Entity for which we need to prepare schema</param>
        /// <returns>Entity Operation result object</returns>
        public static EntityOperationResult PrepareEntityOperationResultSchema(Entity entity)
        {
            return PrepareEntityOperationResultSchema(entity, -1);
        }

        /// <summary>
        /// Removes parametes from Information, Warning and Error Collection of EOR
        /// </summary>
        /// <param name="entityOperationResult"></param>
        /// <returns></returns>
        public static void RemoveParamsFromEOR(EntityOperationResult entityOperationResult)
        {
            if (entityOperationResult != null)
            {
                // Remove params from Information
                InformationCollection informationCollection = entityOperationResult.Informations;
                if (informationCollection != null && informationCollection.Count > 0)
                {
                    informationCollection.ToList().ForEach(information => information.Params = null);
                }

                // Remove params from Warning
                WarningCollection warningCollection = entityOperationResult.Warnings;
                if (warningCollection != null && warningCollection.Count > 0)
                {
                    warningCollection.ToList().ForEach(warning => warning.Params = null);
                }

                // Remove params from Error
                ErrorCollection errorCollection = entityOperationResult.Errors;
                if (errorCollection != null && errorCollection.Count > 0)
                {
                    errorCollection.ToList().ForEach(error => error.Params = null);
                }
            }
        }

        /// <summary>
        /// Returns schema of the operationresult for the relationship objects
        /// </summary>
        /// <param name="relationships">Collection of relationships</param>
        /// <param name="relationshipId">Relationship Id</param>
        /// <returns>Collection of relationship Operationresult</returns>
        public static RelationshipOperationResultCollection PrepareRelationshipOperationResultsSchema(RelationshipCollection relationships, Int32 relationshipId)
        {
            RelationshipOperationResultCollection relationshipOperationResultCollection = null;

            if (relationships != null)
            {
                relationshipOperationResultCollection = new RelationshipOperationResultCollection();
                foreach (Relationship relationship in relationships)
                {
                    relationship.Id = relationshipId;

                    RelationshipOperationResult relationshipOperationResult = new RelationshipOperationResult(relationshipId, relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName, relationship.ReferenceId);
                    relationshipOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

                    if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                    {
                        foreach (Attribute attr in relationship.RelationshipAttributes)
                        {
                            AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attr.Id, attr.Name, attr.LongName, attr.AttributeModelType, attr.Locale);
                            relationshipOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                        }
                    }

                    if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                    {
                        relationshipOperationResult.RelationshipOperationResultCollection = PrepareRelationshipOperationResultsSchema(relationship.RelationshipCollection, relationshipId);
                    }

                    relationshipOperationResultCollection.Add(relationshipOperationResult);
                    relationshipId++;
                }
            }
            return relationshipOperationResultCollection;
        }

        /// <summary>
        /// Map object action to corresponding user Action
        /// </summary>
        /// <param name="objectAction">Indicates the action of object.</param>
        /// <returns>Returns the user action.</returns>
        public UserAction ObjectActionToUserActionMap(ObjectAction objectAction)
        {
            UserAction userAction = UserAction.Unknown;

            switch (objectAction)
            {
                case ObjectAction.Create:
                    userAction = UserAction.Add;
                    break;
                case ObjectAction.Update:
                    userAction = UserAction.Update;
                    break;
                case ObjectAction.Delete:
                    userAction = UserAction.Delete;
                    break;
                case ObjectAction.Read:
                    userAction = UserAction.View;
                    break;
                default:
                    userAction = UserAction.Unknown;
                    break;
            }

            return userAction;
        }

        /// <summary>
        /// Get errors from error collection as a string with separated with the "Delimiter" value
        /// </summary>
        /// <param name="errors">Error collection to merge</param>
        /// <param name="delimiter">Delimiter to be used to separate the messages</param>
        /// <returns>Merged errors</returns>
        public static String GetErrorString(ErrorCollection errors, String delimiter)
        {
            StringBuilder sb = new StringBuilder();
            String errorString = String.Empty;

            if (errors != null && errors.Count > 0)
            {
                foreach (Error error in errors)
                {
                    sb.Append(errorString + delimiter + error.ErrorMessage);
                }
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Splits Objects in requested batch
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="sourceCollection">Collection Objects to split</param>
        /// <param name="batchSize">size in which batch should split</param>
        /// <returns>Collection of Objects in Batch</returns>
        public static Collection<IDataModelObjectCollection> Split<T>(T sourceCollection, Int32 batchSize) where T : IDataModelObjectCollection,  new()
        {
            T masterCollection = new T();
            Collection<IDataModelObjectCollection> batchCollection = new Collection<IDataModelObjectCollection>();
            Int32 count = 0;

            //Start Batching
            foreach (var obj in sourceCollection)
            {
                masterCollection.AddDataModelObject(obj as IDataModelObject);
                count++;

                if (count < batchSize)
                {
                    continue;
                }
                else
                {
                    batchCollection.Add(masterCollection as IDataModelObjectCollection);
                    masterCollection = new T();
                    count = 0;
                }
            }

            if (masterCollection.Count > 0)
            {
                batchCollection.Add(masterCollection as IDataModelObjectCollection);
            }

            return batchCollection;
        }

        /// <summary>
        /// Add attribute operators in the attribute operator collection based on name value pair
        /// </summary>
        /// <param name="nameValuePair">Indicates the name value pair representing the name of the operator and its value</param>
        /// <param name="attributeOperators">Indicates the attribute operator collection</param>
        /// <param name="seperator">Indicates the seperator used for seperating operator name and value</param>
        public static void AddAttributeOperators(NameValueCollection nameValuePair, ref Collection<String> attributeOperators, String seperator)
        {
            foreach (String name in nameValuePair.AllKeys)
            {
                attributeOperators.Add(String.Concat(name, seperator, nameValuePair[name]));
            }
        }

        /// <summary>
        /// Validate if string provided is valid email address or not
        /// </summary>
        /// <param name="emailString">E-mail as a strign</param>
        /// <returns>[True] if email is valid, [False] otherwise</returns>
        public static Boolean IsValidEmail(String emailString)
        {
            if (String.IsNullOrEmpty(emailString))
                return false;

            // suggested by Microsoft approach to validate emails
            // https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx

            try
            {
                emailString = Regex.Replace(emailString, @"(@)(.+)$",
                    match =>
                    {
                        // Use IdnMapping class to convert Unicode domain names.
                        // IdnMapping class with default property values.
                        IdnMapping idn = new IdnMapping();

                        String domainName = match.Groups[2].Value;

                        domainName = idn.GetAscii(domainName);

                        return match.Groups[1].Value + domainName;
                    }, RegexOptions.None, TimeSpan.FromMilliseconds(200));
           
                return Regex.IsMatch(emailString,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// compare properties of generic types string or int or long
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributeToCompare"></param>
        /// <param name="supersetProperty"></param>
        /// <param name="subsetProperty"></param>
        /// <param name="operationResult"></param>
        /// <returns></returns>
        public static Boolean BusinessObjectPropertyCompare<T>(String attributeToCompare, T supersetProperty, T subsetProperty, OperationResult operationResult) where T: IConvertible
        {
            Boolean result = true;
            if (Convert.ToString(supersetProperty) != Convert.ToString(subsetProperty))
            {
                result = false;
                operationResult.AddOperationResult("-1", String.Format(_errorMessage, attributeToCompare, supersetProperty.ToString(), subsetProperty.ToString()), OperationResultType.Error);
            }

            return result;

        }

        /// <summary>
        /// compare 2 attribute collections
        /// </summary>
        /// <param name="superSetCollection"></param>
        /// <param name="subSetCollection"></param>
        /// <param name="operationResult"></param>
        /// <returns>true if 2 attribute collection are equal, otherwise return false and update entity operation result with error message</returns>
        public static Boolean BusinessObjectAttributeOperationResultCompare(AttributeCollection superSetCollection, AttributeCollection subSetCollection, EntityOperationResult operationResult)
        {
            Boolean result = true;

            AttributeOperationResultCollection attributeOperationResults = superSetCollection.GetSuperSetOperationResult(subSetCollection, operationResult);

            if (attributeOperationResults.OperationResultStatus != OperationResultStatusEnum.Successful)
            {
                result = false;
                operationResult.SetAttributeOperationResults(attributeOperationResults);
            }

            return result;
        }

        /// <summary>
        /// compare 2 relationshipCollection using OperationResult
        /// </summary>
        /// <param name="supersetRelationshipCollection"></param>
        /// <param name="subsetRelationshipCollection"></param>
        /// <param name="operationResult"></param>
        /// <param name="compareIds"></param>
        /// <returns>true and successful operationresult if 2 objects are identical, otherwise false and update relationship operation result with error message</returns>
        public static Boolean BusinessObjectRelationshipOperationResultCompare(RelationshipCollection supersetRelationshipCollection, RelationshipCollection subsetRelationshipCollection, EntityOperationResult operationResult, Boolean compareIds = false)
        {
            Boolean result = true;
            RelationshipOperationResultCollection relationshipOperationResults = supersetRelationshipCollection.GetSuperSetOfOperationResult(subsetRelationshipCollection, compareIds);
            if (relationshipOperationResults.OperationResultStatus != OperationResultStatusEnum.Successful)
            {
                result = false;
                operationResult.SetRelationshipOperationResults(relationshipOperationResults);
            }

            return result;
        }

        /// <summary>
        /// Compare 2 attribute collections using OperationResults
        /// </summary>
        /// <param name="superSetCollection"></param>
        /// <param name="subSetCollection"></param>
        /// <param name="operationResult"></param>
        /// <param name="compareIds"></param>
        /// <returns>true and sucessful operationresult if 2 attributes model are same, otherwise false and update operation result with error message</returns>
        public static Boolean BusinessObjectAttributeOperationResultCompare(AttributeModelCollection superSetCollection, AttributeModelCollection subSetCollection, OperationResult operationResult, Boolean compareIds = false)
        {
            //dont overwrite operationResult here - only update it
            var attributeModelOperationResult = new OperationResult();

            superSetCollection.GetSuperSetOperationResult(subSetCollection, attributeModelOperationResult, compareIds);
            
            foreach (var error in attributeModelOperationResult.Errors)
            {
                operationResult.Errors.Add(error);
            }

            operationResult.RefreshOperationResultStatus();

            if (operationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helper  method for comparing hierarchy relationship using OperationResults
        /// for now there is no HierarchyRelationshipOperationResult so operationresult is used ; will update this when HierarchyRelationshipOperationResult is introduced
        /// <param name="supersetHierarchyRelationshipCollection"></param>
        /// <param name="subsetHierarchyRelationshipCollection"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        /// </summary>
        public static Boolean BusinessObjectHierarchyRelationshipOperationResultCompare(HierarchyRelationshipCollection supersetHierarchyRelationshipCollection, HierarchyRelationshipCollection subsetHierarchyRelationshipCollection, EntityOperationResult entityOperationResult, Boolean compareIds = false)
        {
            var hierarchyRelationshipOperationResult = new OperationResult();
            supersetHierarchyRelationshipCollection.GetSuperSetOfOperationResult(subsetHierarchyRelationshipCollection, hierarchyRelationshipOperationResult, compareIds);
            hierarchyRelationshipOperationResult.RefreshOperationResultStatus();
            if (hierarchyRelationshipOperationResult.HasError)
            {

                foreach (var error in hierarchyRelationshipOperationResult.GetErrors())
                {
                    entityOperationResult.AddOperationResult("-1",error.ErrorMessage, OperationResultType.Error);
                }

                entityOperationResult.Informations.AddRange(hierarchyRelationshipOperationResult.Informations);
            }

            entityOperationResult.RefreshOperationResultStatus();

            if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supersetExtensionRelationshipCollection"></param>
        /// <param name="subsetExtensionRelationshipCollection"></param>
        /// <param name="entityOperationResult"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public static Boolean BusinessObjectExtensionRelationshipOperationResultCompare(ExtensionRelationshipCollection supersetExtensionRelationshipCollection, ExtensionRelationshipCollection subsetExtensionRelationshipCollection, OperationResult entityOperationResult, Boolean compareIds = false)
        {
            var extensionRelationshipResult = new OperationResult();
            supersetExtensionRelationshipCollection.GetSuperSetOperationResult(subsetExtensionRelationshipCollection, extensionRelationshipResult, compareIds);
            extensionRelationshipResult.RefreshOperationResultStatus();
            if (extensionRelationshipResult.HasError)
            {
                foreach (var error in extensionRelationshipResult.GetErrors())
                {
                    entityOperationResult.AddOperationResult("-1", error.ErrorMessage, OperationResultType.Error);
                }

            }

                entityOperationResult.Informations.AddRange(extensionRelationshipResult.Informations);


            entityOperationResult.RefreshOperationResultStatus();
            if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if any simple attributes have multiple values.
        /// </summary>
        /// <param name="entity">Indicates the entity object.</param>
        /// <returns>Returns entity operation result specifying whether any simple attributes have multiple values.</returns>
        public static EntityOperationResult CheckMultipleValuesForSimpleAttributes(Entity entity)
        {
            EntityOperationResult entityOperationResult = new EntityOperationResult();
            AttributeOperationResultCollection attributeORs = new AttributeOperationResultCollection();
            if (entity != null && entity.Attributes != null && entity.Attributes.Count > 0)
            {
                foreach(Attribute attribute in entity.Attributes)
                {
                    IValueCollection attributeCurrentValues = attribute.GetCurrentValuesInvariant();

                    if (!attribute.IsCollection && attributeCurrentValues != null && attributeCurrentValues.Count > 1)
                    {
                        String warningCode = "114082";

                        String warningMessage = String.Format("Multiple Value(s) found for the non-collection attribute: {0}", attribute.LongName);
                        
                        AttributeOperationResult attributeOR = new AttributeOperationResult(attribute.Id, attribute.Name, 
                                                                                            attribute.LongName, attribute.AttributeModelType, attribute.Locale);

                        Warning warning = new Warning(warningCode, warningMessage, new Collection<Object>(){attribute.LongName});
                        attributeOR.Warnings.Add(warning);
                        
                        attributeORs.Add(attributeOR);
                    }
                }
            }

            entityOperationResult.AttributeOperationResultCollection = attributeORs;
            return entityOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathString"></param>
        /// <returns></returns>
        public static Boolean FilePathHasInvalidFileNameChars(String filePathString)
        {
            // Get a list of invalid file characters.
            Char[] invalidFileChars = Path.GetInvalidFileNameChars();

            Boolean result = false;
            if (String.IsNullOrEmpty(filePathString))
            {
                return result;
            }

            if (filePathString.IndexOf('\\') >= 0 || System.IO.Path.GetFullPath(filePathString) != filePathString)
            {
                return true;
            }

            foreach (Char charAtThePoint in filePathString)
            {
                if (invalidFileChars.Contains(charAtThePoint))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FilePathHasInvalidChars(string path)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    // Careful!
                    //    Path.GetDirectoryName("C:\Directory\SubDirectory")
                    //    returns "C:\Directory", which may not be what you want in
                    //    this case. You may need to explicitly add a trailing \
                    //    if path is a directory and not a file path. As written, 
                    //    this function just assumes path is a file path.
                    string fileName = System.IO.Path.GetFileName(path);
                    string fileDirectory = System.IO.Path.GetDirectoryName(path);

                    // we don't need to do anything else,
                    // if we got here without throwing an 
                    // exception, then the path does not
                    // contain invalid characters
                }
                catch (ArgumentException)
                {
                    // Path functions will throw this 
                    // if path contains invalid chars
                    ret = true;
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns internal unique key by combining container id + entity type id
        /// </summary>
        /// <param name="containerId">Indicates the container identifier.</param>
        /// <param name="entityTypeId">Indicates the entity type identifier.</param>
        public static Int32 GetInternalUniqueKeyBasedOnParam(Int32 containerId, Int32 entityTypeId)
        {
            return (containerId << 7) + entityTypeId;
        }

        #region Private Methods

        private static EntityOperationResult PrepareEntityOperationResultSchema(Entity entity, Int64 entityIdToBeCreated = -1)
        {
            EntityOperationResult entityOperationResult = null;

            if (entity != null)
            {
                entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);

                if (entity.Id < 1)
                {
                    entity.Id = entityIdToBeCreated;
                    entityOperationResult.EntityId = entityIdToBeCreated;
                }

                entityOperationResult.ReferenceId = entity.ReferenceId;

                if (entity.Attributes != null && entity.Attributes.Count > 0)
                {
                    foreach (Attribute attr in entity.Attributes)
                    {
                        AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attr.Id, attr.Name, attr.LongName, attr.AttributeModelType, attr.Locale);
                        entityOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }

                if (entity.Relationships != null && entity.Relationships.Count > 0)
                {
                    //Declare relationship Id which will be incremented and assigned to each relationship operation result so that we can identify the operation result uniquely
                    Int32 relationshipId = 1;

                    entityOperationResult.RelationshipOperationResultCollection = PrepareRelationshipOperationResultsSchema(entity.Relationships, relationshipId);
                }
            }

            return entityOperationResult;
        }

        #endregion

        #endregion
    }
}