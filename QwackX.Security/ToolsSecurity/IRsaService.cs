namespace ToolsSecurity;

public interface IRsaService
{
    bool IsPublicKeyOnly { get; }
    byte[] Keys { get; }
    byte[] PublicKey { get; }

    byte[] Decrypt(byte[] cypher);
    string DecryptAsString(byte[] cypher);
    byte[] Encrypt(byte[] data);
    byte[] Encrypt(string data);
}