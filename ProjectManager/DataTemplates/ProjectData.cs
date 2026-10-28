using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Durable;
using System.Collections.Generic;

namespace ProjectManager.DataTemplates;

//Tasky
[Entity("tasks")]
public partial class ProjectData : ObservableObject
{
    //vytvoří id pro tasky
    [Property("Id", Flags.Integer | Flags.PrimaryKey | Flags.AutoIncrement)] 
    public int Id { get; set; }
    
    //kontrola, jestli je task splněný nebo ne
    [property: Property("Finished", Flags.Integer, 1)] [ObservableProperty]
    private bool _finished = false;

    //hodnota spojující každý Task s jeho projektem
    [Property("project_id")] [ForeignKey(typeof(Project), "Id")]
    public int ProjectId{get;set;}
    
    //hodnota spojující každý Task s jeho rodičem
    [Property("parent_id")] [ForeignKey(typeof(ProjectData), "Id")] 
    public int? ParentId{get;set;}
    
    //jméno Tasku
    [Property("Name",Flags.String,64)]
    public string Name { get; set; } = "Untitled";
    
    //děti určitého Tasku
    public List<ProjectData> Childerns = new List<ProjectData>();

    //poloha Tasku pro Canvas
    public double X;
    public double Y;


    
    
   // datum deadLine Tasku
    public DateTimeOffset? Date { get; set; } 
}
//Project
[Entity("projects")]
public partial class Project : ObservableObject
{
    //id pro každý projekt
    [Property("Id", Flags.Integer | Flags.PrimaryKey | Flags.AutoIncrement)]
    public int Id { get; set; }

    //hodnota udávající jestli je projekt hotov či nikoliv
    [property: Property("Finished", Flags.Integer, 1)] [ObservableProperty]
    private bool _finished = false;

    //string pro jméno projektu
    [Property("Name", Flags.String)]
    public string Name { get; set; } = "Untitled";
    
    //datum deadline
    public DateTimeOffset? Date { get; set; }

    public List<ProjectData> Tasks = new List<ProjectData>();

}
