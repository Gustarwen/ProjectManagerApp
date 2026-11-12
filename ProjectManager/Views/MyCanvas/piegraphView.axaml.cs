using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ProjectManager.Views;
public partial class piegraphView : UserControl
{
    public double alpha = 120.0/360.0*(Math.PI*2);

    private List<Line> lines = new();
        

        private void GraphVisual()
        {
            double fullCircle = 2 * Math.PI;
            int steps = 360; // 1 step per degree is plenty

            // Background circle (Blue)
            for (int i = 0; i < steps; i++)
            {
                double startAngle = i * fullCircle / steps;
                double endAngle = (i + 1) * fullCircle / steps;

                lines.Add(new Line
                {
                    Stroke = Brushes.Blue,
                    StrokeThickness = 5,
                    ZIndex = 1,
                    StartPoint = new Point(Math.Cos(startAngle) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(startAngle) * MyPie.Height/2 + MyPie.Height/2),
                    EndPoint = new Point(Math.Cos(endAngle) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(endAngle) * MyPie.Height/2 + MyPie.Height/2)
                });
            }

            // Highlighted slice (Red)
            int alphaSteps = (int)(alpha * steps / fullCircle);
            for (int i = 0; i < alphaSteps; i++)
            {
                double startAngle = i * fullCircle / steps;
                double endAngle = (i + 1) * fullCircle / steps;

                lines.Add(new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 5,
                    ZIndex = 2,
                    StartPoint = new Point(Math.Cos(startAngle-90.0/360.0*Math.PI*2) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(startAngle-90.0/360.0*Math.PI*2) * MyPie.Height/2 + MyPie.Height/2),
                    EndPoint = new Point(Math.Cos(endAngle-90.0/360.0*Math.PI*2) * MyPie.Width/2 + MyPie.Width/2, Math.Sin(endAngle-90.0/360.0*Math.PI*2) * MyPie.Height/2 + MyPie.Height/2)
                });
            }
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
                GraphVisual();
        
                foreach (Line line in lines)
                {
                    Console.WriteLine(line.StartPoint.X + "," + line.StartPoint.Y+" color "+line.Stroke);
            
                    MyPie.Children.Add(line);
                }
            }
           
        };

       
        
    }
}