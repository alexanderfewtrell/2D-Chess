using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class DataAccess
{
    string connection = "URI=file:" + Application.persistentDataPath + "/" + "ChessDatabase";

    // creates the new database and adds columns
    public void NewDataBase()
    {
        CreateGamesTable();
        CreateMovesTable();
    }

    public void CreateGamesTable()
    {
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS Games (GameNameId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, GameName TEXT, WhiteScore INT, BlackScore INT)";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();

        // Close connection
        dbcon.Close();
    }

    public void CreateMovesTable()
    {
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS Moves (MoveId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Piece TEXT NOT NULL, StartCoords TEXT NOT NULL, EndCoords TEXT NOT NULL, PieceTaken TEXT, GameNameId INT, FOREIGN KEY(GameNameId) REFERENCES GAMES(GameNameId))";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();

        // Close connection
        dbcon.Close();
    }

    public void InsertMove(string piece, string startCoords, string endCoords, long gameNameId, string pieceTaken)
    {
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = ($"INSERT INTO Moves (Piece, StartCoords, EndCoords, PieceTaken, GameNameId) VALUES ('{piece}', '{startCoords}', '{endCoords}', '{pieceTaken}', {gameNameId})");
        cmnd.ExecuteNonQuery();

        // Close connection
        dbcon.Close();
    }

    //puts the game name in the database and sets initial scores to 0
    public long AddGameName(string GameName)
    {
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = ($"INSERT INTO Games (GameName, WhiteScore, BlackScore) VALUES ('{GameName}', 0, 0)");
        cmnd.ExecuteNonQuery();

        cmnd.CommandText = "select last_insert_rowid()";
        long Id = (long)cmnd.ExecuteScalar();

        // Close connection
        dbcon.Close();

        return Id;
    }

    //gets the score from the database depending on which color is playing
    public int GetScore(string color)
    {
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();

        string CommandText = "";
        switch (color)
        {
            case "white":
                CommandText = ($"SELECT WhiteScore FROM Games WHERE GameNameId = {GlobalVariables.GameId}");
                break;

            case "black":
                CommandText = ($"SELECT BlackScore FROM Games WHERE GameNameId = {GlobalVariables.GameId}");
                break;
        }

        cmnd.CommandText = CommandText;
        int Score = (int)cmnd.ExecuteScalar();

        // Close connection
        dbcon.Close();

        return Score;
    }

    //updates the database with the new score for the appropriate color
    public void UpdateScore(int newScore, string color)
    {
        string CommandText = "";
        switch (color)
        {
            case "white":
                CommandText = ($"UPDATE Games SET WhiteScore = {newScore} WHERE GameNameId = {GlobalVariables.GameId}");
                break;

            case "black":
                CommandText = ($"UPDATE Games SET BlackScore = {newScore} WHERE GameNameId = {GlobalVariables.GameId}");
                break;
        }
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = CommandText;
        cmnd.ExecuteNonQuery();

        // Close connection
        dbcon.Close();
    }
}
