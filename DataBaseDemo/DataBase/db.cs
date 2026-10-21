using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataBaseDemo.DataBase;

public class ContextDb 
{
    
}
public class db
{
    public static void LoadDataBase()
    {
        string connectionString = "Data Source=SQLite.db";
        Console.WriteLine(connectionString);
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = @"CREATE TABLE IF NOT EXISTS users( Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Email TEXT NOT NULL, Password TEXT NOT NULL);";
            
            using (var command = new SqliteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static void CreateRecord()
    {
    }
}