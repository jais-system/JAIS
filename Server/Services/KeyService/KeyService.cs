using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Server.Services.KeyService;

public class KeyService : IKeyService
{
    public RSA Rsa { get; } = RSA.Create();

    public (string privateKey, string publicKey) GenerateKeys()
    {
        byte[] privateKeyBytes = Rsa.ExportRSAPrivateKey();
        byte[] publicKeyBytes = Rsa.ExportRSAPublicKey();

        string privateKey = Convert.ToBase64String(privateKeyBytes);
        string publicKey = Convert.ToBase64String(publicKeyBytes);

        Console.WriteLine($"{privateKey}\n\n\n{publicKey}");

        return (privateKey, publicKey);
    }

    public string GenerateJwtToken()
    {
        DateTime now = DateTime.Now;
        long unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(Rsa), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };

        var jwt = new JwtSecurityToken(
            audience: "SomeAudience",
            issuer: "SomeIssuer",
            claims: new Claim[] {
                new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            },
            notBefore: now,
            expires: now.AddMinutes(30),
            signingCredentials: signingCredentials
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }
}