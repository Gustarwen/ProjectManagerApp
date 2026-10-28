using System;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class CanvasNodesViewModel:ViewModelBase
{
    public Project Project;
    public  CanvasNodesViewModel(Project selectedProject)
    {
        Project = selectedProject;
    }
    
 
 
    public  void OnClicked(ProjectData selectedTask)
    {
       

// Spusťte insert uvnitř try-catch, jak máte teď

        Console.WriteLine("dostal jsem se do bodu kdy insertnu task");
        ProjectData newData = new ProjectData();
        newData.Name = selectedTask.Name;
        newData.Finished = selectedTask.Finished;
        newData.ProjectId = Project.Id;
        Console.WriteLine(Project.Id);
        newData.ParentId = selectedTask.Id;
        DataBaseController.InsertTask(newData);

    }
    
}