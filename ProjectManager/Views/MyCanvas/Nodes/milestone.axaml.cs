using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class milestone : task
{
    
    
    public milestone(ProjectData Data):base(Data)
    {
        Data.NodeHeight = 200;
        Data.NodeWidth = 150;
     
        InitializeComponent();
        taskViewModel viewmodel = new taskViewModel();
        Accept.Command = viewmodel.AcceptCommand;
        Accept.CommandParameter = viewmodel;
        Accept.CommandParameter = Data;

        viewmodel.Finished = Data.Finished;
        DataContext = viewmodel;
        data = Data;
        MarkAsInProgress.Command = viewmodel.MarkAsInProgressCommand;
        MarkAsInProgress.CommandParameter = Data;
        MarkAsIssue.Command = viewmodel.MarkAsIssueCommand;
        MarkAsIssue.CommandParameter = Data;
    }
}