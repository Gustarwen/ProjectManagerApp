using System;

namespace ProjectManager.DataTemplates;

public class ProjectData
{
    public bool Finished { get; set; } = false;
    public string Name { get; set; } = "Untitled";
    
    // ZMĚNA TYPU: int na DateTimeOffset?
    public DateTimeOffset? Date { get; set; } 
}