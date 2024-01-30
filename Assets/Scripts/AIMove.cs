using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIMove
{
    public Text ButtonText;

    public void ChangeMode()
    {
        if (GlobalVariables.Mode == "2Player")
        {
            GlobalVariables.Mode = "AI";
        }
        else
        {
            GlobalVariables.Mode = "2Player";
        }
    }

    public void ChangeText()
    {
        if (GlobalVariables.Mode == "2Player")
        {
            GameObject.FindGameObjectWithTag("AIToggleButtonTextTag").GetComponent<Text>().text = "Change to AI Mode";
        }
        else
        {
            GameObject.FindGameObjectWithTag("AIToggleButtonTextTag").GetComponent<Text>().text = "Change to 2 Player Mode";
        }
    }
}
