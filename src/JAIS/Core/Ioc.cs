using System;
using Microsoft.Extensions.DependencyInjection;

namespace JAIS.Core;

public class Ioc
{
    private static IServiceCollection? _serviceCollection;
    private static IServiceProvider? _provider;

    public static void Configure()
    {
        _serviceCollection = new ServiceCollection();
    }

    public static void Build()
    {
        _provider = _serviceCollection.BuildServiceProvider();
    }

    public static void RegisterSingleton<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
    {
        _serviceCollection?.AddSingleton<TInterface, TImplementation>();
    }

    public static void RegisterScoped<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
    {
        _serviceCollection?.AddSingleton<TInterface, TImplementation>();
    }

    internal static TInterface Resolve<TInterface>()
    {
        return _provider!.GetService<TInterface>()!;
    }
}