using System;
using System.ComponentModel.DataAnnotations;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class NodConfiguration : Window
{
    public NodConfiguration(ProjectData selectedTask,CanvasNodesView nodesview)
    {
        InitializeComponent();
       
        Accept.Click += (a, p) =>
        {
          
            var informations = DataContext as CanvasNodesViewModel;
            ProjectData data = new ProjectData();
            data.Projects = selectedTask.Projects;
            
            data.Name = NameBox.Text;
            data.Date = MyDatePicker.SelectedDate;
            data.Id = selectedTask.Id;
         
          
            informations.OnClicked(data);
            nodesview.MapCanvas.Children.Clear();
            nodesview.onUpdate();
            this.Close();
        };
    }
}