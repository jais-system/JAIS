using System.Reflection;
using Common;
using Common.Core;
using Common.Services.ServerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Services.KeyService;

namespace Server;

public class ServerCore
{
    public static void Initialize()
    {
        AppCore.Initialize();

        var keyService = Ioc.Resolve<IKeyService>();
        var serverService = Ioc.Resolve<IServerService>();

        serverService.Port = 5408;

        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls($"http://0.0.0.0:{serverService.Port}");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "SomeIssuer",
                    ValidAudience = "SomeAudience",
                    IssuerSigningKey = new RsaSecurityKey(keyService.Rsa),
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };
            });

        Assembly assembly = typeof(ServerCore).Assembly;
        builder.Services.AddControllers().AddApplicationPart(assembly);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}