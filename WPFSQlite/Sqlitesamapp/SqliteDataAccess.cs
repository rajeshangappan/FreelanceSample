
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
    public class SqliteDataAccess
    {
        static string appDbloc = "DemoDB.db";
        static string fileName = "test.db";
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string dbFilePath = appDbloc;// System.IO.Path.Combine(folderPath, fileName);
        //public static List<PersonModel> LoadPeople()
        //{
        //    using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        //    {
        //        var output = cnn.Query<PersonModel>("select * from Person", new DynamicParameters());
        //        return output.ToList();
        //    }
        //}

        //public static void SavePerson(PersonModel person)
        //{
        //    using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        //    {
        //        cnn.Execute("insert into Person (FirstName, LastName) values (@FirstName, @LastName)", person);
        //    }
        //}

        public static void CreateDbFile()
        {
            var cr = appDbloc;
            var fileloc = dbFilePath;
            if (!File.Exists(fileloc))
            {
                //File.Delete(fileloc);
                //createsqliteDbFile();
                //BackupDatabase(cr, fileloc);
                createsqliteDbFile();
                BackupDatabase(cr, fileloc);
            }
            //else
            //{
            //    createsqliteDbFile();
            //    BackupDatabase(cr, fileloc);
            //}
        }

        private static string LoadConnectionString(string id = "Default")
        {
            var fileloc = dbFilePath;
            string connectionString = string.Format("Data Source={0}", fileloc);
            return connectionString;
        }
        public static void BackupDatabase(string sourceFile, string destFile)
        {
            using (SQLiteConnection source = new SQLiteConnection(String.Format("Data Source = {0}", sourceFile)))
            using (SQLiteConnection destination = new SQLiteConnection(String.Format("Data Source = {0}", destFile)))
            {
                source.Open();
                destination.Open();
                source.BackupDatabase(destination, "main", "main", -1, null, -1);
            }
        }
        private static SQLiteConnection connection()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            return sqlite_conn;
        }

        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection(LoadConnectionString());
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }
        static void CreateTable(SQLiteConnection conn)
        {

            //   SQLiteCommand sqlite_cmd;
            //   string Createsql = "CREATE TABLE SampleTable
            //      (Col1 VARCHAR(20), Col2 INT)";
            //string Createsql1 = "CREATE TABLE SampleTable1
            //   (Col1 VARCHAR(20), Col2 INT)";
            //sqlite_cmd = conn.CreateCommand();
            //   sqlite_cmd.CommandText = Createsql;
            //   sqlite_cmd.ExecuteNonQuery();
            //   sqlite_cmd.CommandText = Createsql1;
            //   sqlite_cmd.ExecuteNonQuery();

        }

        public static void InsertData(PersonModel person)
        {
            CreateTable();
            SQLiteConnection conn = connection();
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = $"insert into Person (FirstName, LastName) values ('{person.FirstName}', '{person.LastName}')";
            sqlite_cmd.ExecuteNonQuery();
        }

        static void CreateTable()
        {
            try
            {
                SQLiteConnection conn = connection();
                var result = CheckIfTableExists(conn, "Person");
                if (result) return;

                SQLiteCommand sqlite_cmd;

                string Createsql = "CREATE TABLE Person(FirstName VARCHAR(20), LastName VARCHAR(20))";
                //sqlite_cmd.CommandText = " DROP Table 'Person'";
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Createsql;
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }

        }
        private static bool CheckIfTableExists(SQLiteConnection conn, string tableName)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = $"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{tableName}';";
                object result = cmd.ExecuteScalar();
                int resultCount = Convert.ToInt32(result);
                if (resultCount > 0)
                    return true;

            }
            return false;
        }
        public static List<PersonModel> ReadData()
        {
            try
            {
                SQLiteConnection conn = connection();
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM Person";
                var result = new List<PersonModel>();
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    var employeeModel = new PersonModel
                    {
                        LastName = sqlite_datareader.GetString(sqlite_datareader.GetOrdinal("LastName")),
                        FirstName = sqlite_datareader.GetString(sqlite_datareader.GetOrdinal("FirstName")),

                    };
                    result.Add(employeeModel);
                }
                conn.Close();
                return result;
            }
            catch (Exception ex)
            {
                return new List<PersonModel>();
            }


        }
        public static void createsqliteDbFile()
        {
            //SQLiteConnection.CreateFile(dbFilePath);
        }
    }
    //public class DbCreator
    //{
    //    SQLiteConnection dbConnection;
    //    SQLiteCommand command;
    //    string sqlCommand;
    //    string dbPath = System.Environment.CurrentDirectory + "\\DB";
    //    public string dbFilePath;

    //    public DbCreator()
    //    {
    //        if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
    //            Directory.CreateDirectory(dbPath);
    //        dbFilePath = dbPath + "\\test.db";
    //    }
    //    public void createDbFile()
    //    {           
    //        if (!System.IO.File.Exists(dbFilePath))
    //        {
    //            SQLiteConnection.CreateFile(dbFilePath);
    //        }
    //    }

    //    public string createDbConnection()
    //    {
    //        string strCon = string.Format("Data Source={0};", dbFilePath);
    //        dbConnection = new SQLiteConnection(strCon);
    //        dbConnection.Open();
    //        command = dbConnection.CreateCommand();
    //        return strCon;
    //    }

    //    //public void createTables()
    //    //{
    //    //    if (!checkIfExist("MY_TABLE"))
    //    //    {
    //    //        sqlCommand = "CREATE TABLE MY_TBALE(idnt_test INTEGER PRIMARY KEY AUTOINCREMENT,code_test_type INTEGER";
    //    //        executeQuery(sqlCommand);
    //    //    }

    //    //}

    //    public bool checkIfExist(string tableName)
    //    {
    //        command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'";
    //        var result = command.ExecuteScalar();

    //        return result != null && result.ToString() == tableName ? true : false;
    //    }

    //    public void executeQuery(string sqlCommand)
    //    {
    //        SQLiteCommand triggerCommand = dbConnection.CreateCommand();
    //        triggerCommand.CommandText = sqlCommand;
    //        triggerCommand.ExecuteNonQuery();
    //    }

    //    public bool checkIfTableContainsData(string tableName)
    //    {
    //        command.CommandText = "SELECT count(*) FROM " + tableName;
    //        var result = command.ExecuteScalar();

    //        return Convert.ToInt32(result) > 0 ? true : false;
    //    }
    //}
}
