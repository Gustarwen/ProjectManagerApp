using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Messaging;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class canvasNodesView : UserControl
{
   
    private bool pan = false;
    private Point start;
// Přidej tyto 3 proměnné pro držení přesného stavu transformace:
    private double currentZoom = 1.0;
    private double offsetX = 0.0;
    private double offsetY = 0.0;
    private double startOffsetX;
    private double startOffsetY;

    private double x;
    private double y;
    private double linex;
    private double liney;
    private Ellipse template;
    private List<Ellipse> mapaobjects;
    private List<ProjectData> currentData = new List<ProjectData>();
    private canvasNodesViewModel viewModel;
    private Matrix originalMatrix = new  Matrix();

    private  void createMap(ProjectData first)
    {
        
            
        List<List<ProjectManager.DataTemplates.ProjectData>> map = new List<List<ProjectData>>();
   
        
        Queue<ProjectData> candidates = new Queue<ProjectData>();
        candidates.Enqueue(first);
        
        
        //vytvoreni nodes
        List<ProjectData> vrstva = new List<ProjectData>(){first};
        while (candidates.Count > 0)
        {
            map.Add(vrstva.ToList());
            List<ProjectData> dalsiVrstva = new List<ProjectData>();
            foreach (ProjectData node in vrstva)
            {
                node.childerns = new List<ProjectData>();

                foreach (ProjectData data in ProjectManager.DataTemplates.DataBaseController._taskrepository.Query()
                             .Execute().Where(i => i.parentId == candidates.First().Id).ToList()
                             .OrderBy(i => i.parentId))
                {
                    Console.WriteLine($"node s nazvem {candidates.First().Name} ma dite {data}");
                    dalsiVrstva.Add(data);
                    candidates.Enqueue(data);
                    candidates.First().childerns.Add(data);
                }

                if (candidates.Count > 0 && candidates.Peek().Id == node.Id)
                {
                    candidates.Dequeue();
                }
            }
            
            

           
            vrstva = dalsiVrstva;

        }
//vykresleni nodes
        var viewModel = this.DataContext as canvasNodesViewModel;
        for (int i = map.Count -1; i >= 0; i--)
        {
            double predchozi = 0;
            foreach (ProjectData node in map[i])
            {
                mapaobjects = new List<Ellipse>();
                
                Button ellipse = new Button
                {
                    Content = node.Name,
                    Background =  Brushes.White,
                    Foreground = Brushes.Black,
                    Height = 30,
                    MaxWidth = 60
                    
                    
                    
                };
                if (viewModel != null)
                {
                    ellipse.Click += (sender, args) =>{viewModel.OnClicked(node); MapCanvas.Children.Clear();onUpdate();} ;
        
                }
                if (node.childerns.Count == 0)
                {
             
                    node.x = predchozi+200;
                    predchozi += 200;
                 
                    node.y = i*100;
                    
                    
                }
                else
                {
                    double Stredx = (node.childerns[0].x + node.childerns[node.childerns.Count - 1].x) / 2.0 ;
              
                    node.x = Stredx;
                    node.y = i*100;
                    predchozi += x;
                    
                    if (node.x < predchozi + 60)
                    {
                        node.x = predchozi + 60;
                    }
                    predchozi = node.x + 60;
                }

                Canvas.SetLeft(ellipse, node.x);
                Canvas.SetTop(ellipse, node.y);
                
                MapCanvas.Children.Add(ellipse);
                
            }
            
           
        }
        candidates.Enqueue(first);
        do
        {
            foreach (ProjectData node in candidates.First().childerns)
            {
                Line line = new Line
                {
                    
                    StrokeThickness = 5,
                    ZIndex = -1
                };
                line.Bind(Shape.StrokeProperty, new DynamicResourceExtension("redfour"));
                line.StartPoint = new Point(node.x, node.y);
                line.EndPoint = new Point(candidates.First().x+20,candidates.First().y);
                
                candidates.Enqueue(node);
                MapCanvas.Children.Add(line);

            }

            candidates.Dequeue();
          








        } while (candidates.Count > 0);
        
    }

    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        var pointerProperties = e.GetCurrentPoint(Container).Properties;

        if (pointerProperties.IsLeftButtonPressed)
        {
            pan = true;
            start = e.GetPosition(Container);
        

            startOffsetX = offsetX;
            startOffsetY = offsetY;

            e.Pointer.Capture(Container);
        }
    }


    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        if (pan)
        {
            Point current = e.GetPosition(Container);
            
            offsetX = startOffsetX + (current.X - start.X);
            offsetY = startOffsetY + (current.Y - start.Y);

            if (MapCanvas.RenderTransform is MatrixTransform matrixTransform)
            {
                matrixTransform.Matrix = new Matrix(currentZoom, 0, 0, currentZoom, offsetX, offsetY);
            }
        }
        
    }


    private void OnPointerReleased(object sender, PointerReleasedEventArgs e)   
    {
        
        pan = false;
        e.Pointer.Capture(null);
    }

  
   
  

    private void OnWheelChanged(object sender, PointerWheelEventArgs e)
    {
        if (MapCanvas.RenderTransform is not MatrixTransform transform)
            return;
        
        Point p = e.GetPosition(Container);

        Matrix m = transform.Matrix;

        double factor = e.Delta.Y > 0 ? 1.1 : 1.0 / 1.1;

        double currentScale = Math.Sqrt(m.M11 * m.M11 + m.M12 * m.M12);
        double newScale = currentScale * factor;

        if (newScale < 0.1 || newScale > 5.0)
            return;
        
        double ratio = factor;

        Matrix zoom = new Matrix(
            ratio, 0,
            0, ratio,
            p.X * (1 - ratio),
            p.Y * (1 - ratio));

        m = m * zoom;
        currentZoom = currentZoom*factor;

        transform.Matrix = m;
        offsetX = m.M31;
        offsetY = m.M32;

    }

    public  void onUpdate()
    {
       
        Console.WriteLine(viewModel.project.Id);
        var freshFirst = ProjectManager.DataTemplates.DataBaseController._taskrepository.Query()
            .Where(p => p.parentId == 0 && p.projectId==viewModel.project.Id).Execute().First();
        
        // 3. Znovu vykreslíme
        createMap(freshFirst);
    }




    public canvasNodesView()
    {
        
        
        
        
        
        Console.WriteLine("spusti se toto?");
        InitializeComponent();
        viewModel = this.DataContext as canvasNodesViewModel;

        DataContextChanged += (sender, args) =>
        {
            viewModel = this.DataContext as canvasNodesViewModel;

            if (viewModel != null)
            {
                if (viewModel.project != null)
                {
                    // Teprve v tento moment máme 100% jistotu, jaké ID projektu kreslíme!
                    Console.WriteLine($"[ÚSPĚCH] Tento canvas bude vykreslovat projekt s ID: {viewModel.project.Id}");
                    
                    // Nyní je to bezpečné, aplikace už nespadne
                    onUpdate();
                }
                else
                {
                    Console.WriteLine("[VAROVÁNÍ] DataContext přiřazen, ale vlastnost 'project' uvnitř canvasNodesViewModelu je NULL!");
                }
            }
            else
            {
                Console.WriteLine("[VAROVÁNÍ] Přiřazený DataContext není typu canvasNodesViewModel!");
            }
        };
      




    }
}