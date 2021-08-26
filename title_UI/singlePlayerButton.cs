using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class singlePlayerButton : MonoBehaviour
{
    public TitleController tc;
    public GameObject Panel;
    public GameObject selectPlayerPanel;
    public Button button;
    public testtest tt;
    
    public void OnClick()
    {
        tc.setPlayerNum(1);
        if (!Panel.activeSelf)
        {
            Panel.SetActive(true);
            selectPlayerPanel.SetActive(false);
            button.GetComponent<testtest>().enabled = true;
            tt.setSwitch();
        }
        else
        {
            FadeManager.Instance.LoadScene("othello", 2.0f);
        }
        //SceneManager.LoadScene("othello");
    }
}
