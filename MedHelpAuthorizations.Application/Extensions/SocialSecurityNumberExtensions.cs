using MedHelpAuthorizations.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Extensions
{
    public class SocialSecurityNumberExtensions
    {
        #region AES Encryption and Decryption Key and IV
        //private static readonly byte[] Key = Encoding.UTF8.GetBytes("MAKV2SPBNI99212");
        //private static readonly byte[] IV = Encoding.UTF8.GetBytes("bQ7dD+g3b1e5W2f4");

        private static readonly byte[] Key = new byte[] { 0x2B, 0x7E, 0x15, 0x16, 0x28, 0xAE, 0xD2, 0xA6, 0xAB, 0xF7, 0x97, 0x46, 0x2E, 0xA4, 0x99, 0x70 };
        private static readonly byte[] IV = new byte[] { 0x8E, 0x12, 0x39, 0x9C, 0x07, 0x72, 0x6F, 0x5A, 0x08, 0x3D, 0xCB, 0x29, 0x8A, 0x50, 0x17, 0x10 };
        #endregion

        /// <summary>
        /// Encrypts a Social Security Number (SSN).
        /// </summary>
        /// <param name="SSNPlainText">The plain text SSN to encrypt.</param>
        /// <returns>The encrypted SSN cipher text.</returns>
        public static string EncryptSSN(string SSNPlainText)
        {
            // Call the AES encryption method from the helper class
            var EncryptedSSN = EncryptionDecryptionHelpers.Encrypt(SSNPlainText);
            return EncryptedSSN;
        }

        /// <summary>
        /// Decrypts an encrypted Social Security Number (SSN) using.
        /// </summary>
        /// <param name="SSNCipherText">The cipher text SSN to decrypt.</param>
        /// <returns>The decrypted SSN plain text.</returns>
        public static string DecryptSSN(string SSNCipherText)
        {
            // Call the decryption method from the helper class
            var DecryptedSSN = EncryptionDecryptionHelpers.Decrypt(SSNCipherText);
            return DecryptedSSN;
        }
    }
}
