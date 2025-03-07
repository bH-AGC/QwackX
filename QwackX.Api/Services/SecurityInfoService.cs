using System.Text.Json;
using QwackX.Api.Properties;
using ToolsSecurity;

public class SecurityInfoService
{
    public SecurityInfo SecurityInfo { get; }
    
    private SecurityInfoService(SecurityInfo securityInfo)
    {
        SecurityInfo = securityInfo;
    }
    
    public static SecurityInfoService Create(IRsaService rsaService)
    {
        string json = rsaService.DecryptAsString(Resources.data);
        var securityInfo = JsonSerializer.Deserialize<SecurityInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return new SecurityInfoService(securityInfo);
    }
}
