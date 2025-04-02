using MedHelpAuthorizations.Application.Configurations;
using MedHelpAuthorizations.Application.Multitenancy;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedHelpAuthorizations.Infrastructure.Services.Cryptography
{
    public class TenantCryptographyService : ITenantCryptographyService
    {
        private string _secretKey = string.Empty;
        private static byte[] _iv = null;
        private static byte[] _key = null;
        public TenantCryptographyService(IOptions<AppConfiguration> appConfig)
        {
            _secretKey = appConfig.Value.Secret;
        }
        private byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }
        private static byte[] _IV =
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        };
        public string Encrypt(string tenantId, int clientId)
        {
            string plainText = $"{tenantId};{clientId}";

            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(_secretKey);
            aes.IV = _IV;
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(Encoding.Unicode.GetBytes(plainText));
            cryptoStream.FlushFinalBlock();
            return Convert.ToHexString(output.ToArray());
        }
        public Tuple<string, int> Decrypt(string encryptedClientTenantId)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(_secretKey);
            aes.IV = _IV;
            using MemoryStream input = new(Convert.FromHexString(encryptedClientTenantId));
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            cryptoStream.CopyTo(output);

            string decryptedString = Encoding.Unicode.GetString(output.ToArray());

            var decryptedValues = decryptedString.Split(";");
            return Tuple.Create<string, int>(decryptedValues[0], Convert.ToInt32(decryptedValues[1]));
        }

        public static byte[] GenerateAESKey()
        {
            var rnd = new RNGCryptoServiceProvider();
            var b = new byte[16];
            rnd.GetNonZeroBytes(b);
            return b;
        }
    }

}

