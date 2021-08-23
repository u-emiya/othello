using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPLevelSelectRight : MonoBehaviour
{

    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public TitleController tc;


    public static int CPLevel = 1;

    public void Start()
    {
        CPLevel = tc.getCPLevel();
    }

    public void OnClick()
    {
        CPLevel = tc.getCPLevel();
        CPLevel++;
        if (CPLevel > 3)
            CPLevel = 1;
        if (CPLevel == 2)
        {
            level1.SetActive(false);
            level2.SetActive(true);
        }
        else if (CPLevel == 3)
        {
            level2.SetActive(false);
            level3.SetActive(true);
        }
        else
        {
            level3.SetActive(false);
            level1.SetActive(true);
        }
        tc.setCPLevel(CPLevel);

    }
}
