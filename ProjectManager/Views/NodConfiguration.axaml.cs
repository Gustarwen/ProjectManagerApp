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
            Console.WriteLine("hello...vytvořil jsem ");
            var informations = DataContext as CanvasNodesViewModel;
            ProjectData data = new ProjectData();
            data.ProjectId = selectedTask.ProjectId;
            
            data.Name = NameBox.Text;
            data.Date = MyDatePicker.SelectedDate;
            data.Id = selectedTask.Id;
            Console.WriteLine($"toto je node ktera byla vytvorena {selectedTask.Id} a node dostala {data.ParentId}");
          
            informations.OnClicked(data);
            nodesview.MapCanvas.Children.Clear();
            nodesview.onUpdate();
            this.Close();
        };
    }
}