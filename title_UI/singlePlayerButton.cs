using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class singlePlayerButton : MonoBehaviour
{
    public TitleController tc;
    public GameObject Panel;
    public GameObject selectPlayerPanel;

    public void OnClick()
    {
        tc.setPlayerNum(1);
        if (!Panel.activeSelf)
        {
            Panel.SetActive(true);
            selectPlayerPanel.SetActive(false);
        }
        else
        {
            FadeManager.Instance.LoadScene("othello", 2.0f);
        }
        //SceneManager.LoadScene("othello");
    }
}
