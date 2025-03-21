using System.Text.Json;
using QwackX.Api.Properties;
using ToolSecurity;

public class SecurityInfoService
{
    public SecurityInfo SecurityInfo { get; }
    
    private SecurityInfoService(SecurityInfo securityInfo)
    {
        SecurityInfo = securityInfo;
    }
    
    public static SecurityInfoService Create(IRsaRepository rsaRepository)
    {
        string json = rsaRepository.DecryptAsString(Resources.data);
        var securityInfo = JsonSerializer.Deserialize<SecurityInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return new SecurityInfoService(securityInfo);
    }
}
