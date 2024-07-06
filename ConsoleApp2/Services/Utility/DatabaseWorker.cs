using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Utility;

public class DatabaseWorker(SQLiteConnection connection, SQLiteCommand command) : IDatabaseWorker, IDisposable
{
    public void CreateDatabase(string databaseName)
    {
        if (File.Exists(databaseName) || string.IsNullOrEmpty(databaseName)) return;
        File.Create(databaseName).Close();
    }

    public void DropDatabase(string databaseName)
    {
        if (File.Exists(databaseName))
        {
            File.Delete(databaseName);
        }
    }

    public void CreateTable(string tableName, List<(string attributeName, string attributeSetting)> attributes)
    {
        var sql = $"""
                   CREATE TABLE IF NOT EXISTS {tableName}(
                                        {string.Join(",", attributes.Select(x => x.attributeName + " " + x.attributeSetting))}
                                                             );
                   """;
        ExecuteNonQuery(sql);
    }

    public void DropTable(string tableName)
    {
        var sql = $"DROP TABLE IF EXISTS {tableName})";
        ExecuteNonQuery(sql);
    }

    private void ExecuteNonQuery(string sql)
    {
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        connection.Dispose();
        command.Dispose();
    }

    public void Insert(Dictionary<string, int> words)
    {
        var sql = "INSERT INTO words(word, count_of_word) VALUES";
        for (var i = 0; i < words.Count; i++)
        {
            sql += $"('{words.ElementAt(i).Key}', {words.ElementAt(i).Value}),";
        }

        ExecuteNonQuery(sql.TrimEnd(','));

    }
}