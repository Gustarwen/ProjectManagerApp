using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Durable.Sql;
using ProjectManager.DataTemplates;

namespace ProjectManager.Views;

public partial class connectNodes : Window
{
    public connectNodes(ProjectData node)
    {
        InitializeComponent();
        
    }
}