using System;
using CommunityToolkit.Mvvm.Input;

namespace ProjectManager.DataTemplates;

public class ProjectData
{
    public bool Finished { get; set; } = false;
    public int Index { get; set; } = 0;
    public string Name { get; set; } = "Untitled";

  
    
    
    // ZMĚNA TYPU: int na DateTimeOffset?
    public DateTimeOffset? Date { get; set; } 
}