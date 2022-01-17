using Avalonia.Controls;

namespace JAIS.Entities;

public class MainViewBindings : Notifiable
{
    private bool _showDialog;
    private UserControl? _mainApp;
    private UserControl? _previousMainApp;
    private bool _changeAppInitiated;

    public bool ShowDialog { get => _showDialog; set => Set(ref _showDialog, value); }
    public UserControl? MainApp { get => _mainApp; set => Set(ref _mainApp, value); }
    public UserControl? PreviousMainApp { get => _previousMainApp; set => Set(ref _previousMainApp, value); }
    public bool ChangeAppInitiated { get => _changeAppInitiated; set => Set(ref _changeAppInitiated, value); }
}