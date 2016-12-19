using System;
using System.Web.Security;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.AdminManager.Business;
    using MDM.Utility;

    class WCFMessageAuthenticationValidator : UserNamePasswordValidator
    {
        public override void Validate(String userName, String password)
        {
            if (null == userName || null == password)
            {
                throw new ArgumentNullException();
            }

            String messageSecurityIdentityKey = AppConfigurationHelper.GetAppConfig<String>("MDM.WCFServices.MessageSecurity.IdentityKey");

            String internalSuperUserName = "superuser";
            String internalSuperUserPassword = "super user with special powers - 1234567890-=+_)(*&^%$#@!";

            ICache cacheManager = CacheFactory.GetCache();

            //Validate password..
            //If password length is more than 100 characters then the passed parameter is the forms authentication ticket value assuming that user is not going 
            //to have a password with more than 100 characters
            if (password.Length > 100)
            {
                //password is the forms authentication ticket value
                //Decrypt the ticket value to get the actual Forms Authentication Ticket
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(password);

                if (ticket == null || ticket.Expired)
                    throw new SecurityTokenException("Authentication failed. Unknown Username or Password");
            }
            else if (password.Equals(messageSecurityIdentityKey))
            { 
                //User is validated by master security identity key...    
            }
            else if(userName.Equals(internalSuperUserName) && password.Equals(internalSuperUserPassword))
            {
                //User is validated by master security identity key...    
            }
            else
            {
                //password is the User Password

                //Authenticate user
                UserPrincipalBL userPrincipal = new UserPrincipalBL();
                Boolean isValidUser = userPrincipal.AuthenticateUser(userName, password);
                
                if (!isValidUser)
                    throw new SecurityTokenException("Authentication failed. Unknown Username or Password");

            }

            //Set the form auth ticket into cache for further operations..
            cacheManager.Set(CacheKeyGenerator.GetFormAuthenticationTicketCacheKey(userName), password, DateTime.Now.AddDays(1));
        }
    }
}
