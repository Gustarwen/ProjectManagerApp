using System;
using System.Linq;
using Avalonia.Controls;
using ProjectManager.DataTemplates;
using ProjectManager.Views;

namespace ProjectManager.ViewModels;

public partial class CanvasNodesViewModel:ViewModelBase
{
    public Project Project;
    public  CanvasNodesViewModel(Project selectedProject)
    {
        Project = selectedProject;
    }

    public void showDialog(CanvasNodesView nodesview,Window root,ProjectData selectedProject)
    {
        Window dialog = new NodConfiguration(selectedProject,nodesview);
        dialog.DataContext = this;
        dialog.ShowDialog(root);
        
    }

    public void DeleteTask(ProjectData data)
    {
        DataBaseController.DeleteTask(data);
        
    }
 
    public  void OnClicked(ProjectData selectedTask)
    {
       

// Spusťte insert uvnitř try-catch, jak máte teď

        Console.WriteLine("dostal jsem se do bodu kdy insertnu task");
        ProjectData newData = new ProjectData();
        newData.Name = selectedTask.Name;
        newData.Finished = selectedTask.Finished;
        newData.Projects.Add(Project.Id);
        
        Console.WriteLine(Project.Id);
        newData.ParentId = selectedTask.Id;
        ProjectData parent = DataBaseController.Taskrepository.Query().Where(p=>p.Id==newData.ParentId).Execute().First();
        newData.Finished = parent.Finished;
        DataBaseController.InsertTask(newData);
        

    }
    
}