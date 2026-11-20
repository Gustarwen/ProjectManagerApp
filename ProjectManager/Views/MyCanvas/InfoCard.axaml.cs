using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ProjectManager.Views;

public partial class InfoCard : UserControl
{
    public static readonly StyledProperty<IBrush?> AccentProperty = AvaloniaProperty.Register<InfoCard, IBrush?>("Accent",Brushes.Black);
    public IBrush? Accent{get => GetValue(AccentProperty); set => SetValue(AccentProperty, value); }
    
    
    public static readonly StyledProperty<IBrush?> TitleProperty = AvaloniaProperty.Register<InfoCard, IBrush?>("Title",Brushes.Black);
    public IBrush? Title{get => GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    //
    public static readonly StyledProperty<string?> TitleTextProperty = AvaloniaProperty.Register<InfoCard, string?>("TitleText","");
    public string? TitleText{get => GetValue(TitleTextProperty); set => SetValue(TitleTextProperty, value); }
    
    public static readonly StyledProperty<int?> Number = AvaloniaProperty.Register<InfoCard, int?>("NumberValue",0);
    public int? NumberValue{get => GetValue(Number); set => SetValue(Number, value); }
    
    
    public static readonly StyledProperty<IBrush?> Background = AvaloniaProperty.Register<InfoCard, IBrush?>("BackgroundProperty",Brush.Parse("#003D99"));
    public IBrush? BackgroundProperty{get => GetValue(Background); set => SetValue(Background, value); }
    public InfoCard()
    {
        InitializeComponent();  
    }
}