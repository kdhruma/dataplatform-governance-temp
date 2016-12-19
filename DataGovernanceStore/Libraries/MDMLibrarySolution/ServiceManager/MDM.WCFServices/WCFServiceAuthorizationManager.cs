using System;
using System.Web.Security;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.AdminManager.Business;
    using MDM.Utility;
    using System.ServiceModel.Channels;
    using System.Net;
    using System.ServiceModel.Security;
    using System.Security.Principal;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    class WCFServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Extract the action URI from the OperationContext. Match this against the claims
            // in the AuthorizationContext.
            if (operationContext == null) return false;
            System.ServiceModel.Channels.Message request = operationContext.RequestContext.RequestMessage;
            if (request == null) return false;
            // Get the 'token' out of the header
            HttpRequestMessageProperty prop = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            string token = prop.Headers["token"];

            if (!String.IsNullOrEmpty(token))
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null && !ticket.Expired)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                // if the custom token is not there..look for user credentials.
                String authorization = prop.Headers[HttpRequestHeader.Authorization];
                if (string.IsNullOrEmpty(authorization) == false)
                {
                    // remove the "X-SAML " out of the string...
                    string authType = authorization.Substring(0, 6);
                    authType = authType.Trim();
                    if (string.Compare(authType, "X-SAML", true) == 0)
                    {
                        {
                            // log and throw
                            return true;
                        }
                    }
                    else if (string.Compare(authType.Trim(), "Basic", true) == 0)
                    {
                        return AuthorizeUser(authorization);
                    }
                    return true;
                }
                else
                {
                    // not authorization header..log and throw..
                    return false;
                    //throw new HttpException(403, "Unauthroized request. Only Basic and SAML authentication is supported");
                }
            }
        }

        private bool BasicAuthentication(SecurityMessageProperty securityProps)
        {
            bool result = false;
            if (securityProps == null)
            {
                // log and throw..
                return result;
            }
            // authenticate the client user. Reuse the method already available used for the forensics queries.
            // Get and save the current principal
            IPrincipal authenticatedPrincipal = System.Threading.Thread.CurrentPrincipal;

            // If the current one is generic principal, create a windows principal using the service context.
            if (!(authenticatedPrincipal is WindowsPrincipal) && (securityProps.ServiceSecurityContext.WindowsIdentity != null))
            {
                System.Threading.Thread.CurrentPrincipal = new WindowsPrincipal(securityProps.ServiceSecurityContext.WindowsIdentity);
            }
            //if (ClientUserAuthentication.Instance.IsClientUserAuthorized())
            {
                result = true;
            }
            // reset the current principalto the original value.
            System.Threading.Thread.CurrentPrincipal = authenticatedPrincipal;
            return result;
        }

        private Boolean AuthorizeUser(String authorization)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();  
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            authorization = authorization.Substring(6);
            // Decode the string..
            byte[] encData_byte = Convert.FromBase64String(authorization);

            Int32 charCount = utf8Decode.GetCharCount(encData_byte, 0, encData_byte.Length);    

            char[] decoded_char = new char[charCount];

            utf8Decode.GetChars(encData_byte, 0, encData_byte.Length, decoded_char, 0);                   

            String result = new String(decoded_char);

            String[] securityCredential = result.Split(':');

            if ( securityCredential.Length == 2)
            {
                String userName = securityCredential[0];
                String password = securityCredential[1];

                UserPrincipalBL userPrincipal = new UserPrincipalBL();
                bool returnValue = userPrincipal.AuthenticateUser(userName, password);
                return returnValue;
            }
            return false;
        }
    }
}
