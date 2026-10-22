using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Durable;

namespace ProjectManager.DataTemplates;
[Entity("tasks")]
public partial class ProjectData : ObservableObject
{   
    [Property("Id",Flags.Integer|Flags.PrimaryKey|Flags.AutoIncrement)]
    public int Id { get; set; }

    [property:Property("Finished", Flags.Integer, 1)] [ObservableProperty]
    private bool finished = false;
    [Property("Index",Flags.Integer,0)]
    public int Index { get; set; } = 0;
    [Property("Name",Flags.String,64)]
    public string Name { get; set; } = "Untitled";


    
    
    // ZMĚNA TYPU: int na DateTimeOffset?
    public DateTimeOffset? Date { get; set; } 
}