using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Durable.Sql;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;


public partial class connectNodes : Window
{
    public connectNodesViewModel ViewModel;
    public connectNodes()
    {
        InitializeComponent();
        
    }
}