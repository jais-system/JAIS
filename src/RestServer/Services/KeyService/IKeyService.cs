using System.Security.Cryptography;

namespace RestServer.Services.KeyService;

public interface IKeyService
{
    RSA Rsa { get; }

    (string privateKey, string publicKey) GenerateKeys();
    string GenerateJwtToken();
}