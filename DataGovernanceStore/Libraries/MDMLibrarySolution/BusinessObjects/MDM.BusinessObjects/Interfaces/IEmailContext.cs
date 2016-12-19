using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get email context
    /// </summary>
    public interface IEmailContext
    {
        #region Properties
        
        /// <summary>
        /// Property denoting collection of user logins to send the email
        /// </summary>
        Collection<String> ToMDMUserLoginIds { get; set; }

        /// <summary>
        /// Property denoting collection of email ids to send the email
        /// </summary>
        Collection<String> ToEmailIds { get; set; }

        /// <summary>
        /// Property denoting collection of role names to send the email
        /// </summary>
        Collection<String> ToMDMRoleNames { get; set; }

        /// <summary>
        /// Property denoting collection of user login ids to CC the email
        /// </summary>
        Collection<String> CCMDMUserLoginIds { get; set; }

        /// <summary>
        /// Property denoting collection of email ids to CC the email
        /// </summary>
        Collection<String> CCEmailIds { get; set; }

        /// <summary>
        /// Property denoting collection of role names to CC the email
        /// </summary>
        Collection<String> CCMDMRoleNames { get; set; }

        /// <summary>
        /// Property denoting the template name which is used to send the email
        /// </summary>
        String TemplateName { get; set; }

        /// <summary>
        /// Property denoting Dictionary of (Key,Value) pairs to fill in the template data
        /// </summary>
        Dictionary<String, String> TemplateData { get; set; }

        /// <summary>
        /// Property denoting whether to send one email to everyone or separate emails to each one
        /// </summary>
        Boolean SendMailPerEmailId { get; set; }

        #endregion Properties
    }
}
