using System;
using System.Threading.Tasks;
using Durable.Sql;
using Durable.Sqlite;


namespace ProjectManager.DataTemplates;

public class DataBaseController
{
    
    public static SqliteRepository<ProjectData> _taskrepository = new SqliteRepository<ProjectData>("Data Source=./task.db");
    public static SqliteRepository<Project> _projectrepositroy = new SqliteRepository<Project>("Data Source=./task.db");

    static DataBaseController()
    {
        _taskrepository.InitializeTable(typeof(ProjectData));
        _projectrepositroy.InitializeTable(typeof(Project));
    }



 

    public static void InsertTask(ProjectData data)
    {
        try 
        {
            _taskrepository.Create(data);
            Console.WriteLine("Úkol úspěšně vložen do DB.");
        }
        catch (Exception ex) 
        {
            // ZDE uvidíte přesný důvod, proč se úkol neuložil (např. chyba reflexe, chybějící sloupec apod.)
            Console.WriteLine($"CHYBA PŘI UKLÁDÁNÍ: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Detail chyby: {ex.InnerException.Message}");
            }
        }
    }
    public static void DeleteTask(ProjectData data)
    {
        _taskrepository.Delete(data);
    }
    public static void InsertProject(Project data)
    {
        _projectrepositroy.Create(data);
    }
    public static void DeleteProject(Project data)
    {
        _projectrepositroy.Delete(data);
    }
    
}