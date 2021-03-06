﻿using System;
using System.Text;
using Cosmos.Encryption.Abstractions;
using Cosmos.Encryption.Core;
using Cosmos.Optionals;

namespace Cosmos.Encryption.Symmetric
{
    /// <summary>
    /// XTEA encryption provider
    /// </summary>
    // ReSharper disable once IdentifierTypo
    // ReSharper disable once InconsistentNaming
    public sealed class XTEAEncryptionProvider : ISymmetricEncryption
    {
        private XTEAEncryptionProvider() { }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt(string data, string key, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(data))
                return string.Empty;

            encoding = encoding.SafeValue();
            return Convert.ToBase64String(Encrypt(encoding.GetBytes(data), encoding.GetBytes(key)));
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt(byte[] data, string key, Encoding encoding = null)
        {
            return data.Length == 0
                ? string.Empty
                : Convert.ToBase64String(Encrypt(data, encoding.SafeValue().GetBytes(key)));
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            return data.Length == 0
                ? data
                : XTEACore.Encrypt(data, FixKey(key));
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decrypt(string data, string key, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(data))
                return data;

            encoding = encoding.SafeValue();
            return encoding.GetString(Decrypt(Convert.FromBase64String(data), encoding.GetBytes(key)));
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decrypt(byte[] data, string key, Encoding encoding = null)
        {
            if (data.Length == 0)
                return string.Empty;

            encoding = encoding.SafeValue();
            return encoding.GetString(Decrypt(data, encoding.GetBytes(key)));
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            return data.Length == 0
                ? data
                : XTEACore.Decrypt(data, key);
        }

        private static byte[] FixKey(byte[] key)
        {
            if (key.Length == 16) return key;
            byte[] fixedKey = new byte[16];
            if (key.Length < 16)
            {
                key.CopyTo(fixedKey, 0);
            }
            else
            {
                Array.Copy(key, 0, fixedKey, 0, 16);
            }

            return fixedKey;
        }
    }
}