using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{

    /// <summary>
    /// Interface for password reset request class.
    /// </summary>
    public interface IPasswordResetRequest
    {
        #region Properties

        /// <summary>
        /// Indicates the Id of an object
        /// </summary>
        Int32 Id { get; set; }

        /// <summary>
        /// Indicates the Id of MDMCenter user
        /// </summary>
        Int32 SecurityUserId { get; set; }

        /// <summary>
        /// Indicates the token generated for a password reset request
        /// </summary>
        String Token { get; set; }

        /// <summary>
        /// Indicates the date time stamp for a password reset request
        /// </summary>
        DateTime RequestedDateTime { get; set; }

        /// <summary>
        /// Indicates if the password is reset for the request
        /// </summary>
        Boolean IsPasswordReset { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        /// <example>
        /// Sample XML
        /// <para>
        ///  <![CDATA[
        /// <PasswordResetRequest Id="1" SecurityUserId="1" Token="ewrprsfsflffkndfnsff" RequestedDateTime="10/10/2014 10:10" IsPasswordReset="false"></PasswordResetRequest>
        ///  ]]>
        /// </para>
        /// </example>
        String ToXml();

        #endregion
    }
}
