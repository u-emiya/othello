using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{

    Image buttonImage_;
    public Sprite sprite;
    public void OnClick()
    {
        buttonImage_ = GetComponent<Image>();
        buttonImage_.sprite =sprite;
       // FadeManager.Instance.LoadScene("othello_title", 2.0f);
        //SceneManager.LoadScene("othello");
    }
}