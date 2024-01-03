using System;

public class GameHelper
{
    public string CreateGameName()
    {
        DateTime CurrentTime = DateTime.Now;

        return CurrentTime.ToString();
    }
}
