using System.Threading.Tasks;
using Durable.Sql;
using Durable.Sqlite;


namespace ProjectManager.DataTemplates;

public class DataBaseController
{
    
    public static SqliteRepository<ProjectData> _repository = new SqliteRepository<ProjectData>("Data Source=./task.db");
    static  DataBaseController()
    {
        _repository.InitializeTable(typeof(ProjectData));
    }

 

    public static void InsertProperty(ProjectData data)
    {
        _repository.Create(data);
    }
    public static void DeleteProperty(ProjectData data)
    {
        _repository.Delete(data);
    }
    
}