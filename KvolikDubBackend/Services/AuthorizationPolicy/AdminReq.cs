using Microsoft.AspNetCore.Authorization;

namespace KvolikDubBackend.Services.AuthorizationPolicy;

public class AdminReq : IAuthorizationRequirement
{
    public AdminReq()
    {
    }
}