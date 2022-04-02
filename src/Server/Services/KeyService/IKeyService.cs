using System.Security.Cryptography;

namespace Server.Services.KeyService;

public interface IKeyService
{
    RSA Rsa { get; }

    (string privateKey, string publicKey) GenerateKeys();
    string GenerateJwtToken();
}