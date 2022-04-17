using System;
using Avalonia.Controls;

namespace JAIS.Entities;

public class AppInfo
{
    public string Id { get; set; }
    public Type Type { get; set; }
    public AppAttribute Attribute { get; set; }
    public UserControl? Instance { get; set; }
}