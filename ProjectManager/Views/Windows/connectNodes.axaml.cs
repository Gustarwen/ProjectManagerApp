using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Durable.Sql;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;


public partial class connectNodes : Window
{
    public connectNodesViewModel ViewModel;

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    public connectNodes()
    {
        InitializeComponent();
        
    }
}