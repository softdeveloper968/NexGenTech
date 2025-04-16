using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Buffers.Text;
using System.IO;
using System.Security.Cryptography;

namespace MedHelpAuthorizations.Application.Helpers
{
    public class EncryptionDecryptionHelpers
    {
        
        /// <summary>
        /// Encrypts a plain text string to a Base64-encoded string.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <returns>The encrypted Base64-encoded string.</returns>
        public static string Encrypt(string plainText)
        {
            try
            {
                // Convert the plain text string to bytes using UTF-8 encoding
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(plainText);

                // Convert the bytes to Base64-encoded string
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and rethrow
                throw;
            }
        }

        /// <summary>
        /// Decrypts a Base64-encoded string back to a plain text string.
        /// </summary>
        /// <param name="base64">The Base64-encoded string to decrypt.</param>
        /// <returns>The decrypted plain text string.</returns>
        public static string Decrypt(string base64)
        {
            try
            {
                // Convert the Base64-encoded string to bytes
                byte[] bytes = Convert.FromBase64String(base64);

                // Convert the bytes to a plain text string using UTF-8 encoding
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and rethrow
                throw;
            }
        }
    }
}
