using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace TAS.Services.Common
{
    public static class CryptoManager
    {
        private static readonly string _key = ConfigurationData.NotificationEncryptionKey;
        private static readonly Encoding _encoding = Encoding.ASCII;
        private static readonly IBlockCipher _blockCipher = new AesEngine();
        private static PaddedBufferedBlockCipher _cipher;
        private static IBlockCipherPadding _padding = new Pkcs7Padding();


        public static void SetPadding(IBlockCipherPadding padding)
        {
            if (padding != null)
                _padding = padding;
        }

        public static string Encrypt(string plain)
        {
            byte[] result = BouncyCastleCrypto(true, _encoding.GetBytes(plain), _key);
            return Convert.ToBase64String(result);
        }

        public static string Decrypt(string cipher)
        {
            byte[] result = BouncyCastleCrypto(false, Convert.FromBase64String(cipher), _key);
            return _encoding.GetString(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="forEncrypt"></param>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="CryptoException"></exception>
        private static byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, string key)
        {
            try
            {
                _cipher = _padding == null ? new PaddedBufferedBlockCipher(_blockCipher) : new PaddedBufferedBlockCipher(_blockCipher, _padding);
                byte[] keyByte = _encoding.GetBytes(key);
                _cipher.Init(forEncrypt, new KeyParameter(keyByte));
                return _cipher.DoFinal(input);
            }
            catch (CryptoException ex)
            {
                throw new CryptoException(ex.Message);
            }
        }
    }
}
