using System.Text.RegularExpressions;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace KvolikDubBackend.Services.AuthorizationPolicy;

public class TokenReqHandler : AuthorizationHandler<TokenReq>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public TokenReqHandler(IHttpContextAccessor httpContextAccessor,
        IServiceScopeFactory serviceScopeFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        TokenReq requirement)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var authorizationString = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];
            var token = GetToken(authorizationString);

            using var scope = _serviceScopeFactory.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            var tokenEntity = await appDbContext
                .Tokens
                .Where(x => x.Token == token)
                .FirstOrDefaultAsync();


            if (tokenEntity != null)
            {
                throw new NotAuthorizedException("Not authorized");
            }

            context.Succeed(requirement);
        }
        else
        {
            throw new BadRequestException("Bad request");
        }
    }
    
    private static string GetToken(string? authorizationString)
    {
        const string pattern = @"\S+\.\S+\.\S+";
        var regex = new Regex(pattern);
        if (authorizationString == null)
        {
            throw new NotAuthorizedException("Not authorized");
        }
        var matches = regex.Matches(authorizationString);

        if (matches.Count <= 0)
        {
            throw new NotAuthorizedException("Not authorized");
        }

        var token = matches[0].Value;

        if (token == null)
        {
            throw new NotAuthorizedException("Not authorized");
        }

        return token;
    }
}