using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

public class JwtService
{
    private readonly string _secret;
    private readonly string _issuer;

    public JwtService()
    {
        _secret = ConfigurationManager.AppSettings["JwtSecret"];
        _issuer = ConfigurationManager.AppSettings["JwtIssuer"];
    }
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }

    // this generates access token
    public string GenerateToken(
            long userId,
            string email,
            string userName,
            string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secret));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role ?? "User")
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}