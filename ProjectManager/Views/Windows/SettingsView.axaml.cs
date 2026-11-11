using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ProjectManager.Views;

public partial class SettingsView : Window
{
    public SettingsView()
    {
        InitializeComponent();
        
    }

    private void Green(object? sender,Avalonia.Interactivity.RoutedEventArgs e)
    {
        LinearGradientBrush brush = new LinearGradientBrush();
        
        Application.Current.Resources["redone"] = (Color)Application.Current.Resources["GreenOne"];
        Application.Current.Resources["redtwo"] = (Color)Application.Current.Resources["GreenTwo"];
        Application.Current.Resources["redthree"] = (Color)Application.Current.Resources["GreenThree"];
        Application.Current.Resources["redfour"] = (Color)Application.Current.Resources["GreenFour"];
        brush.GradientStops.Add(new GradientStop((Color)Application.Current.Resources["redone"], 0));
        brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
        brush.StartPoint = new (0,0,RelativeUnit.Relative);
        brush.EndPoint=new (0,1,RelativeUnit.Relative);
        Application.Current.Resources["BackgroundLinearGradient"] = brush;

    }

    private void Red(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LinearGradientBrush brush = new LinearGradientBrush();
        
        Application.Current.Resources["redone"] = (Color)Application.Current.Resources["RedOne"];
        Application.Current.Resources["redtwo"] = (Color)Application.Current.Resources["RedTwo"];
        Application.Current.Resources["redthree"] = (Color)Application.Current.Resources["RedThree"];
        Application.Current.Resources["redfour"] = (Color)Application.Current.Resources["RedFour"];
        brush.GradientStops.Add(new GradientStop((Color)Application.Current.Resources["redone"], 0));
        brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
        brush.StartPoint = new (0,0,RelativeUnit.Relative);
        brush.EndPoint=new (0,1,RelativeUnit.Relative);
        Application.Current.Resources["BackgroundLinearGradient"] = brush;
    }

    private void Blue(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LinearGradientBrush brush = new LinearGradientBrush();
        
        Application.Current.Resources["redone"] = (Color)Application.Current.Resources["BlueOne"];
        Application.Current.Resources["redtwo"] = (Color)Application.Current.Resources["BlueTwo"];
        Application.Current.Resources["redthree"] = (Color)Application.Current.Resources["BlueThree"];
        Application.Current.Resources["redfour"] = (Color)Application.Current.Resources["BlueFour"];
        brush.GradientStops.Add(new GradientStop((Color)Application.Current.Resources["redone"], 0));
        brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
        brush.StartPoint = new (0,0,RelativeUnit.Relative);
        brush.EndPoint=new (0,1,RelativeUnit.Relative);
        Application.Current.Resources["BackgroundLinearGradient"] = brush;
    }

    private void Orange(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LinearGradientBrush brush = new LinearGradientBrush();
        
        Application.Current.Resources["redone"] = (Color)Application.Current.Resources["OrangeOne"];
        Application.Current.Resources["redtwo"] = (Color)Application.Current.Resources["OrangeTwo"];
        Application.Current.Resources["redthree"] = (Color)Application.Current.Resources["OrangeThree"];
        Application.Current.Resources["redfour"] = (Color)Application.Current.Resources["OrangeFour"];
        brush.GradientStops.Add(new GradientStop((Color)Application.Current.Resources["redone"], 0));
        brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
        brush.StartPoint = new (0,0,RelativeUnit.Relative);
        brush.EndPoint=new (0,1,RelativeUnit.Relative);
        Application.Current.Resources["BackgroundLinearGradient"] = brush;
    }

}