using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DaipanButton : MonoBehaviour
{
    public GameObject DaipanController;
    public GameObject GameController;
    public DaipanGauge daipanGauge;

    public void OnClick()
    {
        if (daipanGauge.isDaipan())
        {
            DaipanController.SetActive(true);
            GameController.SetActive(false);
            daipanGauge.gaugeCount = 0;
        }
        
        
    }


}

