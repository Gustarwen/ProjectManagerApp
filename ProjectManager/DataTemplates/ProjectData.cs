using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Durable;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

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
    public bool _finished = false;
    [Property("ProjectId", Flags.Integer)][ForeignKey(typeof(Project), "Id")]
    public int ProjectId { get; set; }
    
    //hodnota spojující každý Task s jeho rodičem
    [Property("parent_id")] [ForeignKey(typeof(ProjectData), "Id")] 
    public int? ParentId{get;set;}
    
    //jméno Tasku
    [Property("Name",Flags.String,64)]
    public string Name { get; set; } = "Untitled";
    
    //děti určitého Tasku
    public List<ProjectData> Childerns = new List<ProjectData>();
    public List<MainProjectData> JoindedProject = new();

    //poloha Tasku pro Canvas
    public double X;
    public double Y;


    
    
   // datum deadLine Tasku
    public DateTimeOffset? Date { get; set; }
   
}

public partial class MainProjectData : ProjectData
{
    
    public bool rolled;

    public MainProjectData(ProjectData project)
    {
        this.rolled = true;
        this.Id = project.Id;
        this.Name = project.Name;
        this.Date = project.Date;
        this.Childerns = project.Childerns;
        this.JoindedProject = project.JoindedProject;
        this.ParentId = project.ParentId;
        this.Y = project.Y;
        this.X = project.X;
        this.Date = project.Date;
        this.ProjectId = project.ProjectId;
    }
}


[Entity("task_task_relationship")]

public partial class Task_Task()
{

    
    [Property("TaskId1",Flags.PrimaryKey|Flags.Integer)][ForeignKey(typeof(ProjectData), "TaskId")]
    public int TaskId { get; set; }
    [Property("TaskId2",Flags.PrimaryKey|Flags.Integer)][ForeignKey(typeof(ProjectData), "TaskId")]
    public int TaskId2 { get; set; }
    
   
}

//Project
[Entity("projects")]
public partial class Project : ObservableObject
{
    [property:Property("Image", Flags.String)] [ObservableProperty][NotifyPropertyChangedFor(nameof(ProjectBitmap))]
    private string _myImage ="";
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
    
    public Bitmap? ProjectBitmap
    {
        get
        {
            if(string.IsNullOrEmpty(MyImage)||!System.IO.File.Exists(MyImage))
                return null;
            try
            {
                return new Bitmap(MyImage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

}


