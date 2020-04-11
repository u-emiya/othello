﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{

    private RaycastHit hit;

    public GameObject koma;
    public GameObject Panel;

    public Text targetText;
    public Text touchText;

    public static int playerNum;

    public void setPlayerNum(int num)
    {
        playerNum = num;
    }

    public int getPlayerNum()
    {
        return playerNum;
    }


    void Start()
    {
        InitializaArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
         //   targetText = this.GetComponent<Text>();
targetText.color = new Color(0.2f, 0.2f, 0.2f, 0.35f);
            Panel.SetActive(true);
            touchText.enabled = false;
        }

    }



    //盤面の初期化を行う。
    private void InitializaArray()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                Vector3 pos;
                if ((i == 4 && j == 3) || (i == 3 && j == 4))
                {
                    GameObject whiteKoma = Instantiate(koma);
                    pos = whiteKoma.transform.position;
                    pos.x = i; pos.z = j; pos.y = 0.35f;
                    whiteKoma.transform.position = pos;
                    reverse(whiteKoma);

          
                }
                else if ((i == 4 && j == 4) || (i == 3 && j == 3))
                {
                    GameObject blackKoma = Instantiate(koma);
                    pos = blackKoma.transform.position;
                    pos.x = i; pos.z = j; pos.y = 0.3f;
                    blackKoma.transform.position = pos;


          
                }
            }

    }

    //コマオブジェクトをひっくり返す。
    public void reverse(GameObject koma)
    {

        Vector3 localAngle = koma.transform.localEulerAngles;
        localAngle.x = 180.0f;
        koma.transform.localEulerAngles = localAngle;

    }
}
