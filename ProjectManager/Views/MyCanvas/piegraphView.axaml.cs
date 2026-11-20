using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;
public partial class piegraphView : UserControl
{
    public double alpha = 120.0/360.0*(Math.PI*2);




   
    public void GraphVisual(double thisalpha)
        {
            alpha = thisalpha;
            MyPie.Children.Clear();
            double thickness = 75;
            double size = this.Bounds.Height;
            double center = size / 2.0;
            double fullCircle = 2 * Math.PI;
            int steps = 360; // 1 step per degree is plenty
            Ellipse ellipse = new()
            {
                Stroke = Brush.Parse("#CCE0FF"),
                StrokeThickness = thickness,
                ZIndex = 1,
            };
            ellipse.Width = size;
            ellipse.Height =size;
      
            
            MyPie.Children.Add(ellipse);
            // Background circle (Blue)
            /*for (int i = 0; i < steps; i++)
            {
                double startAngle = i * fullCircle / steps;
                double endAngle = (i + 1) * fullCircle / steps;

                lines.Add(new Line
                {
                    Stroke = Brushes.Blue,
                    StrokeThickness = 25,
                    ZIndex = 1,
                    StartPoint = new Point(Math.Cos(startAngle) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(startAngle) * MyPie.Height/2 + MyPie.Height/2),
                    EndPoint = new Point(Math.Cos(endAngle) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(endAngle) * MyPie.Height/2 + MyPie.Height/2)
                });
            }*/

            // Highlighted slice (Red)
            int alphaSteps = (int)(alpha * steps / fullCircle);
            Polyline polyLine = new()
            {
                Stroke = Brush.Parse("#0066FF"),
                StrokeThickness = thickness,
                ZIndex = 2,
                StrokeJoin =  PenLineJoin.Round,
                StrokeLineCap =  PenLineCap.Round,

            };
   
           
            for (int i = 0; i < alphaSteps; i++)
            {
                double startAngle = i * fullCircle / steps-Math.PI/2;
                
                /*lines.Add(new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 25,
                    ZIndex = 2,
                    StartPoint = new Point(Math.Cos(startAngle-90.0/360.0*Math.PI*2) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(startAngle-90.0/360.0*Math.PI*2) * MyPie.Height/2 + MyPie.Height/2),
                    EndPoint = new Point(Math.Cos(endAngle-90.0/360.0*Math.PI*2) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(endAngle-90.0/360.0*Math.PI*2) * MyPie.Height/2 + MyPie.Height/2)
                });*/
                double radius = center-thickness/2;
                polyLine.Points.Add(new Point( center + Math.Cos(startAngle)*radius,center + Math.Sin(startAngle)*radius));
                
            }

            var label = new TextBlock
            {
                Text = $"{alpha / fullCircle * 100:0} %",
                FontSize = size*0.25,
                Foreground = Brushes.Black,
                FontWeight = FontWeight.Bold,
               
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            var labelHost = new Grid() { Width = size, Height = size,Children = { label }};
         
// Canvas.Left a Top jsou výchozí 0, takže se překryje přesně s kruhem

            MyPie.Children.Add(polyLine);
            MyPie.Children.Add(labelHost);
        }

        
    
    public piegraphView()
    {
        InitializeComponent();
        this.SizeChanged += (s, e) =>
        {
            if (this.Bounds.Height > 0 && this.Bounds.Width > 0)
            {
                MyPie.Height = this.Bounds.Height;
                MyPie.Width = this.Bounds.Width;
                if (DataContext is piegraphViewModel vm)
                {
                  
                    vm.PropertyChanged+=(a, b) => GraphVisual(vm.Alpha);
                    GraphVisual(vm.Alpha);
                   
                }
             
        
                GraphVisual(alpha);
            }
            
           
        };
       

       
        
    }
}