using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTurnSelect : MonoBehaviour
{
    public GameObject First;
    public GameObject Second;
    public TitleController tc;


    public static int Turn = 1;

  

    public void OnClick()
    {
        Turn= tc.getPlayerTurn();
        Turn *= -1;
        if (Turn == 1)
        {
            Second.SetActive(false);
            First.SetActive(true);
        }
        else 
        {
            First.SetActive(false);
            Second.SetActive(true);
        }
        tc.setPlayerTurn(Turn);
    }
}
