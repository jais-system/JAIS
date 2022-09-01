using System;
using AppCore.Services.AppManager.Entities;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace JAIS.Dialogs.AppSideloadingRequest;

public partial class AppSideloadingRequestDialog : UserControl
{
    public string Description { get; set; }

    public bool? Result { get; set; }
    public event EventHandler<bool> OnResult = delegate { };

    public AppSideloadingRequestDialog()
    {
        
    }

    public AppSideloadingRequestDialog(SideloadingRequest request)
    {
        Description = $"{request.AppName} v{request.Version} by {request.Author}";
        DataContext = this;

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void ApproveClicked(object? sender, RoutedEventArgs e)
    {
        Result = true;
        OnResult.Invoke(this, true);
        MainView.CloseModal();
    }

    private void CancelClicked(object? sender, RoutedEventArgs e)
    {
        Result = false;
        OnResult.Invoke(this, false);
        MainView.CloseModal();
    }
}