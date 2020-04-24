using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class singlePlayerButton : MonoBehaviour
{
    public TitleController tc;
    public void OnClick()
    {
        tc.setPlayerNum(1);
        FadeManager.Instance.LoadScene("othello", 2.0f);
        //SceneManager.LoadScene("othello");
    }
}
