﻿#nullable enable

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace AppCore.Theme;

public enum FluentThemeMode
{
    Light,
    Dark,
}

/// <summary>
/// Includes the fluent theme in an application.
/// </summary>
public class JaisAppTheme : AvaloniaObject, IStyle, IResourceProvider
{
    private Styles _fluentDark = new();
    private Styles _fluentLight = new();
    private Styles _sharedStyles = new();
    private bool _isLoading;
    private IStyle? _loaded;

    /// <summary>
    /// Initializes a new instance of the <see cref="JaisAppTheme"/> class.
    /// </summary>
    /// <param name="baseUri">The base URL for the XAML context.</param>
    public JaisAppTheme(Uri baseUri)
    {
        InitStyles(baseUri);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JaisAppTheme"/> class.
    /// </summary>
    /// <param name="serviceProvider">The XAML service provider.</param>
    public JaisAppTheme(IServiceProvider serviceProvider)
    {
        Uri baseUri = ((IUriContext) serviceProvider.GetService(typeof(IUriContext))!).BaseUri;
        InitStyles(baseUri);
    }


    public static readonly StyledProperty<FluentThemeMode> ModeProperty =
        AvaloniaProperty.Register<JaisAppTheme, FluentThemeMode>(nameof(Mode));
    
    /// <summary>
    /// Gets or sets the mode of the fluent theme (light, dark).
    /// </summary>
    public FluentThemeMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }
    
    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ModeProperty)
        {
            if (Mode == FluentThemeMode.Dark)
            {
                (Loaded as Styles)![1] = _fluentDark[0];
                (Loaded as Styles)![2] = _fluentDark[1];
            }
            else
            {
                (Loaded as Styles)![1] = _fluentLight[0];
                (Loaded as Styles)![2] = _fluentLight[1];
            }
        }
    }

    public IResourceHost? Owner => (Loaded as IResourceProvider)?.Owner;

    /// <summary>
    /// Gets the loaded style.
    /// </summary>
    public IStyle Loaded
    {
        get
        {
            if (_loaded == null)
            {
                _isLoading = true;

                if (Mode == FluentThemeMode.Light)
                {
                    _loaded = new Styles() { _sharedStyles , _fluentLight[0], _fluentLight[1] };
                }
                else if (Mode == FluentThemeMode.Dark)
                {
                    _loaded = new Styles() { _sharedStyles, _fluentDark[0], _fluentDark[1] };
                }
                _isLoading = false;
            }

            return _loaded!;
        }
    }

    bool IResourceNode.HasResources => (Loaded as IResourceProvider)?.HasResources ?? false;

    IReadOnlyList<IStyle> IStyle.Children => _loaded?.Children ?? Array.Empty<IStyle>();

    public event EventHandler OwnerChanged
    {
        add
        {
            if (Loaded is IResourceProvider rp)
            {
                rp.OwnerChanged += value;
            }
        }
        remove
        {
            if (Loaded is IResourceProvider rp)
            {
                rp.OwnerChanged -= value;
            }
        }
    }

    public SelectorMatchResult TryAttach(IStyleable target, IStyleHost? host) => Loaded.TryAttach(target, host);

    public bool TryGetResource(object key, out object? value)
    {
        if (!_isLoading && Loaded is IResourceProvider p)
        {
            return p.TryGetResource(key, out value);
        }

        value = null;
        return false;
    }

    void IResourceProvider.AddOwner(IResourceHost owner) => (Loaded as IResourceProvider)?.AddOwner(owner);
    void IResourceProvider.RemoveOwner(IResourceHost owner) => (Loaded as IResourceProvider)?.RemoveOwner(owner);

    private void InitStyles(Uri baseUri)
    {
        _sharedStyles = new Styles
        {
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Accents/AccentColors.xaml")
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Accents/Base.xaml")
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Controls/FluentControls.xaml")
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Styles/GlobalStyles.xaml")
            }
        };

        _fluentLight = new Styles
        {
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Accents/BaseLight.xaml")
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Accents/FluentControlResourcesLight.xaml")
            }
        };

        _fluentDark = new Styles
        {
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Accents/BaseDark.xaml")
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("avares://AppCore/Theme/Accents/FluentControlResourcesDark.xaml")
            }
        };
    }
}