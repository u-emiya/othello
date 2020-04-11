using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiPlayerButton : MonoBehaviour
{
    public TitleController tc;
    public void OnClick()
    {
        tc.setPlayerNum(2);
        FadeManager.Instance.LoadScene("othello", 2.0f);
       // SceneManager.LoadScene("othello");
    }
}
