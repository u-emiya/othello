﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private int[,] squares = new int[8, 8];
    private const int EMPTY = 0;
    private const int WHITE = 1;
    private const int BLACK = -1;

    public GameController gameController;

    private RaycastHit hit;
    private Camera cameobj;
    private int currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        cameobj = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void gamePlay(int player)
    {
        squares = gameController.getSquares();
        currentPlayer = gameController.getCurrentPlayer();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cameobj.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                int x = (int)hit.collider.gameObject.transform.position.x;
                int z = (int)hit.collider.gameObject.transform.position.z;
                int[] dir = gameController.isPosition(x, z, this.squares);
                if(0<=x&&x<=7 && 0<=z&&z<=7)
                if (squares[z, x] == EMPTY && dir[4] == 9 && hit.collider.gameObject.CompareTag("BoardPiece"))
                {
                    //白のターンのとき
                    if (currentPlayer == WHITE)
                    {
                        //石を置く
                        gameController.putStone(WHITE, x, z,hit);

                        //ひっくり返す
                        gameController.reverseStone(x, z, dir);
                        //Playerを交代
                        gameController.setCurrentPlayer(BLACK);
                    }
                    //黒のターンのとき
                    else if (currentPlayer == BLACK)
                    {
                        //石を置く
                        gameController.putStone(BLACK, x, z,hit);

                        //ひっくり返す
                        gameController.reverseStone(x, z, dir);
                        //Playerを交代
                        gameController.setCurrentPlayer(WHITE);
                    }
                }

            }
        }
    }

}

