using Microsoft.Extensions.DependencyInjection;

namespace AppCore;

public static class DependencyInjector
{
    private static ServiceCollection? _serviceCollection;
    private static IServiceProvider? _provider;

    private static ServiceCollection ServiceCollection => _serviceCollection ??= new ServiceCollection();

    public static event EventHandler AfterBuild = delegate { };

    private static void Build()
    {
        _provider = ServiceCollection.BuildServiceProvider();
        AfterBuild.Invoke(new object(), EventArgs.Empty);
    }

    public static void RegisterSingleton<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
    {
        ServiceCollection.AddSingleton<TInterface, TImplementation>();
    }

    public static void RegisterScoped<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
    {
        ServiceCollection.AddSingleton<TInterface, TImplementation>();
    }

    public static void RegisterTransient<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
    {
        ServiceCollection.AddTransient<TInterface, TImplementation>();
    }

    public static TInterface Resolve<TInterface>()
    {
        if (_provider == null)
        {
            Build();
        }
        
        return _provider!.GetService<TInterface>()!;
    }
}