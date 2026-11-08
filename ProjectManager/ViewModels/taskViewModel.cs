using System;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;
using ProjectManager.Views;

namespace ProjectManager.ViewModels;

public partial class taskViewModel :ViewModelBase
{
    private readonly CanvasNodesViewModel _viewModel;

  [ObservableProperty]
    private bool _finished;

    [ObservableProperty] private bool unRolled = false;
    [RelayCommand]
    private void Accept(ProjectData mytask)
    {
        
        mytask.Finished = !mytask.Finished;
        Console.WriteLine($"stav tasku {mytask.Finished}");
        
       
       
       
        
        if (mytask.Finished)
        {
            Finished = true;
            recursiveAccept(mytask);
        }
        else
        {
            Finished = false;
            recursiveUnAccept(mytask);
        }
        
        DataBaseController.Taskrepository.Update(mytask);
        int finisher = 0;
        int sum = 0;



        
        



    }
    

   

    private void recursiveUnAccept(ProjectData mytask)
    {
       
        if (mytask.ParentId != null)
        {
            ProjectData parent = task.tasks.Where(p => p.data.Id == mytask.ParentId).First().data;
            if (parent != null) recursiveUnAccept(parent);
        }
        
        task taskclass = task.tasks.FirstOrDefault(p => p.data.Id == mytask.Id);
            var viewmodel = taskclass.DataContext as taskViewModel;
            viewmodel.Finished =false;
            mytask.Finished = false;
        
        DataBaseController.Taskrepository.Update(mytask);
     
    }
    private void recursiveAccept(ProjectData mytask)
    {
        mytask.Finished = true;
        DataBaseController.Taskrepository.Update(mytask);
        task viewtask = task.tasks.FirstOrDefault(p=>p.data.Id == mytask.Id);
        if (viewtask != null)
        {
            taskViewModel viewmodel = viewtask.DataContext as taskViewModel;
            if (viewmodel != null)
            {
                viewmodel.Finished = true;
            }
        }

        var childerns = task.tasks.Where(p => p.data != null && p.data.ParentId == mytask.Id).ToList();
        foreach (var data in childerns)
        { 
            recursiveAccept(data.data);
        }
    }

    [RelayCommand]
    private void unRoll()
    {
        UnRolled = !UnRolled;

    }
}