using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Diagnostics;

namespace MDM.Utility
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.ExceptionManager;
    using MDM.ExceptionManager.Handlers;

    /// <summary>
    /// Provides utility methods for Claims based authentication.
    /// </summary>
    public static class ClaimsBasedAuthenticationHelper
    {
        #region Public Methods

        /// <summary>
        /// Returns the MDM role names based on the claims identity.
        /// </summary>
        public static Collection<String> GetUserRoleNamesBasedOnClaims(ClaimsIdentity claimsIdentity)
        {
            // Get user's groups from Claims identity.
            Collection<String> groupsInClaims = GetGroupsFromClaimsIdentity(claimsIdentity);

            // Get Mapped roles for MDM
            Collection<String> mdmRoles = GetMDMRolesBasedOnClaims(groupsInClaims);
            return mdmRoles;
        }

        /// <summary>
        /// Returns an XML representation of the specified claims identity.  
        /// </summary>
        /// <param name="claimsIdentity">The claims identity to be converted into XML</param>
        /// <returns>An XML representation of the ClaimsIdentity in String format</returns>
        public static String ConvertClaimsToXML(ClaimsIdentity claimsIdentity)
        {
            IEnumerable<Claim> claims = claimsIdentity.Claims;
            String valueXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("ClaimsIdentity");
                    xmlWriter.WriteAttributeString("Name", claimsIdentity.Name);

                    xmlWriter.WriteStartElement("Claims");
                    foreach (Claim claim in claims)
                    {
                        xmlWriter.WriteStartElement("Claim");
                        xmlWriter.WriteAttributeString("Type", claim.Type);
                        xmlWriter.WriteAttributeString("Value", claim.Value);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();

                    //Value node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    //get the actual XML
                    valueXml = sw.ToString();
                }
            }
            return valueXml;
        }

        /// <summary>
        /// Writes the specified message into Event log and trace files based on the trace event type for Security service.
        /// </summary>
        public static void WriteLog(TraceEventType traceEventType, String message)
        {
            EventLogHandler eventLogHandler = new EventLogHandler();

            switch (traceEventType)
            {
                case TraceEventType.Error:
                    eventLogHandler.WriteErrorLog(message, 0);
                    break;
                case TraceEventType.Warning:
                    eventLogHandler.WriteWarningLog(message, 0);
                    break;
                case TraceEventType.Information:
                    eventLogHandler.WriteInformationLog(message, 0);
                    break;
            }

            MDMTraceHelper.EmitTraceEvent(traceEventType, message, MDMTraceSource.SecurityService);
        }

        /// <summary>
        /// Returns the ClaimsBasedAuthenticationType configured for the application.  
        /// </summary>
        /// <returns></returns>
        public static ClaimsBasedAuthenticationType GetClaimsBasedAuthenticationType()
        {
            // By default ClaimsBasedAuthenticationType is disabled.
            ClaimsBasedAuthenticationType claimsBasedAuthenticationType = ClaimsBasedAuthenticationType.None;
            try
            {
                String claimsBasedAuthenticationTypeStr = AppConfiguration.GetSetting("ClaimsBasedAuthenticationType");
                Boolean isParsed = Enum.TryParse(claimsBasedAuthenticationTypeStr, out claimsBasedAuthenticationType);
                if (!isParsed)
                {
                    WriteLog(TraceEventType.Error,
                        String.Format("Missing or incorrect value for ClaimsBasedAuthenticationType. Will assume value as {0} and continue", claimsBasedAuthenticationType));
                }
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message);
                WriteLog(TraceEventType.Error,
                    String.Format("Missing or incorrect value for ClaimsBasedAuthenticationType. Will assume value as {0} and continue", claimsBasedAuthenticationType));
            }
            return claimsBasedAuthenticationType;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the MDMRoles based on the external roles specified.
        /// </summary>
        private static Collection<String> GetMDMRolesBasedOnClaims(Collection<String> externalRolesThroughClaims)
        {
            Collection<String> mdmRolesMappedForClaimRoles = new Collection<String>();

            RoleMappingCollection roleMappingCollection = AppConfigurationHelper.GetRoleMappings();
            if (roleMappingCollection != null && roleMappingCollection.Count > 0)
            {
                foreach (RoleMapping roleMapping in roleMappingCollection)
                {
                    if (externalRolesThroughClaims.Contains(roleMapping.ExternalRole))
                    {
                        if (roleMapping.MDMRoles == null || roleMapping.MDMRoles.Count == 0)
                        {
                            WriteLog(TraceEventType.Warning, String.Format("The external role {0} will be skipped as corresponding MDM roles are not mapped",
                                roleMapping.ExternalRole));
                            continue;
                        }
                        else
                        {
                            foreach(String mdmRole in roleMapping.MDMRoles)
                            {
                                if (!mdmRolesMappedForClaimRoles.Contains(mdmRole))
                                    mdmRolesMappedForClaimRoles.Add(mdmRole);
                            }
                        }
                    }
                }
            }
            else
            {
                WriteLog(TraceEventType.Error, "The role mapping configuration for claims based authentication is unavailable");
            }
            return mdmRolesMappedForClaimRoles;
        }

        /// <summary>
        /// Returns the external group names from the claims identity.
        /// </summary>
        /// <param name="claimsIdentity">The claims token that is received</param>
        /// <returns>A collection of external role names</returns>
        private static Collection<String> GetGroupsFromClaimsIdentity(ClaimsIdentity claimsIdentity)
        {
            Collection<String> claimsRolesForUser = new Collection<String>();

            ClaimTypesMapping claimTypesMapping = AppConfigurationHelper.GetClaimTypesMapping();
            if (claimTypesMapping != null && !String.IsNullOrWhiteSpace(claimTypesMapping.ExternalGroupName))
            {
                IEnumerable<Claim> groupClaims = claimsIdentity.FindAll(claimTypesMapping.ExternalGroupName);
                foreach (Claim groupClaim in groupClaims)
                {
                    claimsRolesForUser.Add(groupClaim.Value);
                }
            }
            else
            {
                WriteLog(TraceEventType.Error, "The claim type for external group name mapping is not configured");
            }
            return claimsRolesForUser;
        }

        #endregion
    }
}
