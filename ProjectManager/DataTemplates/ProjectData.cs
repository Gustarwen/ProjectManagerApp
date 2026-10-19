using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProjectManager.DataTemplates;

public partial class ProjectData : ObservableObject
{
    [ObservableProperty]
    private bool finished = false;
    public int Index { get; set; } = 0;
    public string Name { get; set; } = "Untitled";

  
    
    
    // ZMĚNA TYPU: int na DateTimeOffset?
    public DateTimeOffset? Date { get; set; } 
}