using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abPlayer : MonoBehaviour
{

    public GameController gameController;

    private int[,] squaresheet = new int[,] {
        {45,-11,4,-1,-1,4,-11,45},
        {-11,-16,-1,-3,-3,-1,-16,-11},
        {4,-1,2,-1,-1,2,-1,4},
        {-1,-3,-1,0,0,-1,-3,-1},
        {-1,-3,-1,0,0,-1,-3,-1},
        {4,-1,2,-1,-1,2,-1,4},
        {-11,-16,-1,-3,-3,-1,-16,-11},
        {45,-11,4,-1,-1,4,-11,45},
        };
    /*  private int[,] squaresheet = new int[,] {
        {30,-12,0,-1,-1,0,-12,30},
        {-12,-15,-3,-3,-3,-3,-15,-12},
        {0,-3,0,-1,-1,0,-3,0},
        {-1,-3,-1,-1,-1,-1,-3,-1},
        {-1,-3,-1,-1,-1,-1,-3,-1},
        {0,-3,0,-1,-1,0,-3,0},
        {-12,-15,-3,-3,-3,-3,-15,-12},
        {30,-12,0,-1,-1,0,-12,30},
        };*/
    private const int EMPTY = 0;
    private const int WHITE = 1;
    private const int BLACK = -1;

    private int DEEP =5;
    public void setDeep(int d)
    {
        DEEP = d;
    }

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
        int turn= gameController.getTurn();
        currentPlayer = gameController.getCurrentPlayer();
        int saveCP = currentPlayer;
        this.squares = copySquare(gameController.getSquares());
        
        depthNum = DEEP;
        Node n = new Node(0, 0);
        bool flag = true;

        Debug.Log("turn:" + turn);
        if (turn < 44)
        {
            if (this.squares[0,0] == EMPTY && gameController.isPosition(0, 0, this.squares)[4] == 9)
            {
                n = new Node(0, 0);
                flag = false;
            }
            else if (this.squares[0, 7] == EMPTY && gameController.isPosition(7, 0, this.squares)[4] == 9)
            {
                n = new Node(7, 0);
                flag = false;
            }
            else if (this.squares[7, 0] == EMPTY && gameController.isPosition(0, 7, this.squares)[4] == 9)
            {
                n = new Node(0, 7);
                flag = false;
            }
            else if (this.squares[7, 7] == EMPTY && gameController.isPosition(7, 7, this.squares)[4] == 9)
            {
                n = new Node(7, 7);
                flag = false;
            }
        }
        if (flag)
        {
            if (turn > 48)
            {
                setDeep(64-turn+1);
                Debug.Log("set DEEP:" + DEEP);
            }
            n = MiniMax(null, int.MinValue, int.MaxValue);
        }
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
    public int saveX;
    public int saveZ;


    public Node MiniMax(Node p,int alpha ,int beta)
    {
        Node saveNode = new Node(-1, -1);
        int[,] saveSQ = new int[8, 8];
        int current = gameController.getCurrentPlayer();
        //Debug.Log("depth:"+depthNum);
        //printBoard();
        //評価を返す(はず）
        if (depthNum == 0)
        {
            int eva;
            gameController.setCurrentPlayer(current * -1);
            if (gameController.getTurn() > 48)
            {
                eva = totalPoint(current*-1);
               // Debug.Log("evaluation : "+eva);
            }
            else
            {
                eva = evaluation(current*-1);
            }
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
                    saveSQ = copySquare(this.squares);                        
                    if (current == WHITE)
                    {
                        this.squares[j, i] = WHITE;
                        fakeReverseStone(i, j, dir,current);
                        gameController.setCurrentPlayer(BLACK);
                        saveX = i;
                        saveZ = j;
                    }
                    else
                    {
                        this.squares[j, i] = BLACK;
                        fakeReverseStone(i, j, dir,current);
                        gameController.setCurrentPlayer(WHITE);
                        saveX = i;
                        saveZ = j;

                    }
                    depthNum--;
                    n = MiniMax(n, alpha, beta);
                    n.setX(i); n.setZ(j);
                    this.squares = copySquare(saveSQ);
//                    undo(i, j, dir);
                    depthNum++;             

                    //子要素の群から最適なのを選び出す
                    if (depthNum % 2 == DEEP % 2)
                    {
                        if (saveNode.getX() == -1)
                        {
                            saveNode = n;
                            alpha = saveNode.getEva();
                        }
                        else if (n.getEva() > saveNode.getEva())
                        {
                
                            saveNode = n;
                            alpha = saveNode.getEva();
                        }
                        if (saveNode.getEva() > beta)
                        {
                            //Debug.Log("HIT beta");
                            gameController.setCurrentPlayer(current * -1);
                            return saveNode;

                        }
                    }
                    else
                    {
                        if (saveNode.getX() == -1)
                        {
                            saveNode = n;
                            beta = saveNode.getEva();
                        }
                        else if (n.getEva() < saveNode.getEva())
                        {

                            saveNode = n;
                            beta = saveNode.getEva();
                        }
                        if (saveNode.getEva() < alpha)
                        {
                            //Debug.Log("HIT alpha");
                            gameController.setCurrentPlayer(current * -1);
                            return saveNode;
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
       // Debug.Log("evaluation player:" + player);

        int B = evaluatePosition(player)- evaluatePosition(player*-1);
        //int B = evaluatePutPosition(player);
        int C = confirmedPosition(player) - confirmedPosition(player*-1);
        
        int A = isSetEvaluation(player);
        int D = wingMountain(player) - wingMountain(player * -1);
      /*  Debug.Log("evaluation   A  :" + A);
        Debug.Log("evaluation   B  :" + B);
        Debug.Log("evaluation   C  :" + C);*/

        gameController.setCurrentPlayer(player);
    
        return 4 * A + 2 * B + 3 * C+D;
    }
    public int isSetEvaluation(int player)
    {
        int ownCnt = 0;
        int oppositeCnt = 0;
        //Debug.Log("OwnSide");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (this.squares[j, i] == EMPTY && gameController.isPosition(i, j, this.squares)[4] == 9)
                {
                    ownCnt++;
                }
            }
        }
        gameController.setCurrentPlayer(player * -1);
        //Debug.Log("OppositeSide");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (this.squares[j, i] == EMPTY && gameController.isPosition(i, j, this.squares)[4] == 9)
                {
                    oppositeCnt++;
                }
            }
        }
        return ownCnt - oppositeCnt;
    }

    public int totalPoint(int player)
    {
        int ownCnt = 0;
        int oppositeCnt = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] == player)
                    ownCnt++;

                else if (squares[j, i] == player*-1)
                    oppositeCnt++;
            }
        }
        return ownCnt - oppositeCnt;
    }

    public int confirmedPosition(int player)
    {
        int z = 0;
        int x = 0;
        int ownCnt = 0;
        int firstCnt = 0;
        int secondCnt = 0;
        bool apple = false;
        bool banana = false;
        bool cherry = false;
        bool dorian = false;


        for (int i = 0; i < 8; i++)
        {
            if (squares[z, i] == player)
            {
                firstCnt++;
                apple = true;
            }
            else
            {
                break;
            }
        }
        for (int i = 7; i >= 0; i--)
        {
            if (squares[z, i] == player)
            {
                banana = true;
                secondCnt++;
            }
            else
            {
                break;
            }
        }
        if (firstCnt == 8)
            ownCnt += firstCnt;
        else
            ownCnt = firstCnt + secondCnt + ownCnt;
        firstCnt = 0;
        secondCnt = 0;

        z = 7;

        for (int i = 0; i < 8; i++)
        {
            if (squares[z, i] == player)
            {
                cherry = true;
                firstCnt++;
            }
            else
            {
                break;
            }
        }
        for (int i = 7; i >= 0; i--)
        {
            if (squares[z, i] == player)
            {
                dorian = cherry;
                secondCnt++;
            }
            else
            {
                break;
            }
        }
        if (firstCnt == 8)
            ownCnt += firstCnt;
        else
            ownCnt = firstCnt + secondCnt + ownCnt;
        firstCnt = 0;
        secondCnt = 0;

        if (apple)
        {
            for (int j = 1; j < 7; j++)
            {
                if (squares[j, x] == player)
                {
                    firstCnt++;
                }
                else
                {
                    break;
                }
            }
        }
        if (cherry)
        {
            for (int j = 6; j >= 1; j--)
            {
                if (squares[j, x] == player)
                {
                    secondCnt++;
                }
                else
                {
                    break;
                }
            }
        }
        if (firstCnt == 5)
            ownCnt += firstCnt;
        else
            ownCnt = firstCnt + secondCnt + ownCnt;
        firstCnt = 0;
        secondCnt = 0;

        x = 7;

        if (banana)
        {
            for (int j = 1; j < 7; j++)
            {
                if (squares[j, x] == player)
                {
                    firstCnt++;
                }
                else
                {
                    break;
                }
            }
        }
        if (dorian)
        {
            for (int j = 6; j >= 1; j--)
            {
                if (squares[j, x] == player)
                {
                    secondCnt++;
                }
                else
                {
                    break;
                }
            }
        }
        if (firstCnt == 5)
            ownCnt += firstCnt;
        else
            ownCnt = firstCnt + secondCnt + ownCnt;
        firstCnt = 0;
        secondCnt = 0;

        return ownCnt;
    }

    public int evaluatePosition(int player)
    {
        int ownScore = 0 ;
        int oppositeScore = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] == player)
                {
                    ownScore=squaresheet[j, i];
                }else if(squares[j, i] == player*-1)
                {
                    oppositeScore= squaresheet[j, i];

                }

            }
        }
        return ownScore - oppositeScore;
    }

    public int evaluatePutPosition(int player)
    {
        return squaresheet[saveZ, saveX];
    }
    public int wingMountain(int player)
    {
        int z = 0;
        int x = 0;
        int ownCnt = 0;
        int wingCnt = 0;
        int mtCnt = 0;
        bool apple = (squares[0, 0] == player);
        bool banana = (squares[0, 7] == player);
        bool cherry = (squares[7, 0] == player);
        bool dorian = (squares[7, 7] == player);

        if (!apple) { 
            for (int i = 1; i < 7; i++)
            {
                if (squares[z, i] != player)
                {
                    break;
                }
                if (i == 5)
                    wingCnt++;
                if (i == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }
            }
        }
        if (!banana)
        {
            for (int i = 6; i >= 1; i--)
            {
                if (squares[z, i] != player)
                {
                    break;
                }
                if (i == 5)
                    wingCnt++;
                if (i == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }

            }
        }

        z = 7;

        if (!cherry)
        {
            for (int i = 1; i < 7; i++)
            {
                if (squares[z, i] != player)
                {
                    break;
                }
                if (i == 5)
                    wingCnt++;
                if (i == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }
            }
        }
        if (!dorian)
        {
            for (int i = 6; i >= 1; i--)
            {
                if (squares[z, i] != player)
                {
                    break;
                }
                if (i == 5)
                    wingCnt++;
                if (i == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }
            }
        }

        if (!apple)
        {
            for (int j = 1; j < 7; j++)
            {
                if (squares[j, x] != player)
                {
                    break;
                }
                if (j == 5)
                    wingCnt++;
                if (j == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }
            }
        }
        if (!cherry)
        {
            for (int j = 6; j >= 1; j--)
            {
                if (squares[j, x] != player)
                {
                    break;
                }
                if (j == 5)
                    wingCnt++;
                if (j == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }
            }
        }

        x = 7;
        if (!banana)
        {
            for (int j = 1; j < 7; j++)
            {
                if (squares[j, x] != player)
                {
                    break;
                }
                if (j == 5)
                    wingCnt++;
                if (j == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }

            }
        }
        if (!dorian)
        {
            for (int j = 6; j >= 1; j--)
            {
                if (squares[j, x] != player)
                {
                    break;
                }
                if (j == 5)
                    wingCnt++;
                if (j == 6)
                {
                    wingCnt--;
                    mtCnt++;
                }

            }
        }
        ownCnt = mtCnt * 10 - wingCnt * 50;

        return ownCnt;
    }

    /*
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
    */

    //GameControllerクラスにあるやつとは違って、ゲーム上のオブジェクトをひっくり返さない
    public void fakeReverseStone(int x, int z, int[] dir,int player)
    {
        //Debug.Log("player:" + player);
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
       /*     while (true)
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



            }*/
            while (true)
            {
                reverseX += dirx;
                reverseZ += dirz;
                if (this.squares[reverseZ, reverseX] == player)
                    break;
                this.squares[reverseZ, reverseX] *= -1;
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
        /*fakeReverseStone(x, z, dir,(player*-1));
        squares[z, x] = 0;*/
    }

    public int[,] copySquare(int [,] sq)
    {
        int[,] copy = new int[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                copy[j, i] = sq[j, i];
            }
        }
        return copy;

    }
}
