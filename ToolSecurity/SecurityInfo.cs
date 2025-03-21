namespace ToolSecurity
{
    public class SecurityInfo
    {
        public string Login { get; }
        public string Passwd { get; }
        public string SecretKey { get; }

        public SecurityInfo(string login, string passwd, string secretKey)
        {
            Login = login;
            Passwd = passwd;
            SecretKey = secretKey;
        }
    }
}