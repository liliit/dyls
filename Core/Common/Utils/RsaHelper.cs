
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DYLS.Common.Utils
{
    /// <summary>  
    /// 类名：RSACrypt  
    /// 功能：RSA加密、解密、签名、验签  
    /// </summary>  
    public sealed class RsaHelper
    {

        /// <summary>
        /// 公钥
        /// </summary>
        public static string publicKey = ConfigHelper.Get(ConfigHelper.PublicKey);

        /// <summary>
        /// 私钥
        /// </summary>
        public static string privateKey = ConfigHelper.Get(ConfigHelper.PrivateKey);


        /// <summary>  
        /// 签名  
        /// </summary>  
        /// <param name="content">待签名字符串</param>  
        /// <param name="privateKey">私钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>签名后字符串</returns>  
        public static string Sign(string content, string privateKey, string input_charset)
        {
            byte[] data = Encoding.GetEncoding(input_charset).GetBytes(content);
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] signData = rsa.SignData(data, sh);
            return Convert.ToBase64String(signData);
        }

        /// <summary>  
        /// 验签  
        /// </summary>  
        /// <param name="content">待验签字符串</param>  
        /// <param name="signedString">签名</param>  
        /// <param name="publicKey">公钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>true(通过)，false(不通过)</returns>  
        public static bool Verify(string content, string signedString, string publicKey, string input_charset)
        {
            bool result = false;
            byte[] data1 = Encoding.GetEncoding(input_charset).GetBytes(content);
            byte[] data2 = Convert.FromBase64String(signedString);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(data1, sh, data2);
            return result;
        }

        /// <summary>  
        /// 加密  
        /// </summary>  
        /// <param name="resData">需要加密的字符串</param>  
        /// <param name="publicKey">公钥</param>  
        /// <param name="inputCharset">编码格式</param>  
        /// <returns>明文</returns>  s
        public static string EncryptData(string resData, string publicKey, string inputCharset = "utf-8")
        {
            byte[] dataToEncrypt = Encoding.ASCII.GetBytes(resData);
            string result = Encrypt(dataToEncrypt, publicKey, inputCharset);
            return result;
        }


        /// <summary>  
        /// 解密  
        /// </summary>  
        /// <param name="resData">加密字符串</param>  
        /// <param name="privateKey">私钥</param>  
        /// <param name="inputCharset">编码格式</param>  
        /// <returns>明文</returns>  
        public static string DecryptData(string resData, string privateKey, string inputCharset = "utf-8")
        {
            byte[] dataToDecrypt = Convert.FromBase64String(resData);
            string result = "";
            for (int j = 0; j < dataToDecrypt.Length / 128; j++)
            {
                byte[] buf = new byte[128];
                for (int i = 0; i < 128; i++)
                {

                    buf[i] = dataToDecrypt[i + 128 * j];
                }
                result += decrypt(buf, privateKey, inputCharset);
            }
            return result;
        }

        private static string Encrypt(byte[] data, string publicKey, string input_charset)
        {
            RSACryptoServiceProvider rsa = DecodePemPublicKey(publicKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] result = rsa.Encrypt(data, false);

            return Convert.ToBase64String(result);
        }

        private static string decrypt(byte[] data, string privateKey, string input_charset)
        {
            string result = "";
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] source = rsa.Decrypt(data, false);
            char[] asciiChars = new char[Encoding.GetEncoding(input_charset).GetCharCount(source, 0, source.Length)];
            Encoding.GetEncoding(input_charset).GetChars(source, 0, source.Length, asciiChars, 0);
            result = new string(asciiChars);
            return result;
        }

        private static RSACryptoServiceProvider DecodePemPublicKey(string pemstr)
        {
            var pkcs8Publickkey = Convert.FromBase64String(pemstr);
            {
                RSACryptoServiceProvider rsa = DecodeRsaPublicKey(pkcs8Publickkey);
                return rsa;
            }
        }

        private static RSACryptoServiceProvider DecodePemPrivateKey(string pemstr)
        {
            var pkcs8Privatekey = Convert.FromBase64String(pemstr);
            {
                RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8Privatekey);
                return rsa;
            }
        }

        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);
            byte bt = 0;
            ushort twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);
                if (!CompareBytearrays(seq, seqOid))
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)
                    return null;

                bt = binr.ReadByte();
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRsaPrivateKey(rsaprivkey);
                return rsacsp;
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private static RSACryptoServiceProvider DecodeRsaPublicKey(byte[] publickey)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            MemoryStream mem = new MemoryStream(publickey);
            BinaryReader binr = new BinaryReader(mem);
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return null;

                seq = binr.ReadBytes(15);
                if (!CompareBytearrays(seq, SeqOID))
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103)
                    binr.ReadByte();
                else if (twobytes == 0x8203)
                    binr.ReadInt16();
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return null;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102)
                    lowbyte = binr.ReadByte();
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte();
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                int modsize = BitConverter.ToInt32(modint, 0);

                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);

                if (firstbyte == 0x00)
                {
                    binr.ReadByte();
                    modsize -= 1;
                }

                byte[] modulus = binr.ReadBytes(modsize);

                if (binr.ReadByte() != 0x02)
                    return null;
                int expbytes = (int)binr.ReadByte();
                byte[] exponent = binr.ReadBytes(expbytes);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                RSAParameters rsaKeyInfo = new RSAParameters();
                rsaKeyInfo.Modulus = modulus;
                rsaKeyInfo.Exponent = exponent;
                rsa.ImportParameters(rsaKeyInfo);
                return rsa;
            }
            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }
        }

        private static RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters rsAparams = new RSAParameters();
                rsAparams.Modulus = MODULUS;
                rsAparams.Exponent = E;
                rsAparams.D = D;
                rsAparams.P = P;
                rsAparams.Q = Q;
                rsAparams.DP = DP;
                rsAparams.DQ = DQ;
                rsAparams.InverseQ = IQ;
                RSA.ImportParameters(rsAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
        {
            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 609)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }

            int index = 11;
            byte[] pemModulus = new byte[128];
            Array.Copy(keyData, index, pemModulus, 0, 128);

            index += 128;
            index += 2;
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, index, pemPublicExponent, 0, 3);

            index += 3;
            index += 4;
            byte[] pemPrivateExponent = new byte[128];
            Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

            index += 128;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);
            byte[] pemPrime1 = new byte[64];
            Array.Copy(keyData, index, pemPrime1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//346  
            byte[] pemPrime2 = new byte[64];
            Array.Copy(keyData, index, pemPrime2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);
            byte[] pemExponent1 = new byte[64];
            Array.Copy(keyData, index, pemExponent1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);
            byte[] pemExponent2 = new byte[64];
            Array.Copy(keyData, index, pemExponent2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);
            byte[] pemCoefficient = new byte[64];
            Array.Copy(keyData, index, pemCoefficient, 0, 64);

            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            para.D = pemPrivateExponent;
            para.P = pemPrime1;
            para.Q = pemPrime2;
            para.DP = pemExponent1;
            para.DQ = pemExponent2;
            para.InverseQ = pemCoefficient;
            return para;
        }
    }

}
