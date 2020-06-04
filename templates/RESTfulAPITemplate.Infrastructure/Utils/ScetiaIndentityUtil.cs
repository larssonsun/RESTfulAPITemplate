using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Infrastructure.Util
{
    public class ScetiaIndentityUtil : IScetiaIndentityUtil
    {
        private IConfiguration _config;

        public ScetiaIndentityUtil(IConfiguration config)
        {
            _config = config;
        }
        public bool ValidatePassword(string hashedPasswordFromDb, string saltFromDb, string providedPassword)
        {
            string hashedPassowrd = EncryptPassword(providedPassword, 2, saltFromDb);
            return hashedPasswordFromDb == hashedPassowrd;
        }
        private string EncryptPassword(string providedPassword, int passwordFormat, string salt)
        {
            if (passwordFormat == 0) // MembershipPasswordFormat.Clear
                return providedPassword;

            byte[] bIn = Encoding.Unicode.GetBytes(providedPassword);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bRet = null;

            if (passwordFormat == 1)
            { // MembershipPasswordFormat.Hashed 
                HashAlgorithm hm = HashAlgorithm.Create("SHA1");
                if (hm is KeyedHashAlgorithm)
                {
                    KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
                    if (kha.Key.Length == bSalt.Length)
                    {
                        kha.Key = bSalt;
                    }
                    else if (kha.Key.Length < bSalt.Length)
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                        kha.Key = bKey;
                    }
                    else
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        for (int iter = 0; iter < bKey.Length;)
                        {
                            int len = Math.Min(bSalt.Length, bKey.Length - iter);
                            Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
                            iter += len;
                        }
                        kha.Key = bKey;
                    }
                    bRet = kha.ComputeHash(bIn);
                }
                else
                {
                    byte[] bAll = new byte[bSalt.Length + bIn.Length];
                    Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                    Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                    bRet = hm.ComputeHash(bAll);
                }
            }
            else //MembershipPasswordFormat.Encrypted, aka 2
            {
                byte[] bEncrypt = new byte[bSalt.Length + bIn.Length];
                Buffer.BlockCopy(bSalt, 0, bEncrypt, 0, bSalt.Length);
                Buffer.BlockCopy(bIn, 0, bEncrypt, bSalt.Length, bIn.Length);

                // Distilled from MachineKeyConfigSection EncryptOrDecryptData function, assuming AES algo and paswordCompatMode=Framework20 (the default)
                using (var stream = new MemoryStream())
                {
                    // AesCryptoServiceProvider AES
                    // larsson in scetia proj the encrypt method is 3DES
                    var tripleDES = new TripleDESCryptoServiceProvider();
                    tripleDES.Key = HexStringToByteArray(_config["MachineKey:DecryptionKey"]);
                    tripleDES.GenerateIV();
                    tripleDES.IV = new byte[tripleDES.IV.Length];
                    using (var transform = tripleDES.CreateEncryptor())
                    {
                        using (var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                        {
                            stream2.Write(bEncrypt, 0, bEncrypt.Length);
                            stream2.FlushFinalBlock();
                            bRet = stream.ToArray();
                        }
                    }
                }
            }

            return Convert.ToBase64String(bRet);
        }
        private static byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}