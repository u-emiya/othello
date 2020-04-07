using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WhiteCountTxt : MonoBehaviour
{

    private Text targetText;
    public GameController gameController;

    public void showText()
    {
        string wcnt = gameController.getWhiteCnt().ToString();

        this.targetText = this.GetComponent<Text>(); // <---- 追加3
        this.targetText.text = wcnt; // <---- 追加4
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
