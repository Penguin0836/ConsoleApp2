using System.Data.SQLite;
using System.IO;

namespace TestTask
{
    class WorkWithSQLite
    {
        SQLiteConnection con;
        SQLiteCommand cmd;
        public void CreateDatabaseAndTable()
        {
            if (!File.Exists("WordTable.db"))
            {
                SQLiteConnection.CreateFile("WordTable.db");

                string sql = @"CREATE TABLE Words(
                               Word           TEXT      NOT NULL,
                               CountOfWord            INTEGER       NOT NULL
                            );";
                con = new SQLiteConnection("Data Source=WordTable.db;Version=3;");
                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                con = new SQLiteConnection("Data Source=WordTable.db;Version=3;");
            }
        }
        public void AddData(string word, int countOfWord)
        {
            cmd = new SQLiteCommand();
            con = new SQLiteConnection("Data Source=WordTable.db;Version=3;");
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO Words(Word, CountOfWord) VALUES ('" + word + "','" + countOfWord + "')";
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
