using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class computerPlayer : MonoBehaviour
{
    private int[,] squaresheet = new int[,] {
        {30,-12,0,-1,-1,0,-12,30},
        {-12,-15,-3,-3,-3,-3,-15,-12},
        {0,-3,0,-1,-1,0,-3,0},
        {-1,-3,-1,-1,-1,-1,-3,-1},
        {-1,-3,-1,-1,-1,-1,-3,-1},
        {0,-3,0,-1,-1,0,-3,0},
        {-12,-15,-3,-3,-3,-3,-15,-12},
        {30,-12,0,-1,-1,0,-12,30},
};
    private const int EMPTY = 0;
    private const int WHITE = 1;
    private const int BLACK = -1;

    private int[,] squares=new int[8,8];
    private int currentPlayer;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gamePlay(int player)
    {
        int saveX = 0;int saveZ = 0;
        squares = gameController.getSquares();
        currentPlayer = gameController.getCurrentPlayer();
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (squares[j, i] == 0 && gameController.isPosition(i, j)[4]==9)
                {
                    if (squaresheet[saveZ, saveX] < squaresheet[j, i] || saveX == 0)
                    {
                        saveX = i;
                        saveZ = j;
                    }
                }
            
            }
        }
        if (currentPlayer == WHITE)
        {
            //石を置く
            gameController.putStonePosition(WHITE, saveX, saveZ);

            //ひっくり返す
            gameController.reverseStone(saveX, saveZ, gameController.isPosition(saveX, saveZ));
            //Playerを交代
            gameController.setCurrentPlayer(BLACK);
        }
        //黒のターンのとき
        else if (currentPlayer == BLACK)
        {
            //石を置く
            gameController.putStonePosition(BLACK, saveX, saveZ);

            //ひっくり返す
            gameController.reverseStone(saveX, saveZ, gameController.isPosition(saveX, saveZ));
            //Playerを交代
            gameController.setCurrentPlayer(WHITE);
        }

    }
}
