using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlackCountTxt: MonoBehaviour {

    private Text targetText;
    public GameController gameController;

    public void showText()
    {
        string bcnt = gameController.getBlackCnt().ToString();
   
        this.targetText = this.GetComponent<Text>(); 
        this.targetText.text = bcnt; 
    }
        // Use this for initialization
    void Start () {
        showText();
	}
	
	// Update is called once per frame
	void Update () {
        showText();
	}
}
