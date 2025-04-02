using MedHelpAuthorizations.Application.Helpers;

namespace MedHelpAuthorizations.Application.Extensions
{
	public class PinExtensions
	{
		#region AES Encryption and Decryption Key and IV
		private static readonly byte[] Key = new byte[] { 0x2B, 0x7E, 0x15, 0x16, 0x28, 0xAE, 0xD2, 0xA6, 0xAB, 0xF7, 0x97, 0x46, 0x2E, 0xA4, 0x99, 0x70 };
		private static readonly byte[] IV = new byte[] { 0x8E, 0x12, 0x39, 0x9C, 0x07, 0x72, 0x6F, 0x5A, 0x08, 0x3D, 0xCB, 0x29, 0x8A, 0x50, 0x17, 0x10 };
		#endregion

		/// <summary>
		/// Encrypts a PIN.
		/// </summary>
		/// <param name="pinPlainText">The plain text PIN to encrypt.</param>
		/// <returns>The encrypted PIN cipher text.</returns>
		public static string EncryptPin(string pinPlainText)
		{
			// Call the AES encryption method from the helper class
			var encryptedPin = EncryptionDecryptionHelpers.Encrypt(pinPlainText);
			return encryptedPin;
		}

		/// <summary>
		/// Decrypts an encrypted PIN.
		/// </summary>
		/// <param name="pinCipherText">The cipher text PIN to decrypt.</param>
		/// <returns>The decrypted PIN plain text.</returns>
		public static string DecryptPin(string pinCipherText)
		{
			// Call the decryption method from the helper class
			var decryptedPin = EncryptionDecryptionHelpers.Decrypt(pinCipherText);
			return decryptedPin;
		}
	}
}
