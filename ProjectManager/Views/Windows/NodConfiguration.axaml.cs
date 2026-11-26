using System;
using System.ComponentModel.DataAnnotations;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class NodConfiguration : Window
{
    public void pressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            return;
        }
        if (e.ClickCount == 2)
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        else
            BeginMoveDrag(e);
    }
    public NodConfiguration(ProjectData selectedTask,CanvasNodesView nodesview)
    {
        InitializeComponent();
       
        Accept.Click += (a, p) =>
        {
           
            var informations = DataContext as CanvasNodesViewModel;
            if (MyDatePicker.SelectedDate == null||NameBox.Text == null) return;
            ProjectData data = new ProjectData();
            data.Type = 'N';
            data.deadLine =(DateTimeOffset)MyDatePicker.SelectedDate;
            data.ProjectId = selectedTask.ProjectId;
            data.start =DateTime.Now;
            data.Name = NameBox.Text;
            data.Date = MyDatePicker.SelectedDate;
            data.Id = selectedTask.Id;
         
          
            informations.OnClicked(data);
            nodesview.MapCanvas.Children.Clear();
            nodesview.onUpdate();
            this.Close();
        };

        Close.Click += (a, p) =>
        {
            this.Close();
        };
    }
}