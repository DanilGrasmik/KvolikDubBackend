using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace KvolikDubBackend.Services.AuthorizationPolicy;

public class AdminReqHandler : AuthorizationHandler<AdminReq>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public AdminReqHandler(IHttpContextAccessor httpContextAccessor,
        IServiceScopeFactory serviceScopeFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AdminReq requirement)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var userEntity =
                await GetUser(_httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization]);
            if (!userEntity.IsAdmin)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { message = "You must be admin to this request"});
                //throw new ForbiddenException("You must be admin to this request");
            }

            context.Succeed(requirement);
        }
        else
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { message = "Bad request"});
            //throw new BadRequestException("Bad request");
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

    private async Task<UserEntity> GetUser(String authorizationString)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        String username = "";
        var token = GetToken(authorizationString);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var tokenS = jsonToken as JwtSecurityToken;
        var claims = tokenS.Claims;
        foreach (var claim in claims)
        {
            if (claim.Type == "username")
            {
                username = claim.Value;
                break;
            }
        }


        return await appDbContext
            .Users
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync() ?? throw new NotAuthorizedException("Incorrect JWT token");
    }
}