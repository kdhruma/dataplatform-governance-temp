using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
namespace MDM.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicHashingHelper
    {
        #region Private constants

       
        #endregion

        #region Methods

        #region Public Methods

          /// <summary>
        /// Implements MD5 hashing on the string using UTF8 encoding
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static String MD5HashString(String clearText)
        {
            String hashedString = String.Empty;

            if (!String.IsNullOrEmpty(clearText))                       
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(clearText)))
                {
                    hashedString = String.Join(String.Empty, new MD5CryptoServiceProvider().ComputeHash(ms).Select(x => x.ToString("X2")));                    
                }
            }
            return hashedString;
        }
        
        #endregion

        #region Private Methods

       
        
        #endregion

        #endregion
    }
}
