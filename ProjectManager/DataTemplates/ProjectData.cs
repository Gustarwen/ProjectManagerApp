using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Durable;
using System.Collections.Generic;

namespace ProjectManager.DataTemplates;

//Tasky
[Entity("tasks")]
public partial class ProjectData : ObservableObject
{
    [Property("Id", Flags.Integer | Flags.PrimaryKey | Flags.AutoIncrement)]
    public int Id { get; set; }


    public int ProjectId { get; set; }

    [property: Property("Finished", Flags.Integer, 1)] [ObservableProperty]
    private bool finished = false;

    [Property("project_id")] [ForeignKey(typeof(Project), "Id")] [DefaultValue(0)]
    public int projectId{get;set;}
    [Property("parent_id")] [ForeignKey(typeof(Project), "ProjectId")] 
    public int parentId{get;set;}
    
    [Property("Name",Flags.String,64)]
    public string Name { get; set; } = "Untitled";
    public List<ProjectData> childerns;

    public double x;
    public double y;


    
    
    // ZMĚNA TYPU: int na DateTimeOffset?
    public DateTimeOffset? Date { get; set; } 
}
//Project
[Entity("projects")]
public partial class Project
{
    [Property("Id", Flags.Integer | Flags.PrimaryKey | Flags.AutoIncrement)]
    public int Id { get; set; }
    [Property("Finished", Flags.Integer)]
    public double Finished { get; set; }
    [Property("Name", Flags.String)]
    public string Name { get; set; } = "Untitled";
    public DateTimeOffset? Date { get; set; } 
    
    public List<ProjectData> tasks;
    
}
