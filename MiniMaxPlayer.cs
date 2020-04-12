using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxPlayer : MonoBehaviour
{

    public GameController gameController;


    private const int EMPTY = 0;
    private const int WHITE = 1;
    private const int BLACK = -1;

    private const int DEEP = 2;

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

        public Node(int x,int z)
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
            this.evaluation=x;
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
        Node n = MiniMax(null);
        Debug.Log("X:" + n.getX() + ",Z:" + n.getZ());
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

    public Node MiniMax(Node p)
    {
        Node saveNode=new Node(-1,-1);
        int current = gameController.getCurrentPlayer();
        int savePlayer = current;
        //評価を返す(はず）
        if (depthNum == 0)
        {
            int eva = evaluation(current * -1);
            p.setEva(eva);
            gameController.setCurrentPlayer(current*-1);
            return p;
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int[] dir = gameController.isPosition(i, j,this.squares);
                if (this.squares[j, i] == 0 && dir[4] == 9)
                {
                    Debug.Log("ooooo:curent this class:" + current);
                    Debug.Log("ooooo:curent GC   class:" + gameController.getCurrentPlayer());
                    Debug.Log("curent:"+ current+",deothNum:"+depthNum);
                    Debug.Log("X:" +i + ",Z:" + j);
                    printBoard();
                    Node n= new Node(i,j);
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
                    n.setX(i);n.setZ(j);
                    undo(i, j, dir);
                    depthNum++;

                    Debug.Log("result");

                    if (saveNode.getX() == -1)
                    {
                        Debug.Log("X:" + n.getX() + ",Z:" + n.getZ());

                        saveNode = n;
                    }
                    else if (depthNum % 2 == DEEP % 2)
                    {
                        if (n.getEva() < saveNode.getEva())
                        {
                            Debug.Log("X:" + n.getX() + ",Z:" + n.getZ());

                            saveNode = n;
                        }
                    }
                    else
                    {
                        if (n.getEva() > saveNode.getEva())
                        {
                            Debug.Log("X:" + n.getX() + ",Z:" + n.getZ());
                            saveNode = n;
                        }

                    }

                   
                   
                }
                
            }
        }
        gameController.setCurrentPlayer(current * -1);
        return saveNode;
    }


    public int evaluation(int player)
    {
        int count=0;
        for(int i = 0; i < 8; i++)
        {
            for(int j=0; j < 8; j++)
            {
                if (squares[j, i] == player)
                    count++;
            }
        }
        return count;
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

                int cx = reverseX + 2*dirx;
                int cz = reverseZ + 2*dirz;
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
                    Debug.Log("("+i+","+j+")::"+"〇");
                else if (squares[j, i] == -1)
                    Debug.Log("(" + i + "," + j + ")::" + "●");
            }
        }
    }
    //復元するよ
    public void undo(int x,int z,int[] dir)
    {
        fakeReverseStone(x, z, dir);
        squares[z, x] = 0;
    }
}
