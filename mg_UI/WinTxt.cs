using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WinTxt : MonoBehaviour
{

    private Text targetText;
    public GameController gameController;

    public void showText()
    {
        int wcnt = gameController.getWhiteCnt();
        int bcnt = gameController.getBlackCnt();
        string text;
        if (bcnt > wcnt)
        {
            text = "Black Win!!";
        }
        else if (wcnt > bcnt)
        {
            text = "White Win!!";
        }
        else
            text = "ひきにく";

        this.targetText = this.GetComponent<Text>(); 
        this.targetText.text = text; 
    }
    // Use this for initialization
    void Start()
    {
        showText();
    }

    // Update is called once per frame
    void Update()
    {
        showText();
    }
}