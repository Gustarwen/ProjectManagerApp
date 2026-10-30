using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.VisualTree;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class CanvasNodesView : UserControl
{
   
    private bool _pan = false;
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
    private List<task> mapaobjects;
    private List<ProjectData> currentData = new List<ProjectData>();
    private CanvasNodesViewModel viewModel;
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
                node.Childerns = new List<ProjectData>();

                foreach (ProjectData data in ProjectManager.DataTemplates.DataBaseController.Taskrepository.Query()
                             .Execute().Where(i => i.ParentId == candidates.First().Id).ToList()
                             .OrderBy(i => i.ParentId))
                {
                    Console.WriteLine($"node s nazvem {candidates.First().Name} ma dite {data}");
                    dalsiVrstva.Add(data);
                    candidates.Enqueue(data);
                    candidates.First().Childerns.Add(data);
                }

                if (candidates.Count > 0 && candidates.Peek().Id == node.Id)
                {
                    candidates.Dequeue();
                }
            }
            
            

           
            vrstva = dalsiVrstva;

        }
//vykresleni nodes
       
        for (int i = map.Count -1; i >= 0; i--)
        {
            double predchozi = 0;
            foreach (ProjectData node in map[i])
            {
                mapaobjects = new List<task>();

                task ellipse = new task();
                ellipse.Name.Text = node.Name;
                if (viewModel != null)
                {
                    ellipse.Add.Click += (sender, args) =>
                    {
                        Window root =this.FindAncestorOfType<Window>();
                        viewModel.showDialog(this,root,node); } ;
        
                }
                if (node.Childerns.Count == 0)
                {
             
                    node.X = predchozi+500;
                    predchozi += 500;
                 
                    node.Y = i*100;
                    
                    
                }
                else
                {
                    double Stredx = (node.Childerns[0].X + node.Childerns[node.Childerns.Count - 1].X) / 2.0 ;
              
                    node.X = Stredx;
                    node.Y = i*100;
                    predchozi += x;
                    
                    if (node.X < predchozi + 500)
                    {
                        node.X = predchozi + 500;
                    }
                    predchozi = node.X + 500;
                }

                Canvas.SetLeft(ellipse, node.X);
                Canvas.SetTop(ellipse, node.Y);
                
                MapCanvas.Children.Add(ellipse);
                
            }
            
           
        }
        candidates.Enqueue(first);
        do
        {
            foreach (ProjectData node in candidates.First().Childerns)
            {
                Line line = new Line
                {
                    
                    StrokeThickness = 5,
                    ZIndex = -1
                };
                line.Bind(Shape.StrokeProperty, new DynamicResourceExtension("redfour"));
                line.StartPoint = new Point(node.X+125, node.Y+20);
                line.EndPoint = new Point(candidates.First().X+125,candidates.First().Y+20);
                
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
            _pan = true;
            start = e.GetPosition(Container);
        

            startOffsetX = offsetX;
            startOffsetY = offsetY;

            e.Pointer.Capture(Container);
        }
    }


    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        if (_pan)
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
        
        _pan = false;
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
       
        Console.WriteLine(viewModel.Project.Id);
        var freshFirst = ProjectManager.DataTemplates.DataBaseController.Taskrepository.Query()
            .Where(p => (p.ParentId == 0||p.ParentId == null) && p.ProjectId==viewModel.Project.Id).Execute().First();
        
        // 3. Znovu vykreslíme
        createMap(freshFirst);
    }




    public CanvasNodesView()
    {
        
        
        
        
        
        Console.WriteLine("spusti se toto?");
        InitializeComponent();
        viewModel = DataContext as CanvasNodesViewModel;

        DataContextChanged += (sender, args) =>
        {
            viewModel = this.DataContext as CanvasNodesViewModel;

            if (viewModel != null)
            {
                if (viewModel.Project != null)
                {
                    // Teprve v tento moment máme 100% jistotu, jaké ID projektu kreslíme!
                    Console.WriteLine($"[ÚSPĚCH] Tento canvas bude vykreslovat projekt s ID: {viewModel.Project.Id}");
                    
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