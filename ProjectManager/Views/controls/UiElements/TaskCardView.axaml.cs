using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;

namespace ProjectManager.Views;

public partial class TaskCardView : UserControl
{
    private int Id;
    public TaskCardView()
    {
        InitializeComponent();
        Finished.IsCheckedChanged += (a, v) =>
        {
            ProjectData data = new()
            {
                
            };
        };
    }
}