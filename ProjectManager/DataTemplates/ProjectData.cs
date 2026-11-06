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
    private bool _finished = false;
    
    
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
    public List<int> Projects = new();
    public void loadProjects()
    {
       
        foreach (var project in DataBaseController.ProjectTaskRepository.Query().Where(p => p.TaskId == Id).Execute())
        {
            Projects.Add(project.ProjectId);
        }
    }
}


[Entity("project_task")]

public partial class Project_Task()
{

    
    [Property("TaskId",Flags.PrimaryKey|Flags.Integer)][ForeignKey(typeof(ProjectData), "TaskId")]
    public int TaskId { get; set; }
    [Property("ProjectId",Flags.PrimaryKey|Flags.Integer)][ForeignKey(typeof(ProjectData), "ProjectId")]
    public int ProjectId { get; set; }
    
   
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


