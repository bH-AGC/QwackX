using System.Security.Cryptography;
using System.Text;

namespace ToolsSecurity;

public static class StringExtensions
{
    public static byte[] Hash(this string s)
    {
        return SHA384.HashData(Encoding.Default.GetBytes(s));
    }
}