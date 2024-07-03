using System.Data.SQLite;
using System.IO;

namespace TestTask
{
    internal class WorkWithSqLite
    {
        private SQLiteConnection _con;
        private SQLiteCommand _cmd;
        public void CreateDatabaseAndTable()
        {
            if (!File.Exists("WordTable.db"))
            {
                SQLiteConnection.CreateFile("WordTable.db");

                var sql = @"CREATE TABLE Words(
                               Word           TEXT      NOT NULL,
                               CountOfWord            INTEGER       NOT NULL
                            );";
                _con = new SQLiteConnection("Data Source=WordTable.db;Version=3;");
                _con.Open();
                _cmd = new SQLiteCommand(sql, _con);
                _cmd.ExecuteNonQuery();
                _con.Close();
            }
            else
            {
                _con = new SQLiteConnection("Data Source=WordTable.db;Version=3;");
            }
        }
        public void AddData(string word, int countOfWord)
        {
            _cmd = new SQLiteCommand();
            _con = new SQLiteConnection("Data Source=WordTable.db;Version=3;");
            _con.Open();
            _cmd.Connection = _con;
            _cmd.CommandText = "INSERT INTO Words(Word, CountOfWord) VALUES ('" + word + "','" + countOfWord + "')";
            _cmd.ExecuteNonQuery();
            _con.Close();
        }
    }
}
