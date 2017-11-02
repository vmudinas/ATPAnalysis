using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Hygiena.Controllers
{
    public class RsaKeyUtils
    {
        public static RSAParameters GetRandomKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    return rsa.ExportParameters(true);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static void GenerateKeyAndSave(string file)
        {
            var p = GetRandomKey();
            var t = new RsaParametersWithPrivate();
            t.SetParameters(p);
            File.WriteAllText(file, JsonConvert.SerializeObject(t));
        }


        public static RSAParameters GetKeyParameters(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("Check configuration - cannot find auth key file: " + file);
            var keyParams = JsonConvert.DeserializeObject<RsaParametersWithPrivate>(File.ReadAllText(file));
            return keyParams.ToRsaParameters();
        }


        /// <summary>
        ///     Util class to allow restoring RSA parameters from JSON as the normal
        ///     RSA parameters class won't restore private key info.
        /// </summary>
        private class RsaParametersWithPrivate
        {
            public byte[] D { get; set; }
            public byte[] Dp { get; set; }
            public byte[] Dq { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] Modulus { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }

            public void SetParameters(RSAParameters p)
            {
                D = p.D;
                Dp = p.DP;
                Dq = p.DQ;
                Exponent = p.Exponent;
                InverseQ = p.InverseQ;
                Modulus = p.Modulus;
                P = p.P;
                Q = p.Q;
            }

            public RSAParameters ToRsaParameters()
            {
                return new RSAParameters
                {
                    D = D,
                    DP = Dp,
                    DQ = Dq,
                    Exponent = Exponent,
                    InverseQ = InverseQ,
                    Modulus = Modulus,
                    P = P,
                    Q = Q
                };
            }
        }
    }
}