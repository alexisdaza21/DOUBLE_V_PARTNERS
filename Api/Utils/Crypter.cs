using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public class Crypter
    {

        static byte[] key = { };
        static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
        static string sEncryptionKey = "!#$a54?3";

        public string encrypt(string id)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(id);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return "Error encriptando ID del cliente: " + ex.Message;
            }
        }


        public string decrypt(string stringToDecrypt)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
              
            }
        }

        public string Encripta(string Cadena)
        {
            var obj_Sha512 = new SHA512Managed();
            byte[] byt_cadena = Encoding.ASCII.GetBytes(Cadena);
            byte[] byt_cadena_cifrada = obj_Sha512.ComputeHash(byt_cadena);
            return Convert.ToBase64String(byt_cadena_cifrada);
        }

        public string Desencripta(string Cadena)
        {
            byte[] inputBytes = Convert.FromBase64String(Cadena);
            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;
            var cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(cripto.Key, cripto.IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }
            return textoLimpio;
        }
    }
}
