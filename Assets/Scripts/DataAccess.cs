using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class DataAccess
{
    string connection = "URI=file:" + Application.persistentDataPath + "/" + "ChessDatabase";

    public void NewDataBase()
    {
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS Games (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, GameName TEXT )";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();

        // Close connection
        dbcon.Close();
    }

    public void AddGameName(string GameName)
    {
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = ($"INSERT INTO Games (GameName) VALUES ('{GameName}')");
        cmnd.ExecuteNonQuery();

        // Close connection
        dbcon.Close();
    }
}
