using System.Security.Cryptography;
using System.Text;

namespace ToolsSecurity
{
    public class RsaService : IRsaService
    {
        private readonly RSACryptoServiceProvider _provider;

        public RsaService(KeySizes keySize = KeySizes.Default)
        {
            _provider = new RSACryptoServiceProvider((int)keySize);
        }

        public RsaService(byte[] keys)
        {
            _provider = new RSACryptoServiceProvider();
            _provider.ImportCspBlob(keys);
        }

        public byte[] Keys
        {
            get { return _provider.ExportCspBlob(true); }
        }

        public byte[] PublicKey
        {
            get { return _provider.ExportCspBlob(false); }
        }

        public bool IsPublicKeyOnly
        {
            get { return _provider.PublicOnly; }
        }

        public byte[] Encrypt(string data)
        {
            byte[] dataAsByteArray = Encoding.Default.GetBytes(data);
            return Encrypt(dataAsByteArray);
        }

        public byte[] Encrypt(byte[] data)
        {
            return _provider.Encrypt(data, true);
        }

        public byte[] Decrypt(byte[] cypher)
        {
            if (IsPublicKeyOnly)
                throw new InvalidOperationException("Can't decrypt with the public key only");

            return _provider.Decrypt(cypher, true);
        }

        public string DecryptAsString(byte[] cypher)
        {
            byte[] dataAsByteArray = Decrypt(cypher);
            return Encoding.Default.GetString(dataAsByteArray);
        }
    }
}