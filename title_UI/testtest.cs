using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testtest : MonoBehaviour
{
    
    public Button MenuButton;
    private bool switchActive=true;
    private void Start()
    {
        MenuButton.animator.SetTrigger("Highlighted");

    }

    private void Update()
    {
        if (this.isActiveAndEnabled && switchActive)
        {
            MenuButton.animator.SetTrigger("Highlighted");
            switchActive = false;
        }
        
    }

    public void setSwitch()
    {
        switchActive = true;
    }
}
