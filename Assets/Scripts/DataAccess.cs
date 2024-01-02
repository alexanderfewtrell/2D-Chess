using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class DataAccess
{
	public string NewGame()
    {
		DateTime CurrentTime = DateTime.Now;
		string GameName = CurrentTime.ToString();

        // Open connection
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "ChessDatabase";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS Games (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, GameName TEXT )";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = ($"INSERT INTO Games (GameName) VALUES ('{GameName}')");
        cmnd.ExecuteNonQuery();

        // Close connection
        dbcon.Close();
        return GameName;
    }
}
