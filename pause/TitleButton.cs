using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void OnClick()
    {
        FadeManager.Instance.LoadScene("othello_title", 2.0f);
        //SceneManager.LoadScene("othello");
    }
}