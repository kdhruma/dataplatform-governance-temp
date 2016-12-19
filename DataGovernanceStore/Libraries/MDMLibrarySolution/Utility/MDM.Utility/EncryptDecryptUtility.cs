using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MDM.Utility
{
    /// <summary>
    /// Represents the Riversand Certificate Helper class.
    /// It helps to encrypt and decrypt the data using Riversand certificate.
    /// </summary>
    public class EncryptDecryptUtility
    {
        /// <summary>
        /// Encrypt the data using riversand client certificate.
        /// The maximum allowed input decoded text is 117 bytes.
        /// </summary>
        /// <param name="decodedText">Indicates the data to encode</param>
        /// <returns>Returns the encrypted data</returns>
        /// <exception cref="ArgumentNullException">decodedText parameter is null or empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">decodedText parameter length exceeds more than 117 bytes</exception>
        public static String EncryptUsingRSClientCertificate(String decodedText)
        {
            if (String.IsNullOrWhiteSpace(decodedText))
            {
                throw new ArgumentNullException("decodedText", "decodedText cannot be null or empty");
            }
            else if (decodedText.Length > 117)
            {
                throw new ArgumentOutOfRangeException("decodedText", "decodedText parameter length must be less than 117 bytes.");
            }

            X509Certificate2 rsClientCertificate = null;
            RSACryptoServiceProvider cipher = null;
            String publicKey = String.Empty;
            Byte[] decodedTextBytes = null;
            Byte[] encodedTextBytes = null;
            String encodedTextString = String.Empty;

            rsClientCertificate = GetRiversandClientCertificate();

            if (rsClientCertificate != null)
            {
                cipher = (RSACryptoServiceProvider)rsClientCertificate.PublicKey.Key;

                decodedTextBytes = Encoding.UTF8.GetBytes(decodedText);

                encodedTextBytes = cipher.Encrypt(decodedTextBytes, false);

                encodedTextString = Convert.ToBase64String(encodedTextBytes);
            }

            return encodedTextString;
        }

        /// <summary>
        /// Decrypt the data using riversand client certificate
        /// </summary>
        /// <param name="encodedText">Indicates the data to decode</param>
        /// <returns>Returns the decrypted data</returns>
        /// <exception cref="ArgumentNullException">encodedText parameter is null or empty</exception>
        public static String DecryptUsingRSClientCertificate(String encodedText)
        {
            if (String.IsNullOrWhiteSpace(encodedText))
            {
                throw new ArgumentNullException("encodedText", "encodedText cannot be null or empty");
            }

            X509Certificate2 rsClientCertificate = null;
            RSACryptoServiceProvider cipher = null;
            String privateKey = String.Empty;
            Byte[] encodedTextBytes = null;
            Byte[] decodedTextBytes = null;
            String decodedTextString = String.Empty;

            rsClientCertificate = GetRiversandClientCertificate();

            if (rsClientCertificate != null)
            {
                cipher = (RSACryptoServiceProvider)rsClientCertificate.PrivateKey;

                encodedTextBytes = Convert.FromBase64String(encodedText);

                decodedTextBytes = cipher.Decrypt(encodedTextBytes, false);

                decodedTextString = Encoding.UTF8.GetString(decodedTextBytes);
            }

            return decodedTextString;
        }

        /// <summary>
        /// Check the encoded key is valid or not based on the Riversand certificate client.
        /// If the result is true then it is valid else not.
        /// </summary>
        /// <param name="encodedText">Indicates the encoded text</param>
        /// <returns>Returns the boolean result based on the result</returns>
        public static Boolean IsValidRsClientCertificateEncryptionKey(String encodedText)
        {
            Boolean result = true;
            try
            {
                DecryptUsingRSClientCertificate(encodedText);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        #region Helper Methods

        /// <summary>
        /// Get the Riversand certificate
        /// </summary>
        /// <returns>Returns an X.509 certificate</returns>
        private static X509Certificate2 GetRiversandClientCertificate()
        {
            X509Store store = null;
            X509Certificate2Collection rsClientCertificateCollection = null;

            String thumbprint = String.Empty;
            Int32 foundResultCount = 0;
            X509Certificate2 rsClientCertificate = null;

            thumbprint = ConfigurationManager.AppSettings["RSClientCertificateThumbprint"]; //Thumb print will be present in appconfig.

            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            //Find Riversand Certificate
            rsClientCertificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            foundResultCount = rsClientCertificateCollection.Count;

            if (foundResultCount > 0)
            {
                //Take the first certificate as there is only one certificate by thumbprint
                rsClientCertificate = rsClientCertificateCollection[0];
            }
            else
            {
                throw new ArgumentException(String.Format("Certificate Not Found For Thumb Print {0}", thumbprint), "FindByThumbprint");
            }

            return rsClientCertificate;
        }

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        /// <param name="salt">A valued used to generate a key for encryption.</param>
        public static String EncryptStringAES(String plainText, String sharedSecret, String salt)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }
            if (String.IsNullOrEmpty(sharedSecret))
            {
                throw new ArgumentNullException("sharedSecret");
            }
            if (String.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException("salt");
            }

            String outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Encoding.ASCII.GetBytes(salt));

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        /// <param name="salt">A string used to generate encryption is used for decryption.</param>
        public String DecryptStringAES(String cipherText, String sharedSecret, String salt)
        {
            if (String.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException("cipherText");
            }
            if (String.IsNullOrEmpty(sharedSecret))
            {
                throw new ArgumentNullException("sharedSecret");
            }

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            String plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Encoding.ASCII.GetBytes(salt));

                // Create the streams used for decryption.                
                Byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        /// <summary>
        /// Create HMACSHA256 Hash value for the given plain text using given shared secret key
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="sharedSecretKey"></param>
        /// <returns></returns>
        public static String CreateHMACSHA256Hash(String plainText, String sharedSecretKey)
        {
            String hashedText = String.Empty;

            if (String.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }

            sharedSecretKey = sharedSecretKey ?? String.Empty;
            
            var encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(sharedSecretKey);

            byte[] dataBytes = encoding.GetBytes(plainText);
            
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashedData = hmacsha256.ComputeHash(dataBytes);
                hashedText = Convert.ToBase64String(hashedData);
            }

            return hashedText;
        }

        #endregion

        #region Private Methods

        private static Byte[] ReadByteArray(Stream s)
        {
            Byte[] rawLength = new Byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            Byte[] buffer = new Byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        #endregion
    }
}
