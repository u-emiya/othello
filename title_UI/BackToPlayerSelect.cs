using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToPlayerSelect : MonoBehaviour
{
    public GameObject SelectPlayerPanel;
    public GameObject SelectModePanel;
    public Button button;

    public void OnClick()
    {
        SelectPlayerPanel.SetActive(true);
        SelectModePanel.SetActive(false);
        button.GetComponent<testtest>().enabled = false;

    }
}
