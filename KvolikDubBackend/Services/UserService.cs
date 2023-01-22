using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using KvolikDubBackend.Configurations;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KvolikDubBackend.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TokenDto> RegisterUser(UserRegisterDto userRegisterDto)
    {
        //TODO: Confirm password 
         await CheckRegisterValidation(userRegisterDto);
        
        UserEntity userEntity = new UserEntity()
        {
            Id = new Guid(),
            Username = userRegisterDto.username,
            Name = userRegisterDto.name,
            AvatarImageUrl = "https://sun9-east.userapi.com/sun9-58/s/v1/ig2/wgxhVCsTLeNhC3Ue8gnd8n6QkpilZZHTxT61fUXzPXfWqjH8vTsui8fZMdX3VvBHuhMEKYZOkZPosFeMS8CzElzc.jpg?size=712x1044&quality=96&type=album",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.password),
            IsAdmin = false
        };
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        LoginCredentials loginCredentials = new LoginCredentials()
        {
            username = userRegisterDto.username,
            password = userRegisterDto.password
        };

        return await LoginUser(loginCredentials);
    }

    public async Task<TokenDto> LoginUser(LoginCredentials loginCredentials)
    {
        var identity = await GetIdentity(loginCredentials.username, loginCredentials.password);

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: JwtConfig.Issuer,
            audience: JwtConfig.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(JwtConfig.Lifetime),
            signingCredentials: new SigningCredentials(JwtConfig.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var result = new TokenDto()
        {
            token = encodeJwt
        };

        return result;
    }

    public async Task<String> LogoutUser(IHeaderDictionary headers)
    {
        var token = GetToken(headers);
        
        var handler = new JwtSecurityTokenHandler();
        var expiredDate = handler.ReadJwtToken(token).ValidTo;

        var tokenEntity = new TokenEntity
        {
            Id = Guid.NewGuid(),
            Token = token,
            ExpiredDate = expiredDate
        };

        await _context.Tokens.AddAsync(tokenEntity);
        await _context.SaveChangesAsync();
        return "Logged out";
    }

    public async Task<ProfileInfoDto> GetProfile(string username)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync();
        
        ProfileInfoDto profileInfoDto = _mapper.Map<ProfileInfoDto>(userEntity);
        return profileInfoDto;
    }

    public async Task EditProfile(EditProfileDto editProfileDto, String username)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync();

        if (editProfileDto.name == null)
        {
            throw new BadRequestException("name field is required");
        }
        
        userEntity.Name = editProfileDto.name;
        
        await _context.SaveChangesAsync();
    }

    private async Task<ClaimsIdentity> GetIdentity(string username, string password)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.Username == username)
            .FirstOrDefaultAsync() ?? throw new BadRequestException("Login failed");

        if (!BCrypt.Net.BCrypt.Verify(password, userEntity.HashedPassword))
        {
            throw new BadRequestException("Login failed");
        }

        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, userEntity.Username)
        };

        var claimsIdentity = new ClaimsIdentity
        (
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            "User"
        );

        return claimsIdentity;
    }

    private async Task CheckRegisterValidation(UserRegisterDto userRegisterDto)
    {
        if (userRegisterDto.password == null || userRegisterDto.username == null)
        {
            throw new BadRequestException("Incorrect model in request");
        }
        if (userRegisterDto.password.Length < 6 || userRegisterDto.password.Length > 30)
        {
            throw new BadRequestException("Password length must be in range 6 to 30");
        }
        if (userRegisterDto.username.Length < 2 || userRegisterDto.username.Length > 25)
        {
            throw new BadRequestException("Username length must be in range 2 to 25");
        }

        UserEntity? userEntity = await _context
            .Users
            .Where(user => user.Username == userRegisterDto.username)
            .FirstOrDefaultAsync();
        if (userEntity != null)
        {
            throw new ConflictException($"User with username '{userRegisterDto.username}' already exists");
        }
    }
    
    private static string GetToken(IHeaderDictionary headersDictionary)
    {
        var headers = new Dictionary<string, string>();

        foreach (var header in headersDictionary)
        {
            headers.Add(header.Key, header.Value);
        }

        var authorizationHeader = headers["Authorization"];

        var regex = new Regex(@"\S+\.\S+\.\S+");
        var matches = regex.Matches(authorizationHeader);

        if (matches.Count <= 0)
        {
            throw new BadRequestException("Invalid token in authorization header");
        }

        return matches[0].Value;
    }
}