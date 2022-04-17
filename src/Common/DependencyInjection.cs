using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class DependencyInjection
{
    private static IServiceCollection? _serviceCollection;
    private static IServiceProvider? _provider;

    public static event EventHandler AfterBuild = delegate { };

    internal static void Configure()
    {
        _serviceCollection = new ServiceCollection();
    }

    public static void Build()
    {
        _provider = _serviceCollection!.BuildServiceProvider();
        AfterBuild.Invoke(new object(), EventArgs.Empty);
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

    public static void RegisterTransient<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
    {
        _serviceCollection?.AddTransient<TInterface, TImplementation>();
    }

    public static TInterface Resolve<TInterface>()
    {
        return _provider!.GetService<TInterface>()!;
    }
}