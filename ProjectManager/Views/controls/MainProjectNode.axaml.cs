using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class MainProjectNode : UserControl
{
    MainProjectData projectData;
    public MainProjectNode(MainProjectData project)
    {
       
        InitializeComponent();
        taskViewModel viewModel = new taskViewModel();
        projectData = project;
        Name.Text = project.Name;
        rollDown.Click += unRoll;




    }

    public void unRoll(object? sender, RoutedEventArgs routedEventArgs)
    {
        projectData.rolled = !projectData.rolled;
        CanvasNodesView.item.onUpdate();
        Console.WriteLine(projectData.rolled);
    }
}