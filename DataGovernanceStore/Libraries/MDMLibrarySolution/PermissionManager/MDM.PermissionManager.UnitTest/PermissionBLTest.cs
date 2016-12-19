using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDM.PermissionManager.UnitTest
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.EntityManager.Business;
    using MDM.AdminManager.Business;
    using MDMBO = MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Interfaces;

    using MDM.PermissionManager.Business;

    /// <summary>
    /// Summary description for PermissionBLTest
    /// </summary>
    [TestClass]
    public class PermissionBLTest
    {
        public PermissionBLTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Test Methods

        /// <summary>
        /// Get Value based Entity Permission where entity matches the application context for edit and View Permission scenarios for lookup attribute.
        /// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_Entity_Match_ApplicationContext_EditAndViewPermission()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where entity matches the application context for View Permission scenarios for lookup attribute.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_Entity_Match_ApplicationContext_ViewPermission()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get the permission where entity does not matches the application context.
        ///// In this permission definition will be empty.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ImproperValue_Entity_NotMatch_ApplicationContext()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where entity matches the application context for edit and View Permission scenarios for lookupcollection attribute.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_Entity_Match_ApplicationContext_EditAndViewPermission_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where entity matches the application context for View Permission scenarios for lookup Collection attributes.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_Entity_Match_ApplicationContext_ViewPermission_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup attribute doesnot have value for View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_NegativeTest_NoAttributeValue_ViewPermissionConfig_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup collection attribute does not have value for View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_NegativeTest_NoAttributeValue_ViewPermissionConfig_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup attribute does not have value for edit and View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_NegativeTest_NoAttributeValue_EditAndViewPermissionConfig_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup collection attribute does not have value for edit and View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_NegativeTest_NoAttributeValue_EditAndViewPermissionConfig_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where attribute value not match for edit and View Permission scenarios for lookup attribute.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ImproperValue_AttributeValue_NotMatch_EditAndViewPermissionConfig_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup collection attribute value not match for edit and View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ImproperValue_AttributeValue_NotMatch_EditAndViewPermissionConfig_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup attribute value not match for View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ImproperValue_AttributeValue_NotMatch_ViewPermissionConfig_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup collection attribute value not match for View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ImproperValue_AttributeValue_NotMatch_ViewPermissionConfig_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup collection attribute matches one of the values for edit and View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_MatchOneofRecord_EditAndViewPermissionConfig_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user2");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission where lookup collection attribute matches one of the values for View Permission scenarios.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_MatchOneofRecord_ViewPermissionConfig_LookupCollection()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission with single user multiple role for lookup attribute value.
        ///// One role has View permission, and other has View & Edit permission.
        ///// Attribute value have ViewEdit permission values.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_SingleUserMultipleRoles_WithViewEditPermission_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user6");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission with single user multiple role for lookup attribute value.
        ///// One role has View permission, and other has View & Edit permission.
        ///// Attribute value have View permission values.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_SingleUserMultipleRoles_WithViewPermission_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user7");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission with single user multiple role for lookup attribute value.
        ///// Both role have View & Edit permission.
        ///// Attribute value have ViewEdit permission values.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_SingleUserMultipleRoles_WithBothViewEditPermission_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user6");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission with single user multiple role for lookup attribute value.
        ///// Both role have View permission.
        ///// Attribute value have View permission values.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_SingleUserMultipleRoles_WithBothViewPermission_Lookup()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user7");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission for lookup attribute not mapped to style for  View Scenario.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ImproperValue_Lookup_NotMapStyle_View()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get Value based Entity Permission for lookup collection attribute not mapped to style for  View Scenario.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ImproperValue_LookupCollection_NotMapStyle_View()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user3");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get the permission where organization and catalog is not provided and the user is having role as RSAll.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_RSAll_NoOrgNoCatalog()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user1");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get the permission where organization and catalog is not provided and the user is having role as RSAll.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_RSAll_AllOrgAllCatalog_AttributeRsAll()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user1");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get the permission where organization and catalog is not provided and the user is having role as RSAll.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_RSAll_ViewAndEditPermissionConfigured()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user4");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}

        ///// <summary>
        ///// Get the permission where organization and catalog is not provided and the user is having role as RSAll.
        ///// </summary>
        //[TestMethod]
        //[DeploymentItem(@"Data\PermissionBLTest.xml")]
        //public void GetValueBasedEntityPermission_ProperValue_RSAll_ViewPermissionConfigured()
        //{
        //    Permission actualPermission = null;

        //    LoadSecurityPrincipal("user5");

        //    String methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        //    PermissionBL permissionBL = new PermissionBL();
        //    Entity entity = null;
        //    EntityOperationResult eor = null;
        //    IEntityManager iEntityManager = (IEntityManager)new EntityBL();
        //    String expectedPermission = String.Empty;

        //    entity = new Entity();
        //    eor = new EntityOperationResult();

        //    GetInputOutput(methodName, out entity, out eor, out expectedPermission);

        //    actualPermission = permissionBL.GetValueBasedEntityPermission(entity, eor, iEntityManager, callerContext);

        //    String actual = String.Empty;

        //    if (actualPermission != null)
        //    {
        //        actual = JoinUserActionCollection(actualPermission.PermissionSet);
        //    }

        //    Assert.AreEqual(expectedPermission, actual);
        //}
        #endregion

        #region Helper methods

        /// <summary>
        /// Join User Action collection to string.
        /// </summary>
        /// <param name="collection">Indicates the user action collection.</param>
        /// <returns>Returns the string representation of user action collection.</returns>
        private String JoinUserActionCollection(Collection<UserAction> collection)
        {
            String strPermissionSet = String.Empty;

            if (collection != null && collection.Count > 0)
            {
                foreach (UserAction userAction in collection)
                {
                    if (!String.IsNullOrWhiteSpace(strPermissionSet))
                    {
                        strPermissionSet += ",";
                    }

                    strPermissionSet += userAction.ToString();
                }
            }

            return strPermissionSet;
        }

        /// <summary>
        /// Get the input parameter values from Xml.
        /// </summary>
        /// <param name="methodName">Indicates the method name.</param>
        /// <param name="entity">Indicates the entity instance.</param>
        /// <param name="eor">Indicates the Entity Operation Results.</param>
        private void GetInputOutput(String methodName, out Entity entity, out EntityOperationResult eor, out String expectedPermission)
        {
            entity = null;
            eor = null;
            expectedPermission = String.Empty;

            String fileName = "PermissionBLTest.xml";

            //Get Data Xml from config file for method that we are looking at.
            String dataXml = DataReader.ReadMethodData(fileName, methodName);

            if (!String.IsNullOrWhiteSpace(dataXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(dataXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        //Read input parameters from DataXml
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Parameter")
                        {
                            if (reader.HasAttributes)
                            {
                                String paramName = reader.GetAttribute("Name");
                                if (!String.IsNullOrWhiteSpace(paramName))
                                {
                                    String value = reader.ReadInnerXml();
                                    switch (paramName.ToLower())
                                    {
                                        case "entity":
                                            entity = new Entity(value);
                                            break;
                                        case "entityoperationresult":
                                            eor = new EntityOperationResult(value);
                                            break;
                                        default:
                                            throw new Exception(String.Concat("Unknown parameter : ", paramName));
                                    }
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Output")
                        {
                            //Read output for this method.
                            expectedPermission = reader.ReadString();
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

        }

        private static void LoadSecurityPrincipal(String userName)
        {
            Int32 systemId = 0;
            // Int32 userTimeOut = 50;

            //the stamp at the time of loging in
            String timeStamp = DateTime.Now.ToString();

            //The cache key to for the user principal to be stored
            string securityPrincipalCacheKey = "SecurityPrincipal_" + userName.ToLower();

            Hashtable loginData = new Hashtable();
            loginData.Add("SystemId", systemId);
            loginData.Add("TimeStamp", timeStamp);

            //obtain the security principal
            SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();
            SecurityPrincipal currentUserSecurityPrincipal = securityPrincipalBL.Get(userName, Core.MDMCenterSystem.Web);

            ICache cacheMananger = CacheFactory.GetCache();

            String secPrincipalCacheKeyForSystemUser = "SecurityPrincipal_cfadmin_WithoutTimeStamp";
            cacheMananger.Set(secPrincipalCacheKeyForSystemUser, currentUserSecurityPrincipal, DateTime.MaxValue);

            //if (HttpContext.Current != null)
            //{
            //    //Remove cache 
            //    if (HttpContext.Current.Cache[securityPrincipalCacheKey] != null)
            //        HttpContext.Current.Cache.Remove(securityPrincipalCacheKey);

            //    //TODO:: Eventually migrate all the Cache objects into CacheManager
            //    HttpContext.Current.Cache.Insert(securityPrincipalCacheKey,
            //                            currentUserSecurityPrincipal, null,
            //                            System.Web.Caching.Cache.NoAbsoluteExpiration,
            //                            TimeSpan.FromMinutes(userTimeOut));

            //    if (currentUserSecurityPrincipal != null && currentUserSecurityPrincipal.UserIdentity.Name == userName)
            //    {
            //        HttpContext.Current.User = currentUserSecurityPrincipal;
            //    }
            //}
        }

        #endregion
    }
}