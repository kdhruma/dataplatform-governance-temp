using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MDM.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicEncryptionHelper
    {
        #region Private constants

        /// <summary>
        /// 
        /// </summary>
        private static readonly Rijndael EncryptionAlgorithm;

        /// <summary>
        /// 
        /// </summary>
        private const String EncryptionSeed = "MDMCenter_BasicEncrptionKey_Kizhakkekaramel_123456!@#$%^_Padinjaredath_YRIVERSANDY.01";
        //private const String EncryptionSeed = "MDM_Kizramel_123456!@#$%^_YRSY.01";

        /// <summary>
        /// 
        /// </summary>
        private const bool EncryptionEnabled = true;

        /// <summary>
        /// 
        /// </summary>
        private static readonly byte[] Salt = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        #endregion

        #region Static Constructors

        static BasicEncryptionHelper()
        {
            EncryptionAlgorithm = Rijndael.Create();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            // - Turn the input string into a byte array. 
            // - Turn the password into Key and IV (Initialization Vector) (Using salt to make it harder to guess our key using a dictionary attack)
            // - Get the key/IV and do the encryption using the private encrypt method below. 
            // - Turn the resulting byte array into a string. 

            var pdb = new PasswordDeriveBytes(EncryptionSeed, Salt);

            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            byte[] keyBytes = pdb.GetBytes(32); //encryptionAlgorithm.KeySize);
            byte[] ivBytes = pdb.GetBytes(16); //encryptionAlgorithm.BlockSize);

            byte[] encryptedData = EncryptString(clearBytes, keyBytes, ivBytes);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            // - Turn the input string into a byte array.
            // - Turn the password into Key and IV (Initialization Vector) 
            //   (Using salt to make it harder to guess our key using a dictionary attack)
            // - Get the key/IV and do the decryption using the private decrypt method below. 
            // - Turn the resulting byte array into a string. 

            var pdb = new PasswordDeriveBytes(EncryptionSeed, Salt);

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = pdb.GetBytes(32); //encryptionAlgorithm.KeySize);
            byte[] ivBytes = pdb.GetBytes(16); //encryptionAlgorithm.BlockSize);

            byte[] decryptedData = DecryptString(cipherBytes, keyBytes, ivBytes);

            return Encoding.Unicode.GetString(decryptedData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static byte[] EncryptString(byte[] clearData, byte[] key, byte[] iv)
        {
            // We need the IV (Initialization Vector) because the algorithm is operating in its default 
            // mode called CBC (Cipher Block Chaining).  The IV is XORed with the first block (8 byte) 
            // of the data before it is encrypted, and then each encrypted block is XORed with the 
            // following block of plaintext. This is done to make encryption more secure.  There is also 
            // a mode called ECB which does not need an IV, but it is much less secure. 

            // Encrypt a byte array into a byte array using a key and an IV 
            
            // Steps:
            // Create a MemoryStream to accept the encrypted bytes 
            // Now set the key and the IV. 
            // Create a CryptoStream to process our data. 
            // Write the data and make it do the encryption 
            // Close the crypto stream (or do FlushFinalBlock).
            // Get the encrypted data from the MemoryStream.

            var ms = new MemoryStream();
            EncryptionAlgorithm.Key = key;
            EncryptionAlgorithm.IV = iv;

            var cs = new CryptoStream(ms, EncryptionAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            
            var encryptedData = ms.ToArray();

            return encryptedData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static byte[] DecryptString(byte[] cipherData, byte[] key, byte[] iv)
        {
            // Steps:
            // Create a MemoryStream that is going to accept the decrypted bytes 
            // Now set the key and the IV. 
            // Create a CryptoStream through which we are going to be pumping our data. 
            // Write the data and make it do the decryption 
            // Close the crypto stream (or do FlushFinalBlock). 
            // Get the decrypted data from the MemoryStream. 

            var ms = new MemoryStream();
            EncryptionAlgorithm.Key = key;
            EncryptionAlgorithm.IV = iv;

            var cs = new CryptoStream(ms, EncryptionAlgorithm.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

        #endregion

        #endregion
    }
}
