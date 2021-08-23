using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaipanGauge : MonoBehaviour
{

    public GameObject image;
    public GameObject downLayer;
    public GameController gc;
    public DaipanController dc;

    // Start is called before the first frame update
    void Start()
    {
        buttonPosition = image.transform.position;
    }
    public static int maxGauge = 12;
    public static int turn = 1;
    public  float gaugeCount=0;
    private bool isDaipanAction = false;
    Vector3 buttonPosition;

    void Update()
    {
        if (gaugeCount == 0 && image.GetComponent<Image>().fillAmount != 0.0f)
        {
            image.GetComponent<Image>().fillAmount -= 0.005f;
            image.transform.position = buttonPosition;
            isDaipanAction = false;
            downLayer.SetActive(true);
        }
        else
        {

            if (turn != gc.getTurn())
            {
                gaugeCount += 1.0f;
                turn = gc.getTurn();
            }
            float x = gaugeCount / maxGauge;
            if (x > image.GetComponent<Image>().fillAmount)
                image.GetComponent<Image>().fillAmount += 0.01f;
            if (image.GetComponent<Image>().fillAmount >= 1.0)
            {
                isDaipanAction = true;
                downLayer.SetActive(false);
                image.transform.position = buttonPosition + Random.insideUnitSphere * 0.01f;
           
            }
        }
    }

    public bool isDaipan()
    {
        return isDaipanAction;
    }

}
