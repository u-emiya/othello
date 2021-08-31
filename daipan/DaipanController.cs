using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DaipanController : MonoBehaviour
{
    //[SerializeField]
    private Transform m_shootPoint = null;
    //[SerializeField]
    private Transform m_target = null;
    [SerializeField]
    private GameObject m_shootObject = null;
    public GameController gameController;
    private Camera cameobj;
    private RaycastHit hit;

    private IDictionary<int, GameObject> blockMap = new Dictionary<int, GameObject>();
    private IDictionary<int, GameObject> komaMap = new Dictionary<int, GameObject>();
    private IDictionary<int, GameObject> saveKomaMap = new Dictionary<int, GameObject>();
    private IDictionary<int, GameObject> underKomaMap = new Dictionary<int, GameObject>();
    private IDictionary<int, int> whiteOrBrack = new Dictionary<int, int>();

    private int[,] squares = new int[8, 8];
    private int[,] newSquares = new int[10, 10];
   
    //public GameObject DaipanController;
    public GameObject GameController;
    public DaipanGauge daipanGauge;

    //test0828
    public DcToGc dcToGc;

    // Start is called before the first frame update
    void Start()
    {
        cameobj = GameObject.Find("Main Camera").GetComponent<Camera>();
        squares = gameController.getSquares();
        komaMap = gameController.getKomaMap();

        foreach(int key in whiteOrBrack.Keys)
        {
            int underX = key / 10;
            int underZ = key % 10;
            if (squares[underZ, underX] == 0)
            {
                squares[underZ, underX] = whiteOrBrack[key];
                komaMap.Add(key, underKomaMap[key]);
            }
        }

        GameObject[] blocks = GameObject.FindGameObjectsWithTag("BoardPiece");
        for (int i = 0; i < blocks.Length; i++)
        {
            int blockX = (int)blocks[i].transform.position.x;
            int blockZ = (int)blocks[i].transform.position.z;
            //Debug.Log(blocks[i].name + ":" + "x ---" + blockX + ", z ---" + blockZ);
            int key = blockX * 10 + blockZ;
            blockMap.Add(key,blocks[i]);
        }
        m_shootPoint = blockMap[44].transform;
        m_target = blockMap[54].transform;
    }
    int x = 0;
    int z = 0;
     private void Update()
    {

        
        if (Input.GetMouseButtonDown(0))
        {
            bool isCompletedDaipan = false;

            Ray ray = cameobj.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && !hit.collider.gameObject.CompareTag("Table"))
            {

                //new board information
                saveKomaMap = new Dictionary<int, GameObject>();
                newSquares = new int[10, 10];
                squares = gameController.getSquares();
                komaMap = gameController.getKomaMap();


                outPiecesSlide();
                int player = gameController.getCurrentPlayer()*(-1);
                gameController.setCurrentPlayer(player);

                x = Mathf.RoundToInt(hit.collider.gameObject.transform.position.x);
                z = Mathf.RoundToInt(hit.collider.gameObject.transform.position.z);
                movePosition(x, z);
                isCompletedDaipan = true;

                gameController.setKomaMap(saveKomaMap);
            }
            /*
            Debug.Log("this method");
            foreach (KeyValuePair<int, GameObject> item in komaMap)
            {
                Debug.LogFormat("[{0}:{1}]", item.Key, item.Value);
            }*/
            //komaMap = gameController.getKomaMap();
            /*
            Debug.Log("game controller method");
            foreach (KeyValuePair<int, GameObject> item in komaMap)
            {
                Debug.LogFormat("[{0}:{1}]", item.Key, item.Value);
            }*/


            if (isCompletedDaipan)
            {
                //GameController.SetActive(true);
                dcToGc.startTimeKeeper(3.0f);
                this.gameObject.SetActive(false);
            }
            //this.enabled = false;
        }
       
     
    }

    private void Shoot(Vector3 i_targetPosition)
    {
        // とりあえず適当に60度でかっ飛ばすとするよ！
        ShootFixedAngle(i_targetPosition, 85.0f);
    }

    private void ShootFixedAngle(Vector3 i_targetPosition, float i_angle)
    {
        float speedVec = ComputeVectorFromAngle(i_targetPosition, i_angle);
        if (speedVec <= 0.0f)
        {
            // その位置に着地させることは不可能のようだ！
            Debug.LogWarning("!!");
            return;
        }

        Vector3 vec = ConvertVectorToVector3(speedVec, i_angle, i_targetPosition);
        Debug.Log(vec);
        if (float.IsNaN(vec.x))
        {
            Debug.Log("HIT HIT HIT HIT HIT HIT HIT HIT HIT HIT HIT HIT HIT HIT HIT ");
            vec = new Vector3(0f, 7.0f, 0f);
        }
        InstantiateShootObject(vec);
    }

    private float ComputeVectorFromAngle(Vector3 i_targetPosition, float i_angle)
    {
        // xz平面の距離を計算。
        Vector2 startPos = new Vector2(m_shootPoint.transform.position.x, m_shootPoint.transform.position.z);
        Vector2 targetPos = new Vector2(i_targetPosition.x, i_targetPosition.z);
        float distance = Vector2.Distance(targetPos, startPos);

        float x = distance;
        float g = Physics.gravity.y;
        float y0 = m_shootPoint.transform.position.y;
        float y = i_targetPosition.y;

        // Mathf.Cos()、Mathf.Tan()に渡す値の単位はラジアンだ。角度のまま渡してはいけないぞ！
        float rad = i_angle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(rad);
        float tan = Mathf.Tan(rad);

        float v0Square = g * x * x / (2 * cos * cos * (y - y0 - x * tan));

        // 負数を平方根計算すると虚数になってしまう。
        // 虚数はfloatでは表現できない。
        // こういう場合はこれ以上の計算は打ち切ろう。
        if (v0Square <= 0.0f)
        {
            return 0.0f;
        }

        float v0 = Mathf.Sqrt(v0Square);
        return v0;
    }

    private Vector3 ConvertVectorToVector3(float i_v0, float i_angle, Vector3 i_targetPosition)
    {
        Vector3 startPos = m_shootPoint.transform.position;
        Vector3 targetPos = i_targetPosition;
        startPos.y = 0.0f;
        targetPos.y = 0.0f;

        Vector3 dir = (targetPos - startPos).normalized;
        Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 vec = i_v0 * Vector3.right;

        vec = yawRot * Quaternion.AngleAxis(i_angle, Vector3.forward) * vec;

        return vec;
    }

    private void InstantiateShootObject(Vector3 i_shootVector)
    {
        if (m_shootObject == null)
        {
            throw new System.NullReferenceException("m_shootObject");
        }

        if (m_shootPoint == null)
        {
            throw new System.NullReferenceException("m_shootPoint");
        }

        //var obj = Instantiate<GameObject>(m_shootObject, m_shootPoint.position, Quaternion.identity);
        /*
        Debug.Log("-----koma List------");
        foreach(KeyValuePair<int, GameObject> item in komaMap)
        {
           Debug.LogFormat("[{0}:{1}]",item.Key,item.Value);
        }*/
        GameObject obj = shootKoma;
        // Debug.Log("test:"+obj.name);

        var rigidbody = obj.GetComponent<Rigidbody>();
        if (rigidbody == null)
            rigidbody = obj.AddComponent<Rigidbody>();
        if (i_shootVector == null)
        {
            throw new System.NullReferenceException("error");
        }
        // 速さベクトルのままAddForce()を渡してはいけないぞ。力(速さ×重さ)に変換するんだ
        Vector3 force = i_shootVector * rigidbody.mass;

        rigidbody.AddForce(force, ForceMode.Impulse);


        
    }


    // ------      システム内での処理     -----
    //座標(x,z)が台パンの中心地
    public void movePosition(int x, int z)
    {
        int direction;
        int aend = 5, bend = 5, bstart = 3, astart = 3;
        int a, b, c, d, e;

        for (a = astart; a >= 0; a--)
        {
            for (b = bstart; b < bend; b++)
            {
                if (squares[b, a] != 0)
                {
                    direction = decidedDirection(a, b, x, z);
                    sliding(direction, a, b);
                }
            }
            b--;
            for (c = a + 1; c < aend; c++)
            {
                if (squares[b, c] != 0)
                {

                    direction = decidedDirection(c, b, x, z);
                    sliding(direction, c, b);
                }
            }

            c--;
            for (d = b - 1; d >= bstart; d--)
            {
                if (squares[d, c] != 0)
                {

                    direction = decidedDirection(c, d, x, z);
                    sliding(direction, c, d);
                }
            }

            d++;
            for (e = c - 1; e > astart; e--)
            {
                if (squares[d, e] != 0)
                {

                    direction = decidedDirection(e, d, x, z);
                    sliding(direction, e, d);
                }
            }

            e++;
            aend++;
            bend++;
            astart--;
            bstart--;
            if (astart < 0)
                break;
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                squares[j, i] = newSquares[j + 1, i + 1];
            }
        }
      

    }
    /*
    public void movePosition(int x, int z)
    {
        //Debug.Log("before");
        //gameController.printBoard();
        int direction;

        int aend = 8, bend = 8, bstart = 0, astart = 0;
        int a, b, c, d, e;
        int keyValue = 0;
        for (a = astart; a < aend; a++)
        {
            for (b = bstart; b < bend; b++)
            {
                keyValue = 10 * a + b;
                if (squares[b,a]!=0)
                {
                    direction = decidedDirection(a, b, x, z);
                    sliding(direction, a, b);
                }
            }
            b--;
            for (c = a + 1; c < aend; c++)
            {
                keyValue = 10 * c + b;
                if (squares[b, c]!=0)
                {

                    direction = decidedDirection(c, b, x, z);
                    sliding(direction, c, b);
                }
            }

            c--;
            for (d = b - 1; d >= bstart; d--)
            {
                keyValue = 10 * c + d;
                if (squares[d, c] != 0)
                {

                    direction = decidedDirection(c, d, x, z);
                    sliding(direction, c, d);
                }
            }

            d++;
            for (e = c - 1; e > astart; e--)
            {
                keyValue = 10 * e + d;
                if (squares[d, e] != 0)
                {

                    direction = decidedDirection(e, d, x, z);
                    sliding(direction, e, d);
                }
            }

            e++;
            aend--;
            bend--;
            astart++;
            bstart++;
        }

        //debug
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                squares[j, i] = newSquares[j+1 , i+1 ];
            }
        }
        //Debug.Log("after");
        //gameController.printBoard();
        //komaMap = saveKomaMap;             mark

    }
    */

    private GameObject shootKoma = null;
    public void sliding(int direction, int x, int z)
    {
        int i = 0;
        while (i<100)
        {
            i++;
            int val = Random.Range(0, 2);

            if (direction == 0)
            {
                if (val % 3 == 0 && newSquares[ z  ,x ]==0)
                {
                    slidePieceProcess(x, z, x - 1, z - 1);
                }
                else if (val % 3 == 1 && newSquares[ z + 1 ,x ] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z );
                }
                else if (val % 3 == 2 && newSquares[ z  , x + 1] == 0)
                {
                    slidePieceProcess(x, z, x , z - 1);
                }
                else
                    continue;

            }
            else if (direction == 1)
            {
                if (val % 3 == 0 && newSquares[ z + 1 ,x  ] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z);
                }
                else if (val % 3 == 1 && newSquares[ z + 2 ,x ] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z + 1);
                }
                else if(val % 3 == 2 && newSquares[ z  ,x ] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z - 1);
                }
                else
                    continue;

            }
            else if (direction == 2)
            {
                if (val % 3 == 0 && newSquares[z + 2 , x ] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z + 1);
                }
                else if (val % 3 == 1 && newSquares[z + 2, x + 1] == 0)
                {
                    slidePieceProcess(x, z, x , z + 1);
                }
                else if(val % 3 == 2 && newSquares[ z + 1, x] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z );
                }
                else
                    continue;

            }
            else if (direction == 3)
            {
                if (val % 3 == 0 && newSquares[ z  ,x + 1 ] == 0)
                {
                    slidePieceProcess(x, z, x, z - 1);
                }
                else if (val % 3 == 1 && newSquares[ z , x + 2 ] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z - 1);
                }
                else if (val % 3 == 2 && newSquares[ z , x ] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z - 1);
                }
                else
                    continue;

            }
            else if (direction == 5)
            {
                if (val % 3 == 0 && newSquares[z + 2, x + 1] == 0)
                {
                    slidePieceProcess(x, z, x, z + 1);
                }
                else if (val % 3 == 1 && newSquares[z + 2, x + 2] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z + 1);
                }
                else if (val % 3 == 2 && newSquares[z + 2, x ] == 0)
                {
                    slidePieceProcess(x, z, x - 1, z + 1);
                }
                else
                    continue;

            }
            else if (direction == 6)
            {
                if (val % 3 == 0 && newSquares[ z , x + 2 ] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z - 1);
                }
                else if (val % 3 == 1 && newSquares[ z + 1, x + 2] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z);
                }
                else if (val % 3 == 2 && newSquares[z  , x  + 1] == 0)
                {
                    slidePieceProcess(x, z, x, z - 1);
                }
                else
                    continue;

            }
            else if (direction == 7)
            {
                if (val % 3 == 0 && newSquares[ z + 1 , x + 2] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z); 
                }
                else if (val % 3 == 1 && newSquares[z + 2, x + 2] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z + 1);
                }
                else if (val % 3 == 2 && newSquares[ z , x + 2 ] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z - 1);
                }
                else
                    continue;

            }
            else if (direction == 8)
            {
                if (val % 3 == 0 && newSquares[ z + 2, x + 2] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z + 1);
                }
                else if (val % 3 == 1 && newSquares[z + 2, x + 1] == 0)
                {
                    slidePieceProcess(x, z, x, z + 1);
                }
                else if (val % 3 == 2 && newSquares[ z + 1 , x + 2] == 0)
                {
                    slidePieceProcess(x, z, x + 1, z);
                }
                else
                    continue;
            }
            else if (direction == 4)
            {
                //squares[z, x] *= (-1);
                slidePieceProcess(x, z, x , z);
            }
            squares[z, x] = 0;
                break;
        }
        if (i == 100)
        {

            int key = x * 10 + z;
            if (!saveKomaMap.ContainsKey(key))
            {
                //squares[z, x] *= (-1);
                slidePieceProcess(x, z, x, z);
            }
            else
            {
                if (!underKomaMap.ContainsKey(key))
                {
                    var obj = komaMap[key];
                    underKomaMap.Add(key, obj);
                    whiteOrBrack.Add(key, squares[z, x]);
                }
            }
            squares[z, x] = 0;

            /*
            var obj = komaMap[(x*10+z)];
            obj.GetComponent<Collider>().isTrigger = true;
            squares[z, x] = 0;
            */
            /*
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if (newSquares[z + k + 1, x + j + 1] == 0)
                    {
                        slidePieceProcess(x, z, x+j, z+k);
                        squares[z, x] = 0;
                        return;
                    }
                }
            }
            if (newSquares[z, x] == 0)
            {
                slidePieceProcess(x, z, x, z);
            }
            else
            {

                komaMap[x * 10 + z].GetComponent<Collider>().isTrigger = true;
                komaMap.Remove(x * 10 + z);
            }*/
        }

    }

    public int decidedDirection(int x1, int z1, int x2, int z2)
    {
        int a = x1 - x2, b = z1 - z2;
        if (a < 0 && b < 0)
            return 0;
        else if (a < 0 && b == 0)
            return 1;
        else if (a < 0 && b > 0)
            return 2;
        else if (a == 0 && b < 0)
            return 3;
        else if (a == 0 && b > 0)
            return 5;
        else if (a > 0 && b < 0)
            return 6;
        else if (a > 0 && b == 0)
            return 7;
        else if (a > 0 && b > 0)
            return 8;
        return 4;
    }

    public void slideObject(int afterKey, int beforeKey)
    {
        GameObject saveKoma = komaMap[beforeKey];
        komaMap.Remove(beforeKey);
        saveKomaMap.Add(afterKey, saveKoma);

    }

    public void slidePieceProcess(int shoot_x,int shoot_z,int target_x, int target_z)
    {
        Debug.Log("shoot::::( x , z )---( " + shoot_x + " , " + shoot_z + " )");
        Debug.Log("target:::( x , z )---( " + target_x + " , " + target_z + " )");
      
        int shootKey, targetKey;
        newSquares[target_z + 1, target_x + 1] = squares[shoot_z, shoot_x]*(-1);
        shootKey = shoot_x * 10 + shoot_z;
        targetKey = target_x * 10 + target_z;
        shootKoma = komaMap[shootKey];
        m_shootPoint = blockMap[shootKey].transform;
        //if(blockMap.ContainsKey(targetKey))
        m_target = blockMap[targetKey].transform;
        //if (decidedDirection(shoot_x, shoot_z, target_x, target_z) != 4)
            Shoot(m_target.position);
        slideObject(targetKey, shootKey);

        saveOutOfPieces(target_x, target_z);

    }

    private List<GameObject> leftOutPiece = new List<GameObject>();
    private List<GameObject> rightOutPiece = new List<GameObject>();
    private List<GameObject> upOutPiece = new List<GameObject>();
    private List<GameObject> downOutPiece = new List<GameObject>();

    public void saveOutOfPieces(int outx,int outz)
    {
        int shootKey = outx * 10 + outz;
        if (outx == -1)
        {
            shootKoma.tag = "Table";
            leftOutPiece.Add(shootKoma);
        }
        else if (outx == 8)
        {
            shootKoma.tag = "Table";
            rightOutPiece.Add(shootKoma);
        }
        else if (outz == -1)
        {
            shootKoma.tag = "Table";
            downOutPiece.Add(shootKoma);
        }
        else if (outz == 8)
        {
            shootKoma.tag = "Table";
            upOutPiece.Add(shootKoma);
        }
    }
    public void outPiecesSlide()
    {
        int shootKey = 33;
        int targetKey = 23;
        outPieceShoot(leftOutPiece, shootKey, targetKey);

        targetKey = 43;
        outPieceShoot(rightOutPiece, shootKey, targetKey);

        targetKey = 32;
        outPieceShoot(downOutPiece, shootKey, targetKey);

        targetKey = 34;
        outPieceShoot(upOutPiece, shootKey, targetKey);


    }

    public void outPieceShoot(List<GameObject> pieceList,int shootPoint, int targetPoint)
    {
        foreach (GameObject piece in pieceList)
        {
            shootKoma = piece;
            m_shootPoint = blockMap[shootPoint].transform;
            m_target = blockMap[targetPoint].transform;
            Shoot(m_target.position);
        }
    }
}



