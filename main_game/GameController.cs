using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private int[,] squares = new int[8, 8];
    private IDictionary<int, GameObject> map = new Dictionary<int, GameObject>();


    private const int EMPTY = 0;
    private const int WHITE = 1;
    private const int BLACK = -1;

    private int currentPlayer = BLACK;

    private Camera cameobj;
    private RaycastHit hit;

    public GameObject koma;
    public GameObject Panel;
    public GameObject PlayerTurn;
    public GameObject PauseButton;

    public PanelSlider slide;

    public MovePlayer movePlayer;
    public computerPlayer comPlayer;
    public MiniMaxPlayer minmaxPlayer;
    public abPlayer abPlayer;

    public TitleController tc;


    private int blackCnt = 0;
    private int whiteCnt = 0;
    private int passCount = 0;

    private int playerNum = 1;

    public int turn = 1;

    public int getTurn()
    {
        return turn;
    }

    public int getBlackCnt()
    {
        return blackCnt;
    }

    public int getWhiteCnt()
    {
        return whiteCnt;
    }
    public int[,] getSquares()
    {
        return squares;
    }

    public int getCurrentPlayer()
    {
        return currentPlayer;
    }

    public void setCurrentPlayer(int player)
    {
        currentPlayer = player;
    }

    void Start()
    {
        playerNum = tc.getPlayerNum();

        cameobj = GameObject.Find("Main Camera").GetComponent<Camera>();
        InitializaArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer == WHITE)
        {
            if (!isPutStoneAnyposition())
            {
                currentPlayer = BLACK;
                passCount++;
            }
            else
            {
                if (playerNum == 1)
                    comPlayer.gamePlay(WHITE);
                else
                   movePlayer.gamePlay(WHITE);
                passCount = 0; 
            }
        }
        else
        {
            if (!isPutStoneAnyposition())
            {
                currentPlayer = WHITE;
                passCount++;
            }
            else
            {
                movePlayer.gamePlay(BLACK);
                passCount = 0;
            }

        }
        if (!isEmpty() || passCount == 2)
        {
            isWin();
        }
    }

   

    //全ての空いてるマスにおいて、置ける場所があればtrueを、どこにも置けなければfalseを返す。
    private bool isPutStoneAnyposition()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != EMPTY)
                {
                    continue;
                }
                if (isPosition(i, j,this.squares)[4] == 9)
                    return true;
            }
        }
        return false;
    }

    //座標(x,z)に駒が置けるかどうかの確認
    //置ける場合はint[4]=9となる。また、その場所に駒を置いた場合に駒をひっくり返す方向は1となる。
    //置けない場合はint[4]=-9となる。
    public int[] isPosition(int x, int z,int [,] sq)
    {
        int jdgx, jdgz;
        int[] judge = new int[9];
        judge[4] = -9;
        int dir = 0;
        /*if (sq[z, x] != EMPTY)
            return judge;
            */
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    dir++;
                    continue;
                }
                jdgx = x + i;
                jdgz = z + j;
          
                if ((0 <= jdgx && jdgx <= 7) && (0 <= jdgz && jdgz <= 7))
                {
                    if (sq[jdgz, jdgx] != EMPTY && sq[jdgz, jdgx] != currentPlayer)
                    {

                        if (isDirction(i, j, jdgx, jdgz, sq))
                        {
                            judge[dir] = 1;
                            judge[4] = 9;
                        }
                        else
                            judge[dir] = -1;
                    }
                }
                dir++;
            }

        }

        return judge;
    }

    //座標(x,z)に置いたとき、コマをひっくり返す方向を示す配列dirに従って、コマをひっくり返していく。
    public void reverseStone(int x, int z, int[] dir)
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
                if (squares[reverseZ, reverseX] == currentPlayer)
                    break;
                squares[reverseZ, reverseX] *= -1;
                int key = reverseX * 10 + reverseZ;
                GameObject obj = map[key];
                reverse(obj);
            }
            reverseX = x;
            reverseZ = z;

        }
    }

    //座標(x,z)からdirx,dirzに従って駒をひっくり返すことができるかを示す
    private bool isDirction(int dirx, int dirz, int x, int z, int[,] sq)
    {
        while ((0 <= x && x <= 7) && (0 <= z && z <= 7))
        {
            x += dirx;
            z += dirz;
            if ((x < 0 || 7 < x) || (z < 0 || 7 < z))
                break;
            if (sq[z, x] == currentPlayer)
            {
                return true;
            }
            else if (sq[z, x] == EMPTY)
                return false;
            else if (sq[z, x] != currentPlayer)
            {
                continue;
            }

        }

        return false;
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
                  /*
                if ((i == 2 && j == 7) ||
                    (i == 2 && j == 6)||
                    (i == 2 && j == 5)||
                    (i == 1 && j == 5)||
                    (i == 0 && j == 5))
                {*/

                    squares[j, i] = WHITE;
                    GameObject whiteKoma = Instantiate(koma);
                    pos = whiteKoma.transform.position;
                    pos.x = i; pos.z = j; pos.y = 0.35f;
                    whiteKoma.transform.position = pos;
                    reverse(whiteKoma);

                    int key = i * 10 + j;
                    map.Add(key, whiteKoma);

                }
                 else if ((i == 4 && j == 4) || (i == 3 && j == 3))
                  {
              /*  else if ((i == 3 && j == 7) ||
                    (i == 3 && j == 6) ||
                    (i == 3 && j == 5) ||
                    (i == 3 && j == 4) ||
                    (i == 2 && j == 4) ||
                    (i == 1 && j == 4) ||
                    (i == 0 && j == 4))
                {*/
                    squares[j, i] = BLACK;
                    GameObject blackKoma = Instantiate(koma);
                    pos = blackKoma.transform.position;
                    pos.x = i; pos.z = j; pos.y = 0.3f;
                    blackKoma.transform.position = pos;


                    int key = i * 10 + j;
                    map.Add(key, blackKoma);

                }
                else
                {
                    squares[j, i] = EMPTY;
                }
            }

    }
    //勝敗の判定を行う。
    private void isWin()
    {
        int wcnt = 0;
        int bcnt = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] == 1)
                    wcnt++;
                else if (squares[j, i] == -1)
                    bcnt++;
            }
        }
        blackCnt = bcnt;
        whiteCnt = wcnt;
        Panel.SetActive(true);
        PlayerTurn.SetActive(false);
        PauseButton.SetActive(false);
    }
    //コマオブジェクトを引数に従って配置する。
    public bool putStone(int color, int x, int z, RaycastHit hit)
    {
        //Squaresの値を更新
        if (color == WHITE)
            squares[z, x] = WHITE;
        else
            squares[z, x] = BLACK;

        //Stoneを出力
        GameObject stone = Instantiate(koma);
        if (color == WHITE)
            reverse(stone);
        Vector3 pos = hit.collider.gameObject.transform.position;
        pos.y = 0.35f;
        stone.transform.position = pos;

        //置いた駒の情報を登録
        int key = x * 10 + z;
        map.Add(key, stone);
        turn++;
        return true;
    }
    //putStoneメソッドのRaycastHitが無いバージョン
    public bool putStonePosition(int color, int x, int z)
    {
        //Squaresの値を更新
        if (color == WHITE)
            squares[z, x] = WHITE;
        else
            squares[z, x] = BLACK;

        //Stoneを出力
        GameObject stone = Instantiate(koma);
        if (color == WHITE)
            reverse(stone);
        Vector3 pos = new Vector3();
        pos.x = x;
        pos.y = 0.35f;
        pos.z = z;
        stone.transform.position = pos;

        //置いた駒の情報を登録
        int key = x * 10 + z;
        map.Add(key, stone);
        turn++;
        return true;

    }

    //全てのマス目において空マスがあるか確認
    private bool isEmpty()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] == 0)
                    return true;
            }
        }

        return false;
    }

    //コマオブジェクトをひっくり返す。
    public void reverse(GameObject koma)
    {

        Vector3 localAngle = koma.transform.localEulerAngles;
        localAngle.x = 180.0f;
        koma.transform.localEulerAngles = localAngle;

    }
    //デバッグ用
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


}
