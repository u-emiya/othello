using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class backButton : MonoBehaviour
{
    public PanelSlider panel;
    public GameObject GameController;


    public void OnClick()
    {
        panel.SlideOut();
        GameController.SetActive(true);
        

    }


}
