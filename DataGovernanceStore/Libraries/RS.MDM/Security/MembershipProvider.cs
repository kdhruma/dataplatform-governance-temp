
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using MDM.Utility;
namespace RS.MDM.Security
{

    
    /// <summary>
    /// Defines the contract that ASP.NET and WCF Services implements to provide membership services
    ///     using RS.MDM membership provider.
    /// </summary>
    public sealed class MembershipProvider : System.Web.Security.MembershipProvider
    {
        #region Fields

        /// <summary>
        /// field for the application name
        /// </summary>
        private string _applicationName = "Riversand MDM Framework";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
            set
            {
                this._applicationName = value;
            }
        }

        /// <summary>
        /// Gets a flag that indicates if the password can be reset
        /// </summary>
        /// <exclude/>
        public override bool EnablePasswordReset
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a flag to indicates if the password can be retrieved
        /// </summary>
        /// <exclude/>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value that indicates the maximum number of invalid password attempts
        /// </summary>
        /// <exclude/>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// Gets a value that indicates the minimum required non-alphanumberic characters in the password
        /// </summary>
        /// <exclude/>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets a value that indicates the minimum required characters in the password
        /// </summary>
        /// <exclude/>
        public override int MinRequiredPasswordLength
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets a value that indicates the time for attempting the next login after account has been locked out.
        /// </summary>
        /// <exclude/>
        public override int PasswordAttemptWindow
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets a value that indicates the password format
        /// </summary>
        /// <exclude/>
        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return System.Web.Security.MembershipPasswordFormat.Clear;
            }
        }

        /// <summary>
        /// Gets a value that indicates the regular expression to check the password strength
        /// </summary>
        /// <exclude/>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value to indicate if question and answer is required to retrieve a password
        /// </summary>
        /// <exclude/>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value to indicates if the user account needs a unique email
        /// </summary>
        /// <exclude/>
        public override bool RequiresUniqueEmail
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        /// <exclude/>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Processes a request to update the password question and answer for a membership
        ///     user.
        /// </summary>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        /// <returns>true if the password question and answer are updated successfully; otherwise, false.</returns>
        /// <exclude/>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A System.Web.Security.MembershipCreateStatus enumeration value indicating
        ///     whether the user was created successfully.</param>
        /// <returns>A System.Web.Security.MembershipUser object populated with the information
        ///     for the newly created user.</returns>
        /// <exclude/>
        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave
        ///     data related to the user in the database.</param>
        /// <returns>true if the user was successfully deleted; otherwise, false.</returns>
        /// <exclude/>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the
        ///     specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A System.Web.Security.MembershipUserCollection collection that contains a
        ///     page of pageSizeSystem.Web.Security.MembershipUser objects beginning at the
        ///     page specified by pageIndex.</returns>
        /// <exclude/>
        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified
        ///     user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A System.Web.Security.MembershipUserCollection collection that contains a
        ///     page of pageSizeSystem.Web.Security.MembershipUser objects beginning at the
        ///     page specified by pageIndex.</returns>
        /// <exclude/>
        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        /// <exclude/>
        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exclude/>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        /// <exclude/>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        /// <exclude/>
        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        /// <exclude/>
        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exclude/>
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        /// <exclude/>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exclude/>
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <exclude/>
        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>true if the specified username and password are valid; otherwise, false.</returns>
        public override bool ValidateUser(string username, string password)
        {
            System.Data.SqlTypes.SqlInt32 _retValue = -1;
            password = BasicHashingHelper.MD5HashString(password);
            Riversand.StoredProcedures.Security.AuthenticateUser(username, password, out _retValue);
            return (_retValue.Value > 0);
        }

        #endregion
    }
}
