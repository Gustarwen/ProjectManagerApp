using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;
using ProjectManager.Views.Windows;

namespace ProjectManager.Views;

public partial class ProjectAddDialogView : Window
{
    public ProjectAddDialogView()
    {
    

    InitializeComponent();
    List<ProjectData> projectDatas = new ();
        foreach (var project in DataBaseController.Taskrepository.Query().Execute().ToList())
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = project.Name;
            item.Tag = project.Id;
            projectDatas.Add(project);
        }
        Accept.Click += (a, p) =>
        {
           
            var informations = DataContext as CanvasNodesViewModel;
            if (MyDatePicker.SelectedDate == null||NameBox.Text == null) return;
            ProjectData data = new ProjectData();
            data.Type = 'N';
            data.deadLine =(DateTimeOffset)MyDatePicker.SelectedDate;
            data.ProjectId = projectDatas.Where(p=>p.Id == (int)ProjectCombo.Tag).FirstOrDefault().ProjectId;
            data.start =DateTime.Now;
            data.Name = NameBox.Text;
            data.Date = MyDatePicker.SelectedDate;
            data.ParentId = 
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