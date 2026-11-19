
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class task : UserControl
{
    public static readonly List<task> tasks = new List<task>();
    public ProjectData data;

    public task(ProjectData Data)
    {
       
        InitializeComponent();
        taskViewModel viewmodel = new taskViewModel();
        Accept.Command = viewmodel.AcceptCommand;
        Accept.CommandParameter = viewmodel;
        Accept.CommandParameter = Data;

        viewmodel.Finished = Data.Finished;
        DataContext = viewmodel;
        data = Data;
        
   
        
       
    }


    ~task()
    {
        task.tasks.Remove(this);
    }
}