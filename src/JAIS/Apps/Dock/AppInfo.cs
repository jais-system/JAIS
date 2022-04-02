using System;
using Avalonia.Controls;
using JAIS.Entities;

namespace JAIS.Apps.Dock;

public class AppInfo
{
    public string Id { get; set; }
    public Type Type { get; set; }
    public AppAttribute Attribute { get; set; }
    public UserControl? Instance { get; set; }
}