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
    private readonly IHttpContextAccessor  _httpContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public TokenReqHandler(IHttpContextAccessor httpContextAccessor,
        IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceScopeFactory = serviceScopeFactory;
        _httpContext = httpContext;
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
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { message = "Bad token"});
            }

            context.Succeed(requirement);
        }
        else
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { message = "Bad request"});
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