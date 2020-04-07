using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

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
    public PanelSlider slide;


    private int blackCnt = 0;
    private int whiteCnt = 0;
    private int passCount = 0;

    public int getBlackCnt()
    {
        return blackCnt;
    }

    public int getWhiteCnt()
    {
        return whiteCnt;
    }

    public int getCurrentPlayer()
    {
        return currentPlayer;
    }

    void Start () {
        cameobj = GameObject.Find("Main Camera").GetComponent<Camera>();
        InitializaArray();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPlayer == WHITE)
        {
            if (!isPutStoneAnyposition())
            {
                currentPlayer = BLACK;
                passCount++;
            }
            else
            {
                gamePlay(WHITE);
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
                gamePlay(BLACK);
                passCount = 0;
            }

        }
        if (!isEmpty() || passCount==2)
        {
            isWin();
        }
	}

    private void gamePlay(int player)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cameobj.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit))
            {
                int x = (int)hit.collider.gameObject.transform.position.x;
                int z = (int)hit.collider.gameObject.transform.position.z;
                int[] dir= isPosition(x, z);
                if (squares[z, x] == EMPTY && dir[4]==9)
                {
                    //白のターンのとき
                    if (currentPlayer == WHITE)
                    {
                        //Squaresの値を更新
                        squares[z, x] = WHITE;
                 
                        //Stoneを出力
                        GameObject stone = Instantiate(koma);
                        reverse(stone);
                        Vector3 pos = hit.collider.gameObject.transform.position;
                        pos.y = 0.35f;
                        stone.transform.position = pos;
                        
                        //置いた駒の情報を登録
                        int key = x * 10+z;
                        Debug.Log("oiteiru white");
                        Debug.Log("x:" + x + ",z:" + z + "key:" + key);
                        map.Add(key, stone);

                        reverseStone(x, z, dir);
                        //Playerを交代
                        currentPlayer = BLACK;
                    }
                    //黒のターンのとき
                    else if (currentPlayer == BLACK)
                    {
                        //Squaresの値を更新
                        squares[z, x]= BLACK;

                        //Stoneを出力
                        GameObject stone = Instantiate(koma);
                        Vector3 pos = hit.collider.gameObject.transform.position;
                        pos.y = 0.35f;
                        stone.transform.position = pos;

                        int key = x * 10 + z;
                        Debug.Log("oiteiru black");
                        Debug.Log("x:" + x + ",z:" + z + "key:" + key);
                        map.Add(key, stone);

                        reverseStone(x, z, dir);
                          //Playerを交代
                        currentPlayer = WHITE;
                    }
                }
                
            }
        }
    }

    private bool isPutStoneAnyposition()
    {
       for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (squares[j, i] != EMPTY)
                {
                    continue;
                }
                if (isPosition(i, j)[4] == 9)
                    return true;
            }
        }
        return false;
    }

    //座標(x,z)に駒が置けるかどうかの確認
    //置ける場合はint[4]=9となる。また、その場所に駒を置いた場合に駒をひっくり返す方向は1となる。
    //置けない場合はint[4]=-9となる。
    private int[] isPosition(int x,int z)
    {
        int jdgx, jdgz;
        int[] judge = new int[9];
        judge[4] = -9;
        int dir = 0;

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
                Debug.Log("number:" + dir);
                Debug.Log("jdgx:" + jdgx + ",jdgz:" + jdgz);

                if ((0 <= jdgx && jdgx <= 7) && (0 <= jdgz && jdgz <= 7))
                {
                    if (squares[jdgz, jdgx] != EMPTY && squares[jdgz,jdgx] != currentPlayer)
                    {

                        if (isDirction(i, j, jdgx, jdgz))
                        {
                            Debug.Log("HIT");
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
    private void reverseStone(int x,int z,int[] dir)
    {
        int dirx=0, dirz=0;
        int reverseX = x;
        int reverseZ = z;
        for (int i = 0; i < 9; i++)
        {
            Debug.Log("dir[" +i+"]:"+dir[i]);
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
            while(true)
            {
                reverseX += dirx;
                reverseZ += dirz;
                if (squares[reverseZ, reverseX] == currentPlayer)
                    break;
                squares[reverseZ, reverseX] *= -1;
                int key = reverseX * 10 + reverseZ;
                /*Debug.Log("hikkurikaeshiteru");
                Debug.Log("x:" + x + ",z:" + z + "key:" + key);
                Debug.Log("squares:" + squares[z, x]);*/
                GameObject obj = map[key];
                reverse(obj);
            }
            reverseX = x;
            reverseZ = z;



        }
    }

    //座標(x,z)からdirx,dirzに従って駒をひっくり返すことができるかを示す
    private bool isDirction(int dirx,int dirz,int x,int z)
    {
        while((0<=x && x<=7) &&(0<=z && z <= 7))
        {
            x += dirx;
            z += dirz;
            if ((x < 0 || 7 < x) || (z < 0 || 7 < z))
                break;
            Debug.Log("x:" + x + ",z:" + z);
            if (squares[z, x]== currentPlayer)
            {
                return true;
            }
            else if (squares[z, x] == EMPTY)
                return false;
            else if (squares[z, x] != currentPlayer)
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
                    squares[j, i] = WHITE;
                    GameObject whiteKoma = Instantiate(koma);
                    pos = whiteKoma.transform.position;
                    pos.x = i; pos.z = j; pos.y = 0.35f;
                    whiteKoma.transform.position = pos;
                    reverse(whiteKoma);

                    int key = i * 10 + j;
                    Debug.Log("i:" + i + ",j:" + j + "key:" + key);
                    map.Add(key, whiteKoma);

                }
                else if ((i == 4 && j == 4) || (i == 3 && j == 3))
                {
                    squares[j, i]= BLACK;
                    GameObject blackKoma = Instantiate(koma);
                    pos = blackKoma.transform.position;
                    pos.x = i; pos.z = j; pos.y = 0.3f;
                    blackKoma.transform.position = pos;


                    int key = i * 10 + j;

                    Debug.Log("i:"+i+",j:"+j+"key:" + key);
                    map.Add(key, blackKoma);

                }
                else
                {
                    squares[j, i]=EMPTY;
                }
            }

    }
    //勝敗の判定を行う。
    private void isWin()
    {
        int wcnt = 0;
        int bcnt = 0;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (squares[j, i] == 1)
                    wcnt++;
                else if (squares[j, i] == -1)
                    bcnt++;
            }
        }
        /*if (wcnt > bcnt)
            Debug.Log("white player is winner");
        else
            Debug.Log("black player is winner");*/
        blackCnt = bcnt;
        whiteCnt = wcnt;
        Panel.SetActive(true);
        PlayerTurn.SetActive(false);
        slide.SlideIn();

    }
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
    private void reverse(GameObject koma)
    {
        
        Vector3 localAngle = koma.transform.localEulerAngles;
        localAngle.x = 180.0f;
        koma.transform.localEulerAngles = localAngle;

    }
}
