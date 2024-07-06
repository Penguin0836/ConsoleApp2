using System.Collections.Generic;

namespace TestTask.Services.Interfaces;

public interface IDatabaseWorker
{
    void CreateDatabase(string databaseName);
    void DropDatabase(string databaseName);
    void CreateTable(string tableName, List<(string attributeName, string attributeSetting)> attributes);
    void DropTable(string tableName);
    void Insert(Dictionary<string, int> words);
}