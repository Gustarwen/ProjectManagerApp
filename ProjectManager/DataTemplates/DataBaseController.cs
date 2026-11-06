
using Durable.Sqlite;


namespace ProjectManager.DataTemplates;

public class DataBaseController
{
    //definice ovladačů pro SQLite databázi
    public static SqliteRepository<ProjectData> Taskrepository = new SqliteRepository<ProjectData>("Data Source=./task.db");
    public static SqliteRepository<Project> Projectrepositroy = new SqliteRepository<Project>("Data Source=./task.db");
    public static SqliteRepository<Task_Task> ProjectTaskRepository = new ("Data Source=./task.db");

    //konstruktor pro class spravující databázi
    static DataBaseController()
    {
        Taskrepository.InitializeTable(typeof(ProjectData));
        Projectrepositroy.InitializeTable(typeof(Project));
        ProjectTaskRepository.InitializeTable(typeof(Task_Task));
    }



 
    //funkce ukládající Task
    public static void InsertTask(ProjectData data)
    {
       
            Taskrepository.Create(data);
           
           

    }
    public static void DeleteTask(ProjectData data)
    {
        Taskrepository.Delete(data);
        
        Task_Task task = new();
        ;
        task.TaskId = data.Id;
        ProjectTaskRepository.Delete(task);
    }
    public static Project InsertProject(Project data)
    {
        return Projectrepositroy.Create(data);
    }
    public static void DeleteProject(Project data)
    {
        Projectrepositroy.Delete(data);
    }

   

   
}