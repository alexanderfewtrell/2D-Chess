using System;

public class GameHelper
{
    public string CreateGameName()
    {
        DateTime CurrentTime = DateTime.Now;

        return CurrentTime.ToString();
    }

    public void UpdateScore(string piece)
    {
        DataAccess dataAccess = new DataAccess();
        int CurrentScore = dataAccess.GetScore(GlobalVariables.currentPlayer);

        int AdditionalScore = GetAdditionalScore(piece);

        int NewScore = CurrentScore + AdditionalScore;
        dataAccess.UpdateScore(NewScore, GlobalVariables.currentPlayer);
        switch (GlobalVariables.currentPlayer)
        {
            case"white":
                GlobalVariables.WhiteScore = NewScore;
                break;
            case "black":
                GlobalVariables.BlackScore = NewScore;
                break;
        }
    }

    public int GetAdditionalScore(string piece)
    {
        switch (piece)
        {
            case "black_queen":
            case "white_queen":
                return 9;

            case "black_knight":
            case "white_knight":
            case "black_bishop":
            case "white_bishop":
                return 3;

            case "black_rook":
            case "white_rook":

                return 5;

            case "black_pawn":
            case "white_pawn":
                return 1;
        }
        return 0;
    }
}
