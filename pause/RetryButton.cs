using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void OnClick()
    {
        FadeManager.Instance.LoadScene("othello", 2.0f);
        //SceneManager.LoadScene("othello");
    }
}
