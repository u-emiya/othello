using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxPlayer : MonoBehaviour
{

    public GameController gameController;


    private const int EMPTY = 0;
    private const int WHITE = 1;
    private const int BLACK = -1;

    private const int DEEP = 5;

    private int[,] squares = new int[8, 8];
    private int currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public class Node
    {
        int x;
        int z;
        int evaluation;

        public Node(int x, int z)
        {
            this.x = x;
            this.z = z;
            this.evaluation = 0;
        }

        public int getEva()
        {
            return evaluation;
        }

        public void setEva(int x)
        {
            this.evaluation = x;
        }

        public int getX()
        {
            return x;
        }
        public int getZ()
        {
            return z;
        }
        public void setX(int x)
        {
            this.x = x;
        }
        public void setZ(int z)
        {
            this.z = z;
        }

    }
    public void gamePlay(int player)
    {
        currentPlayer = gameController.getCurrentPlayer();
        int saveCP = currentPlayer;
        this.squares = gameController.getSquares();
        depthNum = DEEP;
        abcnt = 0;
        //Debug.Log("MiniMax");
        Node n = MiniMax(null);
        //Debug.Log("X:" + n.getX() + ",Z:" + n.getZ());
        gameController.setCurrentPlayer(saveCP);

        if (currentPlayer == WHITE)
        {
            //石を置く
            gameController.putStonePosition(WHITE, n.getX(), n.getZ());

            //ひっくり返す
            gameController.reverseStone(n.getX(), n.getZ(), gameController.isPosition(n.getX(), n.getZ(), this.squares));
            //Playerを交代
            gameController.setCurrentPlayer(BLACK);
        }
        //黒のターンのとき
        else if (currentPlayer == BLACK)
        {
            //石を置く
            gameController.putStonePosition(BLACK, n.getX(), n.getZ());

            //ひっくり返す
            gameController.reverseStone(n.getX(), n.getZ(), gameController.isPosition(n.getX(), n.getZ(), this.squares));
            //Playerを交代
            gameController.setCurrentPlayer(WHITE);
        }


    }
    private int depthNum;
    private int abcnt = 0;
    private Node abNode;
    private int abEva;

    public Node MiniMax(Node p)
    {
        Node saveNode = new Node(-1, -1);
        int current = gameController.getCurrentPlayer();
        int savePlayer = current;
        int contsw = 0;
        int banana = 0;

        //評価を返す(はず）
        if (depthNum == 0)
        {
            gameController.setCurrentPlayer(current * -1);
            // currentPlayer = current * -1;
            int eva = evaluation(current * -1);
            //int eva = hikiwake(current * -1);
            p.setEva(eva);
            return p;
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int[] dir = gameController.isPosition(i, j, this.squares);
                if (this.squares[j, i] == 0 && dir[4] == 9)
                {
                    Node n = new Node(i, j);
                    if (current == WHITE)
                    {
                        this.squares[j, i] = WHITE;
                        fakeReverseStone(i, j, dir);
                        gameController.setCurrentPlayer(BLACK);
                    }
                    else
                    {
                        this.squares[j, i] = BLACK;
                        fakeReverseStone(i, j, dir);
                        gameController.setCurrentPlayer(WHITE);
                    }
                    depthNum--;
                    n = MiniMax(n);
                    n.setX(i); n.setZ(j);
                    undo(i, j, dir);
                    depthNum++;



                    //  Debug.Log("result");
                    //子要素の群から最適なのを選び出す
                    if (saveNode.getX() == -1)
                    {
                 
                        saveNode = n;
                    }
                    else if (depthNum % 2 == DEEP % 2)
                    {
                        if (n.getEva() > saveNode.getEva())
                        {
                 
                            saveNode = n;
                        }
                    }
                    else
                    {
                        if (n.getEva() < saveNode.getEva())
                        {
                            saveNode = n;
                        }

                    }

                    if (depthNum == 1)
                        banana++;
                  /*  if (abcnt > 0 && banana > 0)
                    {
                        if (depthNum % 2 == DEEP % 2 && saveNode.getEva() > abNode.getEva())
                        {
                            contsw++;
                            break;
                        }
                        else if (depthNum % 2 != DEEP % 2 && saveNode.getEva() < abNode.getEva())
                        {
                            contsw++;
                            break;
                        }
                    }*/
                    banana++;



                }

            }
            if (contsw == 1)
            {
                break;
            }
        }



        abcnt++;
        if (contsw == 1)
        {
            contsw = 0;
        }
        else
        {
            abNode = saveNode;
        }

        gameController.setCurrentPlayer(current * -1);
        return saveNode;
    }

    public int hikiwake(int player)
    {
        int ownCnt = 0;
        int oppositeCnt = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (gameController.isPosition(i, j, this.squares)[4] == 9)
                    ownCnt++;
            }
        }
        gameController.setCurrentPlayer(player * -1);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (gameController.isPosition(i, j, this.squares)[4] == 9)
                    oppositeCnt++;
            }
        }
        gameController.setCurrentPlayer(player);

        int total = ownCnt - oppositeCnt;
        if (total < 0)
            total *= -1;
        return 100 - (total / 64) * 100;
    }

    public int evaluation(int player)
    {
        int ownCnt = 0;
        int oppositeCnt = 0;
        int k = 3;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (gameController.isPosition(i, j, this.squares)[4] == 9)
                    ownCnt++;
            }
        }
        gameController.setCurrentPlayer(player * -1);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (gameController.isPosition(i, j, this.squares)[4] == 9)
                    oppositeCnt++;
            }
        }
        int B = evaluatePosition(player) - evaluatePosition(player * -1);
        gameController.setCurrentPlayer(player);
        int A = ownCnt - oppositeCnt;
        return A + k * B;
    }

    public int evaluatePosition(int player)
    {
        int score = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 0 && j == 0 && squares[j, i] == player)
                    score += 10;
                if ((i == 2 && j == 0 && squares[j, i] == player) ||
                    (i == 0 && j == 2 && squares[j, i] == player) ||
                    (i == 2 && j == 2 && squares[j, i] == player))
                    score += 1;
                if ((i == 1 && j == 0 && squares[j, i] == player) ||
                    (i == 0 && j == 1 && squares[j, i] == player) ||
                    (i == 1 && j == 1 && squares[j, i] == player))
                    score -= 5;

            }
        }
        for (int i = 5; i < 8; i++)
        {
            for (int j = 5; j < 8; j++)
            {
                if (i == 7 && j == 7 && squares[j, i] == player)
                    score += 10;
                if ((i == 7 && j == 5 && squares[j, i] == player) ||
                    (i == 5 && j == 7 && squares[j, i] == player) ||
                    (i == 5 && j == 5 && squares[j, i] == player))
                    score += 1;
                if ((i == 6 && j == 7 && squares[j, i] == player) ||
                    (i == 7 && j == 6 && squares[j, i] == player) ||
                    (i == 6 && j == 6 && squares[j, i] == player))
                    score -= 5;

            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 5; j < 8; j++)
            {
                if (i == 0 && j == 7 && squares[j, i] == player)
                    score += 10;
                if ((i == 2 && j == 5 && squares[j, i] == player) ||
                    (i == 2 && j == 7 && squares[j, i] == player) ||
                    (i == 0 && j == 5 && squares[j, i] == player))
                    score += 1;
                if ((i == 1 && j == 6 && squares[j, i] == player) ||
                    (i == 1 && j == 7 && squares[j, i] == player) ||
                    (i == 0 && j == 6 && squares[j, i] == player))
                    score -= 5;

            }
        }
        for (int i = 5; i < 8; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 7 && j == 0 && squares[j, i] == player)
                    score += 10;
                if ((i == 5 && j == 2 && squares[j, i] == player) ||
                    (i == 7 && j == 2 && squares[j, i] == player) ||
                    (i == 5 && j == 0 && squares[j, i] == player))
                    score += 1;
                if ((i == 6 && j == 1 && squares[j, i] == player) ||
                    (i == 7 && j == 1 && squares[j, i] == player) ||
                    (i == 6 && j == 0 && squares[j, i] == player))
                    score -= 5;

            }
        }

        return score;
    }

    //GameControllerクラスにあるやつとは違って、ゲーム上のオブジェクトをひっくり返さない
    public void fakeReverseStone(int x, int z, int[] dir)
    {
        int dirx = 0, dirz = 0;
        int reverseX = x;
        int reverseZ = z;
        for (int i = 0; i < 9; i++)
        {
            if (dir[i] != 1)
            {
                continue;
            }

            if (i == 0)
            {
                dirx = -1;
                dirz = -1;
            }
            else if (i == 1)
            {
                dirx = -1;
                dirz = 0;
            }
            else if (i == 2)
            {
                dirx = -1;
                dirz = 1;
            }
            else if (i == 3)
            {
                dirx = 0;
                dirz = -1;
            }
            else if (i == 5)
            {
                dirx = 0;
                dirz = 1;
            }
            else if (i == 6)
            {
                dirx = 1;
                dirz = -1;
            }
            else if (i == 7)
            {
                dirx = 1;
                dirz = 0;
            }
            else if (i == 8)
            {
                dirx = 1;
                dirz = 1;
            }
            while (true)
            {
                reverseX += dirx;
                reverseZ += dirz;
                this.squares[reverseZ, reverseX] *= -1;

                int cx = reverseX + 2 * dirx;
                int cz = reverseZ + 2 * dirz;
                if (cx < 0 || cz < 0 || cx > 7 || cz > 7)
                    break;
                if (this.squares[cz, cx] == EMPTY)
                    break;


            }
            reverseX = x;
            reverseZ = z;

        }
    }
    public void printBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] == 1)
                    Debug.Log("(" + i + "," + j + ")::" + "〇");
                else if (squares[j, i] == -1)
                    Debug.Log("(" + i + "," + j + ")::" + "●");
            }
        }
    }
    //復元するよ
    public void undo(int x, int z, int[] dir)
    {
        fakeReverseStone(x, z, dir);
        squares[z, x] = 0;
    }
}
