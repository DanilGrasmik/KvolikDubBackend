using Microsoft.AspNetCore.Authorization;

namespace KvolikDubBackend.Services.AuthorizationPolicy;

public class TokenReq : IAuthorizationRequirement
{
    public TokenReq()
    {
    }
}