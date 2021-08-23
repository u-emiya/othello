using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class homeButton : MonoBehaviour
{
    public PanelSlider panel;
    public GameObject GameController;

    public void OnClick()
    {
        panel.SlideIn();
        GameController.SetActive(false);

//        GetComponent<GameController>().enabled = false;
        

    }


}